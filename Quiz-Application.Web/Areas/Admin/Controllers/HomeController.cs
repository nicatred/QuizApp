using Business.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quiz_Application.Services.Dtos;
using Quiz_Application.Services.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quiz_Application.Web.Areas.Admin.Controllers
{

    [Area("admin")]
    [Authorize(Roles = "admin")]
    public class HomeController : Controller
    {
        private readonly IAdminActionsService _adminActionsService;

        public HomeController(IAdminActionsService adminActionsService)
        {
            _adminActionsService = adminActionsService;
        }

        public async Task<IActionResult> Index()
        {
           var list= _adminActionsService.GetQuestionAndChoises();
            return View(list);
        }

        public async Task<IActionResult> AddQuestion(QuestionFormDto questionFormDto)
        {
            await _adminActionsService.AddQuestion(questionFormDto);
            return RedirectToAction("Index");
        }
        //[HttpGet("AcceptRequest/{vacationRequestId}")]
        //public async Task<IActionResult> AcceptRequest(int vacationRequestId)
        //{
        //    var list = await _adminActionsService.AcceptRequest(vacationRequestId);
        //    if (list.Success)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    return UnprocessableEntity(list.Message);
        //}
        //[HttpGet("RejectRequest/{vacationRequestId}")]

        //public async Task<IActionResult> RejectRequest(int vacationRequestId)
        //{
        //    var list = await _adminActionsService.RejectRequest(vacationRequestId);
        //    if (list.Success)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    return UnprocessableEntity(list.Message);
        //}
    }
}
