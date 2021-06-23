using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace faz1.TableOperations
{
    public partial class DataInput : System.Web.UI.Page
    {
        #region Data Members
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        string ownerID = "";
        string userName = "";
        public static DataTable Table = new DataTable();
        public static DataTable isNullableDT = new DataTable();
        ArrayList ParameterArray = new ArrayList();
        public static ArrayList placeHolder = new ArrayList();
        public static ArrayList pHForNull = new ArrayList();
        #endregion
        protected void Page_Init(object sender, EventArgs e)
        {
            List<string> keys = Request.Form.AllKeys.Where(key => key.Contains("txtbox")).ToList();
            int i = 1;
            foreach (string key in keys)
            {
                CreateTextBox();
                i++;
            }
        }
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
                bindDDL();
            }
        }
        string userConnecitonString()//Creating Connection String for Current User
        {
            string UserCS = cs;
            string patternToReplace = @"\bakaStaj\b";
            UserCS = Regex.Replace(UserCS, patternToReplace, userName);
            return UserCS;
        }

        #region DataBind Operation
        void bindInfo(string tableName)// Binding Table Information

        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string sqlText = "SELECT CREATED_DATE, LAST_MODIFIED_DATE from tbl_bigTable" +
                                 " WHERE NAME_TABLE = @nametable ";

                SqlCommand cmd = new SqlCommand(sqlText);
                cmd.Parameters.AddWithValue("@nametable", tableName);
                cmd.Connection = con;
                con.Open();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                lblCreateDate.Text = dt.Rows[0][0].ToString();
                lblModifyDate.Text = dt.Rows[0][1].ToString();
                myModalLabel.InnerText = ddlTables.SelectedValue;
                if (string.IsNullOrEmpty(lblModifyDate.Text))
                {
                    lblModifyDate.Text = "Güncellenmemiş";
                }
                con.Close();
            }
        } 
        private void bindDDL()// Bindig list of tables
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
                ddlTables.DataSource = dt;
                ddlTables.DataValueField = "TABLE_NAME";
                ddlTables.DataBind();
                con.Close();
            }
        } 

        private void bindGrid(string tablename)// Binding Gridview with selected Tables
        {
            using (SqlConnection con = new SqlConnection(userConnecitonString()))
            {
                string sqlText = "SELECT * FROM " + tablename + "";
                SqlCommand cmd = new SqlCommand(sqlText);
                cmd.Connection = con;
                con.Open();
                Table = new DataTable();

                grdTable.Columns.Clear();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(Table);
                con.Close();
                Table.Columns[0].ColumnName = "Kayıt Sırası(Değer Girilmez)";
                placeHolder.Clear();
                pHForNull.Clear();
                for (int i = 1; i < Table.Columns.Count; i++)
                {
                    switch (Table.Columns[i].DataType.Name)
                    {
                        case "Int32":
                            Table.Columns[i].ColumnName = Table.Columns[i].ColumnName + " (Tam Sayı)";
                            placeHolder.Add(Table.Columns[i].ColumnName);
                            break;
                        case "Decimal":
                            Table.Columns[i].ColumnName = Table.Columns[i].ColumnName + " (Ondalık)";
                            placeHolder.Add(Table.Columns[i].ColumnName);
                            break;
                        case "String":
                            Table.Columns[i].ColumnName = Table.Columns[i].ColumnName + " (Metin)";
                            placeHolder.Add(Table.Columns[i].ColumnName);
                            break;
                        case "DateTime":
                            Table.Columns[i].ColumnName = Table.Columns[i].ColumnName + " (Tarih ve Saat)";
                            placeHolder.Add(Table.Columns[i].ColumnName);
                            break;
                        case "Date":
                            Table.Columns[i].ColumnName = Table.Columns[i].ColumnName + " (Tarih)";
                            placeHolder.Add(Table.Columns[i].ColumnName);
                            break;
                        default:
                            Table.Columns[i].ColumnName = Table.Columns[i].ColumnName + " (Bilinmeyen)";
                            placeHolder.Add(Table.Columns[i].ColumnName);
                            break;
                    }
                }

            }
            using (SqlConnection con = new SqlConnection(userConnecitonString()))
            {
                string sqlText = "SELECT is_nullable FROM INFORMATION_SCHEMA.COLUMNS WHERE table_name = '" + tablename + "'";
                SqlCommand cmd = new SqlCommand(sqlText);
                cmd.Connection = con;
                con.Open();
                isNullableDT = new DataTable();

                grdTable.Columns.Clear();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(isNullableDT);
                string c = isNullableDT.Rows[1][0].ToString();

                con.Close();

            }

            for (int i = 1; i < isNullableDT.Rows.Count; i++)
            {

                switch (isNullableDT.Rows[i][0].ToString()) //Collecting Data From Columns/ Is Columns Nullable?
                {
                    case "YES":
                        pHForNull.Add("(Boş Geçilebilir)");
                        break;
                    case "NO":
                        pHForNull.Add("(Boş Geçilemez)");
                        break;
                    default:
                        break;
                }
            }


            grdTable.DataSource = Table;
            grdTable.DataBind();


            for (int i = 1; i < Table.Columns.Count; i++) // Creating Texbox with number of columns
            {
                CreateTextBox();
            }
        } 

        #endregion

        protected void ddlTables_TextChanged(object sender, EventArgs e)
        {
            if (ddlTables.SelectedValue != "1")
            {
                myPel.Controls.Clear();
                string tableName = ddlTables.SelectedValue.ToString();
                pnlFooterBtns.Visible = true;
                bindGrid(tableName);
                bindInfo(tableName);
                btnInfoModal.Visible = true;
                grdTable.Visible = true;
                myPel.Visible = true;
            }
            else
            {
                btnInfoModal.Visible = false;
                grdTable.Visible = false;
                pnlFooterBtns.Visible = false;
                myPel.Visible = false;
            }
        } //Trigger Binding Selected Tables

        private void CreateTextBox()
        {
            int index = myPel.Controls.OfType<TextBox>().ToList().Count + 1;
            string id = "txtbox" + index;
            TextBox txt = new TextBox();
            txt.ID = id;
            double widths = (100 / (placeHolder.Count + 0.3));
            txt.Width = 350;
          
            txt.MaxLength = 90;
            txt.Attributes.Add("style", "float:left; margin-right:0px");
            txt.Attributes.Add("placeholder", placeHolder[index - 1].ToString() +" "+pHForNull[index-1]);
            myPel.Controls.Add(txt);

            RequiredFieldValidator valReqField = new RequiredFieldValidator();
            RegularExpressionValidator valINT = new RegularExpressionValidator();
            CompareValidator cmpINT = new CompareValidator();

            switch (Table.Columns[index].DataType.Name)
            {

                case "Int32":
                    cmpINT.Operator = ValidationCompareOperator.DataTypeCheck;
                    cmpINT.Type = ValidationDataType.Integer;
                    cmpINT.ControlToValidate = id;
                    cmpINT.ErrorMessage = "Lütfen Tam Sayı Giriniz";
                    cmpINT.Attributes.Add("style", "color:red;float:left;");
                    
                    myPel.Controls.Add(cmpINT);

                    if (isNullableDT.Rows[index][0].ToString() == "NO")
                    {
                        txt.Attributes.Add("required", "true");
                    }
                    break;
                case "Decimal":
                    
                    valINT.ControlToValidate = id;
                    valINT.ErrorMessage = "Lütfen sadece ondalıklı sayı girin (Ör: 12.3)";
                    valINT.ValidationExpression = (@"^[0-9]{1,11}(?:\.([0-9]{1,3})?)?$");
                    valINT.Attributes.Add("style", "color:red;float:left;");
                    myPel.Controls.Add(valINT);
                    if (isNullableDT.Rows[index][0].ToString() == "NO")
                    {
                        txt.Attributes.Add("required", "true");
                    }
                    break;

                case "String":
                    cmpINT.Operator = ValidationCompareOperator.DataTypeCheck;
                    cmpINT.Type = ValidationDataType.String;
                    cmpINT.ControlToValidate = id;
                    cmpINT.ErrorMessage = "Lütfen Metin Giriniz";
                    cmpINT.Attributes.Add("style", "color:red;float:left;");
                    myPel.Controls.Add(cmpINT);

                    if (isNullableDT.Rows[index][0].ToString() == "NO")
                    {
                        txt.Attributes.Add("required", "true");
                    }


                    break; 

                case "DateTime":

                    cmpINT.Operator = ValidationCompareOperator.DataTypeCheck;
                    cmpINT.Type = ValidationDataType.Date;
                    cmpINT.ControlToValidate = id;
                    cmpINT.ErrorMessage = "Lütfen Tarih Giriniz (GG-AA-YYYY)/(YYYY-AA-GG)";
                    cmpINT.Attributes.Add("style", "color:red;float:left;");
                    myPel.Controls.Add(cmpINT);
                    if (isNullableDT.Rows[index][0].ToString() == "NO")
                    {
                        txt.Attributes.Add("required", "true");
                    }

                    break;

            }
        } //Creating Dynamically TextBox
        private void saveData(string sqlText, string tableName)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(userConnecitonString()))//Adding data to Table
                {
                    SqlCommand cmd = new SqlCommand(sqlText);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    myPel.Controls.Clear();
                    bindGrid(ddlTables.SelectedValue);
                }
                using (SqlConnection con = new SqlConnection(cs))// Update the modified Date
                {
                    string mofiedDateSql = "UPDATE tbl_bigTable " +
                        "SET LAST_MODIFIED_DATE = GetDate()" +
                        " WHERE NAME_TABLE = '" + tableName + "'; ";
                    SqlCommand cmd = new SqlCommand(mofiedDateSql);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    bindInfo(tableName);
                }
            }
            catch (Exception myExp)
            {

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (TextBox textBox in myPel.Controls.OfType<TextBox>())
                {
                    ParameterArray.Add(textBox.Text);
                }
                if (Page.IsValid)
                {
                    saveData(GenerateInsertQuery(), ddlTables.SelectedValue);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Kayıt Eklendi','yes')", true);
                }
                   
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Beklenmedik Bir Hata Oluştu','no')", true);
            }
        }
        string GenerateInsertQuery()
        {
            int i = 0;
            string tempstr = "";
            int temp_index = -1;

            string TableName = ddlTables.SelectedValue;
            string Query = "";
            Query = "Insert into  " + TableName + "(";

            for (i = 1; i < Table.Columns.Count; i++)
            {
                int spaceIndex = Table.Columns[i].ColumnName.IndexOf(" ");
                if (i == Table.Columns.Count - 1)
                    Query = Query + Table.Columns[i].ColumnName.Substring(0, spaceIndex + 1);
                else
                    Query = Query + Table.Columns[i].ColumnName.Substring(0, spaceIndex + 1) + ", ";
            }
            Query = Query + ")" + "Values (";
            for (i = 0; i < Table.Columns.Count - 1; i++)
            {
                switch (Table.Columns[i + 1].DataType.Name)
                {
                    case "Int32":
                    case "Decimal":
                        if (i == Table.Columns.Count - 2)
                        {
                            if (!string.IsNullOrEmpty(ParameterArray[i].ToString()))
                            {
                                Query = Query + ParameterArray[i];
                            }
                            else
                            {
                                Query = Query + "null";
                            }
                        }

                        else
                        {
                            if (!string.IsNullOrEmpty(ParameterArray[i].ToString()))
                            {
                                Query = Query + ParameterArray[i] + ", ";
                            }
                            else
                            {
                                Query = Query + "null"+", ";
                            }
                           
                        }

                        break;
                    case "String":
                        if (((string)ParameterArray[i]).Contains("'"))
                        {
                            tempstr = ((string)ParameterArray[i]);
                            ParameterArray[i] = ((string)ParameterArray[i]).Replace("'", "''");
                            temp_index = i;
                        }
                        if (i == Table.Columns.Count - 2)
                        {
                            if (!string.IsNullOrEmpty(ParameterArray[i].ToString()))
                            {
                                Query = Query + "'" + ParameterArray[i] + "' ";
                            }
                            else
                            {
                                Query = Query + "null";
                            }
                        }

                        else
                        {
                            if (!string.IsNullOrEmpty(ParameterArray[i].ToString()))
                            {
                                Query = Query + "'" + ParameterArray[i] + "', ";
                            }
                            else
                            {
                                Query = Query + "null,";
                            }

                        }
                        break;
                    case "DateTime":
                    case "Date":
                        if (((string)ParameterArray[i]).Contains("'"))
                        {
                            tempstr = ((string)ParameterArray[i]);
                            ParameterArray[i] = ((string)ParameterArray[i]).Replace("'", "''");
                            temp_index = i;
                        }
                        if (i == Table.Columns.Count - 2)
                        {
                            
                            if (!string.IsNullOrEmpty(ParameterArray[i].ToString()))
                            {
                                Query = Query + "'" + ParameterArray[i] + "' ";
                            }
                            else
                            {
                                Query = Query + "null";
                            }
                        }
                       
                        else
                        {
                            if (!string.IsNullOrEmpty(ParameterArray[i].ToString()))
                            {
                                Query = Query + "'" + ParameterArray[i] + "', ";
                            }
                            else
                            {
                                Query = Query + "null,";
                            }
                               
                        }
                           
                        break;
                }
            }
            Query = Query + ")";
            return Query;
        }

        protected void grdTable_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[0].Visible = false;
        }
    }


}