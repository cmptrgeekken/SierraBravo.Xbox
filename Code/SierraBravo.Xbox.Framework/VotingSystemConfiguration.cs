/* This program is free software. It comes without any warranty, to
 * the extent permitted by applicable law. You can redistribute it
 * and/or modify it under the terms of the Do What The Fuck You Want
 * To Public License, Version 2, as published by Sam Hocevar. See
 * http://sam.zoy.org/wtfpl/COPYING for more details. */
using System;
using System.Collections.Generic;

namespace SierraBravo.Xbox.Framework
{
    /// <summary>
    /// Basic class for handling configuration settings throughout the program
    /// </summary>
    public static class VotingSystemConfiguration
    {
        /// <summary>
        /// Maximum number of actions a user is able to perform each day
        /// </summary>
        public static int MaximumActionsPerDay { get; set; }

        /// <summary>
        /// Days of the week when actions cannot be performed
        /// </summary>
        public static IEnumerable<DayOfWeek> InvalidActionDays { get; set; }
    }
}
