using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Quiz_Application.Web.Models;
using System;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Quiz_Application.Web.Extensions;
using Quiz_Application.Services.Entities;
using Quiz_Application.Web.Authentication;
using Quiz_Application.Services.Repository.Interfaces;
using Quiz_Application.Web.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Quiz_Application.Web.Controllers
{
    [Authorize(Roles = "candidate")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICandidate<Services.Entities.Candidate> _candidate;

        public HomeController(ILogger<HomeController> logger, ICandidate<Services.Entities.Candidate> candidate)
        {
            _logger = logger;
            _candidate = candidate;
        }

        public async Task<IActionResult> Index()
        {

            var identity = HttpContext.User.Identity.Name;
            IQueryable<Candidate> iqCandidate = await _candidate.SearchCandidate(e => e.UserName.Equals(identity));
            Candidate objCandidate = iqCandidate.FirstOrDefault();
            return View(objCandidate);
        }       


        public async Task<IActionResult> Profile()
        {
            var identity = HttpContext.User.Identity.Name;
            IQueryable<Candidate> iqCandidate = await _candidate.SearchCandidate(e => e.UserName.Equals(identity));
            Candidate objCandidate = iqCandidate.FirstOrDefault();

            ProfileViewModel objModel = new ProfileViewModel()
            {
                Sl_No = objCandidate.Sl_No,
                Name = objCandidate.Name,
                Candidate_ID = objCandidate.Candidate_ID,
                Email = objCandidate.Email,
                Phone = objCandidate.PhoneNumber,
                ImgFile = objCandidate.ImgFile!=null ? objCandidate.ImgFile:null
            };                   
            return View(objModel);
        }

        [HttpPost]
        public async Task<IActionResult> Profile([FromForm] ProfileViewModel argObj)
        {
            int i = 0;
            string UploadFolder = null;
            string UniqueFileName = null;
            string UploadPath = null;
            if (ModelState.IsValid)
            {                
                try
                {
                    if (argObj.file != null)
                    {
                        UploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles/Image");
                        UniqueFileName = Guid.NewGuid().ToString() + "_" + argObj.file.FileName;
                        UploadPath = Path.Combine(UploadFolder, UniqueFileName);
                    }
                    Candidate _objCandidate = await _candidate.GetCandidate(argObj.Sl_No);       
                    _objCandidate.Name = argObj.Name;
                    _objCandidate.Candidate_ID = argObj.Candidate_ID;
                    _objCandidate.PhoneNumber = argObj.Phone;
                    _objCandidate.Email = argObj.Email;
                    if (UniqueFileName != null)
                    { _objCandidate.ImgFile = UniqueFileName; }
                    else
                    { _objCandidate.ImgFile = _objCandidate.ImgFile; }
                    argObj.ImgFile = _objCandidate.ImgFile;
                    i = await _candidate.UpdateCandidate(_objCandidate);
                    if (i > 0)
                    {
                        if (argObj.file != null)
                        {
                        await argObj.file.CopyToAsync(new FileStream(UploadPath, FileMode.Create));
                        }                        
                        ViewBag.Alert = AlertExtension.ShowAlert(Alerts.Success, "Profile updated successfully.");
                    }
                    else                    
                        ViewBag.Alert = AlertExtension.ShowAlert(Alerts.Danger, "An error occurred.");                    
                }
                catch (Exception ex)
                {
                    ViewBag.Alert = AlertExtension.ShowAlert(Alerts.Danger, ex.Message);
                    throw new Exception(ex.Message, ex.InnerException);
                }
            }
            else
                ModelState.AddModelError("Error","Unknown  Error.");
            
            return View(argObj);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> SaveRecoredFile()
        {
            if (Request.Form.Files.Any())
            {
                var file = Request.Form.Files["video-blob"];
                string UploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles/Video");
                string UniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName + ".webm";
                string UploadPath = Path.Combine(UploadFolder, UniqueFileName);
                await file.CopyToAsync(new FileStream(UploadPath, FileMode.Create));
            }
            return Json(HttpStatusCode.OK);
        }

    }
}
