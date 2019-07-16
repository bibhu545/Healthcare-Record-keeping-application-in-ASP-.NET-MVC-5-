using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DataModels
{
    public class User
    {
        public int status = -1;
        public int UserId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public String Address { get; set; }
        public String Phone { get; set; }
        public String Profile { get; set; }
        public Boolean IsActive { get; set; }
        public String SecurityCode { get; set; }
    }
    public class Hospital
    {
        public int status = -1;
        [Display(Name = "Hospital Name")]
        public int HospitalId { get; set; }
        public int UserId { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        public String HospitalName { get; set; }

        [Required(ErrorMessage = "Address is Required")]
        public String Address { get; set; }

        [Required(ErrorMessage = "Contact number is Required")]
        public String Phone1 { get; set; }
        public String Phone2 { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Email is not valid")]
        public String Email { get; set; }
        public int IsPrimary { get; set; }
        public bool SetPrimary { get; set; }   //for mvc checkboxfor control
    }
    public class Speciality
    {
        public int SpecialityId { get; set; }
        public String SpecialityName { get; set; }
    }
    public class Doctor
    {
        public int status = -1;

        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public int HospitalId { get; set; }
        [Display(Name = "Hospital Name")]
        public String HospitalName { get; set; } //for mvc
        [Display(Name = "First Name")]
        public String FirstName { get; set; }
        [Display(Name = "Last Name")]
        public String LastName { get; set; }
        public int Speciality { get; set; }
        [Display(Name = "Speciality")]
        public String SpecialistIn { get; set; } //for mvc
        public String Address { get; set; }
        [Display(Name = "Contact Number")]
        public String Phone1 { get; set; }
        [Display(Name = "Alternative Number")]
        public String Phone2 { get; set; }
        public String Email { get; set; }
        public int IsPrimary { get; set; }
        public bool SetPrimary { get; set; } //for mvc
    }
    public class Document
    {
        public int status = -1;
        public int DocumentId { get; set; }
        public int UserId { get; set; }
        public String HospitalName { get; set; }
        public int HospitalId { get; set; }
        public int DoctorId { get; set; }
        public String DoctorName { get; set; }
        public String IssueDate { get; set; }
        public int DocumentType { get; set; }
        public String DocumentTypeName { get; set; }
        public String Path { get; set; }
        public String UploadDate { get; set; }
    }
    public class RecordType
    {
        public int RecordId { get; set; }
        public String RecordTypeName { get; set; }
    }
}
