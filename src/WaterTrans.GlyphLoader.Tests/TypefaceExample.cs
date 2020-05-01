using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace WaterTrans.GlyphLoader.Tests
{
    [TestClass]
    public class TypefaceExample
    {
        private readonly string[] _fontFiles = {
            "Roboto-Regular.ttf",
            "RobotoMono-Regular.ttf",
            "Lora-VariableFont_wght.ttf",
            "NotoSansJP-Regular.otf",
            "NotoSerifJP-Regular.otf",
        };

        private const string GlyphWarningMessage = @"
            <!DOCTYPE html><html><head><style>dt { font-weight: bold; } svg { border: 1px solid #000; }</style></head>
            <body><h1>This glyph may be incorrect.</h1>
            <dl><dt>File: </dt><dd>{fontFile}</dd></dl>
            <dl><dt>GlyphIndex: </dt><dd>{glyphIndex}</dd></dl>
            <dl><dt>Typeface: </dt><dd><svg width='128' height='128' viewBox='0 0 128 128' xmlns='http://www.w3.org/2000/svg' version='1.1'><path d='{pathData1}' fill='black' stroke='black' stroke-width='1' /></svg></dd></dl>
            <dl><dt>GlyphTypeface: </dt><dd><svg width='128' height='128' viewBox='0 0 128 128' xmlns='http://www.w3.org/2000/svg' version='1.1'><path d='{pathData2}' fill='black' stroke='black' stroke-width='1' /></svg></dd></dl>
            </body></html>
        ";

        public TestContext TestContext { get; set; }

        [TestMethod]
        public void CreateGlyphOutlineFurinkazan()
        {
            string fontPath = System.IO.Path.Combine(Environment.CurrentDirectory, "NotoSansJP-Regular.otf");
            Typeface tf;
            using (var fontStream = System.IO.File.OpenRead(fontPath))
            {
                // Initialize stream only
                tf = new Typeface(fontStream);
            }

            var svg = new System.Text.StringBuilder();
            double unit = 128;
            int roundDigits = 2;
            double x = 20;
            string japaneseText = "���щΎR";

            svg.AppendLine("<svg width='552' height='188' viewBox='0 0 552 188' xmlns='http://www.w3.org/2000/svg' version='1.1'>");

            foreach (char c in japaneseText)
            {
                // Get glyph index
                ushort glyphIndex = tf.CharacterToGlyphMap[(int)c];

                // Get glyph outline
                var geometry = tf.GetGlyphOutline(glyphIndex, unit);

                // Get advanced width
                double advance = tf.AdvanceWidths[glyphIndex] * unit;

                // Get baseline
                double baseline = tf.Baseline * unit;

                // Convert to path mini-language
                string miniLanguage = geometry.Figures.ToString(x, baseline, roundDigits);

                svg.AppendLine($"<path d='{miniLanguage}' fill='black' stroke='black' stroke-width='1' />");
                x += advance;
            }

            svg.AppendLine("</svg>");
            System.Diagnostics.Trace.WriteLine(svg.ToString());
            /* Result
            <svg width='552' height='188' viewBox='0 0 552 188' xmlns='http://www.w3.org/2000/svg' version='1.1'>
            <path d='M64.16,113.02L64.16,93.57L78.62,93.57L78.62,113.02z M101.66,93.57L101.66,113.02L87.07,113.02L87.07,93.57z M96.67,126.21C99.1,129.54 101.41,133.25 103.58,137.09L87.07,138.11L87.07,120.58L109.73,120.58L109.73,85.89L87.07,85.89L87.07,73.47C96.54,72.32 105.38,70.78 112.42,68.86L105.76,61.95C93.6,65.28 71.46,67.84 52.9,69.12C53.92,71.04 55.07,74.24 55.46,76.16C62.88,75.9 70.82,75.26 78.62,74.5L78.62,85.89L56.48,85.89L56.48,120.58L78.62,120.58L78.62,138.62C67.1,139.26 56.74,139.9 48.8,140.29L49.44,148.99C64.54,147.84 86.3,146.3 107.55,144.77C109.34,148.35 110.62,151.81 111.39,154.62L119.2,151.81C117.02,144 110.62,132.22 103.97,123.78z M40.1,48.38L40.1,89.34C40.1,108.93 38.69,134.91 24.99,153.09C27.17,154.24 31.01,156.8 32.54,158.46C47.01,139.14 49.18,110.08 49.18,89.34L49.18,57.09L118.3,57.09C118.69,112.9 118.56,158.21 134.18,158.21C140.7,158.21 142.62,151.81 143.52,135.3C141.73,133.76 139.3,131.07 137.63,128.51C137.38,139.65 136.74,148.35 135.07,148.35C127.52,148.35 127.26,95.87 127.39,48.38z ' fill='black' stroke='black' stroke-width='1' />
            <path d='M208.42,104.06C205.34,100.35 191.78,84.74 187.55,80.77L187.55,77.82L205.73,77.82L205.73,68.74L187.55,68.74L187.55,41.34L178.21,41.34L178.21,68.74L155.42,68.74L155.42,77.82L176.54,77.82C171.68,95.49 161.82,115.2 152.22,125.95C153.89,128.26 156.19,131.97 157.34,134.78C165.02,125.82 172.7,110.98 178.21,95.62L178.21,158.08L187.55,158.08L187.55,92.16C192.8,98.82 199.33,107.65 202.14,112.26z M267.81,77.82L267.81,68.74L243.62,68.74L243.62,41.34L234.27,41.34L234.27,68.74L211.23,68.74L211.23,77.82L232.22,77.82C226.34,98.3 214.43,119.17 202.27,131.07C204.06,133.25 206.62,136.83 207.9,139.39C217.89,129.41 227.49,112.77 234.27,95.23L234.27,158.08L243.62,158.08L243.62,94.85C249.25,111.62 256.67,127.23 264.48,137.09C266.14,134.53 269.47,131.33 271.78,129.66C261.54,118.53 251.81,98.05 246.05,77.82z ' fill='black' stroke='black' stroke-width='1' />
            <path d='M344.48,43.26L334.37,43.26L334.37,84.35C334.37,99.2 325.54,134.14 282.66,150.53C284.83,152.45 287.9,156.42 289.18,158.34C325.28,143.49 336.93,115.2 339.36,102.78C341.92,115.07 354.34,144.38 391.33,158.34C392.74,155.78 395.55,151.68 397.6,149.63C353.57,134.02 344.48,98.94 344.48,84.35z M381.86,66.94C377.63,78.08 369.57,93.44 363.3,102.78L371.23,106.5C377.76,97.41 385.95,82.94 392.1,71.04z M302.11,67.2C300.06,81.41 295.71,95.23 285.47,102.91L293.66,108.54C305.06,99.84 309.28,84.35 311.58,69.12z ' fill='black' stroke='black' stroke-width='1' />
            <path d='M508.83,71.55L508.83,136.7L472.35,136.7L472.35,44.03L462.62,44.03L462.62,136.7L427.42,136.7L427.42,71.68L417.82,71.68L417.82,156.67L427.42,156.67L427.42,146.3L508.83,146.3L508.83,156.16L518.56,156.16L518.56,71.55z ' fill='black' stroke='black' stroke-width='1' />
            </svg>
            */
        }
    }
}