using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
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
            try
            {
                User user = null;
                if (Session["loggedUser"] != null)
                {
                    user = (User)Session["loggedUser"];
                    if(TempData["filteredFiles"] == null)
                    {
                        FileData fileData = new BusinessClass().GetFiles(user.UserId);
                        ViewBag.files = fileData;
                    }
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
        public ActionResult AddNewDocumentRequest(FormCollection form, HttpPostedFileBase[] postedFiles)
        {
            try
            {
                int userid = ((User)Session["loggedUser"]).UserId;
                Document document = new Document();
                document.UserId = userid;
                document.HospitalId = Convert.ToInt32(form["ddlHospital"]);
                document.DoctorId = Convert.ToInt32(form["ddlDoctor"]);
                document.DocumentType = Convert.ToInt32(form["ddlDocumentType"]);
                document.IssueDate = form["txtDate"];

                String path = Server.MapPath(@"~/files/uploads/");
                String folder = path + document.UserId + @"/";
                List<String> ext = new List<string>() { ".png", ".jpg", ".jpeg", ".pdf", ".doc", ".docx" };
                List<String> allFiles = new List<string>();
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                bool fileStatus = true;
                if (postedFiles.Length > 0)
                {
                    foreach (HttpPostedFileBase file in postedFiles)
                    {
                        if (file != null)
                        {
                            String fileName = file.FileName;
                            string extension = Path.GetExtension(fileName);
                            if (ext.IndexOf(extension) < 0)
                            {
                                fileStatus = false;
                            }
                            else
                            {
                                String fileNameToSave = file.FileName.Replace("'", "''");
                                String randomPrefix = new BusinessClass().RandomAlphaNumericString();
                                file.SaveAs(folder + randomPrefix + "_" + fileNameToSave);
                                allFiles.Add(@"/files/uploads/" + document.UserId + @"/" + randomPrefix + "_" + fileNameToSave);
                            }
                        }
                    }
                    if (!fileStatus)
                    {
                        TempData["errorMessage"] = "Please select a valid file (png, jpg, jpeg, doc, docx or pdf only) to upload.";
                        return RedirectToAction("ViewAllDocuments", "Vault");
                    }
                }
                else
                {
                    TempData["errorMessage"] = "Please select a file to upload.";
                    return RedirectToAction("ViewAllDocuments", "Vault");
                }
                document = new BusinessClass().SaveDocument(document, allFiles);
                if (document.status != -1)
                {
                    TempData["successMessage"] = "Document uploaded successfully.";
                    return RedirectToAction("ViewAllDocuments", "Vault");
                }
                else
                {
                    TempData["errorMessage"] = "Some internal error occured. Please try again later.";
                    return RedirectToAction("ViewAllDocuments", "Vault");
                }
            }
            catch(Exception ex)
            {
                new LogAndErrorsClass().CatchException(ex);
                return RedirectToAction("ErrorControl", "Home");
            }
        }

        public ActionResult DeleteDocument(int id)
        {
            try
            {
                int deleted = new BusinessClass().DeleteDocument(id);
                if(deleted > 0)
                {
                    TempData["successMessage"] = "Document Deleted.";
                    return RedirectToAction("ViewAllDocuments", "Vault");
                }
                else
                {
                    TempData["errorMessage"] = "Some internal error occured. Please try again later.";
                    return RedirectToAction("ViewAllDocuments", "Vault");
                }
            }
            catch (Exception ex)
            {
                new LogAndErrorsClass().CatchException(ex);
                return RedirectToAction("ErrorControl", "Home");
            }
        }

        public JsonResult GetDoctorsByHospital(String hospitalId)
        {
            int id = Convert.ToInt32(hospitalId);
            List<Doctor> doctorList = new BusinessClass().GetDoctorsByHospital(id);
            var jsonData = JsonConvert.SerializeObject(doctorList);
            return Json(jsonData);
        }

        [HttpPost]
        public ActionResult GetFilteredFiles(FormCollection form)
        {
            DateTime fromDate = DateTime.Parse(form["txtFromDate"]);
            DateTime toDate = DateTime.Parse(form["txtToDate"]);
            if (fromDate >= toDate)
            {
                TempData["errorMessage"] = "Please select a valid date combination.";
                return RedirectToAction("ViewAllDocuments", "Vault");
            }
            else
            {
                User user = (User)Session["loggedUser"];
                FileData fileData = new BusinessClass().GetFilteredFilesV2(user.UserId, fromDate, toDate);
                TempData["filteredFiles"] = fileData;
                return RedirectToAction("ViewAllDocuments", "Vault");
            }
        }
    }
}