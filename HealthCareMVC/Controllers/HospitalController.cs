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
            try
            {
                User user = null;
                if (Session["loggedUser"] != null)
                {
                    user = (User)Session["loggedUser"];
                    List<Hospital> hospitalList = new BusinessClass().GetHospitals(user.UserId);
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
            }
            catch (Exception ex)
            {
                new LogAndErrorsClass().CatchException(ex);
                return RedirectToAction("ErrorControl", "Home");
            }
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
                if (added > 0)
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
        public ActionResult ViewHospital(int id)
        {
            try
            {
                User user = null;
                if (Session["loggedUser"] != null)
                {
                    user = (User)Session["loggedUser"];
                    Hospital hospital = new BusinessClass().GetHospitalDetails(user.UserId, id);
                    if (hospital.status == -1)
                    {
                        TempData["errorMessage"] = "Please select a valid hospital from the list.";
                        return RedirectToAction("ViewAllHospitals", "Hospital");
                    }
                    else
                    {
                        ViewBag.IsPrimary = hospital.IsPrimary;
                        return View(hospital);
                    }
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

        [Route("EditHospital")]
        public ActionResult EditHospital(int id)
        {
            try
            {
                User user = null;
                if (Session["loggedUser"] != null)
                {
                    user = (User)Session["loggedUser"];
                    Hospital hospital = new BusinessClass().GetHospitalDetails(user.UserId, id);
                    if(hospital.IsPrimary == 1)
                    {
                        hospital.SetPrimary = true;
                    }
                    else
                    {
                        hospital.SetPrimary = false;
                    }
                    if (hospital.status == -1)
                    {
                        TempData["errorMessage"] = "Please select a valid hospital from the list.";
                        return RedirectToAction("ViewAllHospitals", "Hospital");
                    }
                    else
                    {
                        ViewBag.IsPrimary = hospital.IsPrimary;
                        return View(hospital);
                    }
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
        public ActionResult EditHospitalRequest(Hospital hospital)
        {
            try
            {
                hospital.UserId = ((User)Session["loggedUser"]).UserId;
                if (hospital.SetPrimary)
                {
                    hospital.IsPrimary = 1;
                }
                else
                {
                    hospital.IsPrimary = 0;
                }
                hospital = new BusinessClass().EditHospitalDetails(hospital);
                if (hospital.status > 0)
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

        public ActionResult DeleteHospital(int id)
        {
            try
            {
                int deleted = new BusinessClass().DeleteHospital(id);
                if (deleted > 0)
                {
                    TempData["successMessage"] = "Hospital record deleted.";
                    return RedirectToAction("ViewAllHospitals", "Hospital");
                }
                else
                {
                    TempData["errorMessage"] = "Some internal error occured. Please try again.";
                    return RedirectToAction("ViewAllHospitals", "Hospital");
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