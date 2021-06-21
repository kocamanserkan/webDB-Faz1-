using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace faz1.TableOperations
{
    public partial class MyTables : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        string ownerID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                HttpCookie reqCookies = Request.Cookies["sessionInfo"];
                if (reqCookies != null)
                {
                    ownerID = reqCookies["userID"];
                }
                else
                {
                    Response.Redirect("https://localhost:44316/Login.aspx");
                }

                if (!IsPostBack)
                {
                    bindList();
                }
            }
            catch (Exception)
            {

                Response.Redirect("https://localhost:44316/Login.aspx");
            }
           

        }
        private void bindList() // displaying list of user's Table
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string sqlText = "SELECT TOP 10 NAME_TABLE, CREATED_DATE from tbl_bigTable" +
                                 " WHERE OWNER_ID = @ownerID order by CREATED_DATE DESC";
                var html = "";
                SqlCommand cmd = new SqlCommand(sqlText);
                cmd.Parameters.AddWithValue("@ownerID", Convert.ToInt32(ownerID));
                cmd.Connection = con;
                con.Open();
               SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    txtThead.Visible = true;
                    while (rdr.Read())
                    {
                        
                        html += "<tr> <th>" + rdr["NAME_TABLE"] + " </th>   <th>" + rdr["CREATED_DATE"] + " </th>  </tr> ";
                    }
                }
                else
                {
                    txtThead.Visible = false;
                    html = "<tr> <th> Henüz Tablo Oluşturulmamıştır. </th>  </tr> ";
                }

                con.Close();
                tableBody.InnerHtml = html;
            }

        }
        protected void btnOpenCreateField_Click(object sender, EventArgs e)// Redirecting to CreateTable Page
        {
            Response.Redirect("https://localhost:44316/TableOperations/CreateTable.aspx");

        }
    }
}