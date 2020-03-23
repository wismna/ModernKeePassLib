using System;

namespace ModernKeePassLib.Cryptography
{
    public class RNGCryptoServiceProvider: IDisposable
    {
        public void GetBytes(byte[] pb)
        {
            var random = new Random();
            random.NextBytes(pb);
        }

        public void Dispose()
        {
        }
    }
}