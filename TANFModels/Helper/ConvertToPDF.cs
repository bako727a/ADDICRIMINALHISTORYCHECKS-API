using Microsoft.Extensions.Configuration;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Text;

namespace TANFModels.Helper
{
    public class ConvertToPDF
    {
        private IConfiguration _configuration { get; }
        public static IConfiguration _config { get; }
        public ConvertToPDF(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public static int GetPageCount(string pdfFile)
        {
            int pagecount = 0;
            using (PdfDocument outputPDFDocument = new PdfDocument())
            {
                using (PdfDocument inputPDFDocument = PdfReader.Open(pdfFile, PdfDocumentOpenMode.Import))
                {
                    foreach (PdfPage page in inputPDFDocument.Pages)
                    {
                        pagecount++;
                    }
                }
            }
            return pagecount;
        }
        public static int SplitPdfs(List<string> pdfFiles, out List<string> lstErrorDocs, string sourcePath)
        {

            int pagecount = 0; lstErrorDocs = new List<string>();
            foreach (string pdfFile in pdfFiles)
            {
                try
                {

                    pagecount = 0;
                    using (PdfDocument inputPDFDocument = PdfReader.Open(pdfFile, PdfDocumentOpenMode.Import))
                    {
                        pagecount = inputPDFDocument.Pages.Count;
                        if (pagecount > 1)
                        {
                            for (int i = 1; i <= inputPDFDocument.Pages.Count; i++)
                            {
                                using (PdfDocument splitPDFDocument = new PdfDocument())
                                {
                                    splitPDFDocument.Version = inputPDFDocument.Version;
                                    splitPDFDocument.AddPage(inputPDFDocument.Pages[i - 1]);
                                    splitPDFDocument.Save(pdfFile.Replace(".pdf", "").Replace(".PDF", "") + "_" + $"{i:D2}" + "_Of_" + pagecount + ".pdf");
                                }
                            }
                        }
                    }
                    if (pagecount > 1)
                    {
                        System.IO.File.Delete(pdfFile);
                    }
                }

                catch (System.Exception ex)
                {
                    lstErrorDocs.Add(sourcePath);
                    if (File.Exists(pdfFile))
                    {
                        File.Delete(pdfFile);
                    }
                }
            }
            return pagecount;
        }
        public static int MovePDF(string pdfFiles, string outputFilePath)
        {
            int pagecount = 0;
            using (PdfDocument outputPDFDocument = new PdfDocument())
            {
                using (PdfDocument inputPDFDocument = PdfReader.Open(pdfFiles, PdfDocumentOpenMode.Import))
                {
                    outputPDFDocument.Version = inputPDFDocument.Version;

                }
                outputPDFDocument.Save(outputFilePath);
            }
            return pagecount;
        }
        public static int MergeMultiplePDFIntoSinglePDF(List<string> pdfFiles, string outputFilePath)
        {
            int pagecount = 0;
            using (PdfDocument outputPDFDocument = new PdfDocument())
            {
                foreach (string pdfFile in pdfFiles)
                {
                    using (PdfDocument inputPDFDocument = PdfReader.Open(pdfFile, PdfDocumentOpenMode.Import))
                    {
                        outputPDFDocument.Version = inputPDFDocument.Version;
                        foreach (PdfPage page in inputPDFDocument.Pages)
                        {
                            pagecount++;
                            outputPDFDocument.AddPage(page);
                        }
                    }
                }
                outputPDFDocument.Save(outputFilePath);
            }
            return pagecount;
        }
        public static byte[] MergeMultiplePDFIntoSinglePDFReturnBytes(List<string> pdfFiles)
        {
            using (PdfDocument outputPDFDocument = new PdfDocument())
            {
                foreach (string pdfFile in pdfFiles)
                {
                    using (PdfDocument inputPDFDocument = PdfReader.Open(pdfFile, PdfDocumentOpenMode.Import))
                    {
                        outputPDFDocument.Version = inputPDFDocument.Version;
                        foreach (PdfPage page in inputPDFDocument.Pages)
                        {
                            outputPDFDocument.AddPage(page);
                        }
                    }
                }
                using (MemoryStream stream = new MemoryStream())
                {
                    outputPDFDocument.Save(stream, true);
                    return stream.ToArray();
                }
            }
        }

        public static bool IsPDFHeader(string fileName)
        {
            FileStream fs = null;
            try
            {


                using (fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    byte[] buffer = null;
                    BinaryReader br = new BinaryReader(fs);

                    long numBytes = new FileInfo(fileName).Length;

                    buffer = br.ReadBytes(5);

                    var enc = new ASCIIEncoding();
                    var header = enc.GetString(buffer);


                    if (buffer[0] == 0x25 && buffer[1] == 0x50
                        && buffer[2] == 0x44 && buffer[3] == 0x46)
                    {
                        return header.StartsWith("%PDF-");
                    }

                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                fs.Close();
                fs.Dispose();
            }
            return false;
        }
    }
}
