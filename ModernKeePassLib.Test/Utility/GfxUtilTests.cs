using System;
using ModernKeePassLib.Utility;
using Xunit;

namespace ModernKeePassLib.Test.Utility
{
    public class GfxUtilTests
    {
        // 16x16 all white PNG file, base64 encoded
        const string testImageData =
            "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAIAAACQkWg2AAAACXBIWXMAAAsTAAA" +
            "LEwEAmpwYAAAAB3RJTUUH3wMOFgIgmTCUMQAAABl0RVh0Q29tbWVudABDcmVhdG" +
            "VkIHdpdGggR0lNUFeBDhcAAAAaSURBVCjPY/z//z8DKYCJgUQwqmFUw9DRAABVb" +
            "QMdny4VogAAAABJRU5ErkJggg==";

        //[Fact]
        //public void TestLoadImage ()
        //{
        //    var testData = Convert.FromBase64String (testImageData);
        //    var image = GfxUtil.ScaleImage(testData, 16, 16, ScaleTransformFlags.UIIcon);
        //    //var image = GfxUtil.LoadImage(testData);
        //    Assert.Equal(image.Width, 16);
        //    Assert.Equal(image.Height, 16);
        //}
    }
}
