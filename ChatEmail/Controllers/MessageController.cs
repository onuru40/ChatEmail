using ChatEmail.Context;
using ChatEmail.Entities;
using ChatEmail.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;

namespace ChatEmail.Controllers
{
    public class MessageController : Controller
    {
        private readonly ChatContext _ChatContext;
        private readonly UserManager<AppUser> _userManager;

        public MessageController(ChatContext ChatContext, UserManager<AppUser> userManager)
        {
            _ChatContext = ChatContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string search)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var userEmail = user.Email;
            ViewBag.email = userEmail;
            ViewBag.namesurname = user.Name + " " + user.Surname;
            ViewBag.SearchTerm = search;
            var messages = _ChatContext.Messages
                .Where(x => x.ReceiverEmail == userEmail && x.IsRead == true);

            if (!string.IsNullOrEmpty(search))
            {
                messages = messages.Where(x => x.Subject.ToLower().Contains(search.ToLower()));
            }

            return View(messages.ToList());
        }
        [HttpGet]
        public IActionResult NewMessage()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> NewMessage(Message message)
        {
           
            var values = await _userManager.FindByNameAsync(User.Identity.Name);
            string senderEmail = values.Email;
            string senderName=values.Name;

            message.SenderEmail = senderEmail;
            message.SenderName = senderName;
            message.IsRead = true;
            message.SendDate = DateTime.Now;
            _ChatContext.Messages.Add(message);
            _ChatContext.SaveChanges();
            ViewBag.Success = "Gönderim işlemi başarılı!";
            return View("~/Views/Message/NewMessage.cshtml");
        }
        public async Task<IActionResult> SendBox(string search)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            string userEmail = user.Email;
            ViewBag.email = userEmail;
            ViewBag.namesurname = user.Name + " " + user.Surname;
            ViewBag.SearchTerm = search;
            var sentEmails = _ChatContext.Messages
                .Where(x => x.SenderEmail == userEmail && x.IsRead == true);
            if (!string.IsNullOrEmpty(search))
            {
                string term = search.ToLower();
                sentEmails = sentEmails.Where(x =>
                    x.Subject.ToLower().Contains(term) ||
                    x.ReceiverEmail.ToLower().Contains(term) ||
                    x.MessageDetail.ToLower().Contains(term)
                );
            }
            return View(sentEmails.ToList());
        }
        public async Task<IActionResult> MessageDetail(int id)
        {
            var values = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.senderimage = values.ProfileImageUrl;
            var value = _ChatContext.Messages.FirstOrDefault(x => x.MessageId == id);
            return View(value);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeMessageStatus(List<int> MessageId)
        {
            foreach (var id in MessageId)
            {
                var value = await _ChatContext.Messages.FindAsync(id);
                if (value != null)
                {
                    value.IsRead = false;
                   
                }
            }
            await _ChatContext.SaveChangesAsync();
            return RedirectToAction("TrashBox");
        }

        public IActionResult TrashBox()
        {
            var deletedValues = _ChatContext.Messages.Where(x =>x.IsRead==false).ToList();
            return View(deletedValues);
        }
        public async Task<IActionResult> MessageCount()
        {
            var values = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.RecipientEmailAddressCount = _ChatContext.Messages
                         .Where(x => x.ReceiverEmail == values.Email)
                         .Count();

            ViewBag.SenderEmailAddressCount = _ChatContext.Messages
                                     .Where(x => x.SenderEmail == values.Email)
                                     .Count();
            return View();
        }

        

    }
}
