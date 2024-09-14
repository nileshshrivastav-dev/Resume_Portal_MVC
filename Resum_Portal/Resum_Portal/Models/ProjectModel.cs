using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Resum_Portal.Models
{
    public partial class ProjectModel : DbContext
    {
        public ProjectModel()
            : base("name=ProjectModel")
        {
        }

        public virtual DbSet<tbl_User> tbl_User { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
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
        }
    }
}
