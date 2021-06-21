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
    public partial class DataEdit : System.Web.UI.Page
    {
        #region Data members

        public static DataTable Table = new DataTable();
        ArrayList ParameterArray = new ArrayList();
        string ownerID = "";
        string userName = "";
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        #endregion


        #region GridView Operations
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
                bindDropDown();
            CreateTemplatedGridView();
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
            using (SqlConnection con = new SqlConnection(userConnecitonString()))
            {
                string sqlText = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";

                SqlCommand cmd = new SqlCommand(sqlText);
                cmd.Connection = con;
                con.Open();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                ddlTable.DataSource = dt;
                ddlTable.DataValueField = "TABLE_NAME";
                ddlTable.DataBind();

                con.Close();
            }
        }
        protected void TableGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            TableGridView.EditIndex = e.NewEditIndex;
            TableGridView.DataBind();
            Session["SelecetdRowIndex"] = e.NewEditIndex;
        }

        protected void TableGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            TableGridView.EditIndex = -1;
            TableGridView.DataBind();
            Session["SelecetdRowIndex"] = -1;
        }

        protected void TableGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            SqlConnection Connection = new SqlConnection(userConnecitonString());
            GridViewRow row = TableGridView.Rows[e.RowIndex];
            try
            {
                //TextBox ad = TableGridView.Rows[e.RowIndex].FindControl.as TextBox;
                //string asd = ad.Text;
                for (int i = 0; i < Table.Columns.Count; i++)
                {
                    string field_value = ((TextBox)row.FindControl(Table.Columns[i].ColumnName)).Text;
                    ParameterArray.Add(field_value);
                }
                string Query = "";
                if ((int)Session["InsertFlag"] == 1)
                    Query = GenerateInsertQuery();
                else
                    Query = GenerateUpdateQuery();
                SqlCommand Command = new System.Data.SqlClient.SqlCommand(Query, Connection);

                if (Connection.State == ConnectionState.Closed)
                    Connection.Open();
                Command.ExecuteNonQuery();
                Session["InsertFlag"] = (int)Session["InsertFlag"] == 1 ? 0 : 1;

                using (SqlConnection con = new SqlConnection(cs))// Update the modified Date
                {
                    string mofiedDateSql = "UPDATE tbl_bigTable " +
                        "SET LAST_MODIFIED_DATE = GetDate()" +
                        " WHERE NAME_TABLE = '" + ddlTable.SelectedValue + "'; ";
                    SqlCommand cmd = new SqlCommand(mofiedDateSql);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    bindInfo(ddlTable.SelectedValue);
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Kayıt Güncellendi','yes')", true);
            }
            catch (SqlException se)
            {
                msg_lbl.Text = se.ToString();
                MsgPanel.Visible = false;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Güncelleme sırasında hata oluştu \n " + se.ToString() + "','yes')", true);
            }
            finally
            {
                Connection.Close();

            }
            TableGridView.EditIndex = -1;
            CreateTemplatedGridView();

        }

        protected void TableGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
         
            SqlConnection Connection = new System.Data.SqlClient.SqlConnection(userConnecitonString());
            string Query = GenerateDeleteQuery(e.RowIndex);
            SqlCommand Command = new System.Data.SqlClient.SqlCommand(Query, Connection);
            try
            {
                if (Connection.State == ConnectionState.Closed)
                    Connection.Open();
                Command.ExecuteNonQuery();
                Connection.Close();

                using (SqlConnection con = new SqlConnection(cs))// Update the modified Date
                {
                    string mofiedDateSql = "UPDATE tbl_bigTable " +
                        "SET LAST_MODIFIED_DATE = GetDate()" +
                        " WHERE NAME_TABLE = '" + ddlTable.SelectedValue + "'; ";
                    SqlCommand cmd = new SqlCommand(mofiedDateSql);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    bindInfo(ddlTable.SelectedValue);
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Kayıt Silindi!','yes')", true);

            }
            catch (SqlException se)
            {
                msg_lbl.Text = se.ToString();
                MsgPanel.Visible = false;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Kayıt silme esnasında hata oluştu.','no')", true);
            }
            CreateTemplatedGridView();

        }

        protected void TableGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TableGridView.PageIndex = e.NewPageIndex;
            TableGridView.DataBind();
        }

        #endregion // GridView Operations

        #region Methods
        void bindInfo(string tableName) // filling Table Informations
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
                myModalLabel.InnerText = ddlTable.SelectedValue;
                if (string.IsNullOrEmpty(lblModifyDate.Text))
                {
                    lblModifyDate.Text = "Güncellenmemiş";
                }
                con.Close();
            }
        }

        void PopulateDataTable()
        {
            Table = new DataTable();
            TableGridView.Columns.Clear();
            string TableName = (string)Session["TableSelected"];
            SqlConnection Connection = new System.Data.SqlClient.SqlConnection(userConnecitonString());
            SqlDataAdapter adapter = new SqlDataAdapter("Select * From " + TableName, userConnecitonString());
            try
            {
                adapter.Fill(Table);
               

            }
            catch (Exception ex)
            {
                msg_lbl.Text = ex.ToString();
                MsgPanel.Visible = false;

            }
            finally
            {
                Connection.Close();
            }


        }
        void CreateTemplatedGridView()
        {

            // fill the table which is to bound to the GridView
            PopulateDataTable();
            // add templated fields to the GridView
            TemplateField BtnTmpField = new TemplateField();
            BtnTmpField.ItemTemplate =
                new DynamicallyTemplatedGridViewHandler(ListItemType.Item, "...", "Command");
            BtnTmpField.HeaderTemplate =
                new DynamicallyTemplatedGridViewHandler(ListItemType.Header, "İşlemler", "Command");
            BtnTmpField.EditItemTemplate =
                new DynamicallyTemplatedGridViewHandler(ListItemType.EditItem, "...", "Command");
            TableGridView.Columns.Add(BtnTmpField);

            for (int i = 0; i < Table.Columns.Count; i++)
            {
                TemplateField ItemTmpField = new TemplateField();
                // create HeaderTemplate
                ItemTmpField.HeaderTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Header,
                                                              Table.Columns[i].ColumnName,
                                                              Table.Columns[i].DataType.Name);
                // create ItemTemplate
                ItemTmpField.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item,
                                                              Table.Columns[i].ColumnName,
                                                              Table.Columns[i].DataType.Name);
                //create EditItemTemplate
                ItemTmpField.EditItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.EditItem,
                                                              Table.Columns[i].ColumnName,
                                                              Table.Columns[i].DataType.Name);

                TableGridView.Columns.Add(ItemTmpField);
            }
            // bind and display the data
            TableGridView.DataSource = Table;
            TableGridView.DataBind();
        }


        string GenerateUpdateQuery()
        {
            int i = 0;
            string tempstr = "";
            int temp_index = -1;

            string TableName = (string)Session["TableSelected"];
            string Query = "";
            Query = "Update  " + TableName + " set ";
            try
            {
                for (i = 1; i < Table.Columns.Count; i++)
                {
                    switch (Table.Columns[i].DataType.Name)
                    {
                        case "Int32":
                            if (i == Table.Columns.Count - 1)
                                Query = Query + Table.Columns[i].ColumnName + "=" + ParameterArray[i];
                            else
                                Query = Query + Table.Columns[i].ColumnName + "=" + ParameterArray[i] + ", ";
                            break;
                        case "Decimal":

                            if (i == Table.Columns.Count - 1)
                                Query = Query + Table.Columns[i].ColumnName + "=" + ParameterArray[i].ToString().Replace(',', '.');
                            else
                                Query = Query + Table.Columns[i].ColumnName + "=" + ParameterArray[i] + ", ";

                            break;
                        case "String":
                        case "DateTime":
                            if (((string)ParameterArray[i]).Contains("'"))
                            {
                                tempstr = ((string)ParameterArray[i]);
                                ParameterArray[i] = ((string)ParameterArray[i]).Replace("'", "''");
                                temp_index = i;
                            }

                            if (i == Table.Columns.Count - 1)
                                Query = Query + Table.Columns[i].ColumnName + "='" + ParameterArray[i] + "' ";
                            else
                                Query = Query + Table.Columns[i].ColumnName + "='" + ParameterArray[i] + "', ";
                            break;

                    }
                }
                if (temp_index > -1)
                    ParameterArray[temp_index] = tempstr;
                if (Table.Columns[0].DataType.Name == "String" || Table.Columns[0].DataType.Name == "DateTime")
                    Query = Query + " where " + Table.Columns[0].ColumnName + " = '" + Table.Rows[i - 1][Table.Columns[0].ColumnName].ToString() + "'";
                else
                    Query = Query + " where " + Table.Columns[0].ColumnName + " = " + ParameterArray[0];

                return Query;
            }
            catch (Exception ex)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Güncelleme sırasında hata oluştu \n " + ex.ToString() + "','yes')", true);
            }
            return Query;
        }
        string GenerateInsertQuery()
        {
            int i = 0;
            string tempstr = "";
            int temp_index = -1;

            string TableName = (string)Session["TableSelected"];
            string Query = "";
            Query = "Insert into  " + TableName + "(";

            for (i = 0; i < Table.Columns.Count; i++)
            {
                if (i == Table.Columns.Count - 1)
                    Query = Query + Table.Columns[i].ColumnName;
                else
                    Query = Query + Table.Columns[i].ColumnName + ", ";

            }
            Query = Query + ")" + "Values (";
            for (i = 0; i < Table.Columns.Count; i++)
            {

                switch (Table.Columns[i].DataType.Name)
                {

                    case "Boolean":
                    case "Int32":
                    case "Byte":
                    case "Decimal":
                        if ((string)ParameterArray[i] == "True")
                            ParameterArray[i] = "1";
                        else if ((string)ParameterArray[i] == "False")
                            ParameterArray[i] = "0";

                        if (i == Table.Columns.Count - 1)
                            Query = Query + ParameterArray[i];
                        else
                            Query = Query + ParameterArray[i] + ", ";

                        break;
                    case "String":
                    case "DateTime":
                        if (((string)ParameterArray[i]).Contains("'"))
                        {
                            tempstr = ((string)ParameterArray[i]);
                            ParameterArray[i] = ((string)ParameterArray[i]).Replace("'", "''");
                            temp_index = i;
                        }
                        if (i == Table.Columns.Count - 1)
                            Query = Query + "'" + ParameterArray[i] + "' ";
                        else
                            Query = Query + "'" + ParameterArray[i] + "', ";
                        break;

                }


            }

            Query = Query + ")";


            return Query;
        }
        string GenerateDeleteQuery(int index)
        {
            string TableName = (string)Session["TableSelected"];
            string query = "";

            query = "Delete from " + TableName + " where " + Table.Columns[0].ColumnName + "='" + Table.Rows[index][0].ToString() + "'";


            return query;
        }

        #endregion

        protected void ddlTable_SelectedIndexChanged(object sender, EventArgs e) // Changing Table
        {
            Session["TableSelected"] = ddlTable.SelectedValue.ToString();
            pnlEdit.Visible = true;
            CreateTemplatedGridView();
            if (ddlTable.SelectedValue != "1")
            {
                bindInfo(ddlTable.SelectedValue);
                modalBtn.Visible = true;
            }
            else
            {
                modalBtn.Visible = false;
            }
        }

        protected void TableGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[1].Visible = false; // Hiding Entry ID from User
        }
    }
}