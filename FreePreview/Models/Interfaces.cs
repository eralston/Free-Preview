using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace FreePreview.Models
{
    /// <summary>
    /// Contract for a DbContext that manages a collection of PreviewSessions
    /// </summary>
    public interface IPreviewContext
    {
        DbSet<PreviewSession> PreviewSessions { get; set; }
    }

    /// <summary>
    /// Interface for an object that provides a PreviewSession to an attribute
    /// </summary>
    public interface IPreviewContextProvider
    {
        IPreviewContext PreviewContext { get; }
    }
}
