using Microsoft.AspNetCore.Mvc;

namespace ChatEmail.ViewComponents
{
    public class _MessageHeaderComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
