using Microsoft.AspNetCore.Mvc;
using Traffic.Api.Extensions;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Claims;

namespace Traffic.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        [NonAction]
        public string GetUserId()
        {
            var userId = User.GetUserId();
            return userId;
        }

        [NonAction]
        public string GetUrl()
        {
            var route = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}";
            return route;
        }

        //[NonAction]
        //public IActionResult DownloadExcelTemplate(string columnsName, double columnWidth = 20, string sheetName = "TEMPLATE")
        //{
        //    var dt = new DataTable(sheetName);
        //    var columns = columnsName.Split(',');

        //    foreach (var col in columns)
        //    {
        //        dt.Columns.Add(col);
        //    }

        //    return ExportExcel(dt, columnWidth);
        //}

        //[NonAction]
        //public IActionResult ExportExcel(DataTable dt, double columnWidth = 20)
        //{
        //    var fileName = "Traffic_" + DateTime.Now.ToString("ddMMyyyy-hhmmss") + ".xlsx";
        //    using (XLWorkbook wb = new XLWorkbook(XLEventTracking.Disabled))
        //    {
        //        SetFormat(dt, wb, columnWidth);

        //        using (MemoryStream stream = new MemoryStream())
        //        {
        //            wb.SaveAs(stream);
        //            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        //        }
        //    }
        //}

        //private void SetFormat(DataTable dt, XLWorkbook wb, double columnWidth, bool hideGridLines = false)
        //{
        //    var ws = wb.Worksheets.Add(dt.TableName);
        //    ws.FirstCell().InsertTable(dt, false);

        //    var rows = ws.RangeUsed().RowsUsed().Skip(1); // Skip header row
        //    var columns = ws.RangeUsed().ColumnCount();

        //    if (rows.Any())
        //    {
        //        foreach (var row in rows)
        //        {
        //            var rowNumber = row.RowNumber();
        //            for (var i = 1; i <= columns; i++)
        //            {
        //                ws.Cell(rowNumber, i).Style.NumberFormat.Format = "@";
        //                ws.Cell(rowNumber, i).SetDataType(XLDataType.Text);
        //                var value = ws.Cell(rowNumber, i).Value.ToString();
        //                ws.Cell(rowNumber, i).Value = value;
        //            }
        //        }
        //    }

        //    var alphabet = ((char)(97 + columns - 1)).ToString().ToUpper();
        //    var columnRange = $"A1:{alphabet}1";

        //    ws.Range(columnRange).Style.Font.Bold = true;
        //    ws.Range(columnRange).Style.Fill.BackgroundColor = XLColor.FromHtml("#aeaaaa");
        //    ws.Range(columnRange).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //    ws.Columns($"A:{alphabet}").Width = columnWidth;

        //    if (hideGridLines)
        //    {
        //        ws.ShowGridLines = false;
        //        ws.Cells().Style.Border.SetTopBorder(XLBorderStyleValues.Thin);
        //        ws.Cells().Style.Border.SetBottomBorder(XLBorderStyleValues.Thin);
        //        ws.Cells().Style.Border.SetLeftBorder(XLBorderStyleValues.Thin);
        //        ws.Cells().Style.Border.SetRightBorder(XLBorderStyleValues.Thin);
        //        ws.Cells().Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        //        ws.Cells().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        //    }
        //}
    }
}
