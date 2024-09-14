namespace Resum_Portal.Models
{
    using iText.Forms.Form.Element;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_resume
    {
        public int id { get; set; }

        [StringLength(120)]
        public string Email { get; set; }

        [StringLength(200)]
        public string Designation { get; set; }

        [StringLength(250)]
        public string Resume { get; set; }

        [StringLength(120)]
        public string Title { get; set; }

        [StringLength(250)]
        public string KeyWords { get; set; }

        public bool? Status { get; set; }

        [StringLength(120)]
        public string UploadedBy { get; set; }

        [StringLength(250)]
        public string FilePath { get; set; }

        public virtual tbl_Designation tbl_Designation { get; set; }

        public virtual tbl_User tbl_User { get; set; }
    }
}
