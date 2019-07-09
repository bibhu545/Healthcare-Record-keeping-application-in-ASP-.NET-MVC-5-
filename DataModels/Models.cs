using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public int HospitalId { get; set; }
        public int UserId { get; set; }
        public String HospitalName { get; set; }
        public String Address { get; set; }
        public String Phone1 { get; set; }
        public String Phone2 { get; set; }
        public String Email { get; set; }
        public int IsPrimary { get; set; }
        public bool SetPrimary { get; set; }   //for mvc checkboxfor control
    }
    public class Doctor
    {
        public int status = -1;
        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public int HospitalId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public int Speciality { get; set; }
        public String Address { get; set; }
        public String Phone1 { get; set; }
        public String Phone2 { get; set; }
        public String Email { get; set; }
        public int IsPrimary { get; set; }
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
}
