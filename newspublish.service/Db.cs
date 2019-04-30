using Microsoft.EntityFrameworkCore;
using newspublish.mode;
using newspublish.mode.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace newspublish.service
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    public class Db:DbContext
    {
       public Db()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            string connstring = "database=newspublish;Password=123456;User ID=root;server=localhost;pooling=false;CharSet=utf8;port=33316;Allow User Variables=True";
            optionsBuilder.UseMySql(connstring);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Banner> Banner { get; set; }
        public virtual DbSet<News>News { get; set; }
        public virtual DbSet<Newsclassify> Newsclassify { get; set; }
        public virtual DbSet<Newscomment> Newscomment { get; set; }
    }
}
