using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using CCInterfaces.Contracts;
using CCModels.Helper;
using CCModels.Models;

namespace ADDICCWebAPIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        private IConfiguration _configuration { get; }

        public DashboardController(IDashboardService dashboardService, IConfiguration configuration)
        {
            _dashboardService = dashboardService;
            _configuration = configuration;
        }
        //Get/GetDashboardStats
        [HttpGet("GetDashboardStats/{userID}")]
        public async Task<DashboardStats> GetDashboardStats(int userID)
        {
            return await _dashboardService.GetDashboardStats(userID);
        }
        //Post/GetDashboardCaseData
        [HttpPost("GetDashboardCaseData")]
        public async Task<GridResult<AdvanceSearchViewModel>> GetDashboardCaseData(DashboardCaseSearchCriteria criteria)
        {
            return await _dashboardService.GetDashboardCaseData(criteria);
        }
        //POST/GetDashboardCaseInfo
        [HttpPost("GetDashboardCaseInfo")]
        public async Task<GridResult<AdvanceSearchViewModel>> GetDashboardCaseInfo(DashboardCaseSearchCriteria criteria)
        {
            return await _dashboardService.GetDashboardCaseInfo(criteria);
        } 
        //POST/GetDashboardDocData
        [HttpPost("GetDashboardDocData")]
        public async Task<GridResult<DocumentViewModel>> GetDashboardDocData(DashboardCaseSearchCriteria criteria)
        {
            return await _dashboardService.GetDashboardDocData(criteria);
        }
        //POST/GetDashboardCountyData
        [HttpPost("GetDashboardCountyData")]
        public async Task<GridResult<DashCountyViewModel>> GetDashboardCountyData(DashboardCaseSearchCriteria criteria)
        {
            DateTime fromdate = criteria.FromDate.ToLocalTime();
            DateTime toDate = criteria.ToDate.ToLocalTime();
            criteria.FromDate = fromdate;
            criteria.ToDate = toDate;
            return await _dashboardService.GetDashboardCountyData(criteria);
        }
        //POST/DownloadCountyData
        [HttpPost("DownloadCountyData")]
        public PdfViewModel DownloadCountyData(DashboardCaseSearchCriteria criteria)
        {
            DateTime fromdate = criteria.FromDate.ToLocalTime();
            DateTime toDate = criteria.ToDate.ToLocalTime();
            criteria.FromDate = fromdate;
            criteria.ToDate = toDate;
            var result = _dashboardService.GetDashboardCountyData(criteria).Result;

            StringBuilder sb = new StringBuilder();

            sb.Append("<!DOCTYPE html><html>");
            sb.Append("<head><style>table {  font-family: arial, sans-serif;  border-collapse:collapse;  width: 100%;}td, th {  border: 1px solid #dddddd;  text-align: left;  padding: 8px;}tr:nth-child(even) {  background-color: #dddddd;}</style></head>");

            sb.Append("<body>");
            string Imagefile = string.Format(_configuration["ReportDocuments"].Replace(@"~\", "")) + "seal" + ".PNG";
            byte[] StateLogo = PDFGeneratorHelper.imageConversion(Imagefile);
            var imageString = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(StateLogo));
            sb.Append("<h1> <img src='" + imageString + "''> ADDI-CC COUNTY USERS REPORT</h1>");

            sb.Append("<table>");
            sb.Append("<thead>");

            sb.Append("<tr>");
            sb.Append("<th>County</th>");
            sb.Append("<th>User Name</th>");
            sb.Append("<th>User ID</th>");
            sb.Append("<th>Cases Count</th>");
            sb.Append("<th>Document Count</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            foreach (var item in result.Data)
            {
                sb.Append("<tr>");
                sb.Append("<td>" + item.CountyName + "</td>");
                sb.Append("<td>" + item.LastName + ", " + item.FirstName + "</td>");
                sb.Append("<td>" + item.Username + "</td>");
                sb.Append("<td>" + item.CasesCount + "</td>");
                sb.Append("<td>" + item.DocCount + "</td>");
                sb.Append("</tr>");
            }
            sb.Append("<tr>");
            sb.Append("<td></td>");
            sb.Append("<td></td>");
            sb.Append("<td></td>");
            sb.Append("<td>" + result.Data.Sum(x => x.CasesCount) + "</td>");
            sb.Append("<td>" + result.Data.Sum(x => x.DocCount) + "</td>");
            sb.Append("</tr>");
            sb.Append("</tbody>");
            sb.Append("</table>");

            //sb.Append("<footer>");
            sb.Append("Report generated at " + DateTime.Now.ToLongDateString());
            sb.Append("</body></html>");
            string file = string.Format(_configuration["ReportDocuments"].Replace(@"~\", "")) + Guid.NewGuid().ToString() + ".pdf";
            string pdf = PDFGeneratorHelper.PdfSharpConvertToPDF(sb.ToString(), file);
            //  Byte[] fileContent = PdfSharpConvert(sb.ToString());

            try
            {
                return new PdfViewModel() { Name = Convert.ToBase64String(System.IO.File.ReadAllBytes(pdf)) };

            }
            catch (Exception ex) { }

            return null;

        }
    }
}
