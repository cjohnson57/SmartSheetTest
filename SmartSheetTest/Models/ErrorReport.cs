using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartSheetTest.Models
{
    public class ErrorReport
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string UserName { get; set; }
        public string ErrorMessage { get; set; }
        public string Enhancement { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public DateTime Modified { get; set; }
        public HttpPostedFileBase file { get; set; }
    }
}