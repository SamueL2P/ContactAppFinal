
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ContactAppFinal.Data;
using ContactAppFinal.Models;

namespace ContactAppFinal.Controllers
{
    public class StaffController : Controller
    {
        // GET: Staff
        [HttpGet]
        public ActionResult Index(Guid userId)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var contacts = session.Query<Contact>().Where(c => c.User.Id == userId).ToList();
                Session["UserId"] = userId;  
                return View("Index", contacts);
            }
        }


        public ActionResult GetContactList(Guid userId)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var contacts = session.Query<Contact>().Where(c => c.User.Id == userId).ToList();

                if (contacts == null)
                {
                    contacts = new List<Contact>();
                }

                return PartialView("_ContactList", contacts);
            }
        }




        [HttpGet]
        public ActionResult CreateContact()
        {
            return PartialView("_CreateContact");
        }

        [HttpPost]
        public JsonResult CreateContact(Contact contact)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var userId = (Guid)Session["UserId"];
                    var user = session.Get<User>(userId);
                    contact.User = user;
                    session.Save(contact);
                    transaction.Commit();
                    return Json(new { success = true, message = "Contact created successfully!" });
                }
            }
        }

        [HttpGet]
        public ActionResult EditContact(Guid contactId)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var contact = session.Get<Contact>(contactId);
                if(contact.IsActive == false)
                {
                    return Json(new { success = false, message = "Contact not Active" }, JsonRequestBehavior.AllowGet);
                }
                return PartialView("_EditContact", contact);
            }
        }

        [HttpPost]
        public JsonResult EditContact(Contact contact)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var existingContact = session.Get<Contact>(contact.ContactId);
                    if (existingContact == null)
                    {
                        return Json(new { success = false, message = "Contact not found." }, JsonRequestBehavior.AllowGet);
                    }

                    existingContact.FName = contact.FName;
                    existingContact.LName = contact.LName;
                    existingContact.IsActive = contact.IsActive;
                    session.Update(existingContact);
                    transaction.Commit();
                    return Json(new { success = true, message = "Contact updated successfully!" }, JsonRequestBehavior.AllowGet);
                }
            }
        }


        [HttpPost]
        public JsonResult DeleteContact(Guid id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var contact = session.Get<Contact>(id);
                if (contact == null)
                {
                    return Json(new { success = false, message = "Contact not found." });
                }
                if (!contact.IsActive)
                {
                    return Json(new { success = false, message = "Contact Already InActive" });
                }
                using (var transaction = session.BeginTransaction())
                {
                    contact.IsActive = false;
                    session.Update(contact);
                    transaction.Commit();
                    return Json(new { success = true, message = "Contact deleted successfully!" });
                }
            }
        }

        [HttpGet]
        public ActionResult ManageContactDetails(Guid id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var contactdetails = session.Query<ContactDetail>().Where(c => c.Contact.ContactId == id).ToList();
                Session["ContactId"] = id;
                ViewBag.ContactId = id;  
                return View("ManageContactDetails");
            }
        }


        [HttpGet]
        public JsonResult GetData(Guid contactId, int page, int rows, string sidx, string sord, bool _search, string searchField, string searchString, string searchOper)
        {
            using (var session = NHibernateHelper.CreateSession())
            {

                var query = session.Query<ContactDetail>()
                                   .Where(c => c.Contact.ContactId == contactId);

               
                if (_search && !string.IsNullOrEmpty(searchField) && !string.IsNullOrEmpty(searchString))
                {
                    switch (searchField)
                    {
                        case "Type":
                            if (searchOper == "eq")  
                                query = query.Where(c => c.Type == searchString);
                            else if (searchOper == "cn") 
                                query = query.Where(c => c.Type.Contains(searchString));
                            break;
                        case "Value":
                            if (searchOper == "eq")
                                query = query.Where(c => c.Value == searchString);
                            else if (searchOper == "cn")
                                query = query.Where(c => c.Value.Contains(searchString));
                            break;
                    }
                }

                int totalCount = query.Count();

      
                switch (sidx)
                {
                    case "Type":
                        query = sord == "asc" ? query.OrderBy(c => c.Type) : query.OrderByDescending(c => c.Type);
                        break;
                    case "Value":
                        query = sord == "asc" ? query.OrderBy(c => c.Value) : query.OrderByDescending(c => c.Value);
                        break;
                    default:
                        query = sord == "asc" ? query.OrderBy(c => c.ContactDetailId) : query.OrderByDescending(c => c.ContactDetailId);
                        break;
                }

                int totalPages = (int)Math.Ceiling((double)totalCount / rows);

             
                var contactDetails = query.Skip((page - 1) * rows).Take(rows).ToList();

              
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalCount,
                    rows = contactDetails.Select(c => new
                    {
                        c.ContactDetailId,
                        c.Type,
                        c.Value
                    }).ToArray()
                };

                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Add(ContactDetail contactDetail)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var contactId = (Guid)Session["ContactId"];
                    var contact = session.Get<Contact>(contactId);
                    contactDetail.Contact = contact;

                    session.Save(contactDetail);
                    transaction.Commit();
                    return Json(new { success = true, message = "ContactDetail added successfully!" });
                }
            }
        }

        [HttpPost]
        public JsonResult Edit(ContactDetail contactDetail)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {

                    var existingDetail = session.Get<ContactDetail>(contactDetail.ContactDetailId);
                    if (existingDetail == null)
                    {
                        return Json(new { success = false, message = "ContactDetail not found." });
                    }

                    existingDetail.Type = contactDetail.Type;
                    existingDetail.Value = contactDetail.Value;
                    session.Update(existingDetail);
                    transaction.Commit();
                    return Json(new { success = true, message = "ContactDetail updated successfully!" });
                }
            }
        }


        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var contactDetail = session.Get<ContactDetail>(id);
                if (contactDetail == null)
                {
                    return Json(new { success = false, message = "ContactDetail not found." });
                }

                using (var transaction = session.BeginTransaction())
                {
                    session.Delete(contactDetail);
                    transaction.Commit();
                    return Json(new { success = true, message = "ContactDetail deleted successfully!" });
                }
            }
        }

        [HttpPost]
        public JsonResult ToggleIsActive(Guid contactId, bool isActive)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var contact = session.Get<Contact>(contactId);
                    if (contact == null)
                    {
                        return Json(new { success = false, message = "Contact not found." });
                    }

                    contact.IsActive = isActive;
                    session.Update(contact);
                    transaction.Commit();

                    return Json(new { success = true, message = "Contact status updated successfully!" });
                }
            }
        }


    }
}