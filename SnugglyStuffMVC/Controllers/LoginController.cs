using SnugglyStuffMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SnugglyStuffMVC.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(tblUser model)
        {
            try
            {
                if (model.Email == "Admin" && model.Password == "Admin")
                {
                    FormsAuthentication.SetAuthCookie(model.Email, false);

                    return Redirect("~/Items/Index");
                }
                
                ModelState.AddModelError("", "Invalid user/pass");
                return View();
            }
            catch(Exception e)
            {
                ModelState.AddModelError("", "Invalid user/pass");
                return View();
            }
        }
    }
}