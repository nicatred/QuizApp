using Quiz_Application.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Application.Services.Dtos
{
    public class QuestionAndChoises
    {
        public int QuestionId { get; set; }
        public string Question { get; set; }
        public List<Choice> Choices { get; set; }
        public int AnsweId { get; set; }
    }
}
