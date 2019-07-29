using System.Text;
using ModernKeePassLib.Cryptography;
using ModernKeePassLib.Security;
using ModernKeePassLib.Utility;
using NUnit.Framework;

namespace ModernKeePassLib.Test.Security
{
    [TestFixture]
    public class ProtectedObjectsTests
    {
        private readonly Encoding _enc = StrUtil.Utf8;

        [Test]
        public void TestAreBinaryObjectsProtected()
        {
            var pbData = _enc.GetBytes("Test Test Test Test");
            var pb = new ProtectedBinary(true, pbData);
            Assert.That(pb.IsProtected, Is.True);

            var pbDec = pb.ReadData();
            Assert.That(MemUtil.ArraysEqual(pbData, pbDec), Is.True);
            Assert.That(pb.IsProtected, Is.True);

            var pbData2 = _enc.GetBytes("Test Test Test Test");
            var pbData3 = _enc.GetBytes("Test Test Test Test Test");
            var pb2 = new ProtectedBinary(true, pbData2);
            var pb3 = new ProtectedBinary(true, pbData3);
            Assert.That(pb.Equals(pb2), Is.True);
            Assert.That(pb.Equals(pb3), Is.False);
            Assert.That(pb2.Equals(pb3), Is.False);

            Assert.That(pb.GetHashCode(), Is.EqualTo(pb2.GetHashCode()));
            Assert.That(pb.Equals((object) pb2), Is.True);
            Assert.That(pb.Equals((object) pb3), Is.False);
            Assert.That(pb2.Equals((object) pb3), Is.False);
        }

        [Test]
        public void TestIsEmptyProtectedStringEmpty()
        {
            var ps = new ProtectedString();
            Assert.That(ps.Length, Is.EqualTo(0));
            Assert.That(ps.IsEmpty, Is.True);
            Assert.That(ps.ReadString().Length, Is.EqualTo(0));
        }

        [Test]
        public void TestAreEqualStringsProtected()
        {
            var ps = new ProtectedString(true, "Test");
            var ps2 = new ProtectedString(true, _enc.GetBytes("Test"));
            Assert.That(ps.IsEmpty, Is.False);
            var pbData = ps.ReadUtf8();
            var pbData2 = ps2.ReadUtf8();
            Assert.That(MemUtil.ArraysEqual(pbData, pbData2), Is.True);
            Assert.That(pbData.Length, Is.EqualTo(4));
            Assert.That(ps.ReadString(), Is.EqualTo(ps2.ReadString()));
            pbData = ps.ReadUtf8();
            pbData2 = ps2.ReadUtf8();
            Assert.That(MemUtil.ArraysEqual(pbData, pbData2), Is.True);
            Assert.That(ps.IsProtected, Is.True);
            Assert.That(ps2.IsProtected, Is.True);
        }

        [Test]
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

                Assert.That(ps.IsProtected, Is.EqualTo(bProt));
                Assert.That(ps.ReadString(), Is.EqualTo(str));

                ps = ps.WithProtection(bProt);

                x = r.Next(str.Length);
                c = r.Next(str.Length - x + 1);

                str = str.Remove(x, c);
                ps = ps.Remove(x, c);

                Assert.That(ps.IsProtected, Is.EqualTo(bProt));
                Assert.That(ps.ReadString(), Is.EqualTo(str));
            }
        }

        [Test]
        public void TestAreConcatenatedStringsProtected()
        {
            var ps = new ProtectedString(false, "ABCD");
            var ps2 = new ProtectedString(true, "EFG");
            ps += (ps2 + "HI");
            Assert.That(ps.Equals(new ProtectedString(true, "ABCDEFGHI"), true), Is.True);
            Assert.That(ps.Equals(new ProtectedString(false, "ABCDEFGHI"), false), Is.True);
        }
    }
}