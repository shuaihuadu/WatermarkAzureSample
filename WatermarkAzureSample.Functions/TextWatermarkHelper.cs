using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using System.IO;
using System.Text.RegularExpressions;

namespace WatermarkAzureSample.Functions
{
    public class TextWatermarkHelper
    {
        public static void AddWatermark(string text, string extension, Stream imageStream, Stream output)
        {
            using (Image image = Image.Load(imageStream))
            {
                var font = SystemFonts.CreateFont("Arial", 200, FontStyle.Bold);

                TextOptions textOptions = new(font)
                {
                    Origin = new PointF(150, 150),
                    WrappingLength = image.Width - 150,
                    WordBreaking = WordBreaking.Normal
                };
                IBrush brush = Brushes.Horizontal(Color.FromRgba(255, 0, 0, 100), Color.Blue);
                IPen pen = Pens.Solid(Color.White, 5);
                image.Mutate(x => x.SetDrawingTransform(Matrix3x2Extensions.CreateRotationDegrees(45)).DrawText(textOptions, text, brush, pen));
                var encoder = GetEncoder(extension);
                image.Save(output, encoder);
                output.Position = 0;
            }
        }

        private static IImageEncoder GetEncoder(string extension)
        {
            IImageEncoder encoder = new JpegEncoder();
            extension = extension.Replace(".", "");
            var isSupported = Regex.IsMatch(extension, "png|jpg", RegexOptions.IgnoreCase);
            if (isSupported)
            {
                switch (extension.ToLower())
                {
                    case "png":
                        encoder = new PngEncoder();
                        break;
                    case "jpg":
                        encoder = new JpegEncoder();
                        break;
                    default:
                        break;
                }
            }
            return encoder;
        }
    }
}
