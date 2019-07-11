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
    public class DoctorController : Controller
    {
        [Route("ViewDoctors")]
        public ActionResult ViewAllDoctors()
        {
            return View();
        }

        [Route("AddNewDoctor")]
        public ActionResult AddNewDoctor()
        {
            try
            {
                User user = null;
                if (Session["loggedUser"] != null)
                {
                    user = (User)Session["loggedUser"];
                    List<Hospital> hospitals = new BusinessClass().GetHospitals(user.UserId);
                    List<Speciality> specialities = new BusinessClass().GetSpecialities();
                    ViewBag.hospitals = hospitals;
                    ViewBag.specialities = specialities;
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
        public ActionResult AddNewDoctorRequest()
        {
            return View();
        }

        [Route("ViewDoctorDetails")]
        public ActionResult ViewDoctor()
        {
            return View();
        }

        [Route("EditDoctor")]
        public ActionResult EditDoctor()
        {
            return View();
        }
        public ActionResult EditDoctorRequest()
        {
            return View();
        }
        public ActionResult DeleteDoctor()
        {
            return View();
        }
    }
}