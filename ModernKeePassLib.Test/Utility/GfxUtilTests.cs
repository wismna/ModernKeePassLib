using System;
using ModernKeePassLib.Utility;
using NUnit.Framework;

namespace ModernKeePassLib.Test.Utility
{
    [TestFixture]
    public class GfxUtilTests
    {
        // 16x16 all white PNG file, base64 encoded
        private const string TestImageData =
            "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAIAAACQkWg2AAAACXBIWXMAAAsTAAA" +
            "LEwEAmpwYAAAAB3RJTUUH3wMOFgIgmTCUMQAAABl0RVh0Q29tbWVudABDcmVhdG" +
            "VkIHdpdGggR0lNUFeBDhcAAAAaSURBVCjPY/z//z8DKYCJgUQwqmFUw9DRAABVb" +
            "QMdny4VogAAAABJRU5ErkJggg==";

        [Test]
        public void TestLoadImage()
        {
            var testData = Convert.FromBase64String(TestImageData);
            //var image = GfxUtil.ScaleImage(testData, 16, 16, ScaleTransformFlags.UIIcon);
            var image = GfxUtil.LoadImage(testData);
            Assert.That(image.Width, Is.EqualTo(16));
            Assert.That(image.Height, Is.EqualTo(16));
        }
    }
}
