using System.IO;
using System.Security.Cryptography;
using System.Text;
using NUnit.Framework;
using ModernKeePassLib.Serialization;
using ModernKeePassLib.Cryptography;
using ModernKeePassLib.Cryptography.Cipher;
using ModernKeePassLib.Utility;

namespace ModernKeePassLib.Test.Cryptography.Cipher
{
    [TestFixture]
    public class AesTests
    {
        // Test vector (official ECB test vector #356)
        private readonly byte[] _pbReferenceCt = 
        {
            0x75, 0xD1, 0x1B, 0x0E, 0x3A, 0x68, 0xC4, 0x22,
            0x3D, 0x88, 0xDB, 0xF0, 0x17, 0x97, 0x7D, 0xD7
        };
        private readonly byte[] _pbIv = new byte[16];
        private readonly byte[] _pbTestKey = new byte[32];
        private readonly byte[] _pbTestData =
        {
            0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };

        [Test]
        public void TestEncryptStream()
        {
            var a = CryptoUtil.CreateAes();
            if (a.BlockSize != 128) // AES block size
            {
                //Debug.Assert(false);
                a.BlockSize = 128;
            }

            a.IV = _pbIv;
            a.KeySize = 256;
            a.Key = _pbTestKey;
            a.Mode = CipherMode.ECB;
            var iCrypt = a.CreateEncryptor();

            iCrypt.TransformBlock(_pbTestData, 0, 16, _pbTestData, 0);

            Assert.That(MemUtil.ArraysEqual(_pbTestData, _pbReferenceCt), Is.True);
        }

        [Test]
        public void TestDecryptStream()
        {
            // Possible Mono Bug? This only works with size >= 48
            using var inStream = new MemoryStream(new byte[32]);
            inStream.Write(_pbReferenceCt, 0, _pbReferenceCt.Length);
            inStream.Position = 0;
            var aes = new StandardAesEngine();
            using var outStream = aes.DecryptStream(inStream, _pbTestKey, _pbIv);
            var outBytes = new BinaryReaderEx(outStream, Encoding.UTF8, string.Empty).ReadBytes(16);
            Assert.That(MemUtil.ArraysEqual(outBytes, _pbTestData), Is.True);
        }
    }
}
