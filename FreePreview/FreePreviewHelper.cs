using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Threading.Tasks;

namespace FreePreview
{
    /// <summary>
    /// A helper class with extensions
    /// </summary>
    public static class FreePreviewHelper
    {
        /// <summary>
        /// The cookie
        /// </summary>
        internal const string CookieId = "free-preview-session-id";

        /// <summary>
        /// Gets the current PreviewSession for the given controller
        /// The controller must implement the IPreviewContextProvider
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static PreviewSession GetCurrentPreviewSession(this ControllerBase controller)
        {
            // Try to find the cookie in the request
            HttpCookie requestCookie = controller.ControllerContext.HttpContext.GetPreviewSessionCookie();

            if (requestCookie == null)
                return null;

            // Try to get the context from the controller
            IPreviewContext context = controller.GetPreviewContextFromController();

            if (context == null)
                return null;

            // Try to find the preview sessions
            return PreviewSession.Find(context.PreviewSessions, requestCookie.Value);
        }

        /// <summary>
        /// Returns true if the given controller has a live preview session
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static bool IsInPreviewSession(this ControllerBase controller)
        {
            PreviewSession session = controller.GetCurrentPreviewSession();

            return session != null && session.Active;
        }

        /// <summary>
        /// Get preview context from object
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        internal static IPreviewContext GetPreviewContextFromController(this ControllerBase controller)
        {
            IPreviewContextProvider provider = controller as IPreviewContextProvider;

            if (provider == null)
                return null;

            return provider.PreviewContext;
        }

        /// <summary>
        /// Saves changes in the given IPreviewContext, assuming it descends from DbContext
        /// </summary>
        /// <param name="context"></param>
        internal static void SaveChanges(this IPreviewContext context)
        {
            DbContext db = context as DbContext;
            if (db != null)
                db.SaveChanges();
        }

        #region Extensions for Cookie Handling

        /// <summary>
        /// Finds the preview session cookie in the given HttpContext
        /// NOTE: Returns null if not found
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        internal static HttpCookie GetPreviewSessionCookie(this HttpContextBase context)
        {
            return context.Request.Cookies[FreePreviewHelper.CookieId];
        }

        /// <summary>
        /// Sets the preview session cookie in the given HttpContext
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cookie"></param>
        internal static void SetPreviewSessionCookie(this HttpContextBase context, HttpCookie cookie)
        {
            context.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Deletes the preview cookie from the client's browser
        /// </summary>
        /// <param name="context"></param>
        internal static void DeletePreviewSessionCookie(this HttpContextBase context)
        {
            HttpCookie responseCookie = new HttpCookie(FreePreviewHelper.CookieId, "");
            responseCookie.Expires = DateTime.Now.AddDays(-1d);
            context.SetPreviewSessionCookie(responseCookie);
        }

        #endregion
    }
}
