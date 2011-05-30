using System;
using System.Linq;
using System.Web.Mvc;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using SierraBravo.Xbox.Framework;
using SierraBravo.Xbox.Mvc.Classes;

namespace SierraBravo.Xbox.Mvc.Controllers
{
    public class BaseController<T> : Controller
    {
        private const string STATUS_COOKIE = "VGVS_LASTACTION_TIME";

        protected readonly T ControllerService;

        /// <summary>
        /// Initialize a new Windsor container that will be shared across requests.
        /// </summary>
        private static readonly WindsorContainer Container = new WindsorContainer(new XmlInterpreter());

        public BaseController()
        {
            ControllerService = Container.Resolve<T>();
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (VotingSystemConfiguration.InvalidActionDays.Any(d => d == DateTime.Now.DayOfWeek))
            {
                ViewBag.NumberOfActionsLeft = 0;
                ViewBag.ReadOnly = true;
                ViewBag.NoActionReason = "Site is in read-only mode today.";
            }
            else
            {
                var ctx = UserContext.Current;
                if( ctx.LastActionTime.HasValue && ctx.LastActionTime.Value.Date == DateTime.Now.Date && ctx.NumberOfActions >= VotingSystemConfiguration.MaximumActionsPerDay )
                {
                    ViewBag.NumberOfActionsLeft = 0;
                    ViewBag.ReadOnly = true;
                }else
                {
                    ViewBag.ReadOnly = false;
                    ViewBag.NumberOfActionsLeft = VotingSystemConfiguration.MaximumActionsPerDay - ctx.NumberOfActions;
                }
            }
        }

        protected void SetSuccess(string message)
        {
            TempData["Success"] = message;
        }

        protected void SetError(string message)
        {
            TempData["Error"] = message;
        }
    }
}
