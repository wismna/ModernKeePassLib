﻿using System.Text;
using ModernKeePassLib.Cryptography;
using Xunit;

namespace ModernKeePassLib.Test.Cryptography
{
    public class HmacOtpTests
    {
        // Using the test case from Appendix D of RFC 4226

        const string secret = "12345678901234567890";

        static readonly string[] expectedHOTP = new string[]
        {
            "755224", "287082", "359152", "969429", "338314",
            "254676", "287922", "162583", "399871", "520489"
        };

        [Fact]
        public void TestGenerate()
        {
            var secretBytes = Encoding.UTF8.GetBytes(secret);

            for (ulong i = 0; i < 10; i++)
            {
                var hotp = HmacOtp.Generate(secretBytes, i, 6, false, -1);
                Assert.Equal(hotp, expectedHOTP[i]);
            }
        }
    }
}
