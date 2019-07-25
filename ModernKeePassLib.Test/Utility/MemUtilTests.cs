using System.Text;
using ModernKeePassLib.Cryptography;
using ModernKeePassLib.Utility;
using Xunit;

namespace ModernKeePassLib.Test.Utility
{
    public class MemUtilTests
    {
        private byte[] _pb = CryptoRandom.Instance.GetRandomBytes((uint)CryptoRandom.NewWeakRandom().Next(0, 0x2FFFF));

        [Fact]
        public void TestGzip()
        {
            var pbCompressed = MemUtil.Compress(_pb);
            Assert.True(MemUtil.ArraysEqual(MemUtil.Decompress(pbCompressed), _pb));
        }

        [Fact]
        public void TestMemUtil()
        {
            var enc = StrUtil.Utf8;
            _pb = enc.GetBytes("012345678901234567890a");
            var pbN = enc.GetBytes("9012");
            Assert.Equal(9, MemUtil.IndexOf(_pb, pbN));

            pbN = enc.GetBytes("01234567890123");
            Assert.Equal(0, MemUtil.IndexOf(_pb, pbN));

            pbN = enc.GetBytes("a");
            Assert.Equal(21, MemUtil.IndexOf(_pb, pbN));

            pbN = enc.GetBytes("0a");
            Assert.Equal(20, MemUtil.IndexOf(_pb, pbN));

            pbN = enc.GetBytes("1");
            Assert.Equal(1, MemUtil.IndexOf(_pb, pbN));

            pbN = enc.GetBytes("b");
            Assert.True(MemUtil.IndexOf(_pb, pbN) < 0);

            pbN = enc.GetBytes("012b");
            Assert.True(MemUtil.IndexOf(_pb, pbN) < 0);
        }

        [Fact]
        public void TestBase32()
        {
            var pbRes = MemUtil.ParseBase32("MY======");
            var pbExp = Encoding.UTF8.GetBytes("f");
            Assert.True(MemUtil.ArraysEqual(pbRes, pbExp));

            pbRes = MemUtil.ParseBase32("MZXQ====");
            pbExp = Encoding.UTF8.GetBytes("fo");
            Assert.True(MemUtil.ArraysEqual(pbRes, pbExp));

            pbRes = MemUtil.ParseBase32("MZXW6===");
            pbExp = Encoding.UTF8.GetBytes("foo");
            Assert.True(MemUtil.ArraysEqual(pbRes, pbExp));

            pbRes = MemUtil.ParseBase32("MZXW6YQ=");
            pbExp = Encoding.UTF8.GetBytes("foob");
            Assert.True(MemUtil.ArraysEqual(pbRes, pbExp));

            pbRes = MemUtil.ParseBase32("MZXW6YTB");
            pbExp = Encoding.UTF8.GetBytes("fooba");
            Assert.True(MemUtil.ArraysEqual(pbRes, pbExp));

            pbRes = MemUtil.ParseBase32("MZXW6YTBOI======");
            pbExp = Encoding.UTF8.GetBytes("foobar");
            Assert.True(MemUtil.ArraysEqual(pbRes, pbExp));

            pbRes = MemUtil.ParseBase32("JNSXSIDQOJXXM2LEMVZCAYTBONSWIIDPNYQG63TFFV2GS3LFEBYGC43TO5XXEZDTFY======");
            pbExp = Encoding.UTF8.GetBytes("Key provider based on one-time passwords.");
            Assert.True(MemUtil.ArraysEqual(pbRes, pbExp));
        }

        [Fact]
        public void TestMemUtil2()
        { 
            var i = 0 - 0x10203040;
            var pbRes = MemUtil.Int32ToBytes(i);
            Assert.Equal("C0CFDFEF", MemUtil.ByteArrayToHexString(pbRes));
            Assert.Equal(MemUtil.BytesToUInt32(pbRes), (uint)i);
            Assert.Equal(MemUtil.BytesToInt32(pbRes), i);
        }
    }
}