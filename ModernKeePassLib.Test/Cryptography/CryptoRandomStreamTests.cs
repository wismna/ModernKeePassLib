using ModernKeePassLib.Cryptography;
using ModernKeePassLib.Utility;
using Xunit;

namespace ModernKeePassLib.Test.Cryptography
{
    public class CryptoRandomStreamTests
    {
        private void TestGetRandomBytes(CryptoRandomStream stream)
        {
            const uint length = 16;
            var bytes1 = stream.GetRandomBytes(length);
            Assert.Equal(bytes1.Length, (int)length);
            var bytes2 = stream.GetRandomBytes(length);
            Assert.False(MemUtil.ArraysEqual(bytes2, bytes1));
        }

        [Fact]
        public void TestGetRandomBytesCrsAlgorithmSalsa20()
        {
            var stream = new CryptoRandomStream(CrsAlgorithm.Salsa20, new byte[16]);
            TestGetRandomBytes(stream);
        }

        [Fact]
        public void TestGetRandomBytesCrsAlgorithmArcFourVariant()
        {
            var stream = new CryptoRandomStream(CrsAlgorithm.ArcFourVariant, new byte[16]);
            TestGetRandomBytes(stream);
        }

        private void TestGetRandomInt64(CryptoRandomStream stream)
        {
            var value1 = stream.GetRandomUInt64();
            var value2 = stream.GetRandomUInt64();
            Assert.NotEqual(value2, value1);
        }

        [Fact]
        public void TestGetRandomInt64AlgorithmSalsa20()
        {
            var stream = new CryptoRandomStream(CrsAlgorithm.Salsa20, new byte[16]);
            TestGetRandomInt64(stream);
        }

        [Fact]
        public void TestGetRandomInt64AlgorithmArcFourVariant()
        {
            var stream = new CryptoRandomStream(CrsAlgorithm.ArcFourVariant, new byte[16]);
            TestGetRandomInt64(stream);
        }
    }
}

