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
        private readonly long sheetid = 0;
        private readonly string token = "redacted";

        public ActionResult Index()
        {
            List<ErrorReport> model = new List<ErrorReport>();
            SmartsheetClient sheetclient = new SmartsheetBuilder().SetAccessToken(token).Build();
            PaginatedResult<Column> cols = sheetclient.SheetResources.ColumnResources.ListColumns(sheetid, null, null);
            Sheet sheet = sheetclient.SheetResources.GetSheet(sheetid, null, null, null, null, null, null, null); //Gets the sheet id
            foreach(Row r in sheet.Rows)
            {
                ErrorReport newreport = new ErrorReport();
                try
                {
                    newreport.ID = int.Parse(r.Cells[0].Value.ToString());
                }
                catch
                {
                    newreport.ID = 0;
                }
                newreport.Date = DateTime.Parse(r.Cells[1].Value.ToString());
                newreport.UserName = r.Cells[2].Value.ToString();
                try
                {
                    newreport.ErrorMessage = r.Cells[3].Value.ToString();
                }
                catch
                {
                    newreport.ErrorMessage = "";
                }
                try
                {
                    newreport.Enhancement = r.Cells[4].Value.ToString();
                }
                catch
                {
                    newreport.Enhancement = "";
                }
                try
                {
                    newreport.Description = r.Cells[5].Value.ToString();
                }
                catch
                {
                    newreport.Description = "";
                }
                newreport.Category = r.Cells[6].Value.ToString();
                try
                {
                    newreport.Status = r.Cells[7].Value.ToString();
                }
                catch
                {
                    newreport.Status = "";
                }
                newreport.Modified = DateTime.Parse(r.Cells[8].Value.ToString());
                model.Add(newreport);
            }
            return View(model);
        }

        //public ActionResult Form()
        //{
        //    ErrorReport er = new ErrorReport();
        //    try
        //    {
        //        //Intentionally cause error
        //        int zero = 0;
        //        int a = 10 / zero;
        //    }
        //    catch (Exception e)
        //    {
        //        er.timestamp = DateTime.Now;
        //        er.errormessage = e.ToString();
        //    }
        //    return View(er);
        //}

        //[HttpPost]
        //public ActionResult Form([Bind(Include ="")] ErrorReport er)
        //{
        //    AddErrorReportToSmartSheet(er);
        //    return View(er);
        //}

        //public void AddErrorReportToSmartSheet(ErrorReport er)
        //{
        //    //Accesses smartsheet account
        //    SmartsheetClient sheetclient = new SmartsheetBuilder().SetAccessToken(token).Build();
        //    Row r = new Row();
        //    r.ToBottom = true; //Makes it so row will be added below all others
        //    PaginatedResult<Column> cols = sheetclient.SheetResources.ColumnResources.ListColumns(sheetid, null, null);
        //    //Following ten lines are to grab the previous row's id and increment it by one to use for this id
        //    int id = -1; //0 default if tryparse fails
        //    try
        //    {
        //        Sheet sheet = sheetclient.SheetResources.GetSheet(sheetid, null, null, null, null, null, null, null); //Gets the sheet id
        //        Row row = sheet.Rows.OrderByDescending(x => x.CreatedAt).First(); //Gets the last row in the sheet, the one most recently added
        //        long IDColumnId = (long)cols.Data.First(x => x.Title == "ID").Id;
        //        int.TryParse(row.Cells.First(x => x.ColumnId == IDColumnId).Value.ToString(), out id);
        //    }
        //    catch { }
        //    string incrementedid = (id + 1).ToString();
        //    Cell[] cells = new Cell[]
        //    {
        //        //Each cell uses a LINQ query to find the id for the column it should go to
        //        new Cell
        //        {
        //            ColumnId = cols.Data.First(x => x.Title == "ID").Id,
        //            Value = incrementedid
        //        },
        //        new Cell
        //        {
        //            ColumnId = cols.Data.First(x => x.Title == "Date").Id,
        //            Value = er.timestamp
        //        },
        //        new Cell
        //        {
        //            ColumnId = cols.Data.First(x => x.Title == "User Name").Id,
        //            Value = "Test User"
        //        },
        //        new Cell
        //        {
        //            ColumnId = cols.Data.First(x => x.Title == "Error Message").Id,
        //            Value = er.errormessage
        //        },
        //        new Cell
        //        {
        //            ColumnId = cols.Data.First(x => x.Title == "Description").Id,
        //            Value = string.IsNullOrEmpty(er.usermessage) ? "" : er.usermessage //Ternary for if user did not input anything for message
        //        },
        //    };
        //    r.Cells = cells; //Adds cells to the row
        //    sheetclient.SheetResources.RowResources.AddRows(sheetid, new Row[] { r }); //Adds row to the sheet
        //    //If user attached an image
        //    if (er.file != null && er.file.ContentLength > 0)
        //    {
        //        Sheet sheet = sheetclient.SheetResources.GetSheet(sheetid, null, null, null, null, null, null, null); //Gets the sheet id
        //        Row row = sheet.Rows.OrderByDescending(x => x.CreatedAt).First(); //Gets the last row in the sheet, the one just added
        //        string fileName = Path.GetFileName(er.file.FileName);
        //        string path = Path.Combine(Server.MapPath("~/App_Data/"), DateTime.Now.ToString("HH-mm-ss") + "-" + fileName);
        //        er.file.SaveAs(path); //Saves the image onto the server briefly
        //        Attachment attachment = sheetclient.SheetResources.RowResources.AttachmentResources.AttachFile(sheetid, (long)row.Id, path, "application/jpg"); //Attaches image to row
        //        System.IO.File.Delete(path); //Deletes image from server
        //    }         
        //}
    }
}