using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace faz1
{
    public partial class WebForm1 : System.Web.UI.Page
    {

        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        public static DataTable Table = new DataTable();
       
        protected void Page_Load(object sender, EventArgs e)
        {

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

        protected void btnOpen_Click(object sender, EventArgs e)
        { //if File is not selected then return  
            if (Request.Files["FileUpload1"].ContentLength <= 0)
            { return; }

            //Get the file extension  
            string fileExtension = Path.GetExtension(Request.Files["FileUpload1"].FileName);

            //If file is not in excel format then return  
            if (fileExtension != ".xls" && fileExtension != ".xlsx")
            { return; }

            //Get the File name and create new path to save it on server  
            string fileLocation = Server.MapPath("~/uploadFiles/") + Request.Files["FileUpload1"].FileName;

            //if the File is exist on serevr then delete it  
            if (File.Exists(fileLocation))
            {
                File.Delete(fileLocation);
            }
            //save the file lon the server before loading  
            Request.Files["FileUpload1"].SaveAs(fileLocation);

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
            Table.Clear();
            oleDA.Fill(Table);
            objConn.Close();
            oleDA.Dispose();
            objConn.Dispose();

            //Bind the datatable to the Grid  
            GridView1.Columns.Clear();
            GridView1.DataSource = Table;
            GridView1.DataBind();

            //Delete the excel file from the server  
            File.Delete(fileLocation);

            
        }
        string userConnecitonString()
        {
            string UserCS = cs;
            string patternToReplace = @"\bakaStaj\b";
            UserCS = Regex.Replace(UserCS, patternToReplace, "sekoccan");
            return UserCS;
        }

        public StringBuilder createTableOnMSSQL()
        {

            StringBuilder sql = new StringBuilder("CREATE TABLE " + txtTableName.Text + " (" );
            foreach (DataColumn col in Table.Columns)
            { 
                sql.Append(""+col.ColumnName+" ");
                sql.Append(NetType2SqlType(col.DataType.ToString(), col.MaxLength) + ", "); 

            }

            sql.Append(")");
            return sql;

        }


        protected void importDB_Click(object sender, EventArgs e)
        {


             string a = createTableOnMSSQL().ToString();





        }

        private String NetType2SqlType(String netType, int maxLength)
        {
            String sqlType = "";

            // Map the .NET type to the data source type.
            // This is not perfect because mappings are not always one-to-one.
            switch (netType)
            {
                case "System.Boolean":
                    sqlType = "[bit]";
                    break;
                case "System.Byte":
                    sqlType = "[tinyint]";
                    break;
                case "System.Int16":
                    sqlType = "[smallint]";
                    break;
                case "System.Int32":
                    sqlType = "[int]";
                    break;
                case "System.Int64":
                    sqlType = "[bigint]";
                    break;
                case "System.Byte[]":
                    sqlType = "[binary]";
                    break;
                case "System.Char[]":
                    sqlType = "[nchar] (" + maxLength + ")";
                    break;
                case "System.String":
                    if (maxLength == 0x3FFFFFFF)
                        sqlType = "[ntext]";
                    else
                        sqlType = " varchar (120)";
                    break;
                case "System.Single":
                    sqlType = "[real]";
                    break;
                case "System.Double":
                    sqlType = "float";
                    break;
                case "System.Decimal":
                    sqlType = "[decimal]";
                    break;
                case "System.DateTime":
                    sqlType = "[datetime]";
                    break;
                case "System.Guid":
                    sqlType = "[uniqueidentifier]";
                    break;
                case "System.Object":
                    sqlType = "[sql_variant]";
                    break;
            }

            return sqlType;
        }
    }
}