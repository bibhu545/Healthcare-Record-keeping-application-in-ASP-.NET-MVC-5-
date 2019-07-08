using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DataModels;

namespace DataAccess
{
    public class DataAccessClass
    {
        String connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        SqlConnection conn = null;
        public int SaveUserToDB(User user)
        {
            int added = -1;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand scmd = new SqlCommand("CreateNewUserWithCode", conn);
                scmd.CommandType = CommandType.StoredProcedure;
                scmd.Parameters.AddWithValue("@firstname", user.FirstName);
                scmd.Parameters.AddWithValue("@lastname", user.LastName);
                scmd.Parameters.AddWithValue("@email", user.Email);
                scmd.Parameters.AddWithValue("@password", user.Password);
                scmd.Parameters.AddWithValue("@activationlink", user.SecurityCode);
                added = scmd.ExecuteNonQuery();
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return added;
        }
        public User GetCodeFromDB(User user, int id)
        {
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand scmd = new SqlCommand("SELECT activationlink FROM health_users WHERE userid = " + id, conn);
                SqlDataAdapter sda = new SqlDataAdapter(scmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count == 1)
                {
                    user.status = 1;
                    user.SecurityCode = dt.Rows[0]["activationlink"].ToString();
                }
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return user;
        }
        public int ActivaeUserToDB(User user)
        {
            int activated = -1;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand scmd = new SqlCommand("ActivateUser", conn);
                scmd.CommandType = CommandType.StoredProcedure;
                scmd.Parameters.AddWithValue("@email", user.Email);
                activated = scmd.ExecuteNonQuery();
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return activated;
        }
        public User LoginFromDB(String email, String password)
        {
            User user = new User();
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand scmd = new SqlCommand("SELECT * FROM health_users WHERE email = '" + email + "' AND password = '" + password + "'", conn);
                SqlDataAdapter sda = new SqlDataAdapter(scmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count == 1)
                {
                    user.status = 1;
                    user.UserId = Convert.ToInt32(dt.Rows[0]["userid"]);
                    user.FirstName = dt.Rows[0]["firstname"].ToString();
                    user.LastName = dt.Rows[0]["lastname"].ToString();
                    user.Email = dt.Rows[0]["email"].ToString();
                    user.Password = dt.Rows[0]["password"].ToString();
                    user.Address = dt.Rows[0]["address"].ToString();
                    user.Phone = dt.Rows[0]["phone"].ToString();
                    user.Profile = dt.Rows[0]["profile"].ToString();
                    user.SecurityCode = dt.Rows[0]["activationlink"].ToString();
                    if (Convert.ToInt32(dt.Rows[0]["active"]) == 1)
                    {
                        user.IsActive = true;
                    }
                    else
                    {
                        user.IsActive = false;
                    }
                }
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return user;
        }
    }
}
