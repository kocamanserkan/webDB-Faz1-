using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace faz1
{
    public partial class Login : System.Web.UI.Page
    {
        string userID;
        string userAd;
        string userSoyad;
        string userMail;
        string userRole;

        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        string url = ConfigurationManager.AppSettings["BaseUrl"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie reqCookies = Request.Cookies["userInfo"];//Cookieden Beni hatırla verilerin çekilmesi
            if (reqCookies != null)
            {
                txtUserName.Text = reqCookies["UserName"].ToString();
                ckbRemember.Checked = true;
            }
        }

        private List<bool> checkUserFromDB() //DB CHECK OPERATIONS
        {
            List<bool> userCheck = new List<bool>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string sqlText = "SELECT ID_USER, USERNAME, PASSWORD, STATUS, NAME, LASTNAME, EMAIL_USER, ROLE  from tbl_User" +
                                 " WHERE username like @username and password = @password";

                SqlCommand cmd = new SqlCommand(sqlText);
                cmd.Parameters.AddWithValue("@username", txtUserName.Text);
                cmd.Parameters.AddWithValue("@password", txtPassword.Text);

                cmd.Connection = con;
                con.Open();
                DataSet ds = new DataSet();
                ds.Clear();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    userID = dt.Rows[0].Field<int>(0).ToString();
                    userAd = dt.Rows[0][4].ToString();
                    userSoyad = dt.Rows[0][5].ToString();
                    userMail = dt.Rows[0][6].ToString();
                    userRole = dt.Rows[0][7].ToString();

                    bool userActive = (dt.Rows[0].Field<int>(3).ToString() == "1"); //Cheking is User active
                    bool successLogin = ((ds.Tables[0].Rows.Count > 0));    //Cheking is User registered  
                    userCheck.Add(successLogin);
                    userCheck.Add(userActive);

                    return userCheck;
                }
                else
                {
                    bool control = false;
                    userCheck.Add(control);

                    return userCheck;
                }

            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkUserFromDB()[0])
                {
                    if (checkUserFromDB()[1])
                    {
                        HttpCookie sessionInfo = new HttpCookie("sessionInfo");
                        sessionInfo["userName"] = txtUserName.Text;
                        sessionInfo["userID"] = userID.ToString();
                        sessionInfo["userAd"] = userAd.ToString();
                        sessionInfo["userSoyad"] = userSoyad.ToString();
                        sessionInfo["userMail"] = userMail;
                        sessionInfo["userRole"] = userRole;
                        Response.Cookies.Add(sessionInfo);

                        Session["userName"] = txtUserName.Text;
                        Session["userID"] = userID;
                        Session["userAd"] = userAd;
                        Session["userSoyad"] = userSoyad;
                        Session["userMail"] = userMail;
                        Session["userRole"] = userRole;


                        //cookie'e user bilgilerinin yazılması
                        if (ckbRemember.Checked)
                        {
                            HttpCookie userInfo = new HttpCookie("userInfo");
                            userInfo["userName"] = txtUserName.Text;
                            userInfo["userID"] = userID;
                            Response.Cookies.Add(userInfo);
                 
                        }

                        alertMsg.InnerText = "Giriş Başarılı. Yönlendiliyorsunuz..";
                        alertMsg.Attributes.Add("class", "alert alert-success");
                        alertMsg.Attributes.Remove("hidden");

                        Response.AddHeader("REFRESH", "1;URL=" + url + "/UserDashboard.aspx");
                    }
                    else
                    {
                        alertMsg.InnerHtml = "Hesabınız <strong>deaktif</strong> durumdadır. </br> Mailinizi kontrol ediniz.";
                        alertMsg.Attributes.Add("class", "alert alert-error");
                        alertMsg.Attributes.Remove("hidden");
                    }

                }
                else
                {
                    alertMsg.InnerText = "Kullancı Adı veya Parola Hatalı!";
                    alertMsg.Attributes.Add("class", "alert alert-error");
                    alertMsg.Attributes.Remove("hidden");
                }

            }
            catch (Exception)
            {
                alertMsg.InnerText = "Beklenmedik bir hata oluştu.";
                alertMsg.Attributes.Add("class", "alert alert-error");
                alertMsg.Attributes.Remove("hidden");
            }



        }
    }
}