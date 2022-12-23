using System.Text;

namespace MyTestConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            const string fontFileName = @"C:\Users\冷怀晶\Downloads\HYXiaoSongRongW.ttf";
            const string svgFileName = $"{fontFileName}.svg";

            var s = GlyphLoader.RenderLibrary.RenderHelper4Hanyi.HanyiWebsiteFontDetailBanner(fontFileName);

            using (var sw = new StreamWriter(svgFileName))
            {
                sw.Write(s);
                sw.Flush();
            }
            Console.WriteLine("success");
            Console.ReadLine();
        }
    }
}