using System;
using System.Collections.Generic;
using ModernKeePassLib.Cryptography;
using ModernKeePassLib.Cryptography.Cipher;
using ModernKeePassLib.Utility;
using Xunit;

namespace ModernKeePassLib.Test.Cryptography.Cipher
{
    public class Salsa20Tests
    {
        [Fact]
        public void TestSalsa20Cipher()
        {
            var r = CryptoRandom.NewWeakRandom();

            // Test values from official set 6, vector 3
            var pbKey = new byte[] {
                0x0F, 0x62, 0xB5, 0x08, 0x5B, 0xAE, 0x01, 0x54,
                0xA7, 0xFA, 0x4D, 0xA0, 0xF3, 0x46, 0x99, 0xEC,
                0x3F, 0x92, 0xE5, 0x38, 0x8B, 0xDE, 0x31, 0x84,
                0xD7, 0x2A, 0x7D, 0xD0, 0x23, 0x76, 0xC9, 0x1C
            };
            var pbIv = new byte[] { 0x28, 0x8F, 0xF6, 0x5D,
                0xC4, 0x2B, 0x92, 0xF9 };
            var pbExpected = new byte[] {
                0x5E, 0x5E, 0x71, 0xF9, 0x01, 0x99, 0x34, 0x03,
                0x04, 0xAB, 0xB2, 0x2A, 0x37, 0xB6, 0x62, 0x5B
            };

            var pb = new byte[16];
            var c = new Salsa20Cipher(pbKey, pbIv);
            c.Encrypt(pb, 0, pb.Length);
            Assert.True(MemUtil.ArraysEqual(pb, pbExpected));

            // Extended test
            var pbExpected2 = new byte[] {
                0xAB, 0xF3, 0x9A, 0x21, 0x0E, 0xEE, 0x89, 0x59,
                0x8B, 0x71, 0x33, 0x37, 0x70, 0x56, 0xC2, 0xFE
            };
            var pbExpected3 = new byte[] {
                0x1B, 0xA8, 0x9D, 0xBD, 0x3F, 0x98, 0x83, 0x97,
                0x28, 0xF5, 0x67, 0x91, 0xD5, 0xB7, 0xCE, 0x23
            };

            var nPos = Salsa20ToPos(c, r, pb.Length, 65536);
            Array.Clear(pb, 0, pb.Length);
            c.Encrypt(pb, 0, pb.Length);
            Assert.True(MemUtil.ArraysEqual(pb, pbExpected2));

            Salsa20ToPos(c, r, nPos + pb.Length, 131008);
            Array.Clear(pb, 0, pb.Length);
            c.Encrypt(pb, 0, pb.Length);
            Assert.True(MemUtil.ArraysEqual(pb, pbExpected3));

            var d = new Dictionary<string, bool>();
            const int nRounds = 100;
            for (var i = 0; i < nRounds; ++i)
            {
                var z = new byte[32];
                c = new Salsa20Cipher(z, MemUtil.Int64ToBytes(i));
                c.Encrypt(z, 0, z.Length);
                d[MemUtil.ByteArrayToHexString(z)] = true;
            }
            Assert.Equal(nRounds, d.Count);
        }

        private static int Salsa20ToPos(Salsa20Cipher c, Random r, int nPos,
            int nTargetPos)
        {
            var pb = new byte[512];

            while (nPos < nTargetPos)
            {
                var x = r.Next(1, 513);
                var nGen = Math.Min(nTargetPos - nPos, x);
                c.Encrypt(pb, 0, nGen);
                nPos += nGen;
            }

            return nTargetPos;
        }
    }
}