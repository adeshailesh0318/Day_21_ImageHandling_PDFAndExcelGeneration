using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Day21_FileHandling_API.Models;

namespace Day21_FileHandling_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        [HttpGet("excel")]
        public IActionResult GenerateExcel()
        {
            var users = new List<UserReport>
            {
                new UserReport
                {
                    Id = 1,
                    Name = "Shailesh",
                    Email = "shailesh@gmail.com",
                    Role = "Developer",
                    Salary = 35000
                },

                new UserReport
                {
                    Id = 2,
                    Name = "Rahul",
                    Email = "rahul@gmail.com",
                    Role = "Tester",
                    Salary = 30000
                },

                new UserReport
                {
                    Id = 3,
                    Name = "Amit",
                    Email = "amit@gmail.com",
                    Role = "Manager",
                    Salary = 50000
                }
            };

            using var package = new ExcelPackage();

            var worksheet =
                package.Workbook.Worksheets.Add("Employees");

            // Header
            worksheet.Cells[1, 1].Value = "ID";
            worksheet.Cells[1, 2].Value = "Name";
            worksheet.Cells[1, 3].Value = "Email";
            worksheet.Cells[1, 4].Value = "Role";
            worksheet.Cells[1, 5].Value = "Salary";

            // Data
            int row = 2;

            foreach (var user in users)
            {
                worksheet.Cells[row, 1].Value = user.Id;
                worksheet.Cells[row, 2].Value = user.Name;
                worksheet.Cells[row, 3].Value = user.Email;
                worksheet.Cells[row, 4].Value = user.Role;
                worksheet.Cells[row, 5].Value = user.Salary;

                row++;
            }

            // Auto fit
            worksheet.Cells[
                worksheet.Dimension.Address
            ].AutoFitColumns();

            // Generate Excel
            var fileBytes = package.GetAsByteArray();

            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "EmployeeReport.xlsx"
            );
        }
    }
}