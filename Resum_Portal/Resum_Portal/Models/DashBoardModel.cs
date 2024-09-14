using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Resum_Portal.Models
{
    public class DashBoardModel
    {
        public List<tbl_User> Users { get; set; } // Assuming tbl_User is your entity for users
        public tbl_User UserData { get; set; }
        public tbl_resume Resumes { get; set; }
        public List<tbl_Designation> Designations { get; set; }
        public List<tbl_resume> Resumesdata { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalResumes { get; set; }
    }
}