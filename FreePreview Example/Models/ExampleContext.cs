using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

using FreePreview;

namespace FreePreview_Example.Models
{
    public class ExampleContext : DbContext, IPreviewContext
    {
        public ExampleContext() : base("name=ExampleContext") { }

        public DbSet<PreviewSession> PreviewSessions { get; set; }
    }
}
