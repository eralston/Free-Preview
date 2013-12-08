Free-Preview
============

A module for ASP.Net MVC that helps you offer a free preview mode for your app using Action Filters

What is a Free Preview?
-----------------------
Some web apps have a "no login demo" mode where the user can walk through functionality as if they were logged in, without creating an account. This "free preview" mode demonstrates the application to them as-if they were logged in, which may help overcome any shyness they have to signup for your app.

What Does This Module Do?
-------------------------
This module provides a toolkit of action filters for ASP.Net MVC, plus a few extension methods for ControllerBase, that setup and manage preview sessions using cookies and entity framework to provide a consistent context for your user during a "Preview Session". 

If you think using the "Authorize" action filter is a simple way to restrict logged in users, you'll feel right at home using the action filters in FreePreview to do things like "StartSession", "EndSession", and "AuthorizeOrPreview" to control access to your app while providing a "no login demo" mode.

Installation
------------
To use FreePreview, either download the code using git, build the "FreePreview" project, and include an assembly reference in your project to the "FreePreview.dll" assembly.

OR

Install the package from NuGet (https://www.nuget.org/packages/FreePreview/) using the following command in the package manager console in Visual Studio:

```
Install-Package FreePreview
```

Example
------------
To see the FreePreview module in action, you can download this repo using git, open the "FreePreview" solution, then build and run the "FreePreview Example" MVC project. It will provide example actions, visited by just using buttons, to show starting and ending a preview session, plus features like allowing, disallowing, or redirecting actions based on whether the user has a live preview session.

Usage
-----------
Before you start marking up controllers and actions with filters, you must ensure that the controller being filtered on will be able to provide a proper DbContext instance to the filters. To do this, you must make classes that implement a set of interfaces, as outlined below:

1) Make a DbContext child class that implements the FreePreview.IPreviewContext interface:

```CSharp
using FreePreview;

public class ExampleContext : DbContext, IPreviewContext
{
    public ExampleContext() : base("name=ExampleContext") { }

    // This property is required by FreePreview.IPreviewContext
    public DbSet<PreviewSession> PreviewSessions { get; set; }
}
```

2) Using the DbContext subclass, make a Controller subclass that implements the FreePreview.IPreviewContextProvider interface:

```CSharp
using FreePreview;

public class HomeController : Controller, IPreviewContextProvider
{
    Models.ExampleContext _context = new Models.ExampleContext();
    
    // This property is required by FreePreview.IPreviewContextProvider
    IPreviewContext IPreviewContextProvider.PreviewContext
    {
        get { return _context; }
    }
        
    public ActionResult Index() { return View(); }
}
```

3) Now that your controller can provide the proper data context for the actions filters, you can now apply any of the action filters in the FreePreview namespace to the controller class or controller's methods to control user access:

```CSharp
// Start a new preview session for the current user, setting them up with a cookie to track them
// If the current user already has a live session, this does nothing
[StartPreview]
public ActionResult StartPreview() { return View(); }

// Allows authenticated users and users with a live preview session fire the action
// Otherwise, behaves identically to the "Authorize" attribute
[AuthorizeOrPreview]
public ActionResult AuthorizedOrPreview()  { return View(); }

// Redirects anyone in a free preview to the "Home" controller, choosing the "StartPreview" action
// Otherwise, behaves identically to the "Authorize" attribute, letting in authenticated users and redirecting unauthenticated users to your login page
[RedirectPreview(PreviewController = "Home", PreviewAction = "StartPreview")]
public ActionResult RedirectPreviewDenyAnonymous() { return View(); }

// Redirects anyone in a live preview session to the "Preview" controller to fire the "Start" action
// Otherwise, allows anyone who is anonymous or anyone who is authenticated to access the action
[RedirectPreview(AllowAnonymous = true, PreviewController = "Preview", PreviewAction = "Start")]
public ActionResult RedirectPreviewAllowAnonymous() { return View(); }

// Ends the current preview session
// If the current user does not have a preview session, this does nothing
[EndPreview]
public ActionResult EndPreview()  { return View(); }
```

If your app relies on data unique to each user, it is possible to fetch the current preview session object using an extension method:

```CSharp
public class HomeController : Controller, IPreviewContextProvider
{
    Models.ExampleContext _context = new Models.ExampleContext();

    public ActionResult Index()
    {
        // Get the preview session for the current user
        PreviewSession session = this.GetCurrentPreviewSession();
        
        // TODO: Use the session.Id or session.SessionId property on the preview session to query for unique user data
        
        return View();
    }
}
```

NOTE: this extension method does require the controller to implement the FreePreview.IPreviewContextProvider interface.
