﻿using System;
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
        public DataTable GetSpecialitiesFromDB()
        {
            DataTable dt = null;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand scmd = new SqlCommand("select * from speciality", conn);
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
                SqlCommand scmd = new SqlCommand("DeleteHospital", conn);
                scmd.CommandType = CommandType.StoredProcedure;
                scmd.Parameters.AddWithValue("@hospitalid", hospitalid);
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
        public Doctor AddDoctorToDB(Doctor doctor)
        {
            doctor.status = -1;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand scmd = new SqlCommand("CreateDoctors", conn);
                scmd.CommandType = CommandType.StoredProcedure;
                scmd.Parameters.AddWithValue("@userid", doctor.UserId);
                scmd.Parameters.AddWithValue("@hospitalid", doctor.HospitalId);
                scmd.Parameters.AddWithValue("@firstname", doctor.FirstName);
                scmd.Parameters.AddWithValue("@lastname", doctor.LastName);
                scmd.Parameters.AddWithValue("@speciality", doctor.Speciality);
                scmd.Parameters.AddWithValue("@address", doctor.Address);
                scmd.Parameters.AddWithValue("@phone1", doctor.Phone1);
                scmd.Parameters.AddWithValue("@phone2", doctor.Phone2);
                scmd.Parameters.AddWithValue("@email", doctor.Email);
                scmd.Parameters.AddWithValue("@isprimary", doctor.IsPrimary);
                doctor.status = scmd.ExecuteNonQuery();
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return doctor;
        }
        public int DeleteDoctorFromDB(int doctorid)
        {
            int deleted = -1;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand scmd = new SqlCommand("DELETE FROM doctors WHERE doctorid = " + doctorid, conn);
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
        public DataTable GetDoctorsFromDB(int userid, int doctorid)
        {
            DataTable dt;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand scmd = new SqlCommand("GetDoctorDetailsV2", conn);
                scmd.CommandType = CommandType.StoredProcedure;
                scmd.Parameters.AddWithValue("@doctorid", doctorid);
                scmd.Parameters.AddWithValue("@userid", userid);
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
        public Doctor UpdateDoctorToDB(Doctor doctor)
        {
            doctor.status = -1;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand scmd = new SqlCommand("UpdateDoctor", conn);
                scmd.CommandType = CommandType.StoredProcedure;
                scmd.Parameters.AddWithValue("@doctorid", doctor.DoctorId);
                scmd.Parameters.AddWithValue("@userid", doctor.UserId);
                scmd.Parameters.AddWithValue("@hospitalid", doctor.HospitalId);
                scmd.Parameters.AddWithValue("@firstname", doctor.FirstName);
                scmd.Parameters.AddWithValue("@lastname", doctor.LastName);
                scmd.Parameters.AddWithValue("@speciality", doctor.Speciality);
                scmd.Parameters.AddWithValue("@address", doctor.Address);
                scmd.Parameters.AddWithValue("@phone1", doctor.Phone1);
                scmd.Parameters.AddWithValue("@phone2", doctor.Phone2);
                scmd.Parameters.AddWithValue("@email", doctor.Email);
                scmd.Parameters.AddWithValue("@isprimary", doctor.IsPrimary);
                doctor.status = scmd.ExecuteNonQuery();
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return doctor;
        }
        public DataTable GetDoctorsByHospitalFromDB(int hospitalId)
        {
            DataTable dt;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand scmd = new SqlCommand("select * from doctors where hospitalid=" + hospitalId, conn);
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
        public DataTable GetRecordTypesFromDB()
        {
            DataTable dt;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand scmd = new SqlCommand("select * from recordtypes", conn);
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
        public Document SaveDocumentToDB(Document document, List<String> allFiles)
        {
            document.status = -1;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                String insertCommand = String.Empty;
                foreach (String path in allFiles)
                {
                    insertCommand += "INSERT INTO documents(userid, hospitalid, doctorid, issuedate, recordtype, filepath) values(" + document.UserId + ", " + document.HospitalId + ", " + document.DoctorId + ", '" + document.IssueDate + "', " + document.DocumentType + ",'" + path + "');";
                }
                SqlCommand scmd = new SqlCommand(insertCommand, conn);
                document.status = scmd.ExecuteNonQuery();
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return document;
        }
        public DataTable GetFilesFromDB(int userid)
        {
            DataTable dt;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand scmd = new SqlCommand("GetFilesFromDB", conn);
                scmd.CommandType = CommandType.StoredProcedure;
                scmd.Parameters.AddWithValue("@userid", userid);
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
        public DataTable GetFilteredFilesFromDBV2(int userid, DateTime startDate, DateTime endDate)
        {
            DataTable dt;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand scmd = new SqlCommand("GetFormattedFilesFromDBV2", conn);
                scmd.CommandType = CommandType.StoredProcedure;
                scmd.Parameters.AddWithValue("@userid", userid);
                scmd.Parameters.AddWithValue("@startdate", startDate);
                scmd.Parameters.AddWithValue("@enddate", endDate);
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
        public int DeleteDocumentFromDB(int documentid)
        {
            int deleted = -1;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand scmd = new SqlCommand("DELETE FROM documents WHERE documentid = " + documentid, conn);
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

        public int GetTotalDoctorsFormDb(int userid)
        {
            int totalDoctors = 0;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand scmd = new SqlCommand("select count(*) from doctors where userid = " + userid, conn);
                totalDoctors = Convert.ToInt32(scmd.ExecuteScalar());
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return totalDoctors;
        }
        public int GetTotalHospitalsFormDb(int userid)
        {
            int totalHospitals = 0;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand scmd = new SqlCommand("select count(*) from hospitals where userid = " + userid, conn);
                totalHospitals = Convert.ToInt32(scmd.ExecuteScalar());
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return totalHospitals;
        }
        public int GetTotalDocumentsFormDb(int userid)
        {
            int totalDocuments = 0;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand scmd = new SqlCommand("select count(*) from documents where userid = " + userid, conn);
                totalDocuments = Convert.ToInt32(scmd.ExecuteScalar());
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return totalDocuments;
        }
    }
}
