﻿using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace faz1.TableOperations
{
    public partial class TableViewing : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
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
        void bindInfo(string tableName)
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
                myModalLabel.InnerText = ddlSelectedTable.SelectedValue;
                if (string.IsNullOrEmpty(lblModifyDate.Text))
                {
                    lblModifyDate.Text = "Güncellenmemiş";
                }
                con.Close();
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
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                grdTable.DataSource = dt;
                grdTable.DataBind();
                con.Close();
            }
        }

        protected void ddlSelectedTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSelectedTable.SelectedValue != "1")
            {
                pnlTableInfo.Visible = true;
                pnlDocumantary.Visible = true;
                pnlGrd.Visible = true;
                bingGrid(ddlSelectedTable.SelectedValue);
                bindInfo(ddlSelectedTable.SelectedValue);
            }
            else
            {
                pnlGrd.Visible = false;
                pnlTableInfo.Visible = false;
                pnlDocumantary.Visible = false;
            }
        }
        protected void btnSaveExcel_Click(object sender, EventArgs e)
        {
            try
            {
                ExportGridToExcel();
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Beklenmedik bir hata oluştu.','yes')", true);
            }
        }
        private void ExportGridToExcel()
        {
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=" + ddlSelectedTable.SelectedValue + ".xls");
            Response.ContentType = "File/Data.xls";
            StringWriter StringWriter = new System.IO.StringWriter();
            HtmlTextWriter HtmlTextWriter = new HtmlTextWriter(StringWriter);

            grdTable.RenderControl(HtmlTextWriter);
            Response.Write(StringWriter.ToString());
            Response.End();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Tablo Excel olarak indirildi.','yes')", true);
        }

        private void ExportGridToPDF()
        {
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + ddlSelectedTable.SelectedValue + ".pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            grdTable.RenderControl(hw);
            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();
            grdTable.AllowPaging = true;
            grdTable.DataBind();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            // controller   
        }

        protected void btnSavePdf_Click(object sender, EventArgs e)
        {
            try
            {
                ExportGridToPDF();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Tablo PDF olarak indirildi','yes')", true);
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertMsg('Beklenmedik bir hata oluştu.','no')", true);
            }

        }

        protected void btnGetScript_Click(object sender, EventArgs e)
        { 
        //{
        //    Server myServer = new Server("Server =.;");
        //    Scripter scripter = new Scripter(myServer);
        //    Database myAdventureWorks = myServer.Databases["akaStaj"];
        //    Urn[] DatabaseURNs = new Urn[] { myAdventureWorks.Urn };
        //    StringCollection scriptCollection = scripter.Script(DatabaseURNs);
        //    string aa = "";
        //    foreach (string script in scriptCollection)
        //    {
        //        aa += script + "\n";
        //    }

        //    string cc = aa;

        }

        protected void grdTable_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[0].Visible = false; // Hiding Entry ID from User
        }


       
    }
}