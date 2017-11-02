using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using ProjectManager.Entities;
using System.Web.UI;

namespace ProjectManager.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void FormsAuthentication_OnAuthenticate(Object sender, FormsAuthenticationEventArgs e)
        {
            if (FormsAuthentication.CookiesSupported == true)
            {
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    {
                        ProjectManagerContext db = new ProjectManagerContext();

                        string email = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                        string roles = string.Empty;

                        User user = db.Users.SingleOrDefault(u => u.Email == email);
                        roles = user.Roles;

                        e.User = new System.Security.Principal.GenericPrincipal(new System.Security.Principal.GenericIdentity(email, "Forms"), roles.Split(';'));
                    }
                    catch (Exception)
                    {
                      
                    }
                }
            }
        }
    }
}
