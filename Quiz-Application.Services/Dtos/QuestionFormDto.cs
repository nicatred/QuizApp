using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Application.Services.Dtos
{
    public class QuestionFormDto
    {
        public string Question { get; set; }
        public List<string> Choices { get; set; }
        public int AnswerId { get; set; }
        public int ExamId { get; set; }
    }
}
