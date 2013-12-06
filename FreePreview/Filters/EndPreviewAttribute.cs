using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Threading.Tasks;

using FreePreview.Models;

namespace FreePreview.Filters
{
    /// <summary>
    /// Ends the preview session, clearing the preview session cookie for this user
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class EndPreviewAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Called after the action is completed by the user
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // Clear the cookie from the client
            filterContext.HttpContext.DeletePreviewSessionCookie();

            // If we're authenticated, then just abort
            if (filterContext.HttpContext.Request.IsAuthenticated)
                return;

            // Try to find the cookie in the request
            HttpCookie requestCookie = filterContext.HttpContext.GetPreviewSessionCookie();

            if (requestCookie == null)
                return;

            // Try to get the context from the controller
            IPreviewContext context = filterContext.Controller.GetPreviewContextFromController();

            if (context == null)
                return;

            // Try to find the preview sessions
            PreviewSession session = PreviewSession.Find(context.PreviewSessions, requestCookie.Value);

            if (session == null)
                return;

            // Kill the session in the DB and save changes
            session.Active = false;
            context.SaveChanges();
        }
    }
}
