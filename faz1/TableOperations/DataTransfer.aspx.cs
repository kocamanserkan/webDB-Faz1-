using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
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


        public DataTable ReadExcel(string fileName, string fileExt)
        {
            string conn = string.Empty;
            Table = new DataTable();
            if (fileExt.CompareTo(".xls") == 0)
                conn = @"provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=2';"; //for below excel 2007  
            else
                conn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties='Excel 12.0;HDR=NO';"; //for above excel 2007  
            using (OleDbConnection con = new OleDbConnection(conn))
            {
                try
                {
                    OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [Sheet1$]", con); //here we read data from sheet1  
                    oleAdpt.Fill(Table); //fill excel data into dataTable  
                }
                catch { }
            }
            return Table;
        }



        void CastandConvert(DataTable mainTable, DataTable excelTable)
        {
            string errorMsg = "";
            excelTable.Columns.Add("ERRORMESSAGE", typeof(string));

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

                                errorMsg += (c + 1) + ".kolondaki değer Tam Sayı'ya çevrilemedi.";

                                string a = excelTable.Rows[i][c].ToString();
                            }

                            break;
                        case "Decimal":

                            try
                            {
                                excelTable.Rows[i][c] = Convert.ToDecimal(excelTable.Rows[i][c]);
                            }
                            catch (Exception)
                            {

                                errorMsg += (c + 1) + ".kolondaki değer Ondalığa çevrilemedi.";
                                string a = excelTable.Rows[i][c].ToString();
                            }
                            break;
                        case "String":
                            try
                            {
                                excelTable.Rows[i][c] = Convert.ToString(excelTable.Rows[i][c]);
                            }
                            catch (Exception)
                            {

                                errorMsg += (c + 1) + ".kolondaki değer Metin'e çevrilemedi.";
                                string a = excelTable.Rows[i][c].ToString();
                            }

                            break;
                        case "DateTime":
                            try
                            {
                                excelTable.Rows[i][c] = Convert.ToDateTime(excelTable.Rows[i][c]);
                            }
                            catch (Exception)
                            {

                                errorMsg += (c + 1) + ".kolondaki değer Tarih ve Saat'e çevrilemedi.";
                                string a = excelTable.Rows[i][c].ToString();
                            }

                            break;
                        case "Date":
                            break;
                        default:

                            break;
                    }
                }

                excelTable.Rows[i]["ERRORMESSAGE"] = errorMsg;
            }

            if (errorMsg.Length > 0)
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

            string strFilePath = string.Empty;

            if (fuImgPath.HasFile)
            {

                string extension = System.IO.Path.GetExtension(fuImgPath.FileName);

                string fileExtension = System.IO.Path.GetExtension(fuImgPath.FileName);

                //If file is not in excel format then return  
                if (fileExtension != ".xls" && fileExtension != ".xlsx")
                { return; }

                //Get the File name and create new path to save it on server  
                string fileLocation = Server.MapPath("~/uploadFiles/") + (fuImgPath.FileName);

                //if the File is exist on serevr then delete it  
                if (File.Exists(fileLocation))
                {
                    File.Delete(fileLocation);
                }
                //save the file lon the server before loading  
                fuImgPath.SaveAs(fileLocation);

                //Create the QueryString for differnt version of fexcel file  
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

                //Get the data from the excel sheet1 which is default  
                string query = "select * from  [Sheet1$]";
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

                    grdExcel.DataSource = Table;
                    grdExcel.DataBind();

                    //Delete the excel file from the server  
                    File.Delete(fileLocation);

                    CastandConvert(Table, dt);
                }
                else
                {

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Kolon Sayıları Eş Değil','no')", true);
                }
            }
            else{
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Lütfen dosya seçiniz','no')", true);
            }


        }

        protected void btnFinal_Click(object sender, EventArgs e)
        {
            DataTable insertTable = new DataTable();
            insertTable = ViewState["excelDT"] as DataTable;
            BulkInsert(insertTable, ddlSelectedTable.SelectedValue);

            bingGrid(ddlSelectedTable.SelectedValue);

        }

        public string BulkInsert(DataTable dt, string KaydedilecekTAbloAdı)
        {

         
            if(dt.Columns.IndexOf("ERRORMESSAGE") !=-1)
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
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[e.Row.Cells.Count-1].Text = "Hata Mesajı";
            }
        }
    }
}