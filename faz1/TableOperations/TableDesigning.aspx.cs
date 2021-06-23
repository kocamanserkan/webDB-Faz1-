using System;
using System.Collections;
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
    public partial class TableDesigning : System.Web.UI.Page
    {
        #region Data members

        DataTable dt = new DataTable();
        string ownerID = "";
        string userName = "";
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie reqCookies = Request.Cookies["sessionInfo"];
            if (reqCookies != null)
            {
                ownerID = reqCookies["userID"];
                userName = reqCookies["userName"];
            }
            else
            {
                Response.Redirect("https://localhost:44316/Login.aspx");
            }
            if (!IsPostBack)
            {
                bindDropDown();
                

            }

        }
        string userConnecitonString()
        {
            string UserCS = cs;
            string patternToReplace = @"\bakaStaj\b";
            UserCS = Regex.Replace(UserCS, patternToReplace, userName);
            return UserCS;
        }

        #region Bind Operations
        void bindDropDown()
        {
            using (SqlConnection con = new SqlConnection(userConnecitonString()))
            {
                string sqlText = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
                SqlCommand cmd = new SqlCommand(sqlText);
                cmd.Parameters.AddWithValue("@ownerID", Convert.ToInt32(ownerID));
                cmd.Connection = con;
                con.Open();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                ddlTables.DataValueField = "TABLE_NAME";
                ddlTables.DataSource = dt;
                ddlTables.DataBind();
                con.Close();
            }
        }

        void bindGrid()
        {
            using (SqlConnection con = new SqlConnection(userConnecitonString()))
            {
                string sqlText = "SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + ddlTables.SelectedValue + "' ";

                SqlCommand cmd = new SqlCommand(sqlText);

                cmd.Connection = con;
                con.Open();
                DataTable dt = new DataTable();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dt.Rows[i];
                    if (dr["COLUMN_NAME"].ToString() == "entryID")//Primary Key Kolon Gösterilmiyor
                        dr.Delete();
                }
                dt.AcceptChanges();

                con.Close();
                foreach (DataRow row in dt.Rows)// Data tiplerinin Türkçeleştirilmesi
                {
                    switch (row["DATA_TYPE"].ToString())
                    {
                        case "int":
                            row["DATA_TYPE"] = "Tam Sayı";
                            break;
                        case "varchar":
                            row["DATA_TYPE"] = "Metin";
                            break;
                        case "date":
                            row["DATA_TYPE"] = "Tarih";
                            break;
                        case "datetime":
                            row["DATA_TYPE"] = "Tarih ve Saat";
                            break;
                        case "decimal":
                            row["DATA_TYPE"] = "Ondalık";
                            break;
                        default:
                            break;
                    }
                }

                grdModifyTable.DataSource = dt;
                grdModifyTable.DataBind();
                ViewState["dt"] = dt;
                ArrayList sqlDeleteArray = new ArrayList(); //ViewState de çalışıcak Arraylar
                ArrayList sqlUpdateArray = new ArrayList();
                ArrayList sqlUpdateType = new ArrayList();
                ViewState["arDelete"] = sqlDeleteArray;
                ViewState["arUpdate"] = sqlUpdateArray;
                ViewState["arUpdateType"] = sqlUpdateType;
            }
        }

        void bindViewState()// Onaylanmadan önceki grid view bind işlemi
        {
            grdModifyTable.DataSource = ViewState["dt"] as DataTable;
            grdModifyTable.DataBind();

        }
        #endregion

        protected void ddlTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTables.SelectedValue != "1")
            {
                myModalLabel.InnerText = ddlTables.SelectedValue;
                bindGrid();
                
                pnlAlter.Visible = true;
                pnlFooter.Visible = true;
            }
            else
            {
               
                pnlAlter.Visible = false;
                pnlFooter.Visible = false;
                DataTable dtEmpty = new DataTable();
                grdModifyTable.DataSource = dtEmpty;
                grdModifyTable.DataBind();
                
            }
        }  //Trigger Bind

        #region GridView Operation

        protected void OnDelete(object sender, EventArgs e)
        {
            GridViewRow row = (sender as LinkButton).NamingContainer as GridViewRow;
            DataTable dt = ViewState["dt"] as DataTable;
            string dropColumnName = dt.Rows[row.RowIndex]["COLUMN_NAME"].ToString();
            dt.Rows.RemoveAt(row.RowIndex);
            ViewState["dt"] = dt;
            try
            {
                string sqltextDel = "Alter Table " + ddlTables.SelectedValue + " DROP COLUMN " + dropColumnName + " ";
                using (SqlConnection con = new SqlConnection(userConnecitonString()))
                {
                    SqlCommand cmd = new SqlCommand(sqltextDel);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    bindGrid();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Kolon Silindi','yes')", true);

                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Kolon Silinme Sırasında Hata Oluştu.','no')", true);
            }

        }

        protected void grdModifyTable_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                DataTable dt = ViewState["dt"] as DataTable;
                ArrayList updateColumnArray = ViewState["arUpdate"] as ArrayList;
                ArrayList updateColumType = ViewState["arUpdateType"] as ArrayList;
                string oldColunName = dt.Rows[e.RowIndex]["COLUMN_NAME"].ToString();
                string oldDataType = dt.Rows[e.RowIndex]["DATA_TYPE"].ToString();
                TextBox txtNewColumnName = grdModifyTable.Rows[e.RowIndex].FindControl("txt_KolonAd") as TextBox;
                DropDownList ddlNewDataType = grdModifyTable.Rows[e.RowIndex].FindControl("ddlDataTypeNew") as DropDownList;

                bool isColumnNameChanged = (oldColunName != txtNewColumnName.Text);
                bool isColumnDataTypeChanged = (oldDataType != ddlNewDataType.SelectedItem.Text);
                bool contains = dt.AsEnumerable().Any(row => txtNewColumnName.Text.ToLower() == row.Field<String>("COLUMN_NAME").ToLower());

                if (isColumnNameChanged && isColumnDataTypeChanged)
                {
                    if (contains == false)
                    {
                        string sqlUpNameText = "sp_rename '" + ddlTables.SelectedValue + "." + oldColunName + "', '" + txtNewColumnName.Text + "', 'COLUMN'";
                        string sqlFinalUpdateText = "";
                        string sqlUpDataType = "ALTER TABLE " + ddlTables.SelectedValue + " ";
                        string sqlUpDataTypeConvert = "TRUNCATE TABLE " + ddlTables.SelectedValue + " ALTER TABLE " + ddlTables.SelectedValue + " ";

                        switch (ddlNewDataType.SelectedValue)
                        {
                            case "varchar":
                                sqlUpDataType += ("ALTER COLUMN " + txtNewColumnName.Text + " " + ddlNewDataType.SelectedValue + "(120)");
                                sqlFinalUpdateText = sqlUpNameText + "  " + sqlUpDataType;
                                break;
                            case "decimal":
                                sqlUpDataTypeConvert += ("ALTER COLUMN " + txtNewColumnName.Text + " " + ddlNewDataType.SelectedValue + "(9,3)" + ";");
                                sqlFinalUpdateText = sqlUpNameText + "  " + sqlUpDataTypeConvert;
                                break;
                            case "int":
                            case "datetime":
                                sqlUpDataTypeConvert += ("ALTER COLUMN " + txtNewColumnName.Text + " " + ddlNewDataType.SelectedValue + ";");
                                sqlFinalUpdateText = sqlUpNameText + "  " + sqlUpDataTypeConvert;
                                break;
                            default:
                                break;
                        }

                        using (SqlConnection con = new SqlConnection(userConnecitonString()))
                        {

                            SqlCommand cmd = new SqlCommand(sqlFinalUpdateText);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }

                        dt.Rows[e.RowIndex]["COLUMN_NAME"] = txtNewColumnName.Text;
                        dt.Rows[e.RowIndex]["DATA_TYPE"] = ddlNewDataType.SelectedItem.Text;
                        ViewState["dt"] = dt;

                        grdModifyTable.EditIndex = -1;
                        bindGrid();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Kolon Adı ve Veri Tipi Güncellendi','yes')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Kolon Mevcut','no')", true);
                    }

                }
                else if (isColumnNameChanged == false && isColumnDataTypeChanged)
                {
                    string sqlUpNameText = "sp_rename '" + ddlTables.SelectedValue + "." + oldColunName + "', '" + txtNewColumnName.Text + "', 'COLUMN'";
                    string sqlFinalUpdateText = "";
                    string sqlUpDataType = "ALTER TABLE " + ddlTables.SelectedValue + " ";
                    string sqlUpDataTypeConvert = "TRUNCATE TABLE " + ddlTables.SelectedValue + " ALTER TABLE " + ddlTables.SelectedValue + " ";

                    switch (ddlNewDataType.SelectedValue)
                    {
                        case "varchar":
                            sqlUpDataType += ("ALTER COLUMN " + oldColunName + " " + ddlNewDataType.SelectedValue + "(120)");
                            sqlFinalUpdateText = sqlUpNameText + "  " + sqlUpDataType;
                            break;
                        case "decimal":
                            sqlUpDataTypeConvert += ("ALTER COLUMN " + oldColunName + " " + ddlNewDataType.SelectedValue + "(9,3)" + ";");
                            sqlFinalUpdateText = sqlUpNameText + "  " + sqlUpDataTypeConvert;
                            break;
                        case "int":
                        case "datetime":
                            sqlUpDataTypeConvert += ("ALTER COLUMN " + oldColunName + " " + ddlNewDataType.SelectedValue + ";");
                            sqlFinalUpdateText = sqlUpNameText + "  " + sqlUpDataTypeConvert;
                            break;
                        default:
                            break;
                    }

                    using (SqlConnection con = new SqlConnection(userConnecitonString()))
                    {

                        SqlCommand cmd = new SqlCommand(sqlFinalUpdateText);
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }

                    dt.Rows[e.RowIndex]["DATA_TYPE"] = ddlNewDataType.SelectedItem.Text;
                    ViewState["dt"] = dt;

                    grdModifyTable.EditIndex = -1;
                    bindGrid();

                    dt.Rows[e.RowIndex]["DATA_TYPE"] = ddlNewDataType.SelectedItem.Text;
                    ViewState["dt"] = dt;
                    grdModifyTable.EditIndex = -1;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Kolon Veri Tipi Güncellendi','yes')", true);

                }
                else if (isColumnNameChanged && isColumnDataTypeChanged == false)
                {
                    if (contains == false)
                    {
                        //updateColumnArray.Clear();
                        string sqlUpNameText = "sp_rename '" + ddlTables.SelectedValue + "." + oldColunName + "', '" + txtNewColumnName.Text + "', 'COLUMN'";

                        using (SqlConnection con = new SqlConnection(userConnecitonString()))
                        {

                            SqlCommand cmd = new SqlCommand(sqlUpNameText);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Kolon Adı Güncellendi','yes')", true);
                        dt.Rows[e.RowIndex]["COLUMN_NAME"] = txtNewColumnName.Text;
                        ViewState["dt"] = dt;
                        grdModifyTable.EditIndex = -1;
                        bindGrid();

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Kolon Mevcut','no')", true);

                    }

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Bir Değişiklik Yapmadınız','no')", true);


                    grdModifyTable.EditIndex = -1;
                    bindViewState();
                }
            }
            catch (Exception myExp)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Beklenmedik Bir Hata Oluştu. Dönüştürmek istedğiniz veri tipini önce metin türüne daha sonra kendi istediğiniz türe çevirebilirsiniz.','no')", true);
            }
          
        }

        protected void OnCancel(object sender, EventArgs e)
        {
            grdModifyTable.EditIndex = -1;
            bindGrid();
        }

        protected void grdModifyTable_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdModifyTable.EditIndex = e.NewEditIndex;
            bindGrid();
           
        }

        protected void grdModifyTable_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataRowView dRowView = (DataRowView)e.Row.DataItem;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    DropDownList ddl = (DropDownList)e.Row.FindControl("ddlDataTypeNew");
                    switch (dRowView[1].ToString())
                    {
                        case "Metin":
                            ddl.SelectedValue = "varchar";
                            break;
                        case "Tam Sayı":
                            ddl.SelectedValue = "int";
                            break;
                        case "Ondalık":
                            ddl.SelectedValue = "decimal";
                            break;
                        case "Tarih":
                            ddl.SelectedValue = "date";
                            break;
                        case "Tarih ve Saat":
                            ddl.SelectedValue = "datetime";
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        #endregion

        protected void addColumn_Click(object sender, EventArgs e)
        {
            try
            {
                string sqlText = "ALTER TABLE " + ddlTables.SelectedValue + " ADD " + txtNewColumnName.Text + " ";
                switch (ddlNewColumnData.SelectedValue.ToString())
                {
                    case "varchar":
                        if (cbkNewColumn.Checked)
                        {
                            sqlText = sqlText + "varchar(120)";
                        }
                        else
                        {
                            sqlText = sqlText + "varchar(120) NOT NULL ";
                        }

                        break;
                    case "int":
                        if (cbkNewColumn.Checked)
                        {
                            sqlText = sqlText + "int";
                        }
                        else
                        {
                            sqlText = sqlText + "int NOT NULL";
                        }

                        break;
                    case "decimal":
                        if (cbkNewColumn.Checked)
                        {
                            sqlText = sqlText + "decimal(9,3)";
                        }
                        else
                        {
                            sqlText = sqlText + "decimal(9,3) NOT NULL";
                        }

                        break;
                    case "date":
                        if (cbkNewColumn.Checked)
                        {
                            sqlText = sqlText + "date";
                        }
                        else
                        {
                            sqlText = sqlText + "date NOT NULL";
                        }

                        break;
                    case "datetime":
                        if (cbkNewColumn.Checked)
                        {
                            sqlText = sqlText + "datetime";
                        }
                        else
                        {
                            sqlText = sqlText + "datetime NOT NULL";
                        }

                        break;
                    default:
                        break;

                }
                DataTable dt = ViewState["dt"] as DataTable;
                bool contains = dt.AsEnumerable().Any(row => txtNewColumnName.Text.ToLower() == row.Field<String>("COLUMN_NAME").ToLower());
                if (contains == false)
                {
                    using (SqlConnection con = new SqlConnection(userConnecitonString()))
                    {
                        SqlCommand cmd = new SqlCommand(sqlText);
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Tabloya Kolon Eklendi.','yes')", true);
                        bindGrid();
                        txtNewColumnName.Text = string.Empty;

                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Eklemeye Çalıştığınız Kolon Mevcut','no')", true);
                }

            }
            catch (Exception)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Beklenmedik Bir Hata Oluştu','no')", true);
            }

        } //Adding New Column to Existing Table

        protected void btnDeleteTable_Click(object sender, EventArgs e)
        {
            string delTableBigTable = "DELETE FROM tbl_bigTable WHERE NAME_TABLE like '" + ddlTables.SelectedValue + "' ;";
            string dropTableSql = "DROP TABLE " + ddlTables.SelectedValue + ";";
            try
            {
                using (SqlConnection con = new SqlConnection(userConnecitonString()))
                {
                    SqlCommand cmd = new SqlCommand(dropTableSql);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Tablo Silindi','yes')", true);
                    bindGrid();
                }

                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand(delTableBigTable);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Tablo Silindi','yes')", true);
                    ddlTables.Items.Remove(ddlTables.SelectedItem);
                    pnlAlter.Visible = false;
                    pnlFooter.Visible = false;

                    ddlTables.SelectedValue = "1";
                }

            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Tablo Silinirken Hata Oluştu.','no')", true);
            }
        } // Deleting Table

       
    }
}