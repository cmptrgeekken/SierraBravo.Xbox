using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SierraBravo.Xbox.Mvc.Classes
{
    public class UserContext
    {
        private const string CTX_SESSION_KEY = "VGVS_USER_CTX";

        public DateTime? LastActionTime { get; private set; }
        public int NumberOfActions { get; private set; }

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

        public void Save()
        {
            HttpContext.Current.Session[CTX_SESSION_KEY] = this;
        }

        public void IncrementActionCount()
        {
            if( LastActionTime.HasValue && LastActionTime.Value.Date == DateTime.Now.Date )
            {
                NumberOfActions++;
            }else
            {
                LastActionTime = DateTime.Now;
                NumberOfActions = 1;
            }
        }
    }
}