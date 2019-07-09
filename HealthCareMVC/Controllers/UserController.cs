using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
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
            try
            {
                User user = null;
                if (Session["loggedUser"] != null)
                {
                    user = (User)Session["loggedUser"];
                    ViewBag.user = user;
                    return View();
                }
                else if (Session["inactiveUser"] != null)
                {
                    user = (User)Session["inactiveUser"];
                    TempData["errorMessage"] = "Your account is not activated yet.";
                    return RedirectToAction("ConfirmRegistration", "User");
                }
                else
                {
                    TempData["errorMessage"] = "You have to login first.";
                    return RedirectToAction("Login", "Home");
                }
            }
            catch (Exception ex)
            {
                new LogAndErrorsClass().CatchException(ex);
                return RedirectToAction("ErrorControl", "Home");
            }
        }

        [Route("EditProfile")]
        public ActionResult EditProfile()
        {
            try
            {
                User user = null;
                if (Session["loggedUser"] != null)
                {
                    user = (User)Session["loggedUser"];
                    ViewBag.user = user;
                    return View();
                }
                else if (Session["inactiveUser"] != null)
                {
                    user = (User)Session["inactiveUser"];
                    TempData["errorMessage"] = "Your account is not activated yet.";
                    return RedirectToAction("ConfirmRegistration", "User");
                }
                else
                {
                    TempData["errorMessage"] = "You have to login first.";
                    return RedirectToAction("Login", "Home");
                }
            }
            catch (Exception ex)
            {
                new LogAndErrorsClass().CatchException(ex);
                return RedirectToAction("ErrorControl", "Home");
            }
        }

        [HttpPost]
        public ActionResult EditProfileRequest(FormCollection form, HttpPostedFileBase txtProfile)
        {
            try
            {
                User user = (User)Session["loggedUser"];
                String extension = String.Empty;
                string receivedFileName = String.Empty;
                if (txtProfile != null)
                {
                    receivedFileName = Path.GetFileName(txtProfile.FileName);
                    extension = Path.GetExtension(txtProfile.FileName);
                    String path = Server.MapPath("~/files/profiles/");
                    txtProfile.SaveAs(path + "profile_" + user.UserId + extension);
                }
                String databasePath = @"/files/profiles/profile_" + user.UserId + extension;
                user.FirstName = form["txtFirstName"];
                user.LastName = form["txtLastName"];
                user.Phone = form["txtPhone"];
                user.Address = form["txtAddress"];
                if (receivedFileName.Equals(""))
                {
                    user.Profile = "/files/profiles/default-profile.png";
                }
                else
                {
                    user.Profile = databasePath;
                }
                user.Profile = databasePath;
                user = new BusinessClass().EditUserDetails(user);
                if (user.status == 1)
                {
                    Session["loggedUser"] = user;
                    TempData["successMessage"] = "Profile details updated.";
                    return RedirectToAction("ViewProfile", "User");
                }
                else
                {
                    TempData["errorMessage"] = "Some error occured. Please try again.";
                    return RedirectToAction("EditProfile", "User");
                }
            }
            catch (Exception ex)
            {
                new LogAndErrorsClass().CatchException(ex);
                return RedirectToAction("ErrorControl", "Home");
            }
        }

        [Route("ChangePassword")]
        public ActionResult ChangePassword()
        {
            try
            {
                User user = null;
                if (Session["loggedUser"] != null)
                {
                    user = (User)Session["loggedUser"];
                    ViewBag.user = user;
                    return View();
                }
                else if (Session["inactiveUser"] != null)
                {
                    user = (User)Session["inactiveUser"];
                    TempData["errorMessage"] = "Your account is not activated yet.";
                    return RedirectToAction("ConfirmRegistration", "User");
                }
                else
                {
                    TempData["errorMessage"] = "You have to login first.";
                    return RedirectToAction("Login", "Home");
                }
            }
            catch (Exception ex)
            {
                new LogAndErrorsClass().CatchException(ex);
                return RedirectToAction("ErrorControl", "Home");
            }
        }

        [HttpPost]
        public ActionResult ChangePasswordRequest(FormCollection form)
        {
            try
            {
                User user = (User)Session["loggedUser"];
                if(user.Password.Equals(form["txtCurrentPassword"].ToString()))
                {
                    if(form["txtNewPassword"].ToString().Equals(form["txtConfirmPassword"].ToString()))
                    {
                        user.Password = form["txtNewPassword"].ToString().Trim();
                        user = new BusinessClass().updatePassword(user);
                        if (user.status == 1)
                        {
                            Session["loggedUser"] = user;
                            TempData["successMessage"] = "Your Password is updated.";
                            return RedirectToAction("ViewProfile", "User");
                        }
                        else
                        {
                            TempData["errorMessage"] = "Some internal error occured. Please try again.";
                            return RedirectToAction("ChangePassword", "User");
                        }
                    }
                    else
                    {
                        TempData["errorMessage"] = "New passwords do not match.";
                        return RedirectToAction("ChangePassword", "User");
                    }
                }
                else
                {
                    TempData["errorMessage"] = "Current password is not right.";
                    return RedirectToAction("ChangePassword", "User");
                }
            }
            catch (Exception ex)
            {
                new LogAndErrorsClass().CatchException(ex);
                return RedirectToAction("ErrorControl", "Home");
            }
        }
    }
}