using System.Linq;
using ModernKeePassLib.Utility;
using System.Security.Cryptography;
using ModernKeePassLib.Cryptography;
using NUnit.Framework;

namespace ModernKeePassLib.Test.Cryptography.Hash
{
    [TestFixture]
    public class ShaManagedTests
    {
        [Test]
        public void TestSha256()
        {
            var r = CryptoRandom.NewWeakRandom();
            var pbData = new byte[517];
            r.NextBytes(pbData);

            byte[] pbH1;
            using (var h1 = new SHA256Managed())
            {
                var i = 0;
                while (i != pbData.Length)
                {
                    var cb = r.Next(pbData.Length - i) + 1;
                    h1.TransformBlock(pbData, i, cb, pbData, i);
                    i += cb;
                }
                h1.TransformFinalBlock(MemUtil.EmptyByteArray, 0, 0);
                pbH1 = h1.Hash;
            }

            byte[] pbH2;
            using (var h2 = new SHA256Managed())
            {
                pbH2 = h2.ComputeHash(pbData);
            }

            Assert.That(MemUtil.ArraysEqual(pbH1, pbH2), Is.True);
        }

        [Test]
        public void TestSha256ComputeHash()
        {
            var expectedHash = "B822F1CD2DCFC685B47E83E3980289FD5D8E3FF3A82DEF24D7D1D68BB272EB32";
            var message = StrUtil.Utf8.GetBytes("testing123");
            using var result = new SHA256Managed();
            Assert.That(expectedHash, Is.EqualTo(ByteToString(result.ComputeHash(message))));
        }

        [Test]
        public void TestSha512ComputeHash()
        {
            var expectedHash = "4120117B3190BA5E24044732B0B09AA9ED50EB1567705ABCBFA78431A4E0A96B1152ED7F4925966B1C82325E186A8100E692E6D2FCB6702572765820D25C7E9E";
            var message = StrUtil.Utf8.GetBytes("testing123");
            using var result = new SHA512Managed();
            Assert.That(expectedHash, Is.EqualTo(ByteToString(result.ComputeHash(message))));
        }

        private static string ByteToString(byte[] buff)
        {
            var sbinary = buff.Aggregate("", (current, t) => current + t.ToString("X2"));

            return (sbinary);
        }
    }
}