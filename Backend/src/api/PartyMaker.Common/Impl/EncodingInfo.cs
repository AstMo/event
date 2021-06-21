using System.Text;

namespace PartyMaker.Common.Impl
{
    public class EncodingInfo
	{
		public Encoding Encoding { get; }
		public int BomSize { get; }

		public EncodingInfo(Encoding encoding, int bomSize)
		{
			Encoding = encoding;
			BomSize = bomSize;
		}
	}
}