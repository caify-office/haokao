using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace HaoKao.BasicService.Application;

/// <summary>
/// Summary description for ValidateCode.
/// </summary>
public static class ValidateCodeHelper
{
    private static readonly Color[] _colors =
    [
        Color.MediumAquamarine, Color.MediumBlue, Color.MediumOrchid, Color.MediumPurple, Color.MediumSeaGreen,
        Color.MediumSlateBlue,
        Color.MediumSpringGreen, Color.Maroon, Color.MediumTurquoise, Color.MidnightBlue,
        Color.Moccasin, Color.Navy, Color.MediumVioletRed, Color.Magenta,
        Color.LimeGreen, Color.LawnGreen, Color.Olive, Color.OliveDrab, Color.Orange, Color.OrangeRed,
        Color.SkyBlue,
        Color.SlateBlue, Color.SpringGreen, Color.SteelBlue, Color.Tan, Color.Teal, Color.Thistle, Color.Tomato,
        Color.Turquoise, Color.Violet, Color.Wheat, Color.Lavender,
        Color.SeaShell, Color.SandyBrown, Color.PaleGoldenrod, Color.PaleGreen, Color.PaleTurquoise,
        Color.PaleVioletRed, Color.PapayaWhip,
        Color.PeachPuff, Color.Peru, Color.Plum, Color.PowderBlue, Color.Purple, Color.Red,
        Color.RosyBrown, Color.RoyalBlue, Color.SaddleBrown,
        Color.Salmon, Color.SeaGreen, Color.Yellow, Color.Cyan, Color.DarkMagenta, Color.DarkKhaki,
        Color.DarkGreen, Color.DarkGoldenrod, Color.DarkCyan, Color.DarkBlue, Color.Crimson, Color.CornflowerBlue,
        Color.Coral, Color.Chocolate, Color.DarkOliveGreen, Color.Chartreuse, Color.BurlyWood, Color.Brown,
        Color.BlueViolet, Color.Blue, Color.Black, Color.Bisque, Color.Aquamarine,
        Color.AliceBlue, Color.CadetBlue, Color.DarkOrange, Color.YellowGreen, Color.DarkRed, Color.Indigo,
        Color.IndianRed, Color.DarkOrchid, Color.GreenYellow,
        Color.Green, Color.Goldenrod, Color.Gold, Color.Gainsboro, Color.Fuchsia,
        Color.ForestGreen, Color.HotPink, Color.Firebrick, Color.DodgerBlue, Color.DeepSkyBlue, Color.DeepPink,
        Color.DarkViolet, Color.DarkTurquoise, Color.DarkSlateBlue, Color.DarkSeaGreen, Color.DarkSalmon,
    ];

    public static string CreateBase64ImageSrc(string code, int width = 135, int height = 35)
    {
        var r = new Random();
        using var image = new Image<Rgba32>(width, height);

        // 字体
        var font = SystemFonts.CreateFont(SystemFonts.Families.First().Name, 25, FontStyle.Bold);

        image.Mutate(ctx =>
        {
            // 白底背景
            ctx.Fill(Color.White);

            // 画验证码
            for (var i = 0; i < code.Length; i++)
            {
                ctx.DrawText(code[i].ToString(), font, _colors[r.Next(_colors.Length)], new PointF(20 * i + 10, r.Next(2, 12)));
            }

            // 画干扰线
            for (var i = 0; i < 10; i++)
            {
                var pen = new Pen(_colors[r.Next(_colors.Length)], 1);
                var p1 = new PointF(r.Next(width), r.Next(height));
                var p2 = new PointF(r.Next(width), r.Next(height));

                ctx.DrawLines(pen, p1, p2);
            }

            // 画噪点
            for (var i = 0; i < 120; i++)
            {
                var pen = new Pen(_colors[r.Next(_colors.Length)], 1);
                var p1 = new PointF(r.Next(width), r.Next(height));
                var p2 = new PointF(p1.X + 1f, p1.Y + 1f);

                ctx.DrawLines(pen, p1, p2);
            }
        });

        using var ms = new System.IO.MemoryStream();
        image.SaveAsPng(ms);

        return $"data:image/png;base64,{Convert.ToBase64String(ms.ToArray())}";
    }
}