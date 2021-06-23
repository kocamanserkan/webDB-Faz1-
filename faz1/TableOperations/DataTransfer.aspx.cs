using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace faz1.TableOperations
{
    public partial class DataTransfer : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        public static DataTable Table = new DataTable();
        string ownerID = "";
        string userName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                HttpCookie reqCookies = Request.Cookies["sessionInfo"];
                ownerID = reqCookies["userID"];
                userName = reqCookies["userName"];
                if (!IsPostBack)
                {
                    bindDropDown();
                }
            }
            catch (Exception)
            {
                Response.Write("alert('Beklenmedik bir hata oluştu. Giriş sayfasına yönlendiriliyorsunuz.')");
                Response.AddHeader("REFRESH", "2;URL=https://localhost:44316/login.aspx/login.aspx");
            }
        }

        string userConnecitonString()
        {
            string UserCS = cs;
            string patternToReplace = @"\bakaStaj\b";
            UserCS = Regex.Replace(UserCS, patternToReplace, userName);
            return UserCS;
        }

        void bindDropDown()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(userConnecitonString()))
                {
                    string sqlText = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";

                    SqlCommand cmd = new SqlCommand(sqlText);
                    cmd.Connection = con;
                    con.Open();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    ddlSelectedTable.DataSource = dt;
                    ddlSelectedTable.DataValueField = "TABLE_NAME";
                    ddlSelectedTable.DataBind();

                    con.Close();
                }
            }
            catch (Exception myExp)
            {
                //
            }

        }
        void bingGrid(string tablename)
        {
            using (SqlConnection con = new SqlConnection(userConnecitonString()))
            {
                string sqlText = "SELECT * from " + tablename + "";
                SqlCommand cmd = new SqlCommand(sqlText);
                //cmd.Parameters.AddWithValue("@tablename", tablename);
                cmd.Connection = con;
                con.Open();
                Table = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                Table.Clear();
                da.Fill(Table);
                grdTable.DataSource = Table;
                grdTable.DataBind();
                con.Close();
            }
        }

        void bindExcelGrid(string selectedSheet)
        {
            try
            {
                string strConn = ViewState["excelCS"].ToString();
                string query = "select * from  [" + selectedSheet + "$]";
                OleDbConnection objConn;
                OleDbDataAdapter oleDA;
                //DataTable dt = new DataTable();
                objConn = new OleDbConnection(strConn);
                objConn.Open();
                oleDA = new OleDbDataAdapter(query, objConn);
                DataTable dt = new DataTable();
                oleDA.Fill(dt);
                objConn.Close();
                oleDA.Dispose();
                objConn.Dispose();
                //Bind the datatable to the Grid  
                grdExcel.Columns.Clear();

                if (Table.Columns.Count - 1 == dt.Columns.Count)
                {
                    grdExcel.DataSource = dt;
                    grdExcel.DataBind();
                    //Delete the excel file from the server  
                    //File.Delete(fileLocation);
                    CastandConvert(Table, dt);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Kolon Sayıları Eş Değil','no')", true);
                }
            }
            catch (Exception)
            {


                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Excel tablosu doldurulurken hata oluştu.','no')", true);
            }
           
        }
        protected void ddlSelectedTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSelectedTable.SelectedValue != "1")
            {

                lblTableName.Text = ddlSelectedTable.SelectedValue;
                pnlExcelUpload.Visible = true;
                pnlGrd.Visible = true;
                bingGrid(ddlSelectedTable.SelectedValue);

            }
            else
            {
                pnlGrd.Visible = false;
                pnlExcelUpload.Visible = false;
            }
        }

        protected void grdTable_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[0].Visible = false;
        }


        void bindDLLexcelSheets()
        {
            if (fuImgPath.HasFile)
            {
                ddlExcelSheets.Visible = true;
                ddlExcelSheets.Items.Clear();

                string strFilePath = string.Empty;

                if (fuImgPath.HasFile)
                {
                    string extension = System.IO.Path.GetExtension(fuImgPath.FileName);
                    string fileExtension = System.IO.Path.GetExtension(fuImgPath.FileName);

                    if (fileExtension != ".xls" && fileExtension != ".xlsx")  //Checking File extention
                    { return; }

                    string fileLocation = Server.MapPath("~/uploadFiles/") + (fuImgPath.FileName);//File Location

                    if (File.Exists(fileLocation))//if the File is exist on server delete 
                    {
                        File.Delete(fileLocation);
                    }
                    fuImgPath.SaveAs(fileLocation); // Saving File

                    string strConn = "";
                    switch (fileExtension)
                    {
                        case ".xls": //Excel 1997-2003  
                            strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation
            + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                            break;
                        case ".xlsx": //Excel 2007-2010  
                            strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation
            + ";Extended Properties=\"Excel 12.0 xml;HDR=Yes;IMEX=2\"";
                            break;
                    }
                    ViewState["excelCS"] = strConn;
                    //Get the data from the excel sheet1 which is default  
                    
                    OleDbConnection objConn;
                    objConn = new OleDbConnection(strConn);
                    objConn.Open();
                    DataTable dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    objConn.Close();
                    objConn.Dispose();

                    foreach (DataRow item in dt.Rows)
                    {
                        item["TABLE_NAME"] = item["TABLE_NAME"].ToString().Remove(item["TABLE_NAME"].ToString().LastIndexOf('$'));
                    }

                    ddlExcelSheets.DataSource = dt;
                    ddlExcelSheets.DataValueField = "TABLE_NAME";
                    ddlExcelSheets.DataBind();
                    ListItem no1 = new ListItem("Sayfa Seçiniz", "1");
                    ddlExcelSheets.Items.Insert(0, no1);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Lütfen dosya seçiniz','no')", true);
            }

        }


        void CastandConvert(DataTable mainTable, DataTable excelTable)
        {
            DataTable oldExcelTable = new DataTable();
            oldExcelTable = excelTable;
            string errorMsg = "";
            excelTable.Columns.Add("ERRORMESSAGE", typeof(string));
            int errorCount = 0;
            for (int i = 0; i < excelTable.Rows.Count; i++)
            {
                errorMsg = "";

                for (int c = 0; c < excelTable.Columns.Count - 1; c++)
                {
                    switch (mainTable.Columns[c + 1].DataType.Name)
                    {
                        case "Int32":
                            try
                            {
                                excelTable.Rows[i][c] = Convert.ToInt32(excelTable.Rows[i][c]);

                            }
                            catch (Exception)
                            {

                                errorMsg += excelTable.Columns[c].ColumnName + " kolonundaki değer Tam Sayı'ya çevrilemedi. (Örn:12)| ";
                                excelTable.Rows[i][c] = excelTable.Rows[i][c] + " (Hata!)";
                                excelTable.AcceptChanges();
                                errorCount++;
                            }

                            break;
                        case "Decimal":

                            try
                            {
                                excelTable.Rows[i][c] = Convert.ToDecimal(excelTable.Rows[i][c]);
                            }
                            catch (Exception)
                            {

                                errorMsg += excelTable.Columns[c].ColumnName + " kolonundaki değer ondalığa çevrilemedi. (Örn, 33,12)|";
                                excelTable.Rows[i][c] = excelTable.Rows[i][c] + " (Hata!)";
                                string a = excelTable.Rows[i][c].ToString();
                                errorCount++;
                            }
                            break;
                        case "String":
                            try
                            {
                                excelTable.Rows[i][c] = Convert.ToString(excelTable.Rows[i][c]);
                                if (excelTable.Rows[i][c].ToString().Length > 120)
                                {
                                    errorMsg += excelTable.Columns[c].ColumnName + " kolonundaki değer 120 Karekterden Fazla";
                                    excelTable.Rows[i][c] = excelTable.Rows[i][c] + " (Hata!)";
                                }
                            }
                            catch (Exception)
                            {

                                errorMsg += excelTable.Columns[c].ColumnName + " kolonundaki değer Metin'e çevrilemedi.|";
                                string a = excelTable.Rows[i][c].ToString();
                                excelTable.Rows[i][c] = excelTable.Rows[i][c] + " (Hata!)";
                                errorCount++;
                            }

                            break;
                        case "DateTime":
                            try
                            {
                                excelTable.Rows[i][c] = Convert.ToDateTime(excelTable.Rows[i][c]);
                            }
                            catch (Exception)
                            {

                                errorMsg += excelTable.Columns[c].ColumnName + " kolonundaki değer Tarih ve Saat'e çevrilemedi. (Örn:22.06.2021 15:34:32)|";
                                string a = excelTable.Rows[i][c].ToString();
                                excelTable.Rows[i][c] = excelTable.Rows[i][c] + " (Hata!)";
                                errorCount++;
                            }
                            break;
                        case "Date":
                            break;
                        default:

                            break;
                    }
                }

                if (errorMsg.Length > 0)
                {
                    excelTable.Rows[i]["ERRORMESSAGE"] = errorMsg;

                }
                else
                {
                    excelTable.Rows[i]["ERRORMESSAGE"] = "Hata Yok";
                }

            }


            if (errorCount > 0)
            {
                btnFinal.Visible = false;
            }
            else
            {
                btnFinal.Visible = true;
                ViewState["excelDT"] = excelTable;
            }

            grdExcel.DataSource = excelTable;
            grdExcel.DataBind();
        }
        protected void btnTransfer_Click(object sender, EventArgs e)
        {
            bindDLLexcelSheets();
        }

        protected void btnFinal_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable insertTable = new DataTable();
                insertTable = ViewState["excelDT"] as DataTable;
                BulkInsert(insertTable, ddlSelectedTable.SelectedValue);
                bingGrid(ddlSelectedTable.SelectedValue);
                lblMsg.Text = "";
            }
            catch (Exception)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Veri Transferi Sırasında Hata Oluştu','no')", true);
            }

        }

        public string BulkInsert(DataTable dt, string KaydedilecekTAbloAdı)
        {

            if (dt.Columns.IndexOf("ERRORMESSAGE") != -1)
            {
                dt.Columns.Remove("ERRORMESSAGE");
            }
            if (dt.Columns[0].ColumnName != "Better")
            {
                dt.Columns.Add("Better", typeof(Boolean)).SetOrdinal(0);
            }

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(userConnecitonString()))
            {
                bulkCopy.DestinationTableName = KaydedilecekTAbloAdı;
                try
                {
                    bulkCopy.WriteToServer(dt);


                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Veri aktarımı başarıyla tamamlandı.','yes')", true);
                    ViewState["transferCount"] = 1;
                    ddlExcelSheets.Visible = false;
                    dt.Clear();
                    grdExcel.DataSource = dt;
                    grdExcel.DataBind();
                    btnFinal.Visible = false;
                    return ("Aktarım Tamamlandı");

                }
                catch (Exception ex)
                {
                    return (ex.Message);
                }
            }
        }
        protected void grdExcel_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[e.Row.Cells.Count - 1].Text = "Hata Mesajı";

                    e.Row.Cells[e.Row.Cells.Count - 1].BackColor = System.Drawing.Color.FromArgb(1, 242, 92, 92);
                    e.Row.Cells[e.Row.Cells.Count - 1].ForeColor = System.Drawing.Color.FromArgb(1, 0, 0, 0);

                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    string newLine = e.Row.Cells[e.Row.Cells.Count - 1].Text;
                    if (newLine != "Hata Yok")
                    {
                        e.Row.Cells[e.Row.Cells.Count - 1].Text = newLine.Replace("|", "<br />");
                        e.Row.Cells[e.Row.Cells.Count - 1].BackColor = System.Drawing.Color.FromArgb(0, 255, 160, 160);
                        e.Row.Cells[e.Row.Cells.Count - 1].ForeColor = System.Drawing.Color.FromArgb(1, 0, 0, 0);

                    }
                    else
                    {
                        e.Row.Cells[e.Row.Cells.Count - 1].BackColor = System.Drawing.Color.LightGreen;
                        e.Row.Cells[e.Row.Cells.Count - 1].ForeColor = System.Drawing.Color.FromArgb(1, 0, 0, 0);
                    }


                    for (int c = 0; c < e.Row.Cells.Count - 1; c++)
                    {
                        if ((e.Row.Cells[c].Text).Contains("Hata!"))
                        {
                            e.Row.Cells[c].BackColor = System.Drawing.Color.FromArgb(0, 255, 160, 160);
                            e.Row.Cells[c].ForeColor = System.Drawing.Color.FromArgb(1, 0, 0, 0);
                        }
                    }


                }
            }
            catch (Exception)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Excel verilerini gösterme sırasında hata oluştu.','no')", true);
            }

        }

        protected void ddlExcelSheets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlExcelSheets.SelectedValue != "1")
            {
                DataTable emptyDT = new DataTable();
                grdExcel.DataSource = emptyDT;
                grdExcel.DataBind();
                bindExcelGrid(ddlExcelSheets.SelectedValue);

            }

        }
    }
}