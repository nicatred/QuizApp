using AutoMapper;
using Business.Abstract;
using Business.Constants;
using DataAccess.Dtos.Concrete;
using Microsoft.EntityFrameworkCore;
using Quiz_Application.Services.Dtos;
using Quiz_Application.Services.Entities;
using Quiz_Application.Services.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Application.Services.Repository.Base
{
    public class AdminActionsManager : IAdminActionsService
    {
        public async Task AddQuestion(QuestionFormDto entity)
        {
            using (QuizDBContext context = new QuizDBContext())
            {
                Question question = new Question()
                {
                    ExamID = entity.ExamId,
                    DisplayText = entity.Question,
                    CreatedBy = "Admin",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false
                };
                await context.Question.AddAsync(question);
                await context.SaveChangesAsync();

                int lastQuestionId = (await context.Question.LastOrDefaultAsync()).QuestionID;

                List<Choice> newChoises = new List<Choice>();
                foreach (var item in entity.Choices)
                {
                    Choice choice = new Choice()
                    {
                        DisplayText = item,
                        CreatedBy = "Admin",
                        CreatedOn = DateTime.Now,
                        IsDeleted = false,
                        QuestionID = lastQuestionId
                    };
                    newChoises.Add(choice);
                }

                await context.Choice.AddRangeAsync(newChoises);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteQuestion(int questionId)
        {
            using (QuizDBContext context = new QuizDBContext())
            {
                var choises = await context.Choice.Where(x => x.QuestionID == questionId).ToListAsync();
                context.Choice.RemoveRange(choises);
                await context.SaveChangesAsync();
                var deletedQuestion = context.Question.FirstOrDefaultAsync(x => x.QuestionID == questionId);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<QuestionAndChoises>> GetQuestionAndChoises()
        {
           
            List<QuestionAndChoises> questionAndChoisesList = new List<QuestionAndChoises>();
            using (QuizDBContext context = new QuizDBContext())
            {

                var questions = await context.Question.ToListAsync();
                foreach (var question in questions)
                {
                    QuestionAndChoises questionAndChoises = new QuestionAndChoises();
                    questionAndChoises.Question = question.DisplayText;
                    questionAndChoises.QuestionId = question.QuestionID;
                    var choises = context.Choice.Where(x => x.QuestionID == question.QuestionID).ToList();
                    foreach (var choice in choises)
                    {
                        questionAndChoises.Choices.Add(choice);
                    }
                    questionAndChoisesList.Add(questionAndChoises);
                }
                return questionAndChoisesList;
            }
        }
    }
}
