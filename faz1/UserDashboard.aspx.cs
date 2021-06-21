using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace faz1
{
    public partial class UserDashboard : System.Web.UI.Page
    {
        string ownerID;
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        string lblNickName;




        protected void Page_Load(object sender, EventArgs e)
        {

            HttpCookie reqCookies = Request.Cookies["sessionInfo"];
            if (reqCookies == null)
            {
                Response.Redirect("https://localhost:44316/login.aspx");
            }
            else
            {
                lblUserAD.Text = reqCookies["userAd"].ToString();
                lbluserSoyad.Text = reqCookies["userSoyad"].ToString(); ;
                lblUserMail.Text = reqCookies["userMail"].ToString(); ;
                lblNickName = reqCookies["userName"].ToString(); ;
                ownerID = reqCookies["userID"].ToString(); ;
            }
            if (!IsPostBack)
            {
                tableCount();
            }
        }

        
        void tableCount()
        {
            string UserCS = cs;
            string patternToReplace = @"\bakaStaj\b";
            UserCS = Regex.Replace(UserCS, patternToReplace, lblNickName);

            using (SqlConnection con = new SqlConnection(UserCS))
            {
                string sqlText = "SELECT COUNT(TABLE_NAME) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
                SqlCommand cmd = new SqlCommand(sqlText);
               
                cmd.Connection = con;
                con.Open();
                DataSet ds = new DataSet();
                ds.Clear();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
                DataTable dt = ds.Tables[0];

                lblTableCount.Text = dt.Rows[0][0].ToString();
                txtNewName.Text = lblUserAD.Text;
                txtNewLastName.Text = lbluserSoyad.Text;
                txtNewEmail.Text = lblUserMail.Text;

            }
        }

        protected void btnUpdatePerson_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string sqlText = "UPDATE tbl_User  SET NAME = @name, " +
                        "LASTNAME=@lastName, EMAIL_USER = @email  WHERE USERNAME like @username ";

                    SqlCommand cmd = new SqlCommand(sqlText);
                    cmd.Parameters.AddWithValue("@name", txtNewName.Text);
                    cmd.Parameters.AddWithValue("@lastName", txtNewLastName.Text);
                    cmd.Parameters.AddWithValue("@email", txtNewEmail.Text);
                    cmd.Parameters.AddWithValue("@username", lblNickName);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Kullanıcı Bilgilerin Güncellendi','yes')", true);
                    HttpCookie reqCookies = Request.Cookies["sessionInfo"];
                    reqCookies["userAd"] = txtNewName.Text;
                    reqCookies["userSoyad"] = txtNewLastName.Text;
                    reqCookies["userMail"] = txtNewEmail.Text;
                    lblUserAD.Text = reqCookies["userAd"];
                    lbluserSoyad.Text = reqCookies["userSoyad"];
                    lblUserMail.Text = reqCookies["userMail"];
                    lblNickName = reqCookies["userName"];
                    Response.Cookies.Add(reqCookies);
                }
            }
            catch (Exception)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Kullanıcı Bilgilerin Güncellerirken Bir Hata Oluştu','no')", true);
            }



        }
    }
}