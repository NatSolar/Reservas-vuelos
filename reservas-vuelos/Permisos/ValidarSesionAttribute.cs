using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reservas_vuelos.Permisos
{
    public class ValidateSesionAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["usuario"] == null)
            {
                filterContext.Result = new RedirectResult("~/Acceso/InicioSesion");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}