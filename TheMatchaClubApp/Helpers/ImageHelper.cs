using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace TheMatchaClubApp.Helpers
{
    /// <summary>
    /// Handles image cropping to 1:1 ratio and generating text-based
    /// initial placeholders when no image is available.
    /// </summary>
    public static class ImageHelper
    {
        /// <summary>
        /// Crops the given image to a 1:1 square from the center.
        /// </summary>
        public static Image CropToSquare(Image source)
        {
            int side = Math.Min(source.Width, source.Height);
            int x = (source.Width - side) / 2;
            int y = (source.Height - side) / 2;

            var bmp = new Bitmap(side, side);
            using var g = Graphics.FromImage(bmp);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(source, new Rectangle(0, 0, side, side),
                        new Rectangle(x, y, side, side), GraphicsUnit.Pixel);
            return bmp;
        }

        /// <summary>
        /// Crops the given image to fill the target rectangle from the center, maintaining aspect ratio.
        /// </summary>
        public static Image CropToFit(Image source, int targetWidth, int targetHeight)
        {
            float sourceRatio = (float)source.Width / source.Height;
            float targetRatio = (float)targetWidth / targetHeight;

            int cropWidth, cropHeight, x, y;

            if (sourceRatio > targetRatio)
            {
                // Source is wider than target. Crop left/right.
                cropHeight = source.Height;
                cropWidth = (int)(source.Height * targetRatio);
                x = (source.Width - cropWidth) / 2;
                y = 0;
            }
            else
            {
                // Source is taller than target. Crop top/bottom.
                cropWidth = source.Width;
                cropHeight = (int)(source.Width / targetRatio);
                x = 0;
                y = (source.Height - cropHeight) / 2;
            }

            var bmp = new Bitmap(targetWidth, targetHeight);
            using var g = Graphics.FromImage(bmp);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(source, new Rectangle(0, 0, targetWidth, targetHeight),
                        new Rectangle(x, y, cropWidth, cropHeight), GraphicsUnit.Pixel);
            return bmp;
        }

        /// <summary>
        /// Generates a placeholder image with initials (e.g., "ML" for Matcha Latte).
        /// </summary>
        public static Image GenerateInitialPlaceholder(string name, int width = 120, int height = 120)
        {
            string initials = GetInitials(name);
            var bmp = new Bitmap(width, height);
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Background — matcha green gradient
            using var bgBrush = new LinearGradientBrush(
                new Rectangle(0, 0, width, height),
                ColorTranslator.FromHtml("#52B743"),
                ColorTranslator.FromHtml("#3A8F2E"),
                LinearGradientMode.ForwardDiagonal);
            g.FillRectangle(bgBrush, 0, 0, width, height);

            // Text
            int minDim = Math.Min(width, height);
            using var font = new Font("Segoe UI", minDim / 3f, FontStyle.Bold);
            using var textBrush = new SolidBrush(Color.White);
            var textSize = g.MeasureString(initials, font);
            float tx = (width - textSize.Width) / 2f;
            float ty = (height - textSize.Height) / 2f;
            g.DrawString(initials, font, textBrush, tx, ty);

            return bmp;
        }

        /// <summary>
        /// Extracts up to 2 initials from a product/customer name.
        /// E.g., "Matcha Latte" → "ML", "Espresso" → "ES"
        /// </summary>
        private static string GetInitials(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "??";
            var parts = name.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2)
                return $"{char.ToUpper(parts[0][0])}{char.ToUpper(parts[1][0])}";
            return name.Length >= 2
                ? $"{char.ToUpper(name[0])}{char.ToUpper(name[1])}"
                : name.ToUpper();
        }

        /// <summary>
        /// Loads an image from path, or returns a placeholder if the file doesn't exist.
        /// </summary>
        public static Image LoadOrPlaceholder(string imagePath, string name, int width = 120, int height = 120, bool dim = false)
        {
            Image img;
            string fullPath = Program.DataService.GetFullImagePath(imagePath);

            if (!string.IsNullOrWhiteSpace(fullPath) && File.Exists(fullPath))
            {
                try
                {
                    using var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
                    img = CropToFit(Image.FromStream(fs), width, height);
                }
                catch 
                { 
                    img = GenerateInitialPlaceholder(name, width, height); 
                }
            }
            else
            {
                img = GenerateInitialPlaceholder(name, width, height);
            }

            if (dim)
            {
                var dimmedBmp = new Bitmap(img.Width, img.Height);
                using var g = Graphics.FromImage(dimmedBmp);
                
                // Color matrix to reduce alpha to 40%
                ColorMatrix matrix = new ColorMatrix();
                matrix.Matrix33 = 0.4f;
                using var attributes = new ImageAttributes();
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                
                g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, attributes);
                return dimmedBmp;
            }

            return img;
        }
    }
}
