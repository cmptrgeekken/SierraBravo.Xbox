/* This program is free software. It comes without any warranty, to
 * the extent permitted by applicable law. You can redistribute it
 * and/or modify it under the terms of the Do What The Fuck You Want
 * To Public License, Version 2, as published by Sam Hocevar. See
 * http://sam.zoy.org/wtfpl/COPYING for more details. */
using System.Web.Mvc;
using SierraBravo.Xbox.Mvc.Classes;


namespace SierraBravo.Xbox.Mvc.Filters
{
    /// <summary>
    /// This attribute has been developed to provide a simple way of preventing users
    /// from performing an action if the user has already performed the maximum number
    /// of actions allocated for a specific day. Simply adding this attribute to a
    /// controller action will perform a redirect and increment the action counter for
    /// the user as necessary.
    /// </summary>
    public class RequiresActionAttribute : ActionFilterAttribute
    {
        private readonly bool _incrementActionCount;

        public RequiresActionAttribute(bool incrementActionCountOnSuccess)
        {
            _incrementActionCount = incrementActionCountOnSuccess;
        }

        #region Implementation of IActionFilter

        /// <summary>
        /// Called before an action method executes. Redirects a user to the index page if they attempt to access
        /// a page that requires action points when they don't have any.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if( filterContext.Controller.ViewBag.ReadOnly )
            {
                filterContext.Controller.TempData["Error"] = "Cannot access requested action while in read-only mode.";
                filterContext.Result = new RedirectResult("~/");                
            }
        }

        /// <summary>
        /// Increments the action counter if the action this attribute is associated with was successful.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            if( _incrementActionCount && filterContext.Controller.TempData["Error"] == null )
            {
                UserContext.Current.IncrementActionCount();
            }
        }


        #endregion
    }
}