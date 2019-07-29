using System.IO;
using ModernKeePassLib.Serialization;
using ModernKeePassLib.Utility;
using NUnit.Framework;

namespace ModernKeePassLib.Test.Serialization
{
    [TestFixture]
    public class HashedBlockStreamTests
    {
        private static readonly byte[] Data = new byte[16];

        private static readonly byte[] HashStreamData = {
            // The first 4 bytes are an integer indicating the block index
            0x00, 0x00, 0x00, 0x00,
            // Then the SHA-256 hash of the data
            0x37, 0x47, 0x08, 0xFF, 0xF7, 0x71, 0x9D, 0xD5,
            0x97, 0x9E, 0xC8, 0x75, 0xD5, 0x6C, 0xD2, 0x28,
            0x6F, 0x6D, 0x3C, 0xF7, 0xEC, 0x31, 0x7A, 0x3B,
            0x25, 0x63, 0x2A, 0xAB, 0x28, 0xEC, 0x37, 0xBB,
            // then an integer that is the length of the data
            0x10, 0x00, 0x00, 0x00,
            // and finally the data itself
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            // Next, a terminating block
            0x01, 0x00, 0x00, 0x00,
            // terminating block is indicated by a hash of all 0s...
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            // ...and by a size of 0
            0x00, 0x00, 0x00, 0x00
        };

        [Test]
        public void TestRead()
        {
            using var ms = new MemoryStream(HashStreamData);
            using var hbs = new HashedBlockStream(ms, false);
            using var br = new BinaryReader(hbs);
            var bytes = br.ReadBytes(Data.Length);
            Assert.That(MemUtil.ArraysEqual(bytes, Data), Is.True);
            Assert.Throws<EndOfStreamException>(() => br.ReadByte());
        }

        [Test]
        public void TestWrite()
        {
            var buffer = new byte[HashStreamData.Length];
            using var ms = new MemoryStream(buffer);
            using (var hbs = new HashedBlockStream(ms, true))
            {
                using var bw = new BinaryWriter(hbs);
                bw.Write(Data);
            }
            Assert.That(MemUtil.ArraysEqual(buffer, HashStreamData), Is.True);
        }
    }
}

