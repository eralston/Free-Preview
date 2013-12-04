using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Globalization;
using System.Web.Security;

namespace FreePreview.Models
{
    /// <summary>
    /// A model for the transient preview session, client code may use it to hang
    /// </summary>
    public class PreviewSession
    {
        #region Static Members

        /// <summary>
        /// Creates a new session within the given context
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static PreviewSession Create(IPreviewContext context)
        {
            // Make a new session
            PreviewSession session = new PreviewSession();
            session.SessionId = Guid.NewGuid().ToString("N");
            session.CreatedDate = DateTime.Now;
            session.Active = true;

            // Save to the database
            context.PreviewSessions.Add(session);
            context.SaveChanges();

            return session;
        }

        /// <summary>
        /// Finds the preview session matching the given session Id within the given queryable
        /// NOTE: This will return null if not found
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public static PreviewSession Find(IQueryable<PreviewSession> queryable, string sessionId)
        {
            return queryable.Where(p => p.SessionId == sessionId).Take(1).SingleOrDefault();
        }

        #endregion

        #region Instance Members

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool Active { get; set; }

        [Required]
        public string SessionId { get; set; }

        #endregion
    }
}
