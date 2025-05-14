using ChatEmail.Context;
using ChatEmail.Entities;
using ChatEmail.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace ChatEmail.ViewComponents
{
    public class _MessageBodyComponentPartial:ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ChatContext _ChatContext;

        public _MessageBodyComponentPartial(UserManager<AppUser> userManager, ChatContext ChatContext)
        {
            _userManager = userManager;
            _ChatContext = ChatContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var receiverCount = _ChatContext.Messages.Count(x => x.ReceiverEmail == user.Email && x.IsRead == true);
            var senderCount = _ChatContext.Messages.Count(x => x.SenderEmail == user.Email && x.IsRead==true);
            var model = new MessageCountViewModel
            {
                ReceiveMessage = receiverCount,
                SendMessage = senderCount,
            };
            return View(model);
           

           
        }
    }
    
}
