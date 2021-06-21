using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace faz1.Controllers
{
    public class ConfirmAccountController : ApiController
    {
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        private bool checkStatus(string username, string email)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string sqlText = "SELECT USERNAME, EMAIL_USER ,STATUS FROM tbl_User" +
                                 " WHERE username like @username and EMAIL_USER like @email";
                SqlCommand cmd = new SqlCommand(sqlText);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Connection = con;
                con.Open();
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();

                DataTable dt = ds.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    //Cheking is User active
                    bool userActive = (dt.Rows[0].Field<int>(2).ToString() == "1");
                    return userActive;
                }
                else
                {
                    return false;
                }

            }
        }

        public HttpResponseMessage Get(string username, string email, string key) //Parameters that coming from register.aspx.cs
        {
            try
            {
                if (checkStatus(username, email)) //Confirmed account response
                {   
                    var response = Request.CreateResponse(HttpStatusCode.Moved);
                    response.Headers.Location = new Uri("https://localhost:44316/Login.aspx");
                    return response;
                }
                else //inactive user response
                {
                    using (SqlConnection con = new SqlConnection(cs))
                    {
                        string sqlText = "UPDATE tbl_User SET STATUS = 1 WHERE USERNAME like @username and EMAIL_USER like @email " +
                            "CREATE DATABASE "+username+"";
                        SqlCommand cmd = new SqlCommand(sqlText);
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        //Confirm HTML
                        var path = HttpContext.Current.Server.MapPath("~/vendors/confirm.html");
                        var response = new HttpResponseMessage();
                        response.Content = new StringContent(File.ReadAllText(path));
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
                        return response; 
                    }



                }
            }
            catch (Exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, "Beklenmedik bir hata oluştu");
            }

        }








    }

}