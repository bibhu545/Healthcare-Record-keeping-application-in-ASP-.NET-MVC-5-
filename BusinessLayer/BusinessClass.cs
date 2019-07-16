using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Net.Mail;
using System.Net;
using DataAccess;
using DataModels;

namespace BusinessLayer
{
    public class FileData
    {
        public List<int> DocumentId = new List<int>();
        public List<String> FileName = new List<string>();
        public List<String> Extension = new List<string>();
        public List<String> FilePath = new List<string>();
        public List<String> IssueDate = new List<string>();
        public List<String> HospitalName = new List<string>();
        public List<String> DoctorName = new List<string>();
        public List<String> RecordType = new List<string>();
    }
    public class BusinessClass
    {
        public User CreateUser(User user)
        {
            Random random = new Random();
            int otp = random.Next(1999, 9999);

            String mailHead = "Complete your registration at Healthcare.";
            String mailBody = "Please enter " + otp.ToString() + " to confirm your membership";
            var client = new SmtpClient("smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("7c73e9fdbcbfbf", "4708f0b90c37e9"),
                EnableSsl = true
            };
            client.Send("bibhu@demomail.com", user.Email, mailHead, mailBody);

            user.SecurityCode = otp.ToString();

            user.Email = user.Email.Replace("'","''");
            user.FirstName = user.FirstName.Replace("'","''");
            user.LastName = user.LastName.Replace("'","''");
            user.Password = user.Password.Replace("'","''");
            user.Phone = user.Phone.Replace("'","''");
            user.Profile = user.Profile.Replace("'","''");

            user.status = new DataAccessClass().SaveUserToDB(user);
            return user;
        }
        public User GetCode(User user, int id)
        {
            return new DataAccessClass().GetCodeFromDB(user, id);
        }
        public int ActivateUser(User user)
        {
            return new DataAccessClass().ActivaeUserToDB(user);
        }
        public User Login(String email, String password)
        {
            email.Replace(",", "''");
            password.Replace(",", "''");
            return new DataAccessClass().LoginFromDB(email, password);
        }
        public User EditUserDetails(User user)
        {
            user.Email = user.Email.Replace("'", "''");
            user.FirstName = user.FirstName.Replace("'", "''");
            user.LastName = user.LastName.Replace("'", "''");
            user.Password = user.Password.Replace("'", "''");
            user.Phone = user.Phone.Replace("'", "''");
            user.Profile = user.Profile.Replace("'", "''");
            return new DataAccessClass().UpdateUserToDB(user);
        }
        public User updatePassword(User user)
        {
            user.Password = user.Password.Replace("'", "''");
            return new DataAccessClass().UpdatePasswordToDB(user);
        }

        public int AddHospital(Hospital hospital)
        {
            hospital.HospitalName = hospital.HospitalName.Replace("'", "''");
            hospital.Address = hospital.Address.Replace("'", "''");
            hospital.Phone1 = hospital.Phone1.Replace("'", "''");
            hospital.Phone2 = hospital.Phone2.Replace("'", "''");
            hospital.Email = hospital.Email.Replace("'", "''");
            return new DataAccessClass().AddHospitalToDB(hospital);
        }
        public List<Hospital> GetHospitals(int id)
        {
            DataTable dt = new DataAccessClass().GetHospitalsFromDB(id);
            List<Hospital> hospitalList = new List<Hospital>();
            foreach (DataRow row in dt.Rows)
            {
                hospitalList.Add(new Hospital() {
                    HospitalId = Convert.ToInt32(row["hospitalid"]),
                    HospitalName = row["hospitalname"].ToString(),
                    Address = row["address"].ToString(),
                    Phone1 = row["phone1"].ToString(),
                    Phone2 = row["phone2"].ToString(),
                    Email = row["email"].ToString(),
                    IsPrimary = Convert.ToInt32(row["isprimary"])
                });
            }
            return hospitalList;
        }
        public List<Speciality> GetSpecialities()
        {
            DataTable dt = new DataAccessClass().GetSpecialitiesFromDB();
            List<Speciality> specialities = new List<Speciality>();
            foreach (DataRow row in dt.Rows)
            {
                specialities.Add(new Speciality()
                {
                    SpecialityId = Convert.ToInt32(row["specialityid"]),
                    SpecialityName = row["speciality"].ToString()
                });
            }
            return specialities;
        }
        public int DeleteHospital(int hospitalid)
        {
            return new DataAccessClass().DeleteHospitalFromDB(hospitalid);
        }
        public Hospital GetHospitalDetails(int userid, int id)
        {
            return new DataAccessClass().GetHospitalDetailsFromDB(userid, id);
        }
        public Hospital EditHospitalDetails(Hospital hospital)
        {
            hospital.HospitalName = hospital.HospitalName.Replace("'", "''");
            hospital.Address = hospital.Address.Replace("'", "''");
            hospital.Phone1 = hospital.Phone1.Replace("'", "''");
            hospital.Phone2 = hospital.Phone2.Replace("'", "''");
            hospital.Email = hospital.Email.Replace("'", "''");
            return new DataAccessClass().UpdateHospitalToDB(hospital);
        }

