using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
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
            try
            {
                User user = null;
                if (Session["loggedUser"] != null)
                {
                    user = (User)Session["loggedUser"];
                    List<Doctor> hospitalList = new BusinessClass().GetDoctors(user.UserId);
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
                    ViewBag.hospitals = new SelectList(hospitals, "HospitalId", "HospitalName");
                    ViewBag.specialities = new SelectList(specialities, "SpecialityId", "SpecialityName");
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
        public ActionResult AddNewDoctorRequest(Doctor doctor)
        {
            try
            {
                doctor.UserId = ((User)Session["loggedUser"]).UserId;
                if (doctor.SetPrimary)
                {
                    doctor.IsPrimary = 1;
                }
                else
                {
                    doctor.IsPrimary = 0;
                }
                doctor = new BusinessClass().AddDoctor(doctor);
                if (doctor.status > 0)
                {
                    TempData["successMessage"] = "New doctor added.";
                    return RedirectToAction("ViewAllDoctors", "Doctor");
                }
                else
                {
                    TempData["errorMessage"] = "Some internal error occured. Please try again.";
                    return RedirectToAction("AddNewDoctor", "Doctor");
                }
            }
            catch (Exception ex)
            {
                new LogAndErrorsClass().CatchException(ex);
                return RedirectToAction("ErrorControl", "Home");
            }
        }

        [Route("ViewDoctorDetails")]
        public ActionResult ViewDoctor(int id)
        {
            try
            {
                User user = null;
                if (Session["loggedUser"] != null)
                {
                    user = (User)Session["loggedUser"];
                    Doctor doctor = new BusinessClass().GetDoctorDetails(user.UserId, id);
                    if (doctor.status == -1)
                    {
                        TempData["errorMessage"] = "Please select a valid hospital from the list.";
                        return RedirectToAction("ViewAllDoctors", "Doctor");
                    }
                    else
                    {
                        ViewBag.IsPrimary = doctor.IsPrimary;
                        return View(doctor);
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

        [Route("EditDoctor")]
        public ActionResult EditDoctor(int id)
        {
            try
            {
                User user = null;
                if (Session["loggedUser"] != null)
                {
                    user = (User)Session["loggedUser"];
                    Doctor doctor = new BusinessClass().GetDoctorDetails(user.UserId, id);
                    if (doctor.status == -1)
                    {
                        TempData["errorMessage"] = "Please select a valid doctor from the list.";
                        return RedirectToAction("ViewAllDoctors", "Doctor");
                    }
                    else
                    {
                        if (doctor.IsPrimary == 1)
                        {
                            doctor.SetPrimary = true;
                        }
                        else
                        {
                            doctor.SetPrimary = false;
                        }
                        List<Hospital> hospitals = new BusinessClass().GetHospitals(user.UserId);
                        List<Speciality> specialities = new BusinessClass().GetSpecialities();
                        ViewBag.hospitals = new SelectList(hospitals, "HospitalId", "HospitalName");
                        ViewBag.specialities = new SelectList(specialities, "SpecialityId", "SpecialityName");
                        ViewBag.IsPrimary = doctor.IsPrimary;
                        return View(doctor);
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
        public ActionResult EditDoctorRequest(Doctor doctor)
        {
            try
            {
                doctor.UserId = ((User)Session["loggedUser"]).UserId;
                if (doctor.SetPrimary)
                {
                    doctor.IsPrimary = 1;
                }
                else
                {
                    doctor.IsPrimary = 0;
                }
                doctor = new BusinessClass().EditDoctor(doctor);
                if (doctor.status > 0)
                {
                    TempData["successMessage"] = "Doctor details updated.";
                    return RedirectToAction("ViewAllDoctors", "Doctor");
                }
                else
                {
                    TempData["errorMessage"] = "Some internal error occured. Please try again.";
                    return RedirectToAction("AddNewDoctor", "Doctor");
                }
            }
            catch (Exception ex)
            {
                new LogAndErrorsClass().CatchException(ex);
                return RedirectToAction("ErrorControl", "Home");
            }
        }
        public ActionResult DeleteDoctor(int id)
        {
            try
            {
                int deleted = new BusinessClass().DeleteDoctor(id);
                if (deleted > 0)
                {
                    TempData["successMessage"] = "Doctor record deleted.";
                    return RedirectToAction("ViewAllDoctors", "Doctor");
                }
                else
                {
                    TempData["errorMessage"] = "Some internal error occured. Please try again.";
                    return RedirectToAction("ViewAllDoctors", "Doctor");
                }
            }
            catch (Exception ex)
            {
                new LogAndErrorsClass().CatchException(ex);
                return RedirectToAction("ErrorControl", "Home");
            }
        }

        [Route("ViewDoctorsV2")]
        public ActionResult ViewAllDoctorsV2()
        {
            try
            {
                User user = null;
                if (Session["loggedUser"] != null)
                {
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
        public JsonResult GetAllDoctors()
        {
            User user = (User)Session["loggedUser"];
            List<Doctor> doctorList = new BusinessClass().GetDoctors(user.UserId);
            return Json(doctorList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDoctorDetails(String doctorId)
        {
            User user = (User)Session["loggedUser"];
            Doctor doctor = new BusinessClass().GetDoctorDetails(user.UserId, Convert.ToInt32(doctorId));
            return Json(doctor, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetHospitals()
        {
            User user = (User)Session["loggedUser"];
            List<Hospital> hospitals = new BusinessClass().GetHospitals(user.UserId);
            return Json(hospitals, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSpecialities()
        {
            List<Speciality> specialities = new BusinessClass().GetSpecialities();
            return Json(specialities, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteDoctorV2(String doctorId)
        {
            int status = new BusinessClass().DeleteDoctor(Convert.ToInt32(doctorId));
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddNewDoctorV2(String firstName, String lastName, String hospitalId, String specialityId, String address, String email, String contactNumber, String alternativeNumber, String isPrimary)
        {
            Doctor doctor = new Doctor()
            {
                UserId = ((User)Session["loggedUser"]).UserId,
                FirstName = firstName,
                LastName = lastName,
                HospitalId = Convert.ToInt32(hospitalId),
                Speciality = Convert.ToInt32(specialityId),
                Address = address,
                Email = email,
                Phone1 = contactNumber,
                Phone2 = alternativeNumber
            };
            if (Convert.ToInt32(isPrimary) == 1)
            {
                doctor.IsPrimary = 1;
            }
            else
            {
                doctor.IsPrimary = 0;
            }
            doctor = new BusinessClass().AddDoctor(doctor);
            return Json(doctor.status, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditDoctorV2(String doctorId, String firstName, String lastName, String hospitalId, String specialityId, String address, String email, String contactNumber, String alternativeNumber, String isPrimary)
        {
            Doctor doctor = new Doctor()
            {
                DoctorId = Convert.ToInt32(doctorId),
                UserId = ((User)Session["loggedUser"]).UserId,
                FirstName = firstName,
                LastName = lastName,
                HospitalId = Convert.ToInt32(hospitalId),
                Speciality = Convert.ToInt32(specialityId),
                Address = address,
                Email = email,
                Phone1 = contactNumber,
                Phone2 = alternativeNumber
            };
            if (Convert.ToInt32(isPrimary) == 1)
            {
                doctor.IsPrimary = 1;
            }
            else
            {
                doctor.IsPrimary = 0;
            }
            doctor = new BusinessClass().EditDoctor(doctor);
            return Json(doctor.status, JsonRequestBehavior.AllowGet);
        }
    }
}