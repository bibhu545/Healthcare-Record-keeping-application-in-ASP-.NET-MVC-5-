using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
