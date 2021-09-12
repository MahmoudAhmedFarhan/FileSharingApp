using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FileSharingApp.Models;
using FileSharingApp.Data;
using System.Security.Claims;
using FileSharingApp.Helpers.Mail;
using System.Text;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;

namespace FileSharingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IMailHelper _mailHelper;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context,
            IMailHelper mailHelper)
        {
            _logger = logger;
            this._db = context;
            this._mailHelper = mailHelper;
        }

        public IActionResult Index()
        {
            var highestDownloads = _db.Uploads.OrderByDescending(u => u.DownloadCount)
                 .Select(u => new UploadViewModel
                 {
                     Id = u.Id,
                     FileName = u.FileName,
                     OriginalFileName = u.OriginalFileName,
                     ContentType = u.ContentType,
                     Size = u.Size,
                     UploadDate = u.UploadDate,
                     DownloadCount = u.DownloadCount
                 })
                .Take(3);
            ViewBag.Popular = highestDownloads;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

     

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }
        private string UserId
        {
            get
            {
                return User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                //AutoMapper .
                //Save 
                await _db.Contact.AddAsync(new Data.Contact
                {
                    Email = model.Email,
                    Message = model.Message,
                    Name = model.Name,
                    Subject = model.Subject,
                    UserId = UserId
                });
                await _db.SaveChangesAsync();
                TempData["Message"] = "Message has been sent successfully!.";

                //Send Mail
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<h1>File Sharing - Unread Message</h1>");
                sb.AppendFormat("Name : {0} ", model.Name);
                sb.AppendFormat("Email : {0}", model.Email);
                sb.AppendLine();
                sb.AppendFormat("Subject : {0} ", model.Subject);
                sb.AppendFormat("Message : {0} ", model.Message);

                _mailHelper.SendMail(new InputEmailMessage
                {
                    Subject = "You have unread Message",
                    Email = "info@siet.com",
                    Body = sb.ToString()
                });
                return RedirectToAction("Contact");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult About()
        {
            return View();
        }
        [HttpGet]
        public IActionResult SetCulture(string lang, string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(lang))
            {
                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(lang)),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });
            }

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }
            return RedirectToAction("Index");
        }


    }
}
