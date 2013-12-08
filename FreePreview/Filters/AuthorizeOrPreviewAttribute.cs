using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Threading.Tasks;

namespace FreePreview
{
    /// <summary>
    /// Like the Authorize attribute, but with an allowance to let people through if the user is within an active preview session
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class AuthorizeOrPreviewAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Called when checking for authorization
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            // If they're in a preview session, then we leave them alone; otherwise, we treat them like any other user
            if (filterContext.Controller.IsInPreviewSession())
                return;
            else
                base.OnAuthorization(filterContext);
        }
    }
}
