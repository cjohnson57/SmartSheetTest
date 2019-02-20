using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Smartsheet.Api;
using Smartsheet.Api.Models;
using SmartSheetTest.Models;

namespace SmartSheetTest.Controllers
{
    public class HomeController : Controller
    {
        long sheetid = 0;
        string token = "";

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Form()
        {
            ErrorReport er = new ErrorReport();
            try
            {
                int zero = 0;
                int a = 10 / zero;
            }
            catch (Exception e)
            {
                er.guid = Guid.NewGuid();
                er.timestamp = DateTime.Now;
                er.errormessage = e.ToString();
            }
            return View(er);
        }

        [HttpPost]
        public ActionResult Form([Bind(Include ="")] ErrorReport er)
        {
            AddErrorReportToSmartSheet(er);
            return View(er);
        }

        public void AddErrorReportToSmartSheet(ErrorReport er)
        {
            SmartsheetClient sheeit = new SmartsheetBuilder().SetAccessToken(token).Build();
            Row r = new Row();
            r.ToBottom = true;
            PaginatedResult<Column> cols = sheeit.SheetResources.ColumnResources.ListColumns(sheetid, null, null);
            Cell[] cells = new Cell[]
            {
                new Cell
                {
                    ColumnId = cols.Data.First(x => x.Title == "ID").Id,
                    Value = er.guid
                },
                new Cell
                {
                    ColumnId = cols.Data.First(x => x.Title == "TimeStamp").Id,
                    Value = er.timestamp
                },
                new Cell
                {
                    ColumnId = cols.Data.First(x => x.Title == "Exception Message").Id,
                    Value = er.errormessage
                },
                new Cell
                {
                    ColumnId = cols.Data.First(x => x.Title == "User Message").Id,
                    Value = string.IsNullOrEmpty(er.usermessage) ? "" : er.usermessage
                },
            };
            r.Cells = cells;
            
            sheeit.SheetResources.RowResources.AddRows(sheetid, new Row[] { r });
            Sheet sheet = sheeit.SheetResources.GetSheet(sheetid, null, null, null, null, null, null, null);
            var row = sheet.Rows.OrderByDescending(x => x.CreatedAt).First();
            if (er.file != null && er.file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(er.file.FileName);
                var path = Path.Combine(Server.MapPath("~/App_Data/"), DateTime.Now.ToString("HH-mm-ss") + "-" + fileName);
                er.file.SaveAs(path);
                Attachment attachment = sheeit.SheetResources.RowResources.AttachmentResources.AttachFile(sheetid, (long)row.Id, path, "application/jpg");
                System.IO.File.Delete(path);
            }

            
        }
    }
}