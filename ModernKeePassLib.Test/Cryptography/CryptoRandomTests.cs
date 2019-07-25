using ModernKeePassLib.Cryptography;
using Xunit;

namespace ModernKeePassLib.Test.Cryptography
{
    public class CryptoRandomTests
    {
        [Fact]
        public void TestAddEntropy()
        {
            // just making sure it does not throw an exception
            CryptoRandom.Instance.AddEntropy(new byte[1]);
        }

        [Fact]
        public void TestGetRandomBytes()
        {
            const int length = 32;
            var bytes1 = CryptoRandom.Instance.GetRandomBytes(length);
            Assert.Equal(bytes1.Length, length);
            var bytes2 = CryptoRandom.Instance.GetRandomBytes(length);
            Assert.NotEqual(bytes2, bytes1);
        }

        [Fact]
        public void TestGeneratedBytesCount()
        {
            const int length = 1;
            CryptoRandom.Instance.GetRandomBytes(length);
            var count1 = CryptoRandom.Instance.GeneratedBytesCount;
            CryptoRandom.Instance.GetRandomBytes(length);
            var count2 = CryptoRandom.Instance.GeneratedBytesCount;
            Assert.True(count2 > count1);
        }
    }
}

