using System.Web;
using System.Web.Mvc;

namespace PsiCollaboratorManager.Annotations
{
    public class CheckSessionTimeOut : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["UserAccount"] == null)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.ClearContent();
                    filterContext.HttpContext.Items["AjaxPermissionDenied"] = true;
                    HttpContext.Current.Response.StatusCode = 401;
                }
                else
                {
                    filterContext.Result = new RedirectResult("~/UserAccount/Login");
                    return;
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}