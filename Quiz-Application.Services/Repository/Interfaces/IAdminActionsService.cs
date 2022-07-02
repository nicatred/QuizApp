﻿using DataAccess.Dtos.Concrete;
using Quiz_Application.Services.Dtos;
using Quiz_Application.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Application.Services.Repository.Interfaces
{
    public interface IAdminActionsService
    {
        //Task<QuestionFormDto> GetExam(int id);
        //Task<IQueryable<QuestionFormDto>> SearchExam(Expression<Func<QuestionFormDto, bool>> search = null);
        Task AddQuestion(NewQuestion entity);
        Task Edit(QuestionAndChoises entity);
        Task DeleteQuestion(int questionId);
        Task<List<QuestionAndChoises>> GetQuestionAndChoises ();
        Task<List<Exam>> GetExams ();
        Task AddExam (Exam exam);

        //Task<IEnumerable<QuestionFormDto>> GetExamList();

    }
}
