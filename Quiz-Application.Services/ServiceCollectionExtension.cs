using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quiz_Application.Services.Repository.Interfaces;
using Quiz_Application.Services.Repository.Base;
using Business.Abstract;
using Business.Concrete;

namespace Quiz_Application.Services
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICandidate<Entities.Candidate>, CandidateService<Entities.Candidate>>();
            services.AddScoped<IExam<Entities.Exam>, ExamService<Entities.Exam>>();
            services.AddScoped<IQuestion<Entities.Question>, QuestionService<Entities.Question>>();
            services.AddScoped<IResult<Entities.Result>, ResultService<Entities.Result>>();
            services.AddScoped<IAdminActionsService, AdminActionsManager>();
            services.AddScoped<IAuthService, AuthManager>();
            return services;
        }
    }
}
