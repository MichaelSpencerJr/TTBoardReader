using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace TTImageProcessor
{
    public static class ImageUtil
    {
        /// <summary>
        /// Loads image file into bitmap without retaining a lock on the file
        /// </summary>
        /// <param name="path">Path to image file to load</param>
        /// <returns>Image or Bitmap object created from file</returns>
        public static Bitmap LoadBitmap(string path)
        {
            try
            {
                using (var fileBmp = new Bitmap(path))
                {
                    return new Bitmap(fileBmp);
                }
            }
            catch (Exception ex)
            {
                Logger.Err(1807162055, $"Unable to load bitmap from file {path}: {ex}");
                return null;
            }
        }

        public static Bitmap DecimateColors(Bitmap original,
            byte redMask, byte greenMask, byte blueMask)
        {
            try
            {
                var retval = new Bitmap(original.Width, original.Height);
                var originalData = original.LockBits(
                    new Rectangle(0, 0, original.Width, original.Height),
                    ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                var targetData = retval.LockBits(
                    new Rectangle(0, 0, original.Width, original.Height),
                    ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
                unsafe
                {
                    for (var col = 0; col < original.Height; col++)
                    {
                        var originalColPtr = (byte*)originalData.Scan0 + originalData.Stride * col;
                        var targetColPtr = (byte*)targetData.Scan0 + targetData.Stride * col;
                        for (var row = 0; row < original.Width; row++)
                        {
                            targetColPtr[row * 3 + 0] = (byte)(originalColPtr[row * 3 + 0] & redMask);
                            targetColPtr[row * 3 + 1] = (byte)(originalColPtr[row * 3 + 1] & greenMask);
                            targetColPtr[row * 3 + 2] = (byte)(originalColPtr[row * 3 + 2] & blueMask);
                        }
                    }
                }
                original.UnlockBits(originalData);
                retval.UnlockBits(targetData);
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Err(1807162126, ex.ToString());
                return null;
            }

        }
    }
}
