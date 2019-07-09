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
    public class HospitalController : Controller
    {
        [Route("ViewHospitals")]
        public ActionResult ViewAllHospitals()
        {
            //try
            //{
                User user = null;
                if (Session["loggedUser"] != null)
                {
                    user = (User)Session["loggedUser"];
                    List<Hospital> hospitalList= new BusinessClass().GetHospitals(user.UserId);
                    return View(hospitalList);
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
            //}
            //catch (Exception ex)
            //{
            //    new LogAndErrorsClass().CatchException(ex);
            //    return RedirectToAction("ErrorControl", "Home");
            //}
        }

        [Route("AddNewHospital")]
        public ActionResult AddNewHospital()
        {
            try
            {
                User user = null;
                if (Session["loggedUser"] != null)
                {
                    user = (User)Session["loggedUser"];
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
        public ActionResult AddNewHospitalRequest(Hospital hospital)
        {
            try
            {
                hospital.UserId = ((User)Session["loggedUser"]).UserId;
                if(hospital.SetPrimary)
                {
                    hospital.IsPrimary = 1;
                }
                else
                {
                    hospital.IsPrimary = 0;
                }
                int added = new BusinessClass().AddHospital(hospital);
                if (added != -1)
                {
                    TempData["successMessage"] = "New hospital added.";
                    return RedirectToAction("ViewAllHospitals", "Hospital");
                }
                else
                {
                    TempData["errorMessage"] = "Some internal error occured. Please try again.";
                    return RedirectToAction("AddNewHospital", "Hospital");
                }
            }
            catch (Exception ex)
            {
                new LogAndErrorsClass().CatchException(ex);
                return RedirectToAction("ErrorControl", "Home");
            }
        }

        [Route("ViewHospitalDetails")]
        public ActionResult ViewHospital()
        {
            return View();
        }

        [Route("EditHospital")]
        public ActionResult EditHospital()
        {
            return View();
        }
    }
}