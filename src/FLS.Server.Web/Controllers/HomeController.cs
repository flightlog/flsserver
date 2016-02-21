using System.Web.Mvc;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// MVC Home controller
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Title = "Flight Logging System";

            return View();
        }

        public ActionResult Test()
        {
            return View();
        }
    }
}