        public Doctor GetDoctorDetails(int userid, int doctorid)
        {
            DataTable dt = new DataAccessClass().GetDoctorsFromDB(userid, doctorid);
            Doctor doctor = new Doctor();
            int records = dt.Rows.Count;
            if (records > 0)
            {
                doctor.status = 1;
                doctor.DoctorId = Convert.ToInt32(dt.Rows[0]["doctorid"].ToString());
                doctor.FirstName = dt.Rows[0]["firstname"].ToString();
                doctor.LastName = dt.Rows[0]["lastname"].ToString();
                doctor.HospitalId = Convert.ToInt32(dt.Rows[0]["hospitalid"].ToString());
                doctor.HospitalName = dt.Rows[0]["hospitalname"].ToString();
                doctor.Speciality = Convert.ToInt32(dt.Rows[0]["specialityid"].ToString());
                doctor.SpecialistIn = dt.Rows[0]["speciality"].ToString();
                doctor.Address = dt.Rows[0]["address"].ToString();
                doctor.Phone1 = dt.Rows[0]["phone1"].ToString();
                doctor.Phone2 = dt.Rows[0]["phone2"].ToString();
                doctor.Email = dt.Rows[0]["email"].ToString();
                doctor.IsPrimary = Convert.ToInt32(dt.Rows[0]["isprimary"].ToString());
            }
            else
            {
                doctor.status = -1;
            }
            return doctor;
        }
        public int DeleteDoctor(int doctorid)
        {
            return new DataAccessClass().DeleteDoctorFromDB(doctorid);
        }
        public List<Doctor> GetDoctors(int userid)
        {
            DataTable dt = new DataAccessClass().GetDoctorsFromDB(userid, 0);
            List<Doctor> doctorList = new List<Doctor>();
            foreach (DataRow row in dt.Rows)
            {
                doctorList.Add(new Doctor()
                {
                    DoctorId = Convert.ToInt32(row["doctorid"]),
                    FirstName = row["firstname"].ToString(),
                    HospitalName = row["hospitalname"].ToString(),
                    Address = row["address"].ToString(),
                    Phone1 = row["phone1"].ToString(),
                    IsPrimary = Convert.ToInt32(row["isprimary"])
                });
            }
            return doctorList;
        }
        public Doctor AddDoctor(Doctor doctor)
        {
            doctor.FirstName = doctor.FirstName.Replace("'", "''");
            doctor.LastName = doctor.LastName.Replace("'", "''");
            doctor.Address = doctor.Address.Replace("'", "''");
            doctor.Email = doctor.Email.Replace("'", "''");
            doctor.Phone1 = doctor.Phone1.Replace("'", "''");
            doctor.Phone2 = doctor.Phone2.Replace("'", "''");
            return new DataAccessClass().AddDoctorToDB(doctor);
        }
        public Doctor EditDoctor(Doctor doctor)
        {
            doctor.FirstName = doctor.FirstName.Replace("'", "''");
            doctor.LastName = doctor.LastName.Replace("'", "''");
            doctor.Address = doctor.Address.Replace("'", "''");
            doctor.Email = doctor.Email.Replace("'", "''");
            doctor.Phone1 = doctor.Phone1.Replace("'", "''");
            doctor.Phone2 = doctor.Phone2.Replace("'", "''");
            return new DataAccessClass().UpdateDoctorToDB(doctor);
        }

