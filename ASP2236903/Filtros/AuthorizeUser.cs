using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASP2236903.Models;

namespace ASP2236903.Filtros
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AuthorizeUser: AuthorizeAttribute
    {
        private usuario oUsuario;
        private inventario2021Entities db = new inventario2021Entities();
        private int idRol;

        public AuthorizeUser(int idRol = 0)
        {
            this.idRol = idRol;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                oUsuario = (usuario)HttpContext.Current.Session["User"];
                if (oUsuario == null)
                {
                    filterContext.Result = new RedirectResult("~/Home/Index");
                }
                else
                {
                    var lstMisOperaciones = from m in db.usuariorol
                                            where m.idRol == idRol && m.idUsuario == oUsuario.id
                                            select m;

                    if (lstMisOperaciones.ToList().Count() == 0)
                    {
                        filterContext.Result = new RedirectResult("~/Home/Index");
                    }
                }
            }
            catch (Exception)
            {
                filterContext.Result = new RedirectResult("~/Home/Index");
            }
        }
    }
}