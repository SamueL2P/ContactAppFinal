using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContactAppFinal.Data;
using ContactAppFinal.Models;

namespace ContactAppFinal.Controllers
{
    public class AdminController : Controller
    {   
        // GET: Admin
        public ActionResult Index()
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var currentUser = User.Identity.Name;

                var data = session.Query<User>().Where(u => u.IsAdmin == false && u.UserName != currentUser).ToList();
                return View(data);
            }
            
        }

        public  ActionResult ViewAdmins()
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var currentUser = User.Identity.Name;
                var data = session.Query<User>().Where(u => u.IsAdmin == true && u.UserName != currentUser).ToList();

                return View(data);
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]

        [HttpPost]
        public ActionResult Create(User user)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var txt = session.BeginTransaction())
                {
                   
                   user.Role.User = user;
                   if(user.IsAdmin == true)
                    {
                        user.Role.RoleName = "Admin";
                    }
                    else
                    {
                        user.Role.RoleName = "Staff";
                    }
                    
                    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                    user.IsActive = true;
                    session.Save(user);
                    txt.Commit();
                    return RedirectToAction("Index");
                }
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(Guid id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var user = session.Get<User>(id);

                if (user == null)
                {
                    return HttpNotFound();
                }
                if (!user.IsActive)
                {
                    TempData["ErrorMessage"] = "Cannot update as the user is not active.";
                    return RedirectToAction("Index");
                }

                return View(user);
            }
        }

        // POST: Admin/Edit/{id}
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(User user)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var txt = session.BeginTransaction())
                {
                    var existingUser = session.Get<User>(user.Id);

                    
                    if (existingUser == null)
                    {
                        return HttpNotFound();
                    }
                    if (!existingUser.IsActive)
                    {
                        TempData["ErrorMessage"] = "Cannot update as the user is not active.";
                        return RedirectToAction("Index");
                    }


                    existingUser.UserName = user.UserName;
                    existingUser.FName = user.FName;
                    existingUser.LName = user.LName;

                    if (!string.IsNullOrEmpty(user.Password))
                    {
                        existingUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                    }

                    existingUser.IsAdmin = user.IsAdmin;
                    existingUser.IsActive = user.IsActive;

                    var role = session.Query<Role>().FirstOrDefault(r => r.RoleName == user.Role.RoleName);
                    if (role == null)
                    {
                        role = new Role { RoleName = user.Role.RoleName };
                        session.Save(role);
                    }
                    existingUser.Role = role;

                    session.Update(existingUser);
                    txt.Commit();

                    return RedirectToAction("Index");
                }
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult Delete(Guid userId)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var txt = session.BeginTransaction())
                {
                    var user = session.Get<User>(userId);

                    if (user == null)
                    {
                        return Json(new { success = false, message = "User not found." });
                    }

          
                    user.IsActive = !user.IsActive;
                    session.Update(user);
                    txt.Commit();

                    return Json(new { success = true });
                }
            }
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult ToggleIsActive(Guid userId, bool isActive)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    var user = session.Get<User>(userId);

                    if (user == null)
                    {
                        return Json(new { success = false, message = "User not found." });
                    }

                    user.IsActive = isActive;
                    session.Update(user);
                    tx.Commit();

                    return Json(new { success = true });
                }
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult ToggleIsAdmin(Guid userId, bool isAdmin)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    var user = session.Get<User>(userId);

                    if (user == null)
                    {
                        return Json(new { success = false, message = "User not found." });
                    }

                    user.IsAdmin = isAdmin;
                    session.Update(user);
                    tx.Commit();

                    return Json(new { success = true });
                }
            }
        }


    }
}