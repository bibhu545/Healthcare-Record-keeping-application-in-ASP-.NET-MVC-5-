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
            return new DataAccessClass().LoginFromDB(email, password);
        }
        public User EditUserDetails(User user)
        {
            return new DataAccessClass().UpdateUserToDB(user);
        }
        public User updatePassword(User user)
        {
            return new DataAccessClass().UpdatePasswordToDB(user);
        }
        public int AddHospital(Hospital hospital)
        {
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
        public int DeleteHospital(int hospitalid)
        {
            return new DataAccessClass().DeleteHospitalFromDB(hospitalid);
        }
        public Hospital GetHospitalDetails(int id)
        {
            return new DataAccessClass().GetHospitalDetailsFromDB(id);
        }
        public Hospital EditHospitalDetails(Hospital hospital)
        {
            return new DataAccessClass().UpdateHospitalToDB(hospital);
        }
    }
}
