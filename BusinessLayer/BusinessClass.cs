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
                doctor.HospitalName = dt.Rows[0]["hospitalname"].ToString();
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
        public DataTable GetDoctorsByHospital(int hospitalid)
        {
            return new DataAccessClass().GetDoctorsByHospitalFromDB(hospitalid);
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
    }
}
