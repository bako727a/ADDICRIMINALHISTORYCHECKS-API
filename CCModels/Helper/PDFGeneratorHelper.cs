using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace CCModels.Helper
{
    public sealed class PDFGeneratorHelper
    {
        private IConfiguration _configuration { get; }
        public PDFGeneratorHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static string PdfSharpConvertToPDF(String html, string file)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var pdf = PdfGenerator.GeneratePdf(html, PdfSharp.PageSize.A4); // 'TheArtOfDev' resolved
                pdf.Save(file);
            }
            return file;
        }

        public static Byte[] PdfSharpConvert(String html)
        {
            Byte[] res = null;
            using (MemoryStream ms = new MemoryStream())
            {
                var pdf = PdfGenerator.GeneratePdf(html, PdfSharp.PageSize.A4); // 'TheArtOfDev' resolved
                pdf.Save(ms);
                res = ms.ToArray();
            }
            return res;
        }

        public static byte[] imageConversion(string imageName)
        {
            FileStream fs = new FileStream(imageName, FileMode.Open, FileAccess.Read);
            byte[] imgByteArr = new byte[fs.Length];
            fs.Read(imgByteArr, 0, Convert.ToInt32(fs.Length));
            fs.Close();
            return imgByteArr;
        }
    }
}
