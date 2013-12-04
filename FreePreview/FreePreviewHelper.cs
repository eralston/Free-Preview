using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Threading.Tasks;

using FreePreview.Models;

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
        /// Finds the current PreviewSession, based on the controller's current request and the given collection of sessions
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="sessions"></param>
        /// <returns></returns>
        public static PreviewSession GetCurrentSession(this Controller controller, IQueryable<PreviewSession> sessions)
        {
            HttpCookie cookie = controller.HttpContext.GetPreviewSessionCookie();
            if (cookie == null)
                return null;
            return PreviewSession.Find(sessions, cookie.Value);
        }

        /// <summary>
        /// Get preview context from object
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static IPreviewContext GetPreviewContextFromController(this ControllerBase controller)
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
        public static void SaveChanges(this IPreviewContext context)
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
        public static HttpCookie GetPreviewSessionCookie(this HttpContextBase context)
        {
            return context.Request.Cookies[FreePreviewHelper.CookieId];
        }

        /// <summary>
        /// Sets the preview session cookie in the given HttpContext
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cookie"></param>
        public static void SetPreviewSessionCookie(this HttpContextBase context, HttpCookie cookie)
        {
            context.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Deletes the preview cookie from the client's browser
        /// </summary>
        /// <param name="context"></param>
        public static void DeletePreviewSessionCookie(this HttpContextBase context)
        {
            HttpCookie responseCookie = new HttpCookie(FreePreviewHelper.CookieId, "");
            responseCookie.Expires = DateTime.Now.AddDays(-1d);
            context.SetPreviewSessionCookie(responseCookie);
        }

        #endregion
    }
}
