using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCInterfaces.Contracts;
using CCModels.Helper;
using CCModels.Models;
using CCRepo.Repos;

namespace CCInterfaces.Services
{
   public class ImportService :IImportService
    {
        private IConfiguration _configuration { get; }
        private readonly ImportRepo _repository;
        public ImportService(ImportRepo repository, IConfiguration configuration)
        {
            _configuration = configuration;
            _repository = repository;
        }
        public int ImportDocuments(DocumentViewModel document)
        {
            var DocumentList = _repository.GetDocumentInfo(document);
            List<string> filestomove = new List<string>();
            List<string> listfiles = new List<string>();
            document.CreatedDate = DateTime.Now;
            ScannedAndCreateDocumentInfo ScannedAndCreateDocumentInfo = new ScannedAndCreateDocumentInfo();
            ScannedAndCreateDocumentInfo.CaseInfoId = document.CaseInfoID;
            ScannedAndCreateDocumentInfo.ReviewDate = document.ReviewDate;
            ScannedAndCreateDocumentInfo.DocumentTypeID = document.DocumentTypeID;
            ScannedAndCreateDocumentInfo.DocumentSubTypeId = document.DocumentSubTypeID;
            string sourcePath = string.Format(_configuration["DocumentsSourcePath"].Replace(@"~\", "")) + document.Username;
            string destinationPath = string.Format(_configuration["DocumentsDestinationPath"].Replace(@"~\", ""));
            string filePath = Guid.NewGuid() + ".pdf";
            int pageCount = 0;
            var finalPath = sourcePath + "\\";
            DirectoryInfo di = new DirectoryInfo(finalPath);
            FileInfo[] Files = di.GetFiles("*.pdf");

            if (DocumentList.Count() == 0)
            {
                foreach (var item in document.DataPdfFiles)
                {
                    if (System.IO.File.Exists(finalPath + item.Name))
                    {
                        filestomove.Add(finalPath + item.Name);
                    }
                    else
                    {
                        filestomove.Add(finalPath + item.Name);
                        finalPath = destinationPath + "\\";
                    }
                }
                string DocfinalPath = GetDestFolderPath() + filePath;
                pageCount = ConvertToPDF.MergeMultiplePDFIntoSinglePDF(filestomove, DocfinalPath);
                document.PageCount = pageCount;
                document.FilePath = DocfinalPath;
                //document.Description = "All these pages are imported at the " + document.CountyName;
                var Saveddocument = _repository.SaveDocument(document).Result;
                for (int i = 1; i <= pageCount; i++)
                {
                    foreach (var item in document.DataPdfFiles)
                    {
                        ScannedAndCreateDocumentInfo.DocumentId = Saveddocument;
                        ScannedAndCreateDocumentInfo.DocumentNotesDescription = "Page " + i + " was imported on " + document.CreatedDate + " and scanned on " + item.ScanedDateTime;
                        //ScannedAndCreateDocumentInfo.DocumentNotesDescription = "Page " + i + " was imported on " + document.CreatedDate + " and scanned on " + item.ScanedDateTime + ", and the county is " + document.CountyName + ".";
                        _repository.DocumentNotes(ScannedAndCreateDocumentInfo);
                        break;
                    }

                }
                foreach (var item in filestomove)
                {
                    System.IO.File.Delete(item);
                }
                return Saveddocument;
            }
            else
            {
                listfiles.Add(DocumentList[0].FilePath);
                foreach (var item in document.DataPdfFiles)
                {
                    if (System.IO.File.Exists(finalPath + item.Name))
                    {
                        listfiles.Add(finalPath + item.Name);
                        filestomove.Add(finalPath + item.Name);
                    }
                    else
                    {
                        listfiles.Add(finalPath + item.Name);
                        finalPath = destinationPath + "\\";
                        filestomove.Add(finalPath + item.Name);
                    }
                }
                filePath = Guid.NewGuid() + ".pdf";
                string UpdatedDocPath = GetDestFolderPath() + filePath;
                pageCount = ConvertToPDF.MergeMultiplePDFIntoSinglePDF(listfiles, UpdatedDocPath);
                int? PageNumber = DocumentList[0].PageCount + 1;
                int? updatepagecount = (pageCount) - (DocumentList[0].PageCount);
                // string DocumentDescription = "";
                for (int i = 0; i < updatepagecount; i++)
                {
                    int? DocPageNumber = (PageNumber) + i;
                    foreach (var item in document.DataPdfFiles)
                    {
                        ScannedAndCreateDocumentInfo.DocumentId = DocumentList[0].DocumentID;
                        ScannedAndCreateDocumentInfo.DocumentNotesDescription = "Page " + DocPageNumber + " was updated on " + document.CreatedDate + " and scanned on " + item.ScanedDateTime;
                        // ScannedAndCreateDocumentInfo.DocumentNotesDescription = "Page " + DocPageNumber + " was updated on " + document.CreatedDate + " and scanned on " + item.ScanedDateTime + ", and the county is " + document.CountyName + ".";
                        _repository.DocumentNotes(ScannedAndCreateDocumentInfo);
                        break;
                    }
                }
                DocumentList[0].PageCount = pageCount;
                DocumentList[0].LastUpdatedBy = document.LastUpdatedBy;
                DocumentList[0].FilePath = UpdatedDocPath;
                //var DocumentDescription = DocumentList[0].Description +"," + " All these pages are updated at the " + document.CountyName;
                //DocumentList[0].Description = DocumentDescription;
                _repository.UpdateDocument(DocumentList[0]);
                foreach (var item in filestomove)
                {
                    System.Threading.Thread.Sleep(3000);
                    System.IO.File.Delete(item);
                }
            }
            return document.DocumentID;
        }

        public string GetDestFolderPath()
        {
            string destinationPath = string.Format(_configuration["DocumentsDestinationPath"].Replace(@"~\", ""));
            if (!Directory.Exists(destinationPath + @"\" + DateTime.Now.Year))
            {
                Directory.CreateDirectory(destinationPath + @"\" + DateTime.Now.Year);
            }
            if (!Directory.Exists(destinationPath + @"\" + DateTime.Now.Year + @"\" + DateTime.Now.Month))
            {
                Directory.CreateDirectory(destinationPath + @"\" + DateTime.Now.Year + @"\" + DateTime.Now.Month);
            }
            destinationPath = destinationPath + DateTime.Now.Year + @"\" + DateTime.Now.Month;
            string[] existingDirectories = Directory.GetDirectories(destinationPath);
            int i = 1;
            if (existingDirectories.Any().ToStr() != destinationPath + @"\" + DateTime.Now.Day.ToInt() + @"\" + i)
            {
                //destinationPath = destinationPath + @"\" + DateTime.Now.Day.ToInt() + @"\";
                destinationPath = destinationPath + @"\" + DateTime.Now.Day.ToInt() + @"\" + i + @"\";
                if (!Directory.Exists(destinationPath))
                {
                    Directory.CreateDirectory(destinationPath);
                }
                else
                {
                    if (Directory.GetFiles(destinationPath).Count() > 10000)
                    {
                        for (int x = i + 1; x < 100; x++)
                        {
                            destinationPath = string.Format(_configuration["DocumentsDestinationPath"].Replace(@"~\", "")) + DateTime.Now.Year + @"\" + DateTime.Now.Month + @"\" + (DateTime.Now.Day.ToInt()) + @"\" + x + @"\";
                            break;
                        }
                        if (!Directory.Exists(destinationPath))
                        {
                            Directory.CreateDirectory(destinationPath);
                        }
                    }
                    else
                    {
                        if (!Directory.Exists(destinationPath))
                            Directory.CreateDirectory(destinationPath);
                        return destinationPath;
                    }
                }
            }
            else
            {
                if (Directory.GetFiles(destinationPath).Count() < 25000)
                {
                    destinationPath = destinationPath + @"\" + (DateTime.Now.Day.ToInt() + 1) + @"\";
                }
                else
                {
                    destinationPath = destinationPath + @"\" + DateTime.Now.Day.ToInt() + @"\";
                }
            }
            if (!Directory.Exists(destinationPath))
                Directory.CreateDirectory(destinationPath);
            return destinationPath;
        }
    }
}
