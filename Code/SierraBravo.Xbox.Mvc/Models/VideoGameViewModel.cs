using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SierraBravo.Xbox.Common.DTOs;

namespace SierraBravo.Xbox.Mvc.Models
{
    public class VideoGameViewModel
    {
        public IEnumerable<VideoGame> OwnedGames { get; set; }
        public IEnumerable<VideoGame> WantedGames { get; set; }
    }
}