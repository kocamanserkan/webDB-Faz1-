using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit.Bundling;
using DevExpress.Web;
using Microsoft.Build.Framework.XamlTypes;

namespace faz1.Administration
{
    public partial class PanelofAdmin : System.Web.UI.Page
    {
        #region DataMembers
        string url = ConfigurationManager.AppSettings["BaseUrl"].ToString();
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        string userRole;
        string userName;

        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                HttpCookie reqCookies = Request.Cookies["sessionInfo"];
                if (reqCookies != null)
                {
                    userRole = reqCookies["userRole"].ToString();
                    userName = reqCookies["userName"].ToString();

                    bindCombo();
                    bindGrid();
                }
                else
                {
                    Response.Write("Oturum Süreniz Bitti. Giriş Sayfasına Yönlendiriliyorsunuz.");
                    System.Threading.Thread.Sleep(1000);
                    Response.Redirect(url + "/login.aspx");
                }
            }
            catch (Exception)
            {
                Response.Write("Oturum Süreniz Bitti. Giriş Sayfasına Yönlendiriliyorsunuz.");
                System.Threading.Thread.Sleep(1000);
                Response.Redirect(url + "/login.aspx");
            }



        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //bindDll();
        }


        #region Bind Methods
        void bindGrid()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string sqlText = "";
                    switch (userRole)
                    {
                        case "sys_admin":
                            sqlText = "SELECT * From tbl_User WHERE ACTIVE = 1  AND ROLE = 'basic_user' or Role = 'sys_mod'";
                            break;
                        case "sys_mod":
                            sqlText = "SELECT * From tbl_User WHERE ACTIVE = 1 AND ROLE = 'basic_user'";
                            break;
                        default:
                            break;
                    }


                    SqlCommand cmd = new SqlCommand(sqlText);
                    cmd.Connection = con;
                    con.Open();
                    DataSet ds = new DataSet();
                    ds.Clear();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();
                    DataTable dt = ds.Tables[0];
                    grdUsers.DataSource = dt;
                    grdUsers.DataBind();

                }
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        void bindCombo()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string sqlText = "select * from tbl_Roles order by roles asc";

                    SqlCommand cmd = new SqlCommand(sqlText);

                    cmd.Connection = con;
                    con.Open();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                    switch (userRole)
                    {
                        case "sys_admin":
                            dt.Rows.Remove(dt.Rows[1]);
                            dt.Columns.Add("TEXTROLE");
                            dt.Rows[0]["TEXTROLE"] = "Genel Kullanıcı";
                            dt.Rows[1]["TEXTROLE"] = "Sistem Moderatör";

                            break;
                        case "sys_mod":
                            dt.Rows.Remove(dt.Rows[2]);
                            dt.Rows.Remove(dt.Rows[1]);
                            dt.Columns.Add("TEXTROLE");
                            dt.Rows[0]["TEXTROLE"] = "Genel Kullanıcı";
                            break;
                        default:
                            break;
                    }

                    var comboColumn = ((GridViewDataComboBoxColumn)grdUsers.Columns["ROLE"]);
                    comboColumn.PropertiesComboBox.DataSource = dt;
                    comboColumn.PropertiesComboBox.TextField = "TEXTROLE";
                    comboColumn.PropertiesComboBox.ValueField = "ROLES";
                    comboColumn.PropertiesComboBox.ValueType = typeof(string);
                    comboColumn.PropertiesComboBox.DropDownStyle = DropDownStyle.DropDown;


                }

            }
            catch (Exception)
            {

               
            }
         
        }

        void bindDll()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string sqlText = "select * from tbl_Roles order by roles asc";
                    SqlCommand cmd = new SqlCommand(sqlText);
                    cmd.Connection = con;
                    con.Open();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();

                    switch (userRole)
                    {
                        case "sys_admin":
                            dt.Rows.Remove(dt.Rows[1]);
                            dt.Columns.Add("TEXTROLE");
                            dt.Rows[0]["TEXTROLE"] = "Genel Kullanıcı";
                            dt.Rows[1]["TEXTROLE"] = "Sistem Moderatör";

                            break;
                        case "sys_mod":
                            dt.Rows.Remove(dt.Rows[2]);
                            dt.Rows.Remove(dt.Rows[1]);
                            dt.Columns.Add("TEXTROLE");
                            dt.Rows[0]["TEXTROLE"] = "Genel Kullanıcı";
                            break;
                        default:
                            break;
                    }


                    ddlRole.DataSource = dt;
                    ddlRole.DataValueField = "ROLES";
                    ddlRole.DataTextField = "TEXTROLE";
                    ddlRole.DataBind();
                    ListItem li = new ListItem();
                    li.Value = "0";
                    li.Text = "Rol Seçiniz";
                    ddlRole.Items.Insert(0, li);


                }
            }
            catch (Exception)
            {

                
            }
            
        }
        #endregion


        #region GridView Style And Edit Opr.

        protected void grdNew_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {
                int userID = Convert.ToInt32(e.Keys[0]);
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string sqlText = @" UPDATE tbl_User
                                     SET ACTIVE=0 WHERE ID_USER=@idUser";

                    SqlCommand cmd = new SqlCommand(sqlText);

                    cmd.Parameters.AddWithValue("@idUser", userID);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();


                }
                e.Cancel = true;
            }
            catch (Exception)
            {

                
            }
           
        }

        protected void grid_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            string name = e.GetValue("NAME").ToString();
            if (name == "ALİ")
                e.Row.BackColor = System.Drawing.Color.LightCyan;

        }

        protected void grdUsers_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                if (e.DataColumn.FieldName != "ROLE") return;

                switch (e.CellValue.ToString())
                {
                    case "sys_admin":
                        e.Cell.Text = "Sistem Admin";
                        e.Cell.BackColor = System.Drawing.Color.PaleVioletRed;
                        break;
                    case "basic_user":
                        e.Cell.Text = "Genel Kullanıcı";
                        e.Cell.BackColor = System.Drawing.Color.LightCyan;
                        break;
                    case "sys_mod":
                        e.Cell.Text = "Moderatör";
                        e.Cell.BackColor = System.Drawing.Color.Aqua;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {

                
            }
           

        }


        protected void grdUsers_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            bool error = false;
            try
            {
                string userID = e.Keys[0].ToString();
                string name = e.NewValues["NAME"].ToString();
                string lastName = e.NewValues["LASTNAME"].ToString();
                string email = e.NewValues["EMAIL_USER"].ToString();
                string userName = e.NewValues["USERNAME"].ToString();
                string role = e.NewValues["ROLE"].ToString();



                using (SqlConnection con = new SqlConnection(cs))
                {
                    string sqlText = @" UPDATE tbl_User
                                 SET NAME=@name, LASTNAME=@lastname, EMAIL_USER=@mail, USERNAME=@username, ROLE=@role  WHERE ID_USER=@idUser";

                    SqlCommand cmd = new SqlCommand(sqlText);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@lastname", lastName);
                    cmd.Parameters.AddWithValue("@mail", email);
                    cmd.Parameters.AddWithValue("@username", userName);
                    cmd.Parameters.AddWithValue("@role", role);
                    cmd.Parameters.AddWithValue("@idUser", Convert.ToInt32(userID));
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    bindGrid();
                    e.Cancel = true;
                    grdUsers.CancelEdit();
                    
                }



            }
            catch (Exception myExp)
            {

                error = true;

            }



        }
        protected void grdUsers_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            int userID = Convert.ToInt32(e.Keys[0]);
            using (SqlConnection con = new SqlConnection(cs))
            {
                string sqlText = @" UPDATE tbl_User
                                 SET ACTIVE=0 WHERE ID_USER=@idUser";

                SqlCommand cmd = new SqlCommand(sqlText);

                cmd.Parameters.AddWithValue("@idUser", userID);
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                bindGrid();
                e.Cancel = true;
                grdUsers.CancelEdit();

            }

        }

        protected void grdUsers_DataBound(object sender, EventArgs e)
        {

        }
        #endregion


        #region CallBacks
        protected void ASPxCallback1_Callback(object source, CallbackEventArgs e)
        {
            string chekUserName = txtUserName.Text;
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string sqlText = @" SELECT Count(USERNAME) as CountUserName from tbl_User where USERNAME like @username";
                SqlCommand cmd = new SqlCommand(sqlText);

                cmd.Parameters.AddWithValue("@username", chekUserName);
                cmd.Connection = con;
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
            }
            int count = Convert.ToInt32(dt.Rows[0]["CountUserName"]);
            if (count == 1)
            {
                e.Result = "nok";
            }
            else
            {
                e.Result = "ok";
            }
        } // insta check username


        protected void clbPanelDDL_Callback(object sender, CallbackEventArgsBase e)
        {
            bindDll();

        }
        #endregion



        protected void btnSaveNewUser_Click(object sender, EventArgs e)
        {
            string sqlText = "";
            string createDB="";
            string roleSql = hfDDLSelected.Value.ToString();
            try
            {
                int status;
                if (checkSendMail.Checked)
                {
                    status = 0;
                }
                else
                {
                    status = 1;
                }
                using (SqlConnection con = new SqlConnection(cs))
                {
                   
                    if (checkSendMail.Checked)
                    {
                        Mail.ConfirmMail sendMail = new Mail.ConfirmMail();
                        string link = "https://localhost:44316/confirmation/confirmAccount?username=" + txtUserName.Text + "&email=" + txtEmail.Text + "&key=ksQVLEfdtksfc9zk8BT8r9sVRrvDUmqX";
                        sendMail.MailGonder(txtEmail.Text, link, txtName.Text);

                    }
                    else
                    {
                      createDB = "CREATE DATABASE "+ txtUserName.Text + "";
                    }

                    if(createDB != "")
                    {
                         sqlText = @"INSERT INTO tbl_User 
                                    VALUES(@name,@surname,@mail,@username,@pass,@status,@role,@active) 
                                     " + createDB + "";
                    }
                    else
                    {
                        sqlText = @"INSERT INTO tbl_User 
                                    VALUES(@name,@surname,@mail,@username,@pass,@status,@role,@active)";
                    }
                    


                    SqlCommand cmd = new SqlCommand(sqlText);

                    cmd.Parameters.AddWithValue("@name", txtName.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@surname", txtLastName.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@mail", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@username", txtUserName.Text);
                    cmd.Parameters.AddWithValue("@pass", txtPass.Text);
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@role", roleSql);
                    cmd.Parameters.AddWithValue("@active", 1);

                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    txtEmail.Text = string.Empty;
                    txtName.Text = string.Empty;
                    txtLastName.Text = string.Empty;
                    txtUserName.Text = string.Empty;
                    txtPass.Text = string.Empty;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Kullanıcı Eklendi','yes')", true);
                    bindGrid();
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Kullanıcı ekleme sırasında hata oluştu','no')", true);
            }
        }

     
      
    }
}