using CCInterfaces.Contracts;
using CCModels.Helper;
using CCModels.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using OpenAI;
using OpenAI.Chat;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text.Json;
using System.Threading.Tasks;

namespace ADDICCWebAPIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly IAdvanceSearchService _searchService;
        private readonly IImportService _importService;
        private readonly ICaseManagementService _caseManagement;
        private IConfiguration _configuration { get; }
        public readonly ILookupService _lookupService;
        public ImportController(IConfiguration configuration, ILookupService lookupService, IImportService importService, IAdvanceSearchService SeachService, ICaseManagementService caseManagement)
        {
            _importService = importService;
            _configuration = configuration;
            _lookupService = lookupService;
            _searchService = SeachService;
            _caseManagement = caseManagement;
        }
        //Get/GetSourceFiles
        [HttpGet("GetSourceFiles")]
        public List<PdfViewModel> GetSourceFiles()
        {
            List<string> errorDocs = new List<string>();
            List<string> pdflist = new List<string>();
            List<SelectListItem> selectlist = new List<SelectListItem>();
            List<SelectListItem> finallist = new List<SelectListItem>();
            List<PdfViewModel> list = new List<PdfViewModel>();
            string sourcePath = string.Format(_configuration["DocumentsSourcePath"].Replace(@"~\", "")) + User.Identity.Name.Replace("DHRAL\\", "");
            DirectoryInfo di = new DirectoryInfo(sourcePath);
            FileInfo[] Files = di.GetFiles("*.pdf").Take(100).ToArray();
            foreach (FileInfo i in Files)
            {
                pdflist.Add(i.FullName);
            }
            ConvertToPDF.SplitPdfs(pdflist, out errorDocs, sourcePath);
            Files = di.GetFiles("*.pdf");
            Files = Files.Take(100).ToArray();
            foreach (FileInfo i in Files)
            {
                selectlist.Add(new SelectListItem()
                {
                    Text = i.Name,
                    Value = i.Name
                });
                finallist.Add(new SelectListItem()
                {
                    Text = i.Name,
                    Value = i.Name
                });
            }
            for (int i = 0; i < Files.Count(); i++)
            {

                try
                {
                    string FileName = sourcePath + '\\' + Files[i].Name;
                    bool result = ConvertToPDF.IsPDFHeader(FileName);
                    if (result != false)
                    {
                        list.Add(new PdfViewModel()
                        {
                            Sno = i + 1,
                            Name = Files[i].Name,
                            ScanedDateTime = Files[i].CreationTime
                        });
                    }
                    else
                    {
                        if (System.IO.File.Exists(FileName))
                        {
                            //var myFile = System.IO.File.Create(FileName);
                            //FileName.Remove();
                            System.IO.File.Delete(FileName);
                        }
                    }
                }
                catch (System.Exception)
                {
                    if (System.IO.File.Exists(Files[i].Name))
                    {
                        System.IO.File.Delete(Files[i].Name);
                    }
                }
            }
            return list;
        }
        //POST/GetFile
        [HttpPost("GetFile")]
        public PdfViewModel GetFile(PdfViewModel model)
        {
            string sourcePath = string.Format(_configuration["DocumentsSourcePath"].Replace(@"~\", "")) + User.Identity.Name.Replace("DHRAL\\", "");
            string filepath = sourcePath + '\\' + model.Name;
            if (System.IO.File.Exists(filepath))
            {
                return new PdfViewModel() { Name = Convert.ToBase64String(System.IO.File.ReadAllBytes(filepath)) };
            }
            else
            {
                return null;
            }
        }
        //Get/ViewFile
        [HttpGet("ViewFile")]
        public PdfViewModel ViewFile(string pdfname)
        {
            string sourcePath = string.Format(_configuration["DocumentsSourcePath"].Replace(@"~\", "")) + User.Identity.Name.Replace("DHRAL\\", "");
            DirectoryInfo di = new DirectoryInfo(sourcePath);
            FileInfo[] Files = di.GetFiles("*.pdf");


            string destinationPath = string.Format(_configuration["DocumentsDestinationPath"].Replace(@"~\", ""));
            //string sourcePath = string.Format(_configuration["DocumentsSourcePath"].Replace(@"~\", "")) + User.Identity.Name.Replace("DHRAL\\", "");
            if (pdfname == "SampleViewDoc.pdf" && Files.Count() != 0)
            {
                string FileName = sourcePath + '\\' + Files[0].Name;
                pdfname = FileName;
            }
            string filepath = pdfname;
            string importviewfile = destinationPath + pdfname;
            string sourcefile = sourcePath + '\\' + pdfname;
            string file = string.Format(_configuration["DocumentsDestinationPath"].Replace(@"~\", "")) + pdfname;
            try
            {
                if (System.IO.File.Exists(filepath))
                {
                    return new PdfViewModel() { Name = Convert.ToBase64String(System.IO.File.ReadAllBytes(filepath)) };
                }
                if (System.IO.File.Exists(importviewfile))
                {
                    return new PdfViewModel() { Name = Convert.ToBase64String(System.IO.File.ReadAllBytes(importviewfile)) };
                }
                if (System.IO.File.Exists(sourcefile))
                {
                    return new PdfViewModel() { Name = Convert.ToBase64String(System.IO.File.ReadAllBytes(sourcefile)) };
                }
            }
            catch (Exception) { }

            return null;
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return new System.Text.ASCIIEncoding().GetString(plainTextBytes);
        }
        //POST/RemoveFile
        [HttpPost("RemoveFile")]
        public List<PdfViewModel> RemoveFile(PdfViewModel model)
        {
            List<PdfViewModel> list = new List<PdfViewModel>();
            string sourcePath = string.Format(_configuration["DocumentsSourcePath"].Replace(@"~\", "")) + User.Identity.Name.Replace("DHRAL\\", "");
            string filepath = sourcePath + "\\" + model.Name;
            if (System.IO.File.Exists(filepath))
            {
                System.IO.File.Delete(filepath);
            }
            DirectoryInfo di = new DirectoryInfo(sourcePath);
            FileInfo[] Files = di.GetFiles("*.pdf");
            for (int i = 0; i < Files.Count(); i++)
            {
                list.Add(new PdfViewModel()
                {
                    Sno = i + 1,
                    Name = Files[i].Name,
                    ScanedDateTime = Files[i].CreationTime
                });
            }
            return list;
        }
        //Post/ImportDocuments
        [HttpPost("ImportDocuments")]
        public int ImportDocuments([FromForm] DocumentViewModel document)
        {
            int result = 0;
            if (document.ReviewPeriod != null)
            {
                document.ReviewDate = document.ReviewPeriod.Value.ToShortDateString();
            }
           // DocumentViewModel ImportedDocument = new DocumentViewModel();
            try
            {
                IFormFile file = ConvertFileToiFormfile(document.FilePath);
                PdfExtractedDto fileinfo = UploadPDF(file);
                document.Username = User.Identity.Name.Replace("DHRAL\\", "");
                var user = _lookupService.GetUserList(document.Username, 0).Result.FirstOrDefault();
                var CaseInfo = _searchService.caserecordinfo(document.CaseInfoID).Result.FirstOrDefault();
                document.CountyID = CaseInfo.CountyID;
                document.CountyName = CaseInfo.CountyName;
                document.CreatedBy = user.ID;
                document.LastUpdatedBy = user.ID;
                document.PdfExtractData = fileinfo.ExtractedText;
                result = _importService.ImportDocuments(document);
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        [NonAction]
        public IFormFile ConvertFileToiFormfile(string FilePath)
        {
            var fileInfo = new FileInfo(FilePath);
            var fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
            return new FormFile(fileStream, 0, fileStream.Length, fileInfo.Name, fileInfo.Name)
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/pdf"
            };
        }


        //Get/RotateFile
        [HttpGet("RotateFile")]
        public PdfViewModel RotateFile(string pdfname)
        {
            string sourcePath = string.Format(_configuration["DocumentsSourcePath"].Replace(@"~\", "")) + User.Identity.Name.Replace("DHRAL\\", "");
            string outputFile = Guid.NewGuid() + ".pdf";
            int pagerotate = 1;
            string inputFile = sourcePath + "\\" + pdfname;
            using (FileStream outStream = new FileStream(outputFile, FileMode.Create))
            {
                using (iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(inputFile))
                {

                    iTextSharp.text.pdf.PdfStamper stamper = new iTextSharp.text.pdf.PdfStamper(reader, outStream);
                    iTextSharp.text.pdf.PdfDictionary pageDict = reader.GetPageN(pagerotate);
                    int desiredRot = 90; // 90 degrees clockwise from what it is now
                    iTextSharp.text.pdf.PdfNumber rotation = pageDict.GetAsNumber(iTextSharp.text.pdf.PdfName.ROTATE);
                    if (rotation != null)
                    {
                        desiredRot += rotation.IntValue;
                        desiredRot %= 360; // must be 0, 90, 180, or 270
                    }
                    pageDict.Put(iTextSharp.text.pdf.PdfName.ROTATE, new iTextSharp.text.pdf.PdfNumber(desiredRot));
                    stamper.Close();
                }
            }
            System.IO.File.Delete(inputFile);
            System.IO.File.Move(outputFile, inputFile);
            PdfViewModel pdf = new PdfViewModel();
            pdf.Name = outputFile;
            return pdf;
        }



        //POST/UploadFile
        [HttpPost("UploadFile"), DisableRequestSizeLimit]
        public List<PdfViewModel> UploadFile()
        {
            try
            {
                var file = Request.Form.Files[0];
                string newPath = "Upload";
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }

                string sourcePath = string.Format(_configuration["DocumentsSourcePath"].Replace(@"~\", "")) + User.Identity.Name.Replace("DHRAL\\", "");
                if (!Directory.Exists(sourcePath))
                {
                    Directory.CreateDirectory(sourcePath);
                }

                string fileName = ""; string fullPath = "";
                if (file.Length > 0)
                {
                    fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    fullPath = Path.Combine(newPath, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }
                HostingEnvironment hostingEnvironment = new HostingEnvironment();
                fileName = System.IO.Path.GetFileName(fileName);
                System.IO.File.Move(fullPath, sourcePath + "\\" + Guid.NewGuid() + "-" + fileName);

                return GetSourceFiles();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("upload")]
        public PdfExtractedDto UploadPDF(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            using var memoryStream = new MemoryStream();
            file.CopyToAsync(memoryStream);

            memoryStream.Position = 0;

            var text = "";

            using (var pdf = UglyToad.PdfPig.PdfDocument.Open(memoryStream))
            {
                foreach (var page in pdf.GetPages())
                {
                    text += page.Text + "\n";
                }
            }

            //Save the data into the database!!
            var pdfDoc = new PdfExtractedDto
            {
                FileName = file.FileName,
                ExtractedText = text
            };
            return pdfDoc;
        }

        [HttpGet("download/{id}")]
        public IActionResult DownloadPdf(int id)
        {
            var user = User.Identity.Name.Replace("DHRAL\\", "");
            var userinfo = _lookupService.GetUserList(user, 0).Result.FirstOrDefault();
            var pdfDoc = _caseManagement.documentrecordinfo(id, userinfo.ID).Result;
            //var pdfDoc = await _dbContext.PdfDocuments.FindAsync(id);
            if (pdfDoc == null) return NotFound();

            using var memoryStream = new MemoryStream();
            var writer = new iText.Kernel.Pdf.PdfWriter(memoryStream);
            var pdf = new iText.Kernel.Pdf.PdfDocument(writer);
            var document = new iText.Layout.Document(pdf);

            document.Add(new iText.Layout.Element.Paragraph(pdfDoc.PDFextractdata));
            document.Close();

            memoryStream.Position = 0;
            return File(memoryStream.ToArray(), "application/pdf", pdfDoc.FilePath);
        }
    }
}
