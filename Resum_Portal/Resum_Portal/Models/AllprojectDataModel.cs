using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Resum_Portal.Models
{
    public partial class AllprojectDataModel : DbContext
    {
        public AllprojectDataModel()
            : base("name=AllprojectDataModel")
        {
        }

        public virtual DbSet<tbl_Designation> tbl_Designation { get; set; }
        public virtual DbSet<tbl_resume> tbl_resume { get; set; }
        public virtual DbSet<tbl_User> tbl_User { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tbl_Designation>()
                .Property(e => e.Designation)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_resume>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_resume>()
                .Property(e => e.Designation)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_resume>()
                .Property(e => e.Resume)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_resume>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_resume>()
                .Property(e => e.KeyWords)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_resume>()
                .Property(e => e.UploadedBy)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_User>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_User>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_User>()
                .Property(e => e.Gender)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_User>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_User>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_User>()
                .Property(e => e.Role)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_User>()
                .Property(e => e.Designation)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_User>()
                .Property(e => e.Photo)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_User>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);
        }
    }
}
