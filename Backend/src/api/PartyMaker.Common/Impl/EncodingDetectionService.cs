using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PartyMaker.Common.Impl
{
	class EncodingDetectionService : IEncodingDetectionService
	{
		private const long DefaultHeuristicSampleSize = 0x10000;

		public EncodingInfo Detect(string filePath)
		{
			using var stream = File.OpenRead(filePath);
			return Detect(stream);
		}

		public EncodingInfo Detect(Stream stream)
		{
			return Detect(stream, DefaultHeuristicSampleSize);
		}

		public EncodingInfo Detect(Stream stream, long heuristicSampleSize)
		{
			if (stream == null)
				throw new ArgumentNullException(nameof(stream), "Must provide a valid stream!");

			if (!stream.CanRead)
				throw new ArgumentException("Provided file stream is not readable!", nameof(stream));

			if (!stream.CanSeek)
				throw new ArgumentException("Provided file stream cannot seek!", nameof(stream));

			var originalPos = stream.Position;
			stream.Seek(0, SeekOrigin.Begin);

			//BOM detection
			var bomBytes = new byte[stream.Length > 4 ? 4 : stream.Length];
			stream.Read(bomBytes, 0, bomBytes.Length);

			var encodingFound = DetectByBom(bomBytes);
			if (encodingFound != null)
			{
				stream.Seek(originalPos, SeekOrigin.Begin);
				return encodingFound;
			}

			//Heuristics detection
			var sampleBytes = new byte[heuristicSampleSize > stream.Length ? stream.Length : heuristicSampleSize];
			Array.Copy(bomBytes, sampleBytes, bomBytes.Length);
			if (stream.Length > bomBytes.Length)
				stream.Read(sampleBytes, bomBytes.Length, sampleBytes.Length - bomBytes.Length);
			encodingFound = DetectXmlSequence(sampleBytes) ?? DetectByHeuristics(sampleBytes);

			stream.Seek(originalPos, SeekOrigin.Begin);
			return encodingFound;
		}

		public EncodingInfo Detect(byte[] data)
		{
			if (data == null)
				throw new ArgumentNullException(nameof(data), "Must provide a valid text data byte array!");

			//BOM detection
			var encodingFound = DetectByBom(data);
			if (encodingFound != null)
			{
				return encodingFound;
			}

			//Heuristics detection
			encodingFound = DetectXmlSequence(data) ?? DetectByHeuristics(data);

			return encodingFound;
		}

		private EncodingInfo DetectByBom(byte[] bom)
		{
			if (bom == null)
				throw new ArgumentNullException(nameof(bom), "Must provide a valid BOM byte array!");

			if (bom.Length < 2)
				return null;

			//UTF16 LE
			if (
				bom[0] == 0xff && bom[1] == 0xfe &&
				(bom.Length < 4 || bom[2] != 0 || bom[3] != 0)
			)


				return new EncodingInfo(Encoding.Unicode, 2);

			//UTF16 BE
			if (bom[0] == 0xfe && bom[1] == 0xff)
				return new EncodingInfo(Encoding.BigEndianUnicode, 2);

			if (bom.Length < 3)
				return null;

			//UTF8
			if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf)
				return new EncodingInfo(Encoding.UTF8, 3);

			//UTF7
			if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76)
				return new EncodingInfo(Encoding.UTF7, 4);

			if (bom.Length < 4)
				return null;

			//UTF32 LE
			if (bom[0] == 0xff && bom[1] == 0xfe && bom[2] == 0 && bom[3] == 0)
				return new EncodingInfo(Encoding.UTF32, 4);

			//UTF32 BE
			if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff)
				return new EncodingInfo(Encoding.GetEncoding(12001), 4);

			return null;
		}

		private EncodingInfo DetectByHeuristics(byte[] sample)
		{
			long oddBinaryNullsInSample = 0;
			long evenBinaryNullsInSample = 0;
			long suspiciousUtf8BytesTotal = 0;
			var skipUtf8Bytes = 0;

			long i = 0;
			while (i < sample.Length)
			{
				//binary null distribution
				if (sample[i] == 0)
				{
					if (i % 2 == 0)
						evenBinaryNullsInSample++;
					else
						oddBinaryNullsInSample++;
				}

				//suspicious sequences (look like UTF-8)
				if (skipUtf8Bytes == 0)
				{
					var lengthFound = DetectSuspiciousUtf8SequenceLength(sample, i);
					if (lengthFound > 0)
					{
						suspiciousUtf8BytesTotal += lengthFound;
						skipUtf8Bytes = lengthFound - 1;
					}
				}
				else
				{
					skipUtf8Bytes--;
				}

				i++;
			}

			// UTF-16 LE - in english / european environments, this is usually characterized by a 
			// high proportion of odd binary nulls (starting at 0), with (as this is text) a low 
			// proportion of even binary nulls.
			// The thresholds here used (less than 20% nulls where you expect non-nulls, and more than
			// 60% nulls where you do expect nulls) are completely arbitrary.
			if (((evenBinaryNullsInSample * 2.0) / sample.Length) < 0.2
				&& ((oddBinaryNullsInSample * 2.0) / sample.Length) > 0.6
			)
				return new EncodingInfo(Encoding.Unicode, 0);


			// UTF-16 BE - in english / european environments, this is usually characterized by a 
			// high proportion of even binary nulls (starting at 0), with (as this is text) a low 
			// proportion of odd binary nulls.
			// The thresholds here used (less than 20% nulls where you expect non-nulls, and more than
			// 60% nulls where you do expect nulls) are completely arbitrary.
			if (((oddBinaryNullsInSample * 2.0) / sample.Length) < 0.2
				&& ((evenBinaryNullsInSample * 2.0) / sample.Length) > 0.6
			)
				return new EncodingInfo(Encoding.BigEndianUnicode, 0);


			// UTF-8
			if (sample.Length - suspiciousUtf8BytesTotal == 0)
				return new EncodingInfo(Encoding.UTF8, 0);

			return null;
		}

		private static int DetectSuspiciousUtf8SequenceLength(byte[] sample, long i)
		{
			if (sample[i] <= 0x7F)
			{
				return 1;
			}

			if (sample.Length > i + 1 &&
				0xC0 <= sample[i] && sample[i] <= 0xDF &&
				0x80 <= sample[i + 1] && sample[i + 1] <= 0xBF)
			{
				return 2;
			}

			if (sample.Length > i + 2 &&
				0xE0 <= sample[i] && sample[i] <= 0xEF &&
				0x80 <= sample[i + 1] && sample[i + 1] <= 0xBF &&
				0x80 <= sample[i + 2] && sample[i + 2] <= 0xBF)
			{
				return 3;
			}

			if (sample.Length > i + 3 &&
				0xF0 <= sample[i] && sample[i] <= 0xF7 &&
				0x80 <= sample[i + 1] && sample[i + 1] <= 0xBF &&
				0x80 <= sample[i + 2] && sample[i + 2] <= 0xBF &&
				0x80 <= sample[i + 3] && sample[i + 3] <= 0xBF)
			{
				return 4;
			}

			return 0;
		}

		private static EncodingInfo DetectXmlSequence(byte[] sample)
		{
			//Searching xml open tag
			var xmlOpenTag = new byte[] { 0x3C, 0x3F, 0x78, 0x6D, 0x6C };//<?xml
			const byte xmlCloseTag = 0x3E; //?>

			var haveOpenTag = xmlOpenTag.Select((x, i) => sample.Length > i && x == sample[i]).All(x => x);
			if (!haveOpenTag)
			{
				return null;
			}

			var closeTagIndex = Array.FindIndex(sample, xmlOpenTag.Length, x => x == xmlCloseTag);
			if (closeTagIndex < xmlOpenTag.Length)
			{
				return null;
			}

			//Searching encoding attribute
			var tag = Encoding.ASCII.GetString(sample, 0, closeTagIndex - 1); //get open tag attributes, without '<?xml' and '?>'
			var encodingReg = new Regex("encoding\\s*=\\s*['\"]([^'\"]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			var encodingMatch = encodingReg.Match(tag);
			if (!encodingMatch.Success || encodingMatch.Groups.Count < 2)
			{
				return null;
			}

			try
			{
				return new EncodingInfo(Encoding.GetEncoding(encodingMatch.Groups[1].Value), 0);
			}
			catch (ArgumentException)
			{
				return null;
			}
		}
    }
}