/* This program is free software. It comes without any warranty, to
 * the extent permitted by applicable law. You can redistribute it
 * and/or modify it under the terms of the Do What The Fuck You Want
 * To Public License, Version 2, as published by Sam Hocevar. See
 * http://sam.zoy.org/wtfpl/COPYING for more details. */
using System.Collections.Generic;
using SierraBravo.Xbox.Common.DTOs;

namespace SierraBravo.Xbox.Mvc.Models
{
    public class VideoGameViewModel
    {
        public IEnumerable<VideoGame> OwnedGames { get; set; }
        public IEnumerable<VideoGame> WantedGames { get; set; }
    }
}