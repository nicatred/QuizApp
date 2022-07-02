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
        private readonly QuizDBContext _dbContext;
        private DbSet<Question> _dbSetQuestion;
        private DbSet<Choice> _dbSetChoice;
        private DbSet<Answer> _dbSetAnswer;
        public AdminActionsManager(QuizDBContext dbContext)
        {
            _dbContext = dbContext;
            _dbSetQuestion = dbContext.Set<Question>();
            _dbSetChoice = dbContext.Set<Choice>();
            _dbSetAnswer = dbContext.Set<Answer>();

        }

        public async Task AddQuestion(NewQuestion entity)
        {
            Question question = new Question()
            {
                ExamID = entity.ExamId,
                DisplayText = entity.Question,
                CreatedBy = "Admin",
                CreatedOn = DateTime.Now,
                IsDeleted = false
            };
            _dbSetQuestion.Add(question);
            _dbContext.SaveChanges();

            int lastQuestionId = _dbSetQuestion.OrderByDescending(x => x.QuestionID).FirstOrDefault().QuestionID;

            for (int i = 0; i < 4; i++)
            {
                Choice choice = new Choice()
                {
                    DisplayText = entity.Choices[i],
                    CreatedBy = "Admin",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                    QuestionID = lastQuestionId
                };
                _dbSetChoice.Add(choice);
                _dbContext.SaveChanges();
                if (i == entity.AnswerId)
                {
                    var answerChoice = _dbSetChoice.OrderByDescending(x => x.CreatedOn).FirstOrDefault(x => x.QuestionID == lastQuestionId);
                    Answer answer = new Answer
                    {
                        Sl_No = entity.Sl_No,
                        ChoiceID = answerChoice.ChoiceID,
                        QuestionID = lastQuestionId,
                        CreatedBy = entity.Sl_No.ToString(),
                        CreatedOn = DateTime.Now,
                        DisplayText = answerChoice.DisplayText
                    };
                    _dbSetAnswer.Add(answer);
                }

                _dbContext.SaveChanges();
            }
        }

        public async Task DeleteQuestion(int questionId)
        {

            var choises = await _dbContext.Choice.Where(x => x.QuestionID == questionId).ToListAsync();
            _dbContext.Choice.RemoveRange(choises);
            await _dbContext.SaveChangesAsync();
            var deletedQuestion = await _dbContext.Question.FirstOrDefaultAsync(x => x.QuestionID == questionId);
            _dbSetQuestion.Remove(deletedQuestion);
            await _dbContext.SaveChangesAsync();

        }

        public async Task Edit(QuestionAndChoises entity)
        {
            using (QuizDBContext context = new QuizDBContext())
            {
                var question = await context.Question.FirstOrDefaultAsync(x => x.QuestionID == entity.QuestionId);
                question.DisplayText = entity.Question;
                question.ModifiedOn = DateTime.Now;
                context.Set<Question>().Update(question);
                foreach (var item in entity.Choices)
                {
                    var choice = context.Set<Choice>().Update(item);
                }
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<Exam>> GetExams()
        {
            return await _dbContext.Exam.ToListAsync();
        }

        public async Task<List<QuestionAndChoises>> GetQuestionAndChoises()
        {
            List<QuestionAndChoises> questionAndChoisesList = new List<QuestionAndChoises>();
            var questions = await _dbSetQuestion.ToListAsync();
            foreach (var question in questions)
            {

                QuestionAndChoises questionAndChoises = new QuestionAndChoises();
                questionAndChoises.Choices = new List<Choice>();
                questionAndChoises.Question = question.DisplayText;
                questionAndChoises.QuestionId = question.QuestionID;

                var choises = _dbSetChoice.Where(x => x.QuestionID == question.QuestionID).ToList();

                foreach (var choice in choises)
                {
                    questionAndChoises.Choices.Add(choice);
                }
                questionAndChoises.AnsweId = (await _dbContext.Answer.Where(x => x.QuestionID == question.QuestionID).FirstOrDefaultAsync()).ChoiceID;
                questionAndChoisesList.Add(questionAndChoises);
            }
            return questionAndChoisesList;
        }

        public async Task AddExam(Exam exam)
        {
            exam.CreatedBy = "Admin";
            exam.CreatedOn = DateTime.Now;
            exam.Duration = 1.30m;
            exam.FullMarks = 10m;
            await  _dbContext.Exam.AddAsync(exam);
            await _dbContext.SaveChangesAsync();
        }
    }
}
