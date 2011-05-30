/* This program is free software. It comes without any warranty, to
 * the extent permitted by applicable law. You can redistribute it
 * and/or modify it under the terms of the Do What The Fuck You Want
 * To Public License, Version 2, as published by Sam Hocevar. See
 * http://sam.zoy.org/wtfpl/COPYING for more details. */
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SierraBravo.Xbox.Common.DTOs;
using SierraBravo.Xbox.Framework;
using SierraBravo.Xbox.Repositories;

namespace SierraBravo.Xbox.Mvc
{
    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_BeginRequest()
        {
            
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            int maxActionsPerDay;
            if (Int32.TryParse(ConfigurationManager.AppSettings["VideoGameVotingSystem.MaxActionsPerDay"],out maxActionsPerDay))
            {
                VotingSystemConfiguration.MaximumActionsPerDay = maxActionsPerDay;
            }else
            {
                throw new ConfigurationErrorsException("Application Configuration Setting 'VideoGameVotingSystem.MaxActionsPerDay' not set.");
            }


            var invalidDays = new List<DayOfWeek>();
            if( !String.IsNullOrEmpty(ConfigurationManager.AppSettings["VideoGameVotingSystem.InvalidActionDays"]) )
            {
                foreach (var day in ConfigurationManager.AppSettings["VideoGameVotingSystem.InvalidActionDays"].Split(','))
                {
                    DayOfWeek dow;
                    if (Enum.TryParse(day, true, out dow))
                    {
                        invalidDays.Add(dow);
                    }
                    else
                    {
                        throw new ConfigurationErrorsException("Application Configuration Setting 'VideoGameVotingSystem.InvalidActionDays' incorrectly formatted. Must consist of the full name of a day of the week.");
                    }
                }
            }
            VotingSystemConfiguration.InvalidActionDays = invalidDays;
        }
    }
}