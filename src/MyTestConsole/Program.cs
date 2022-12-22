using System.Text;
using WaterTrans.GlyphLoader;

namespace MyTestConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            double unit = 16;
            double x = 20;

            Typeface tf;
            using (var fs = File.OpenRead(@"C:\Users\冷怀晶\Downloads\HYDingXiaoDianW.ttf"))
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                tf = new Typeface(fs);
            }

            var arr = new[,]
            {
                {"冈", "水", "霜", "雨", "阳", "岁", "藏", "往", "张", "昃", "荒", "黄"},
                {"昆", "丽", "为", "致", "调", "成", "冬", "暑", "列", "盈", "洪", "玄"},
                {"出", "生", "结", "腾", "吕", "余", "收", "来", "宿", "月", "宙", "地"},
                {"玉", "金", "露", "云", "律", "闰", "秋", "寒", "辰", "日", "宇", "天"}
            };
            const string s1 = "ABCabc123";
            const string s2 = "，。：；！？";

            const int spaceX = 5;
            const int spaceY = 1;
            const int em = 30;

            var row = arr.GetLength(0);//const int row = 4;
            var col = arr.GetLength(1);//const int col = 12;
            var sb = new StringBuilder();
            for (var i = 0; i < row; i++)
            {
                for (var j = 0; j < col; j++)
                {
                    var glyphIndex = tf.CharacterToGlyphMap[arr[i, j][0]];
                    var geometry = tf.GetGlyphOutline(glyphIndex, unit);
                    var advanceWidth = tf.AdvanceWidths[glyphIndex] * unit;
                    var baseline = tf.Baseline * unit;
                    var mini= geometry.Figures.ToString(x, y + baseline);

                    svg.AppendLine($"<path d='{miniLanguage}' fill='black' stroke='black' stroke-width='0' />");
                    x += advanceWidth;
                }
            }
            foreach (var uni in unis)
            {
                var glyphIndex = tf.CharacterToGlyphMap[uni];
                var geometry = tf.GetGlyphOutline(glyphIndex, unit);
                var baseline = tf.Baseline * unit;
                var advanceWidth = tf.AdvanceWidths[glyphIndex] * unit;
                var mini = geometry.Figures.ToString(x, baseline);
                var pathStr = $"<path d=\"{mini}\"/>";
                sb.AppendLine(pathStr);
                x += advanceWidth;
            }
            System.Diagnostics.Debug.WriteLine(sb);
        }
    }
}