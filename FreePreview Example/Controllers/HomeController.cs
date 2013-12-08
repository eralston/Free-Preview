using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using FreePreview;

namespace FreePreview_Example.Controllers
{
    public class HomeController : Controller, IPreviewContextProvider
    {
        Models.ExampleContext _context = new Models.ExampleContext();

        public ActionResult Index()
        {
            PreviewSession session = this.GetCurrentPreviewSession();
            return View();
        }

        [StartPreview]
        public ActionResult StartPreview() { return View(); }

        [AuthorizeOrPreview]
        public ActionResult AuthorizedOrPreview()  { return View(); }

        [Authorize]
        public ActionResult AuthorizedOnly() { return View(); }

        [RedirectPreview(AllowAnonymous = true)]
        public ActionResult RedirectPreviewAllowAnonymous() { return View(); }

        [RedirectPreview(PreviewController = "Home", PreviewAction = "StartPreview")]
        public ActionResult RedirectPreviewDenyAnonymous() { return View(); }

        [EndPreview]
        public ActionResult EndPreview() { return View(); }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _context.Dispose();
            base.Dispose(disposing);
        }

        IPreviewContext IPreviewContextProvider.PreviewContext
        {
            get { return _context; }
        }
    }
}