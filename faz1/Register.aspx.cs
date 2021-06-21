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
    public partial class Register : System.Web.UI.Page
    {
        //Connection String
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        string baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
        protected void Page_Load(object sender, EventArgs e)
        {

           


        }

        //User register DB check işlemleri
        private bool checkUserFromDB()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string sqlText = "SELECT USERNAME from tbl_User" +
                                 " WHERE username like @username";

                SqlCommand cmd = new SqlCommand(sqlText);
                cmd.Parameters.AddWithValue("@username", txtUserName.Text);
                cmd.Connection = con;
                con.Open();
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();

                bool userAvaible = ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count == 0));
                return userAvaible;
            }

        }

        //User register DB işlemleri
        private void addRegister()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string sqlText = "INSERT INTO tbl_User" +
                                 " VALUES (@name, @lastName, @email, @username, @password, 0)";

                SqlCommand cmd = new SqlCommand(sqlText);
                cmd.Parameters.AddWithValue("@name", txtName.Text.ToUpper());
                cmd.Parameters.AddWithValue("@lastName", txtLastName.Text.ToUpper());
                cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@username", txtUserName.Text);
                cmd.Parameters.AddWithValue("@password", txtPassword.Text);
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

            }
        }




        protected void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkUserFromDB())
                {
                    addRegister();
                    
                    Mail.ConfirmMail confirmMail = new Mail.ConfirmMail();
                    string link = "https://localhost:44316/confirmAccount?username=" + txtUserName.Text + "&email=" + txtEmail.Text + "&key=ksQVLEfdtksfc9zk8BT8r9sVRrvDUmqX";
                    //Confirmation mail
                    confirmMail.MailGonder(txtEmail.Text, link, txtName.Text.ToUpper());
                    alertMsg.InnerHtml = "Kayıt Başarılı! Aktivasyon linki <br/>" + txtEmail.Text + "</strong> adresine gönderilmiştir.";
                    alertMsg.Attributes.Add("class", "alert alert-success");
                    alertMsg.Attributes.Remove("hidden");
                    Response.AddHeader("REFRESH", "2;URL=" + baseUrl + "/Login.aspx");
                    txtName.Text = string.Empty;
                    txtEmail.Text = string.Empty;
                    txtUserName.Text = string.Empty;
                    txtLastName.Text = string.Empty;

                }
                else
                {
                    alertMsg.InnerHtml = "<strong>" + txtUserName.Text + "</strong> kullanımda. Lütfen başka bir Kullanıcı Adı seçiniz.";
                    alertMsg.Attributes.Add("class", "alert alert-error");
                    alertMsg.Attributes.Remove("hidden");
                }
            }
            catch (Exception)
            {
                alertMsg.InnerText = "Kayıt Esnasında Hata Oluştu";
                alertMsg.Attributes.Add("class", "alert alert-error");
                alertMsg.Attributes.Remove("hidden");
            }

        }
    }
}