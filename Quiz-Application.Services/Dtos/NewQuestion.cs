using Quiz_Application.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Application.Services.Dtos
{
    public class NewQuestion
    {
        public string Question { get; set; }
        public string[] Choices { get; set; }
        public int AnswerId { get; set; }
        public int ExamId { get; set; }
        public int Sl_No { get; set; }
        public List<Exam> Exams { get; set; }
        public Exam Exam { get; set; }
    }
}
