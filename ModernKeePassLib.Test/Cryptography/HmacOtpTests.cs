using System.Text;
using ModernKeePassLib.Cryptography;
using NUnit.Framework;

namespace ModernKeePassLib.Test.Cryptography
{
    [TestFixture]
    public class HmacOtpTests
    {
        // Using the test case from Appendix D of RFC 4226

        private const string Secret = "12345678901234567890";

        private static readonly string[] ExpectedHotp = {
            "755224", "287082", "359152", "969429", "338314",
            "254676", "287922", "162583", "399871", "520489"
        };

        [Test]
        public void TestGenerate()
        {
            var secretBytes = Encoding.UTF8.GetBytes(Secret);

            for (ulong i = 0; i < 10; i++)
            {
                var hotp = HmacOtp.Generate(secretBytes, i, 6, false, -1);
                Assert.That(ExpectedHotp[i], Is.EqualTo(hotp));
            }
        }
    }
}
