using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using StudentAccounting.Data.EntityModels;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace StudentAccounting.Data
{
    public class StudentAccountingContext : IdentityDbContext
    {
        public DbSet<InstitutionEntityModel> Institutions { get; set; }
        public DbSet<PracticEntityModel> Practics { get; set; }
        public DbSet<StudentsEntityModel> Students { get; set; }

        // для последующей работы с БД :

        // public static StudentAccountingContext db = new StudentAccountingContext();

        public StudentAccountingContext(DbContextOptions<StudentAccountingContext> options) : base(options)
        {
            //Создание БД
            Database.EnsureCreated();

            if (Roles.Count() == 0)
            {
                Roles.Add(new IdentityRole { Name = "HR-Manager", NormalizedName = "HR-MANAGER" });
                Roles.Add(new IdentityRole { Name = "Mentor", NormalizedName = "MENTOR" });
                Roles.Add(new IdentityRole { Name = "Director", NormalizedName = "DIRECTOR" });
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<InstitutionEntityModel>().HasMany(s => s.Students).WithOne(i => i.Institution).OnDelete(DeleteBehavior.SetNull);

        }
    }
}
