using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Quiz_Application.Services.Entities;

namespace Quiz_Application.Services
{
    public class QuizDBContext : IdentityDbContext<Candidate>
    {
        public QuizDBContext(DbContextOptions<QuizDBContext> options) : base(options)
        {
        }
        public QuizDBContext()
        {

        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Data Source=COMPUTER\\NICATRED;Initial Catalog=ExamDB;Trusted_connection=true;");
        //    base.OnConfiguring(optionsBuilder);
        //}
        public virtual DbSet<Candidate> Candidate { get; set; }
        public virtual DbSet<Exam> Exam { get; set; }
        public virtual DbSet<Question> Question { get; set; }
        public virtual DbSet<Choice> Choice { get; set; }
        public virtual DbSet<Answer> Answer { get; set; }
        public virtual DbSet<Result> Result { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuizAttempt>(eb =>
            {
                eb.HasNoKey();
                eb.ToView(null);
            });

            modelBuilder.Entity<QuizReport>(eb =>
            {
                eb.HasNoKey();
                eb.ToView(null);
            });
            base.OnModelCreating(modelBuilder);
        }
       
    }
}
