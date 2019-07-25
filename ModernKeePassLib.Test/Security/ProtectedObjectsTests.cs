using System.Text;
using ModernKeePassLib.Cryptography;
using ModernKeePassLib.Security;
using ModernKeePassLib.Utility;
using Xunit;

namespace ModernKeePassLib.Test.Security
{
    public class ProtectedObjectsTests
    {
        private readonly Encoding _enc = StrUtil.Utf8;

        [Fact]
        public void TestAreBinaryObjectsProtected()
        {
            var pbData = _enc.GetBytes("Test Test Test Test");
            var pb = new ProtectedBinary(true, pbData);
            Assert.True(pb.IsProtected);

            var pbDec = pb.ReadData();
            Assert.True(MemUtil.ArraysEqual(pbData, pbDec));
            Assert.True(pb.IsProtected);

            var pbData2 = _enc.GetBytes("Test Test Test Test");
            var pbData3 = _enc.GetBytes("Test Test Test Test Test");
            var pb2 = new ProtectedBinary(true, pbData2);
            var pb3 = new ProtectedBinary(true, pbData3);
            Assert.True(pb.Equals(pb2));
            Assert.False(pb.Equals(pb3));
            Assert.False(pb2.Equals(pb3));

            Assert.Equal(pb.GetHashCode(), pb2.GetHashCode());
            Assert.True(pb.Equals((object) pb2));
            Assert.False(pb.Equals((object) pb3));
            Assert.False(pb2.Equals((object) pb3));
        }

        [Fact]
        public void TestIsEmptyProtectedStringEmpty()
        {
            var ps = new ProtectedString();
            Assert.Equal(0, ps.Length);
            Assert.True(ps.IsEmpty);
            Assert.Equal(0, ps.ReadString().Length);
        }

        [Fact]
        public void TestAreEqualStringsProtected()
        {
            var ps = new ProtectedString(true, "Test");
            var ps2 = new ProtectedString(true, _enc.GetBytes("Test"));
            Assert.False(ps.IsEmpty);
            var pbData = ps.ReadUtf8();
            var pbData2 = ps2.ReadUtf8();
            Assert.True(MemUtil.ArraysEqual(pbData, pbData2));
            Assert.Equal(4, pbData.Length);
            Assert.Equal(ps.ReadString(), ps2.ReadString());
            pbData = ps.ReadUtf8();
            pbData2 = ps2.ReadUtf8();
            Assert.True(MemUtil.ArraysEqual(pbData, pbData2));
            Assert.True(ps.IsProtected);
            Assert.True(ps2.IsProtected);
        }

        [Fact]
        public void TestIsRandomStringProtected()
        {
            var r = CryptoRandom.NewWeakRandom();
            var str = string.Empty;
            var ps = new ProtectedString();
            for (var i = 0; i < 100; ++i)
            {
                var bProt = ((r.Next() % 4) != 0);
                ps = ps.WithProtection(bProt);

                var x = r.Next(str.Length + 1);
                var c = r.Next(20);
                var ch = (char) r.Next(1, 256);

                var strIns = new string(ch, c);
                str = str.Insert(x, strIns);
                ps = ps.Insert(x, strIns);

                Assert.Equal(bProt, ps.IsProtected);
                Assert.Equal(str, ps.ReadString());

                ps = ps.WithProtection(bProt);

                x = r.Next(str.Length);
                c = r.Next(str.Length - x + 1);

                str = str.Remove(x, c);
                ps = ps.Remove(x, c);

                Assert.Equal(bProt, ps.IsProtected);
                Assert.Equal(str, ps.ReadString());
            }
        }

        [Fact]
        public void TestAreConcatenatedStringsProtected()
        {
            var ps = new ProtectedString(false, "ABCD");
            var ps2 = new ProtectedString(true, "EFG");
            ps += (ps2 + "HI");
            Assert.True(ps.Equals(new ProtectedString(true, "ABCDEFGHI"), true));
            Assert.True(ps.Equals(new ProtectedString(false, "ABCDEFGHI"), false));
        }
    }
}