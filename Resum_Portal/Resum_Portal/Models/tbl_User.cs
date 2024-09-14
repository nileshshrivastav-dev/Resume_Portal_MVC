namespace Resum_Portal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_User()
        {
            tbl_resume = new HashSet<tbl_resume>();
        }

        [Key]
        [StringLength(120)]
        public string Email { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public long? Mobile { get; set; }

        [StringLength(50)]
        public string Gender { get; set; }

        [StringLength(250)]
        public string Address { get; set; }

        [StringLength(20)]
        public string password { get; set; }

        [StringLength(50)]
        public string Role { get; set; }

        public bool? Status { get; set; }

        [StringLength(200)]
        public string Designation { get; set; }

        [StringLength(200)]
        public string Photo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date_of_Joining { get; set; }

        [StringLength(120)]
        public string CreatedBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_resume> tbl_resume { get; set; }
    }
}
