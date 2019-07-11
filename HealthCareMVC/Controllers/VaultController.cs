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
    public class VaultController : Controller
    {
        [Route("MyDocuments")]
        public ActionResult ViewAllDocuments()
        {
            return View();
        }

        [Route("AddNewDocument")]
        public ActionResult AddNewDocument()
        {
            try
            {
                User user = null;
                if (Session["loggedUser"] != null)
                {
                    user = (User)Session["loggedUser"];
                    List<Hospital> hospitals = new BusinessClass().GetHospitals(user.UserId);
                    List<RecordType> recordTypes = new BusinessClass().GetRecordTypes();
                    ViewBag.hospitals = hospitals;
                    ViewBag.recordTypes = recordTypes;
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
        public ActionResult AddNewDocumentRequest()
        {
            return View();
        }

        public ActionResult DeleteDocument()
        {
            return View();
        }

        public JsonResult GetDoctorsByHospital(String hospitalId)
        {
            int id = Convert.ToInt32(hospitalId);
            List<Doctor> doctorList = new BusinessClass().GetDoctorsByHospital(id);
            var jsonData = JsonConvert.SerializeObject(doctorList);
            return Json(jsonData);
        }
    }
}