using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using ModernKeePassLib.Keys;
using ModernKeePassLib.Utility;
using Windows.Storage;
using Windows.Storage.Streams;
using NUnit.Framework;

namespace ModernKeePassLib.Test.Keys
{
    [TestFixture]
    public class KcpKeyFileTests
    {
        private StorageFile _file;
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

        [SetUp]
        public async Task SetUp()
        {
            var folder = await StorageFolder.GetFolderFromPathAsync(Path.GetTempPath());
            _file = await folder.CreateFileAsync(TestCreateFile, CreationCollisionOption.ReplaceExisting);
        }

        [TearDown]
        public async Task TearDown()
        {
            await _file.DeleteAsync();
        }

        [Test]
        public async Task TestConstruct()
        {
            var expectedKeyData = new byte[]
            {
                0x95, 0x94, 0xdc, 0xb9, 0x91, 0xc6, 0x65, 0xa0,
                0x81, 0xf6, 0x6f, 0xca, 0x07, 0x1a, 0x30, 0xd1,
                0x1d, 0x65, 0xcf, 0x8d, 0x9c, 0x60, 0xfb, 0xe6,
                0x45, 0xfc, 0xc8, 0x92, 0xbd, 0xeb, 0xaf, 0xc3
            };

            await using (var fs = await _file.OpenStreamForWriteAsync())
            {
                await using var sw = new StreamWriter(fs);
                sw.Write(ExpectedFileStart);
                sw.Write(TestKey);
                sw.Write(ExpectedFileEnd);
            }

            var fileBytes = (await FileIO.ReadBufferAsync(_file)).ToArray();

            var keyFile = new KcpKeyFile(fileBytes);
            var keyData = keyFile.KeyData.ReadData();

            Assert.That(MemUtil.ArraysEqual(keyData, expectedKeyData), Is.True);
        }

        [Test]
        public async Task TestCreate()
        {
            var fileBytes = KcpKeyFile.Create(null);
            await FileIO.WriteBytesAsync(_file, fileBytes);
            var fileContents = await FileIO.ReadTextAsync(_file);

            Assert.That(fileContents.Length, Is.EqualTo(185));
            Assert.That(fileContents.StartsWith(ExpectedFileStart), Is.True);
            Assert.That(fileContents.EndsWith(ExpectedFileEnd), Is.True);
        }
    }
}

