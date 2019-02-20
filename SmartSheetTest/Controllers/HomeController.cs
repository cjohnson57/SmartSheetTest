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
                //Intentionally cause error
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
            //Accesses smartsheet account
            SmartsheetClient sheetclient = new SmartsheetBuilder().SetAccessToken(token).Build();
            Row r = new Row();
            r.ToBottom = true; //Makes it so row will be added below all others
            PaginatedResult<Column> cols = sheetclient.SheetResources.ColumnResources.ListColumns(sheetid, null, null);
            Cell[] cells = new Cell[]
            {
                //Each cell uses a LINQ query to find the id for the column it should go to
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
                    Value = string.IsNullOrEmpty(er.usermessage) ? "" : er.usermessage //Ternary for if user did not input anything for message
                },
            };
            r.Cells = cells; //Adds cells to the row
            sheetclient.SheetResources.RowResources.AddRows(sheetid, new Row[] { r }); //Adds row to the sheet
            //If user attached an image
            if (er.file != null && er.file.ContentLength > 0)
            {
                Sheet sheet = sheetclient.SheetResources.GetSheet(sheetid, null, null, null, null, null, null, null); //Gets the sheet id
                Row row = sheet.Rows.OrderByDescending(x => x.CreatedAt).First(); //Gets the last row in the sheet, the one just added
                string fileName = Path.GetFileName(er.file.FileName);
                string path = Path.Combine(Server.MapPath("~/App_Data/"), DateTime.Now.ToString("HH-mm-ss") + "-" + fileName);
                er.file.SaveAs(path); //Saves the image onto the server briefly
                Attachment attachment = sheetclient.SheetResources.RowResources.AttachmentResources.AttachFile(sheetid, (long)row.Id, path, "application/jpg"); //Attaches image to row
                System.IO.File.Delete(path); //Deletes image from server
            }         
        }
    }
}