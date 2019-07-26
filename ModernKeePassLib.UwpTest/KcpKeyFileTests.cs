using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using ModernKeePassLib.Keys;
using ModernKeePassLib.Utility;
using Xunit;

namespace ModernKeePassLib.UwpTest
{
    public class KcpKeyFileTests
    {
        private const string TestCreateFile = "TestCreate.xml";
        private const string TestKey = "0123456789";

        private const string ExpectedFileStart =
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
            "<KeyFile>\r\n" +
            "\t<Meta>\r\n" +
            "\t\t<Version>1.00</Version>\r\n" +
            "\t</Meta>\r\n" +
            "\t<Key>\r\n" +
            "\t\t<Data>";

        private const string ExpectedFileEnd = "</Data>\r\n\t</Key>\r\n</KeyFile>";

        [Fact]
        public async Task TestConstruct()
        {
            var expectedKeyData = new byte[]
            {
                0x95, 0x94, 0xdc, 0xb9, 0x91, 0xc6, 0x65, 0xa0,
                0x81, 0xf6, 0x6f, 0xca, 0x07, 0x1a, 0x30, 0xd1,
                0x1d, 0x65, 0xcf, 0x8d, 0x9c, 0x60, 0xfb, 0xe6,
                0x45, 0xfc, 0xc8, 0x92, 0xbd, 0xeb, 0xaf, 0xc3
            };

            var folder = await StorageFolder.GetFolderFromPathAsync(Path.GetTempPath());
            var file = await folder.CreateFileAsync(TestCreateFile, CreationCollisionOption.ReplaceExisting);
            var token = StorageApplicationPermissions.FutureAccessList.Add(file);
            using (var fs = await file.OpenStreamForWriteAsync())
            {
                using (var sw = new StreamWriter(fs))
                {
                    sw.Write(ExpectedFileStart);
                    sw.Write(TestKey);
                    sw.Write(ExpectedFileEnd);
                }
            }

            try
            {
                var keyFile = new KcpKeyFile(token);
                var keyData = keyFile.KeyData.ReadData();
                Assert.True(MemUtil.ArraysEqual(keyData, expectedKeyData));
            }
            finally
            {
                await file.DeleteAsync();
            }
        }

        [Fact]
        public async Task TestCreate()
        {
            var folder = await StorageFolder.GetFolderFromPathAsync(Path.GetTempPath());
            var file = await folder.CreateFileAsync(TestCreateFile, CreationCollisionOption.ReplaceExisting);
            var token = StorageApplicationPermissions.FutureAccessList.Add(file);
            KcpKeyFile.Create(token, null);
            try
            {
                var fileContents = await FileIO.ReadTextAsync(file);

                Assert.Equal(185, fileContents.Length);
                Assert.StartsWith(ExpectedFileStart, fileContents);
                Assert.EndsWith(ExpectedFileEnd, fileContents);
            }
            finally
            {
                await file.DeleteAsync();
            }
        }
    }
}

