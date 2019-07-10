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
        public User UpdateUserToDB(User user)
        {
            user.status = -1;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand scmd = new SqlCommand("UpdateUserDetails", conn);
                scmd.CommandType = CommandType.StoredProcedure;
                scmd.Parameters.AddWithValue("@userid", user.UserId);
                scmd.Parameters.AddWithValue("@firstname", user.FirstName);
                scmd.Parameters.AddWithValue("@lastname", user.LastName);
                scmd.Parameters.AddWithValue("@email", user.Email);
                scmd.Parameters.AddWithValue("@phone", user.Phone);
                scmd.Parameters.AddWithValue("@address", user.Address);
                scmd.Parameters.AddWithValue("@profile", user.Profile);
                user.status = scmd.ExecuteNonQuery();
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
        public User UpdatePasswordToDB(User user)
        {
            user.status = -1;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand scmd = new SqlCommand("ChangePassword", conn);
                scmd.CommandType = CommandType.StoredProcedure;
                scmd.Parameters.AddWithValue("@userid", user.UserId);
                scmd.Parameters.AddWithValue("@password", user.Password);
                user.status = scmd.ExecuteNonQuery();
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
        public int AddHospitalToDB(Hospital hospital)
        {
            int added = -1;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand scmd = new SqlCommand("AddHospital", conn);
                scmd.CommandType = CommandType.StoredProcedure;
                scmd.Parameters.AddWithValue("@userid", hospital.UserId);
                scmd.Parameters.AddWithValue("@hospitalname", hospital.HospitalName);
                scmd.Parameters.AddWithValue("@address", hospital.Address);
                scmd.Parameters.AddWithValue("@phone1", hospital.Phone1);
                scmd.Parameters.AddWithValue("@phone2", hospital.Phone2);
                scmd.Parameters.AddWithValue("@email", hospital.Email);
                scmd.Parameters.AddWithValue("@isprimary", hospital.IsPrimary);
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
        public DataTable GetHospitalsFromDB(int userid)
        {
            DataTable dt = null;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand scmd = new SqlCommand("select * from hospitals where userid = " + userid + " order by isprimary desc", conn);
                SqlDataAdapter sda = new SqlDataAdapter(scmd);
                dt = new DataTable();
                sda.Fill(dt);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return dt;
        }
        public int DeleteHospitalFromDB(int hospitalid)
        {
            int deleted = -1;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand scmd = new SqlCommand("DELETE FROM hospitals WHERE hospitalid = " + hospitalid, conn);
                deleted = scmd.ExecuteNonQuery();
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return deleted;
        }
        public Hospital GetHospitalDetailsFromDB(int userid, int id)
        {
            Hospital hospital = new Hospital();
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand scmd = new SqlCommand("select * from hospitals Where userid = " + userid + " and hospitalid = " + id, conn);
                SqlDataAdapter sda = new SqlDataAdapter(scmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                int records = dt.Rows.Count;
                if (records > 0)
                {
                    hospital.status = 1;
                    hospital.HospitalId = Convert.ToInt32(dt.Rows[0]["hospitalid"].ToString());
                    hospital.UserId = Convert.ToInt32(dt.Rows[0]["userid"].ToString());
                    hospital.HospitalName = dt.Rows[0]["hospitalname"].ToString();
                    hospital.Address = dt.Rows[0]["address"].ToString();
                    hospital.Phone1 = dt.Rows[0]["phone1"].ToString();
                    hospital.Phone2 = dt.Rows[0]["phone2"].ToString();
                    hospital.Email = dt.Rows[0]["email"].ToString();
                    hospital.IsPrimary = Convert.ToInt32(dt.Rows[0]["isprimary"].ToString());
                }
                else
                {
                    hospital.status = -1;
                }
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return hospital;
        }
        public Hospital UpdateHospitalToDB(Hospital hospital)
        {
            hospital.status = -1;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand scmd = new SqlCommand("UpdateHospital", conn);
                scmd.CommandType = CommandType.StoredProcedure;
                scmd.Parameters.AddWithValue("@hospitalid", hospital.HospitalId);
                scmd.Parameters.AddWithValue("@userid", hospital.UserId);
                scmd.Parameters.AddWithValue("@hospitalname", hospital.HospitalName);
                scmd.Parameters.AddWithValue("@address", hospital.Address);
                scmd.Parameters.AddWithValue("@phone1", hospital.Phone1);
                scmd.Parameters.AddWithValue("@phone2", hospital.Phone2);
                scmd.Parameters.AddWithValue("@email", hospital.Email);
                scmd.Parameters.AddWithValue("@isprimary", hospital.IsPrimary);
                hospital.status = scmd.ExecuteNonQuery();
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return hospital;
        }
    }
}
