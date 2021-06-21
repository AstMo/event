using PartyMaker.Common.Impl;
using System.IO;

namespace PartyMaker.Common
{
    public interface IEncodingDetectionService
	{
		EncodingInfo Detect(string filePath);
		EncodingInfo Detect(Stream stream);
		EncodingInfo Detect(Stream stream, long heuristicSampleSize);
		EncodingInfo Detect(byte[] data);
	}
}
