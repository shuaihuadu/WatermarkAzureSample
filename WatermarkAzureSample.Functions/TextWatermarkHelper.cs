using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using System.IO;
using System.Text.RegularExpressions;

namespace WatermarkAzureSample.Functions;

public class TextWatermarkHelper
{
    public static void AddWatermark(string text, string extension, Stream imageStream, Stream output)
    {
        using (Image image = Image.Load(imageStream))
        {
            var font = SystemFonts.CreateFont("Arial", 240, FontStyle.Bold);
            using (var image2 = image.Clone(ctx => ctx.ApplyScalingWaterMark(font, text, Color.Blue, 5, true)))
            {
                var encoder = GetEncoder(extension);
                image2.Save(output, encoder);
                output.Position = 0;
            }
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
