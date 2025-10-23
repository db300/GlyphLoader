using System;
using System.IO;
using System.Text;
using WaterTrans.GlyphLoader;

namespace GlyphLoader.RenderLibrary
{
    /// <summary>
    /// 渲染帮助类(汉仪)
    /// </summary>
    public static class RenderHelper4Hanyi
    {
        /// <summary>
        /// 汉仪官网字体详情页头图
        /// </summary>
        /// <param name="fileName">字体文件名</param>
        /// <returns>头图svg字符串</returns>
        public static string HanyiWebsiteFontDetailBanner(string fileName)
        {
            Typeface tf;
            using (var fs = File.OpenRead(fileName))
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                tf = new Typeface(fs);
            }

            var row = arr.GetLength(0);//const int row = 4;
            var col = arr.GetLength(1);//const int col = 12;
            var width = col * em + (col - 1) * spaceX;
            var height = (row + 1) * em + row * spaceY;

            var sb = new StringBuilder();
            sb.AppendLine($"<svg width='{width}' height='{height}' viewBox='0 0 {width} {height}' xmlns='http://www.w3.org/2000/svg' version='1.1'>");

            //渲染中文
            for (var i = 0; i < row; i++)
            {
                for (var j = 0; j < col; j++)
                {
                    var glyphIndex = tf.CharacterToGlyphMap[arr[i, j][0]];
                    var geometry = tf.GetGlyphOutline(glyphIndex, em);
                    var baseline = tf.Baseline * em;
                    var mini = geometry.Figures.ToString(j * (em + spaceX), i * (em + spaceY) + baseline);
                    sb.AppendLine($"<path d='{mini}' fill='{color}' stroke='{color}' stroke-width='0' />");
                }
            }

            //渲染英文数字
            var x = 0D;
            var y = row * (em + spaceY);
            foreach (var c in s1)
            {
                var glyphIndex = tf.CharacterToGlyphMap[c];
                var geometry = tf.GetGlyphOutline(glyphIndex, em);
                var advanceWidth = tf.AdvanceWidths[glyphIndex] * em;
                var baseline = tf.Baseline * em;
                var mini = geometry.Figures.ToString(x, y + baseline);
                sb.AppendLine($"<path d='{mini}' fill='{color}' stroke='{color}' stroke-width='0' />");
                x += advanceWidth;
            }

            //渲染标点符号
            var length = s2.Length;
            x = width - length * em - (length - 1) * spaceX;
            for (var i = 0; i < length; i++)
            {
                var glyphIndex = tf.CharacterToGlyphMap[s2[i]];
                var geometry = tf.GetGlyphOutline(glyphIndex, em);
                var baseline = tf.Baseline * em;
                var mini = geometry.Figures.ToString(x + i * (em + spaceX), y + baseline);
                sb.AppendLine($"<path d='{mini}' fill='{color}' stroke='{color}' stroke-width='0' />");
            }

            sb.AppendLine("</svg>");
#if DEBUG
            System.Diagnostics.Debug.WriteLine(sb);
#endif
            return sb.ToString();
        }

        private static readonly string[,] arr = new[,]
        {
            {"玉", "金", "露", "云", "律", "闰", "秋", "寒", "辰", "日", "宇", "天"},
            {"出", "生", "结", "腾", "吕", "余", "收", "来", "宿", "月", "宙", "地"},
            {"昆", "丽", "为", "致", "调", "成", "冬", "暑", "列", "盈", "洪", "玄"},
            {"冈", "水", "霜", "雨", "阳", "岁", "藏", "往", "张", "昃", "荒", "黄"}
        };
        private const string s1 = "ABCabc123";
        private const string s2 = "，。：；！？";
        private const int spaceX = 5;
        private const int spaceY = 1;
        private const double em = 30;
        private const string color = "white";//正式用
        //private const string color = "black";//测试用
    }

    public static class RenderHelper4HanyiLatin
    {
        public static string HanyiWebsiteFontDetailBanner(string fileName)
        {
            Typeface tf;
            using (var fs = File.OpenRead(fileName))
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                tf = new Typeface(fs);
            }

            //计算整图文件的宽高
            var width = 0D;
            for (var i = 0; i < ss.Length; i++)
            {
                width = Math.Max(width, ss[i].Length * ems[i]);
            }
            var height = ss.Length * spaceY;
            for (var i = 0; i < ems.Length; i++)
            {
                height += ems[i];
            }

            var sb = new StringBuilder();
            sb.AppendLine($"<svg width='{width}' height='{height}' viewBox='0 0 {width} {height}' xmlns='http://www.w3.org/2000/svg' version='1.1'>");

            var y = 15F;
            for (var i = 0; i < ss.Length; i++)
            {
                var em = ems[i];
                var s = ss[i];
                //计算当前文本行宽度
                var width0 = 0D;
                foreach (var c in s)
                {
                    var glyphIndex = tf.CharacterToGlyphMap[c];
                    var advanceWidth = tf.AdvanceWidths[glyphIndex] * em;
                    width0 += advanceWidth;
                }
                var x = (width - width0) / 2;
                foreach (var c in s)
                {
                    var glyphIndex = tf.CharacterToGlyphMap[c];
                    var geometry = tf.GetGlyphOutline(glyphIndex, em);
                    var advanceWidth = tf.AdvanceWidths[glyphIndex] * em;
                    var baseline = tf.Baseline * em;
                    var mini = geometry.Figures.ToString(x, y + baseline);
                    sb.AppendLine($"<path d='{mini}' fill='{color}' stroke='{color}' stroke-width='0' />");
                    x += advanceWidth;
                }
                y += em + spaceY;
            }

            sb.AppendLine("</svg>");
#if DEBUG
            System.Diagnostics.Debug.WriteLine(sb);
#endif
            return sb.ToString();
        }

        private static readonly string[] ss = new[]
        {
            "ABCDEFG",
            "abcdefghijklmnopqrstuvwxyz",
            "0123456789"
        };
        private static readonly int[] ems = new[] { 50, 20, 30 };
        private const int spaceY = 15;
        private const string color = "white";//black
    }
}
