using Microsoft.AspNetCore.Mvc;

namespace ChatEmail.ViewComponents
{
    public class _MessageFooterComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
