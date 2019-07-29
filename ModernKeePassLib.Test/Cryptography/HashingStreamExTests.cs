using System.IO;
using ModernKeePassLib.Cryptography;
using ModernKeePassLib.Utility;
using NUnit.Framework;

namespace ModernKeePassLib.Test.Cryptography
{
    [TestFixture]
    public class HashingStreamExTests
    {
        private const string Data = "test";

        // The expected hash includes the \n added by WriteLine
        private static readonly byte[] Sha256HashOfData =
        {
            0xf2, 0xca, 0x1b, 0xb6, 0xc7, 0xe9, 0x07, 0xd0,
            0x6d, 0xaf, 0xe4, 0x68, 0x7e, 0x57, 0x9f, 0xce,
            0x76, 0xb3, 0x7e, 0x4e, 0x93, 0xb7, 0x60, 0x50,
            0x22, 0xda, 0x52, 0xe6, 0xcc, 0xc2, 0x6f, 0xd2
        };

        [Test]
        public void TestRead()
        {
            // if we use larger size, StreamReader will read past newline and cause bad hash
            var bytes = new byte[Data.Length + 1];
            using (var memoryStream1 = new MemoryStream(bytes))
            {
                using var sw = new StreamWriter(memoryStream1);
                // set NewLine to ensure we don't run into cross-platform issues on Windows
                sw.NewLine = "\n";
                sw.WriteLine(Data);
            }

            using var memoryStream2 = new MemoryStream(bytes);
            using var hs = new HashingStreamEx(memoryStream2, false, null);
            using (var sr = new StreamReader(hs))
            {
                var read = sr.ReadLine();
                Assert.That(read, Is.EqualTo(Data));
            }
            // When the StreamReader is disposed, it calls Dispose on the HasingStreamEx, which computes the hash.
            Assert.That(MemUtil.ArraysEqual(hs.Hash, Sha256HashOfData), Is.True);
        }

        [Test]
        public void TestWrite()
        {
            var bytes = new byte[16];
            using (var memoryStream1 = new MemoryStream(bytes))
            {
                using var hs = new HashingStreamEx(memoryStream1, true, null);
                using (var sw = new StreamWriter(hs))
                {
                    // set NewLine to ensure we don't run into cross-platform issues on Windows
                    sw.NewLine = "\n";
                    sw.WriteLine(Data);
                }
                // When the StreamWriter is disposed, it calls Dispose on the HasingStreamEx, which computes the hash.
                Assert.True(MemUtil.ArraysEqual(hs.Hash, Sha256HashOfData));
            }

            using var memoryStream2 = new MemoryStream(bytes);
            using var sr = new StreamReader(memoryStream2);
            var read = sr.ReadLine();

            Assert.That(read, Is.EqualTo(Data));
        }
    }
}

