using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuSolver.Database.Entities;

namespace SudokuSolver.Database.Context
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext() : base() { }
        public DbSet<User> Users { get; set; }
        public DbSet<SavingSudoku> SavingSudokus { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"
                Data Source=DESKTOP-BH18TPA\SQLEXPRESS;
                Initial Catalog=sudokudatabase;
                Integrated Security=True;
                Connect Timeout=30;
                Encrypt=False;
                Trust Server Certificate=False;
                Application Intent=ReadWrite;
                Multi Subnet Failover=False"
            );
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SavingSudoku>().HasOne(s => s.User).WithMany(u => u.SavingSudokus).HasForeignKey(s => s.IdUser);
        }
    }
}
