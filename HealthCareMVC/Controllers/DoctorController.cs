using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            return View();
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