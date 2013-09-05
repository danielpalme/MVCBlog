using System.Drawing.Imaging;
using System.IO;

namespace Palmmedia.Common
{
    /// <summary>
    /// Provides methods for image processing.
    /// </summary>
    public static class ImageHelper
    {
        /// <summary>
        /// Creates a thumbnail to a given image.
        /// </summary>
        /// <param name="file">The image.</param>
        /// <param name="width">The maximum width of the thumbnail.</param>
        /// <param name="height">The maximum height of the thumbnail.</param>
        /// <param name="relative">Determines whether the image is resized relatively.</param>
        /// <returns>The resized image.</returns>
        public static byte[] MakeThumbnail(byte[] file, int width, int height, bool relative)
        {
            System.Drawing.Image image = null;

            using (var input = new MemoryStream(file))
            {
                image = System.Drawing.Image.FromStream(new MemoryStream(file));
            }

            if (relative)
            {
                if (image.Width > image.Height)
                {
                    height = image.Height * width / image.Width;
                    if (width > image.Width)
                    {
                        return file;
                    }
                }
                else
                {
                    width = image.Width * height / image.Height;
                    if (height > image.Height)
                    {
                        return file;
                    }
                }
            }

            var scaledImage = image.GetThumbnailImage(width, height, null, new System.IntPtr());
            using (var result = new MemoryStream())
            {
                scaledImage.Save(result, ImageFormat.Jpeg);
                return result.ToArray();
            }
        }
    }
}
