using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace faz1
{
    public partial class Site : System.Web.UI.MasterPage
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["userName"] == null)
                {
                    HttpCookie reqCookies = Request.Cookies["sessionInfo"];

                    if (reqCookies == null)
                    {
                        Response.Redirect("https://localhost:44316/login.aspx");
                    }
                    else
                    {
                        userField.InnerText = reqCookies["userName"].ToString();
                        Session["userName"] = reqCookies["userName"];
                        Session["userID"] = reqCookies["userID"];
                    }

                }
                else
                {
                    userField.InnerText = Session["userName"].ToString();
                }
            }

            catch
            {
                Response.Write("alert('Beklenmedik bir hata oluştu. Giriş sayfasına yönlendiriliyorsunuz.')");
                Response.AddHeader("REFRESH", "2;URL=https://localhost:44316/login.aspx/login.aspx");

            }
            
        }
    }
}