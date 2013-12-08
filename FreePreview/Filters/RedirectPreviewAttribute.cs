using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace FreePreview
{
    /// <summary>
    /// This attribute causes an incoming request to be redirected to a different controller/action pair if the current request has a live preview session
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class RedirectPreviewAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Gets or sets the controller to which control is redirected if the current request has a live preview session
        /// </summary>
        public string PreviewController { get; set; }

        /// <summary>
        /// Gets or sets the action to which control is redirected if the current request has a live preview session
        /// </summary>
        public string PreviewAction { get; set; }

        /// <summary>
        /// Gets or sets the bool that allows anonymous users into the system
        /// </summary>
        public bool AllowAnonymous { get; set; }

        /// <summary>
        /// Redirects the action to the action provided by the controller and action properties
        /// </summary>
        /// <param name="filterContext"></param>
        private void Redirect(AuthorizationContext filterContext)
        {
            // Default to "Index" action
            string actionName = this.PreviewAction ?? "Index";

            // Default to the current controller
            string controllerName = this.PreviewController;
            if (string.IsNullOrEmpty(controllerName))
            {
                RouteData routeData = filterContext.HttpContext.Request.RequestContext.RouteData;
                controllerName = routeData.GetRequiredString("controller");
            }

            // Redirect to the new controller and action
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = controllerName, action = actionName }));
        }

        /// <summary>
        /// Called as the action requires authorization, redirecting to the given Preview controller and aaction if we're in a preview session
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            // If we have a live preview session, then redirect
            if (filterContext.Controller.IsInPreviewSession())
                Redirect(filterContext);
            else
            {
                // If we allow anonymous, then just let anyone through
                if (AllowAnonymous)
                    return;
                else
                    base.OnAuthorization(filterContext);
            }
        }
    }
}