        public List<RecordType> GetRecordTypes()
        {
            DataTable dt = new DataAccessClass().GetRecordTypesFromDB();
            List<RecordType> recordTypes = new List<RecordType>();
            foreach (DataRow row in dt.Rows)
            {
                recordTypes.Add(new RecordType()
                {
                    RecordId = Convert.ToInt32(row["recordid"]),
                    RecordTypeName = row["recordtype"].ToString()
                });
            }
            return recordTypes;
        }
        public Document SaveDocument(Document document, List<String> allFiles)
        {
            return new DataAccessClass().SaveDocumentToDB(document, allFiles);
        }
        public List<Doctor> GetDoctorsByHospital(int hospitalid)
        {
            DataTable dt =  new DataAccessClass().GetDoctorsByHospitalFromDB(hospitalid);
            List<Doctor> doctorList = new List<Doctor>();
            foreach (DataRow row in dt.Rows)
            {
                doctorList.Add(new Doctor() {
                    DoctorId = Convert.ToInt32(row["doctorid"]),
                    FirstName = row["firstname"].ToString()
                });
            }
            return doctorList;
        }
        public FileData GetFiles(int userid)
        {
            DataTable dt = new DataAccessClass().GetFilesFromDB(userid);
            FileData fileData = new FileData();
            foreach (DataRow row in dt.Rows)
            {
                String file = row["filepath"].ToString();
                String fName = file.Substring(file.LastIndexOf("/"), file.Length - file.LastIndexOf("/")).Substring(1);
                String extension = fName.Substring(fName.LastIndexOf(".")).Substring(1); ;
                fileData.FileName.Add(fName.Replace("." + extension, "").Substring(7));
                fileData.Extension.Add(extension);
                fileData.DocumentId.Add(Convert.ToInt32(row["documentid"]));
                fileData.FilePath.Add(row["filepath"].ToString());
                fileData.IssueDate.Add(row["issuedate"].ToString());
                fileData.HospitalName.Add(row["hospitalname"].ToString());
                fileData.DoctorName.Add(row["firstname"].ToString());
                fileData.RecordType.Add(row["recordid"].ToString());
            }
            return fileData;
        }
        public FileData GetFilteredFilesV2(int userid, DateTime startDate, DateTime endDate)
        {
            DataTable dt = new DataAccessClass().GetFilteredFilesFromDBV2(userid, startDate, endDate);
            FileData fileData = new FileData();
            foreach (DataRow row in dt.Rows)
            {
                String file = row["filepath"].ToString();
                String fName = file.Substring(file.LastIndexOf("/"), file.Length - file.LastIndexOf("/")).Substring(1);
                String extension = fName.Substring(fName.LastIndexOf(".")).Substring(1); ;
                fileData.FileName.Add(fName.Replace("." + extension, "").Substring(7));
                fileData.Extension.Add(extension);
                fileData.DocumentId.Add(Convert.ToInt32(row["documentid"]));
                fileData.FilePath.Add(row["filepath"].ToString());
                fileData.IssueDate.Add(row["issuedate"].ToString());
                fileData.HospitalName.Add(row["hospitalname"].ToString());
                fileData.DoctorName.Add(row["firstname"].ToString());
                fileData.RecordType.Add(row["recordid"].ToString());
            }
            return fileData;
        }
        public int DeleteDocument(int documentid)
        {
            return new DataAccessClass().DeleteDocumentFromDB(documentid);
        }

        public int GetTotalDoctors(int userid)
        {
            return new DataAccessClass().GetTotalDoctorsFormDb(userid);
        }
        public int GetTotalHospitals(int userid)
        {
            return new DataAccessClass().GetTotalHospitalsFormDb(userid);
        }
        public int GetTotalDocuments(int userid)
        {
            return new DataAccessClass().GetTotalDocumentsFormDb(userid);
        }


        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
        public string RandomAlphaNumericString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(2, true));
            builder.Append(RandomNumber(10, 99));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }
    }
}
