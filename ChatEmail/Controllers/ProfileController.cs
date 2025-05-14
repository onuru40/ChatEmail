
using ChatEmail.Context;
using ChatEmail.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CoreEmailProject.Controllers
{
    public class UserController : Controller
    {
        private readonly ChatContext _ChatContext;
        private readonly UserManager<AppUser> _userManager;

        public UserController(ChatContext ChatContext, UserManager<AppUser> userManager)
        {
            _ChatContext = ChatContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Profile()
        {
            var values = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.ProfilePicture = values.ProfileImageUrl;
            ViewBag.Name = values.Name;
            ViewBag.Surname = values.Surname;
            ViewBag.Email = values.Email;
            ViewBag.Username = values.UserName;
            ViewBag.RecipientEmailAddressCount = _ChatContext.Messages
                                    .Where(x => x.ReceiverEmail == values.Email)
                                    .Count();

            ViewBag.SenderEmailAddressCount = _ChatContext.Messages
                                                 .Where(x => x.SenderEmail == values.Email)
                                                 .Count();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AppUser updatedUser, string ProfileImageUrl, string newPassword)
        {
            var user = await _userManager.FindByNameAsync(User.Identity?.Name);
            if (user == null)
            {
                return NotFound();
            }

            user.Name = updatedUser.Name;
            user.Surname = updatedUser.Surname;
            user.UserName = updatedUser.UserName;
            user.Email = updatedUser.Email;

            if (!string.IsNullOrEmpty(ProfileImageUrl))
            {
                user.ProfileImageUrl = ProfileImageUrl;
            }

            if (!string.IsNullOrEmpty(newPassword))
            {
                var hasPassword = await _userManager.HasPasswordAsync(user);
                if (hasPassword)
                {
                    var removeResult = await _userManager.RemovePasswordAsync(user);
                    if (!removeResult.Succeeded)
                    {
                        foreach (var error in removeResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View("Profile", user);
                    }
                }

                var addResult = await _userManager.AddPasswordAsync(user, newPassword);
                if (!addResult.Succeeded)
                {
                    foreach (var error in addResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View("Profile", user);
                }
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Profil başarıyla güncellendi!";
                return RedirectToAction("UserLogin", "Login");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View("Profile", user);
            }
        }
    }
}