using System.IO;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.IO;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;

namespace ModernKeePassLib.Cryptography.Cipher
{
    public class StandardAesEngineExt
    {
        internal static Stream CreateStream(Stream s, bool bEncrypt, byte[] pbKey, byte[] pbIV)
        {
            var cbc = new CbcBlockCipher(new AesEngine());
            //var cbc = new CbcBlockCipher(new RijndaelEngine());
            var bc = new PaddedBufferedBlockCipher(cbc, new Pkcs7Padding());
            var kp = new KeyParameter(pbKey);
            var prmIV = new ParametersWithIV(kp, pbIV);
            bc.Init(bEncrypt, prmIV);

            var cpRead = bEncrypt ? null : bc;
            var cpWrite = bEncrypt ? bc : null;
            return new CipherStream(s, cpRead, cpWrite);
        }
    }
}