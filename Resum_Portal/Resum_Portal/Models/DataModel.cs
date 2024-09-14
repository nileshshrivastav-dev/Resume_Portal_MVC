using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Resum_Portal.Models
{
    public class DataModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public long? Mobile { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }
        public int id { get; set; }

        public bool? Status { get; set; }
        public string Designation { get; set; }
        public HttpPostedFileBase[] Resumes { get; set; }
        public string Title { get; set; }
        public string KeyWords { get; set; }
        public string UploadedBy { get; set; }
        public string FilePath { get; set; }

        public DateTime? Date_of_Joining { get; set; }
        public string Photo { get; set; }
        public string CreatedBy { get; set; }

    }
}