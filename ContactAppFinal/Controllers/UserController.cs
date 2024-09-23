using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using ContactAppFinal.Data;
using ContactAppFinal.Models;
using System.Web.Security;
using System.Web.UI.WebControls;
using ContactAppFinal.ViewModels;

namespace ContactAppFinal.Controllers
{
    [AllowAnonymous]
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginVM loginVM)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var txt = session.BeginTransaction())
                {

                    var user = session.Query<User>().SingleOrDefault(u => u.UserName == loginVM.UserName);
                    if (user != null)
                    {
                        if (BCrypt.Net.BCrypt.Verify(loginVM.Password, user.Password))
                        {
                            if (user.IsActive == true) {

                                FormsAuthentication.SetAuthCookie(loginVM.UserName, true);
                                if(user.Role.RoleName == "Admin")
                                {
                                    return RedirectToAction("Index","Admin");
                                }
                                else
                                {
                                    return RedirectToAction("Index","Staff", new {userId = user.Id});
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("", "The User is Not Active");
                            }


                        }
                        else
                        {
                            ModelState.AddModelError("", "UserName/Password dosent match");
                        }
                    }
                   
                    return View();
                }
            }

        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }


    }
}