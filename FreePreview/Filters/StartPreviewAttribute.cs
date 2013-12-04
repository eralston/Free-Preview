using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Threading.Tasks;

using FreePreview;
using FreePreview.Models;

namespace FreePreview.Filters
{
    /// <summary>
    /// This attribute initializes a new preview session, if one is not already in motion
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class StartPreviewAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Called before the action method is called, setting up the preview session
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Try to find the cookie in the request
            HttpCookie requestCookie = filterContext.HttpContext.GetPreviewSessionCookie();

            // If the cookie is already set, then abort
            if (requestCookie != null)
                return;

            IPreviewContext context = filterContext.Controller.GetPreviewContextFromController();

            if (context == null)
                return;

            // Create a new session
            PreviewSession session = PreviewSession.Create(context);

            // Save the cookie to the client
            HttpCookie responseCookie = new HttpCookie(FreePreviewHelper.CookieId, session.SessionId);
            filterContext.HttpContext.SetPreviewSessionCookie(responseCookie);
        }
    }
}
