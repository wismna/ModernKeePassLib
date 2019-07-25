﻿using ModernKeePassLib.Cryptography.KeyDerivation;
using ModernKeePassLib.Keys;
using ModernKeePassLib.Utility;
using Xunit;

namespace ModernKeePassLib.Test.Keys
{
    public class CompositeKeyTests
    {
        [Fact]
        public void TestGenerateKey32()
        {
            var originalKey = new byte[32];
            var expectedKey = new byte[]
            {
                0xF0, 0xED, 0x57, 0xD5, 0xF0, 0xDA, 0xF3, 0x47,
                0x90, 0xD0, 0xDB, 0x43, 0x25, 0xC6, 0x81, 0x2C,
                0x81, 0x6A, 0x0D, 0x94, 0x96, 0xA9, 0x03, 0xE1,
                0x20, 0xD4, 0x3A, 0x3E, 0x45, 0xAD, 0x02, 0x65
            };
            const ulong rounds = 1;

            var composite = new CompositeKey();
            AesKdf kdf = new AesKdf();
            KdfParameters p = kdf.GetDefaultParameters();
            p.SetUInt64(AesKdf.ParamRounds, rounds);
            p.SetByteArray(AesKdf.ParamSeed, originalKey);
            var key = composite.GenerateKey32(p);
            Assert.NotNull(key);
            var keyData = key.ReadData();
            Assert.True(MemUtil.ArraysEqual(keyData, expectedKey));
        }
    }
}
