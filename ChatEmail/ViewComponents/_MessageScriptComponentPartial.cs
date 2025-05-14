using Microsoft.AspNetCore.Mvc;

namespace ChatEmail.ViewComponents
{
    public class _MessageScriptComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
