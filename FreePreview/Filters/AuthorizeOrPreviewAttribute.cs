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
    /// Like the Authorize attribute, but with an allowance to let people through if the user is within an active preview session
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class AuthorizeOrPreviewAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Checks the given filter context to confirm or deny if the user is currently in a live preview session
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        private static bool IsInPreviewSession(AuthorizationContext filterContext)
        {
            // Try to get a preview context, if not available then abort
            IPreviewContext context = filterContext.Controller.GetPreviewContextFromController();

            if (context == null)
                return false;

            // Try to find the cookie in the request
            HttpCookie requestCookie = filterContext.HttpContext.GetPreviewSessionCookie();

            if (requestCookie == null)
                return false;

            // Try to find the session attached to the cookie
            PreviewSession session = PreviewSession.Find(context.PreviewSessions, requestCookie.Value);

            return session != null && session.Active;
        }

        /// <summary>
        /// Called when checking for authorization
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            // If they're in a preview session, then we leave them alone; otherwise, we treat them like any other user
            if (IsInPreviewSession(filterContext))
                return;
            else
                base.OnAuthorization(filterContext);
        }
    }
}
