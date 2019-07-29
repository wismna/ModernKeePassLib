﻿using ModernKeePassLib.Cryptography;
using NUnit.Framework;

namespace ModernKeePassLib.Test.Cryptography
{
    [TestFixture]
    public class CryptoRandomTests
    {
        [Test]
        public void TestAddEntropy()
        {
            // just making sure it does not throw an exception
            Assert.DoesNotThrow(() => CryptoRandom.Instance.AddEntropy(new byte[1]));
        }

        [Test]
        public void TestGetRandomBytes()
        {
            const int length = 32;
            var bytes1 = CryptoRandom.Instance.GetRandomBytes(length);
            var bytes2 = CryptoRandom.Instance.GetRandomBytes(length);

            Assert.That(length, Is.EqualTo(bytes1.Length));
            Assert.That(bytes1, Is.Not.EqualTo(bytes2));
        }

        [Test]
        public void TestGeneratedBytesCount()
        {
            const int length = 1;
            CryptoRandom.Instance.GetRandomBytes(length);
            var count1 = CryptoRandom.Instance.GeneratedBytesCount;
            CryptoRandom.Instance.GetRandomBytes(length);
            var count2 = CryptoRandom.Instance.GeneratedBytesCount;

            Assert.That(count2, Is.GreaterThan(count1));
        }
    }
}

