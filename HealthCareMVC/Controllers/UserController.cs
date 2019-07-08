using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModels;
using BusinessLayer;
using LogsAndError;

namespace HealthCareMVC.Controllers
{
    public class UserController : Controller
    {
        [Route("UserHome")]
        public ActionResult UserHome()
        {
            try
            {
                if (Session["loggedUser"] == null && Session["inactiveUser"] == null)
                {
                    TempData["errorMessage"] = "You have to login first.";
                    return RedirectToAction("Login", "Home");
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                new LogAndErrorsClass().CatchException(ex);
                return RedirectToAction("ErrorControl", "Home");
            }
        }

        public ActionResult ConfirmRegistration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ConfirmRegistrationRequest(FormCollection form)
        {
            try
            {
                if (Session["loggedUser"] != null)
                {
                    TempData["errorMessage"] = "You are already an active user.";
                    return RedirectToAction("UserHome", "User");
                }
                else if (Session["inactiveUser"] == null)
                {
                    TempData["errorMessage"] = "Some error occured.";
                    return RedirectToAction("UserHome", "User");
                }
                else
                {
                    User user = (User)Session["inactiveUser"];
                    user = new BusinessClass().GetCode(user, user.UserId);
                    if (form["txtSecurityCode"].Equals(user.SecurityCode))
                    {
                        int activated = new BusinessClass().ActivateUser(user);
                        if (activated > 0)
                        {
                            TempData["successMessage"] = "Your account is now activated.";
                            return RedirectToAction("UserHome", "User");
                        }
                        else
                        {
                            TempData["errorMessage"] = "Some internal error occured. Please try again.";
                            return RedirectToAction("ConfirmRegistration", "User");
                        }
                    }
                    else
                    {
                        TempData["errorMessage"] = "Invalid security code. Please try again.";
                        return RedirectToAction("ConfirmRegistration", "User");
                    }
                }
            }
            catch (Exception ex)
            {
                new LogAndErrorsClass().CatchException(ex);
                return RedirectToAction("ErrorControl", "Home");
            }
        }

        [Route("ViewProfile")]
        public ActionResult ViewProfile()
        {
            return View();
        }

        [Route("EditProfile")]
        public ActionResult EditProfile()
        {
            return View();
        }

        [Route("ChangePassword")]
        public ActionResult ChangePassword()
        {
            return View();
        }
    }
}