using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogsAndError;
using DataModels;
using BusinessLayer;

namespace HealthCareMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Route("Login")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginRequest(FormCollection form)
        {
            try
            {
                User user = new User();
                user.Email = form["txtEmail"];
                user.Password = form["txtPassword"];
                user = new BusinessClass().Login(user.Email, user.Password);
                if (user.status == 1)
                {
                    if (user.IsActive)
                    {
                        Session["loggedUser"] = user;
                        Session["inactiveUser"] = null;
                        return RedirectToAction("UserHome", "User");
                    }
                    else
                    {
                        Session["inactiveUser"] = user;
                        TempData["errorMessage"] = "Please Activate your account to use further.";
                        return RedirectToAction("ConfirmRegistration", "User");
                    }
                }
                else
                {
                    TempData["errorMessage"] = "Username or password is invalid.";
                    return RedirectToAction("Login", "Home");
                }
            }
            catch (Exception ex)
            {
                new LogAndErrorsClass().CatchException(ex);
                return RedirectToAction("ErrorControl", "Home");
            }
        }

        [Route("Register")]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterRequest(FormCollection form)
        {
            try
            {
                User user = new User();
                user.FirstName = form["txtFirstName"];
                user.LastName = form["txtLastName"];
                user.Email = form["txtEmail"];
                user.Password = form["txtPassword"];
                user = new BusinessClass().CreateUser(user);
                if (user.status != -1)
                {
                    Session["inactiveUser"] = user;
                    TempData["successMessage"] = "Successfully signed up.";
                    return RedirectToAction("ConfirmRegistration", "User");
                }
                else
                {
                    TempData["errorMessage"] = "Some error occured. Please try again.";
                    return RedirectToAction("Register", "Home");
                }
            }
            catch (Exception ex)
            {
                new LogAndErrorsClass().CatchException(ex);
                return RedirectToAction("ErrorControl", "Home");
            }
        }

        [Route("ForgotPassword")]
        [HttpPost]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [Route("Logout")]
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ErrorControl()
        {
            return View();
        }
    }
}