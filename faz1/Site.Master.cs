using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace faz1
{
    public partial class Site : System.Web.UI.MasterPage
    {
        
        string url = ConfigurationManager.AppSettings["BaseUrl"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["userName"] == null)
                {

                    HttpCookie reqCookies = Request.Cookies["sessionInfo"];
                    if (reqCookies == null)
                    {
                        //Response.Write("Oturum Süreniz Bitti. Giriş Sayfasına Yönlendiriliyorsunuz.");
                        //Response.AddHeader("REFRESH", "1;URL="+url+"/login.aspx");
                        Response.Write("Oturum Süreniz Bitti. Giriş Sayfasına Yönlendiriliyorsunuz.");
                        System.Threading.Thread.Sleep(1000);
                        Response.Redirect(url + "login.aspx");
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
                    HttpCookie reqCookies = Request.Cookies["sessionInfo"];
                    string userRole = reqCookies["userRole"].ToString();
                    switch (userRole)
                    {
                        case "sys_admin":
                            adminPanel.Visible = true;
                            break;
                        case "sys_mod":
                            adminPanel.Visible = true;
                            break;
                        case "basic_user":
                            adminPanel.Visible = false;
                            break;
                        default:
                            adminPanel.Visible = false;
                            break;
                    }

                }
            }

            catch
            {
                //Response.Write("alert('Beklenmedik bir hata oluştu. Giriş sayfasına yönlendiriliyorsunuz.')");
                //Response.AddHeader("REFRESH", "2;URL="+ url +"/login.aspx");
                Response.Write("Oturum Süreniz Bitti. Giriş Sayfasına Yönlendiriliyorsunuz.");
                System.Threading.Thread.Sleep(1000);
                Response.Redirect(url + "login.aspx");
            }
            
          



        }
    }
}