using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using System.Text.RegularExpressions;

namespace WatermarkAzureSample.Tests
{
    [TestClass]
    public class AddWatermarkUnitTest
    {
        [TestMethod]
        public void Test_Add_Watermark()
        {
            string text = "sample text,sample text,sample text,sample text,sample text,sample text,sample text";
            var path = Environment.CurrentDirectory;

            using (var output = new FileStream(path + "\\2.jpg", FileMode.Create))
            {
                var stream = File.OpenRead(path + "\\1.jpg");
                using (var image = Image.Load(stream))
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
                    var encoder = GetEncoder("jpg");
                    image.Save(output, encoder);
                    output.Position = 0;
                }
            }
        }

        private static IImageEncoder GetEncoder(string extension)
        {
            IImageEncoder encoder = null;
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
