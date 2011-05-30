using System;
using System.Web;

namespace SierraBravo.Xbox.Mvc.Classes
{
    /// <summary>
    /// Class used for maintaining the current user context. It utilizes
    /// the Session for the current HttpContext.
    /// </summary>
    public class UserContext
    {
        private const string CTX_SESSION_KEY = "VGVS_USER_CTX";

        public DateTime? LastActionTime { get; private set; }
        public int NumberOfActions { get; private set; }

        /// <summary>
        /// Make the constructor private so that the only (naive) way of creating the user
        /// context is through the 'Current' static property.
        /// </summary>
        private UserContext()
        {
            
        }

        /// <summary>
        /// Get the context instance for the current user. If no such instance exists, create it.
        /// </summary>
        public static UserContext Current
        {
            get
            {
                if( HttpContext.Current.Session[CTX_SESSION_KEY] == null )
                {
                    HttpContext.Current.Session[CTX_SESSION_KEY] = new UserContext
                                                                       {
                                                                           LastActionTime = null, 
                                                                           NumberOfActions = 0
                                                                       };
                }

                return (UserContext)HttpContext.Current.Session[CTX_SESSION_KEY];
            }
        }

        /// <summary>
        /// Increments the current action count. As in most cases (except when constructed via some
        /// unorthodox means such as through reflection), this context will be associated with a session
        /// variable, incrementing the action count automatically propagates to the saved session state.
        /// </summary>
        public void IncrementActionCount()
        {
            if( LastActionTime.HasValue && LastActionTime.Value.Date == DateTime.Now.Date )
            {
                NumberOfActions++;
            }else
            {
                NumberOfActions = 1;
            }
            LastActionTime = DateTime.Now;
        }
    }
}