using Microsoft.AspNetCore.Mvc;

namespace ChatEmail.ViewComponents
{
    public class _MessageHeadComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
