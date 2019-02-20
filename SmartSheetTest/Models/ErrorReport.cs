using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartSheetTest.Models
{
    public class ErrorReport
    {
        public Guid guid { get; set; }
        public DateTime timestamp { get; set; }
        public string errormessage { get; set; }
        public string usermessage { get; set; }
        public HttpPostedFileBase file { get; set; }
    }
}