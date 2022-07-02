using Business.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quiz_Application.Services.Dtos;
using Quiz_Application.Services.Entities;
using Quiz_Application.Services.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quiz_Application.Web.Areas.Admin.Controllers
{

    [Area("admin")]
    [Authorize(Roles = "admin,teacher")]
    public class HomeController : Controller
    {
        private readonly IAdminActionsService _adminActionsService;
        private readonly ICandidate<Candidate> _candidate;

        public HomeController(IAdminActionsService adminActionsService, ICandidate<Candidate> candidate)
        {
            _adminActionsService = adminActionsService;
            _candidate = candidate;
        }

        public async Task<IActionResult> Index()
        {
           var list= _adminActionsService.GetQuestionAndChoises();
            return View(list);
        }
        [HttpPost("AddQuestion")]
        public async Task<IActionResult> AddQuestion(NewQuestion  newQuestion)
        {
            var identity = HttpContext.User.Identity.Name;
            IQueryable<Candidate> iqCandidate = await _candidate.SearchCandidate(e => e.UserName.Equals(identity));
            Candidate _objCandidate = iqCandidate.FirstOrDefault();
            newQuestion.Sl_No = _objCandidate.Sl_No;
            await _adminActionsService.AddQuestion(newQuestion);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ChangeQuestion(int Id)
        {
            var question = (await _adminActionsService.GetQuestionAndChoises()).Where(x=>x.QuestionId==Id).FirstOrDefault();
            return View("FormQuestion", question);
        }
        [HttpGet("AddQuestion")]
        public async Task<IActionResult> AddQuestion()
        {
            NewQuestion question = new NewQuestion();
            question.Exams = await _adminActionsService.GetExams();
            return View("AddQuestion",question);
        }

        public async Task<IActionResult> DeleteQuestion(int id)
        {
            await _adminActionsService.DeleteQuestion(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SaveChangedQuestion(QuestionAndChoises questionAndChoises)
        {
            await _adminActionsService.Edit(questionAndChoises);
            return RedirectToAction("Index");
        }
        [HttpPost("AddExam")]
        public async Task<IActionResult> AddExam(Exam exam)
        {
            await _adminActionsService.AddExam(exam);
            return RedirectToAction("Index");
        }
        [HttpGet("AddExam")]
        public async Task<IActionResult> AddExam()
        {
            Exam exam = new Exam();
            return View("_AddExam",exam);
        }
    }
}
