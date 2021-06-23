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

namespace faz1.TableOperations
{
    public partial class CreateTable : System.Web.UI.Page
    {
        string tableName = "";
        string ownerID = "";
        string userName = "";
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        string url = ConfigurationManager.AppSettings["BaseUrl"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("kolonAdi");
                dt.Columns.Add("kolonData");
                dt.Columns.Add("nullable");
                ViewState["dt"] = dt;
                bindGrid();
            }

            HttpCookie reqCookies = Request.Cookies["sessionInfo"];
            if (reqCookies != null)
            {
                ownerID = reqCookies["userID"];
                userName = reqCookies["userName"];
            }
            else
            {
                Response.Redirect(""+ url + "/Login.aspx");
            }

        }

        #region Methods
        protected void bindGrid()
        {
            grdMyTable.DataSource = ViewState["dt"] as DataTable;
            grdMyTable.DataBind();
        }
        string userConnecitonString()
        {
            string UserCS = cs;
            string patternToReplace = @"\bakaStaj\b";
            UserCS = Regex.Replace(UserCS, patternToReplace, userName);
            return UserCS;
        }
        private bool createTableOnSQL()
        {
            try
            {
                string sql = "asd";
                string[] tableItems = sql.Split('/');
                tableName = tableItems[tableItems.Length - 2];
                string sqlText = "CREATE TABLE " + tableName + "(";

                for (int i = 0; i < tableItems.Length - 2; i++)
                {
                    sqlText += tableItems[i] + " VARCHAR(50),";
                }
                sqlText = sqlText + ")";

                using (SqlConnection con = new SqlConnection(userConnecitonString()))
                {
                    SqlCommand cmd = new SqlCommand(sqlText);

                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private bool addTabletoList(string tableName, int ownerID)
        {
            string sqlText = "INSERT INTO tbl_bigTable(NAME_TABLE, OWNER_ID)" +
                                 " VALUES (@tableName, @OwnerID)";
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand(sqlText);

                    cmd.Parameters.AddWithValue("@tableName", tableName);
                    cmd.Parameters.AddWithValue("@OwnerID", ownerID);

                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        protected void btnCreateTableSQL_Click(object sender, EventArgs e)
        {
            try
            {
                if (createTableOnSQL())
                {
                    if (addTabletoList(tableName, Convert.ToInt32(ownerID)))
                    {

                    }
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Tablo oluşturma sırasında hata oluştu','yes')", true);
            }
        }
        private string createTableQuery()
        {
            string query = "Create Table " + txtTableName.Text + " (entryID int IDENTITY(1,1) PRIMARY KEY, ";
            try
            {
                DataTable dt = ViewState["dt"] as DataTable;
                foreach (DataRow row in dt.Rows)
                {
                    switch (row["kolonData"].ToString())
                    {
                        case "Metin":
                            if (row["nullable"].ToString() == "Evet")
                            {
                                query += row["kolonAdi"] + " VarChar(120), ";
                            }
                            else
                            {
                                query += row["kolonAdi"] + " VarChar(120) NOT NULL, ";
                            }
                            break;
                        case "Tamsayı":
                            if (row["nullable"].ToString() == "Evet")
                            {
                                query += row["kolonAdi"] + " int, ";
                            }
                            else
                            {
                                query += row["kolonAdi"] + " int NOT NULL, ";
                            }
                            break;
                        case "Ondalık":
                            if (row["nullable"].ToString() == "Evet")
                                query += row["kolonAdi"] + " decimal(9,3), ";
                            else
                                query += row["kolonAdi"] + " decimal(9,3) NOT NULL, ";
                            break;
                        case "Tarih":
                            if (row["nullable"].ToString() == "Evet")
                                query += row["kolonAdi"] + " date , ";
                            else
                                query += row["kolonAdi"] + " date NOT NULL, ";
                            break;
                        case "Tarih ve Saat":
                            if (row["nullable"].ToString() == "Evet")
                                query += row["kolonAdi"] + " datetime, ";
                            else
                                query += row["kolonAdi"] + " datetime NOT NULL, ";
                            break;

                        default:
                            break;
                    }
                }
                query = query + ")";
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Bir Hata Oluştu','No')", true);

            }
            return query;
        }
        private bool checkTableIsAvaible()//cheking is table existing
        {
            string sqlText = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
            try
            {
                using (SqlConnection con = new SqlConnection(userConnecitonString()))
                {
                    SqlCommand cmd = new SqlCommand(sqlText);
                    cmd.Connection = con;
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable checkTable = new DataTable();
                    da.Fill(checkTable);
                    con.Close();
                    bool tableAvaible = true;

                    foreach (DataRow dr in checkTable.Rows)
                    {
                        if (dr["TABLE_NAME"].ToString() == txtTableName.Text)
                        {
                            tableAvaible = false;
                        }
                    }

                    return tableAvaible;
                }

            }
            catch (Exception)
            {

                return false;
            }

        }
        #endregion

        #region GridView Operations
        protected void btnAddColumn_Click(object sender, EventArgs e)
        {
            string nullable = "";
            DataTable dt = ViewState["dt"] as DataTable;
            bool contains = dt.AsEnumerable().Any(row => txtKolonAd.Text.ToLower() == row.Field<String>("kolonAdi").ToLower());

            if (contains == false)
            {

                if (ckbBos.Checked)
                {
                    nullable = "Evet";
                }
                else
                {
                    nullable = "Hayır";
                }

                dt.Rows.Add(txtKolonAd.Text, ddlDataType.SelectedItem, nullable);
                ViewState["dt"] = dt;
                bindGrid();
                txtKolonAd.Text = string.Empty;
                ddlDataType.SelectedValue = "-1";
                ckbBos.Checked = true;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Kolon Mevcut','no')", true);


            }

        }

        protected void grdMyTable_RowEditing1(object sender, GridViewEditEventArgs e)
        {
            grdMyTable.EditIndex = e.NewEditIndex;
            bindGrid();
        }

        protected void OnDelete(object sender, EventArgs e)
        {
            GridViewRow row = (sender as LinkButton).NamingContainer as GridViewRow;
            DataTable dt = ViewState["dt"] as DataTable;
            dt.Rows.RemoveAt(row.RowIndex);
            ViewState["dt"] = dt;
            bindGrid();
        }
        protected void OnCancel(object sender, EventArgs e)
        {
            grdMyTable.EditIndex = -1;
            bindGrid();
        }

        protected void grdMyTable_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            TextBox ad = grdMyTable.Rows[e.RowIndex].FindControl("txt_KolonAd") as TextBox;
            string asd = ad.Text;
            DropDownList ddl = grdMyTable.Rows[e.RowIndex].FindControl("ddlDataTypeNew") as DropDownList;
            CheckBox ckb = grdMyTable.Rows[e.RowIndex].FindControl("cbNew") as CheckBox;

            string nullable_Data = "";
            if (ckb.Checked)
            {
                nullable_Data = "Evet";
            }
            else
            {
                nullable_Data = "Hayır";
            }

            DataTable dt = ViewState["dt"] as DataTable;
            dt.Rows[e.RowIndex]["kolonAdi"] = asd;
            dt.Rows[e.RowIndex]["kolonData"] = ddl.SelectedItem.Text;
            dt.Rows[e.RowIndex]["nullable"] = nullable_Data;

            //dt.Rows[row.RowIndex]["nullable"] = dept;
            ViewState["dt"] = dt;
            grdMyTable.EditIndex = -1;

            bindGrid();
        }

        protected void grdMyTable_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataRowView dRowView = (DataRowView)e.Row.DataItem;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    DropDownList ddl = (DropDownList)e.Row.FindControl("ddlDataTypeNew");
                    CheckBox cbx = (CheckBox)e.Row.FindControl("cbNew");
                    if (dRowView[2].ToString() == "Evet")
                    {
                        cbx.Checked = true;
                    }
                    else
                    {
                        cbx.Checked = false;
                    }
                    ddl.SelectedItem.Text = dRowView[1].ToString();
                }
            }

        }

        protected void btnDeleteAllColumns_Click(object sender, EventArgs e)
        {
            DataTable dt = ViewState["dt"] as DataTable;
            dt.Clear();
            ViewState["dt"] = dt;
            bindGrid();
        }
        #endregion

        protected void btnOnayla_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    if (checkTableIsAvaible())
                    {
                        string sqlText = createTableQuery();
                        using (SqlConnection con = new SqlConnection(userConnecitonString()))
                        {
                            SqlCommand cmd = new SqlCommand(sqlText);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                            addTabletoList(txtTableName.Text, Convert.ToInt32(ownerID));//Adding Table to BiG Table
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Tablo Oluşturuldu','yes')", true);
                            Response.AddHeader("REFRESH", "3;URL=" + url + "/TableOperations/MyTables.aspx");
                        }
                    }

                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Tablo Oluşturulmadı. Tablo Adı Mevcut','no')", true);
                    }
                }
                catch (Exception)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Tablo Oluşturulmadı. Beklenmedik bir Hata Oluştu','no')", true);
                }
            }
        }
        protected void upPanel_Load(object sender, EventArgs e)
        {
        }
    }
}