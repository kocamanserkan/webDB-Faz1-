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
    public partial class WebForm2 : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindGrid();
            }


        }

        void bindGrid()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string sqlText = "SELECT * From tbl_User";

                SqlCommand cmd = new SqlCommand(sqlText);

                cmd.Connection = con;
                con.Open();
                DataSet ds = new DataSet();
                ds.Clear();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
                DataTable dt = ds.Tables[0];
                grid.DataSource = dt;
                grid.DataBind();
            }
        }

        protected void grid_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName != "USERNAME") return;
            if (e.CellValue.ToString().Contains("sekoccan"))
            {
                e.Cell.Text = "ADMİN";
                e.Cell.BackColor = System.Drawing.Color.DarkRed;

            }
        }

        protected void grid_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {

            string name = e.GetValue("NAME").ToString();
            if (name == "ALİ")
                e.Row.BackColor = System.Drawing.Color.LightCyan;

        }

        protected void grid_RowDeleting1(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {

        }

        
    }
}