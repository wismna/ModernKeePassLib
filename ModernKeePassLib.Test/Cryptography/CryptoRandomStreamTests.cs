using ModernKeePassLib.Cryptography;
using ModernKeePassLib.Utility;
using NUnit.Framework;

namespace ModernKeePassLib.Test.Cryptography
{
    [TestFixture]
    public class CryptoRandomStreamTests
    {
        private void TestGetRandomBytes(CryptoRandomStream stream)
        {
            const uint length = 16;
            var bytes1 = stream.GetRandomBytes(length);
            Assert.That((int)length, Is.EqualTo(bytes1.Length));
            var bytes2 = stream.GetRandomBytes(length);
            Assert.That(MemUtil.ArraysEqual(bytes2, bytes1), Is.False);
        }

        [Test]
        public void TestGetRandomBytesCrsAlgorithmSalsa20()
        {
            var stream = new CryptoRandomStream(CrsAlgorithm.Salsa20, new byte[16]);
            TestGetRandomBytes(stream);
        }

        [Test]
        public void TestGetRandomBytesCrsAlgorithmArcFourVariant()
        {
            var stream = new CryptoRandomStream(CrsAlgorithm.ArcFourVariant, new byte[16]);
            TestGetRandomBytes(stream);
        }

        private void TestGetRandomInt64(CryptoRandomStream stream)
        {
            var value1 = stream.GetRandomUInt64();
            var value2 = stream.GetRandomUInt64();
            Assert.That(value2, Is.Not.EqualTo(value1));
        }

        [Test]
        public void TestGetRandomInt64AlgorithmSalsa20()
        {
            var stream = new CryptoRandomStream(CrsAlgorithm.Salsa20, new byte[16]);
            TestGetRandomInt64(stream);
        }

        [Test]
        public void TestGetRandomInt64AlgorithmArcFourVariant()
        {
            var stream = new CryptoRandomStream(CrsAlgorithm.ArcFourVariant, new byte[16]);
            TestGetRandomInt64(stream);
        }
    }
}

