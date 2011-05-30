/* This program is free software. It comes without any warranty, to
 * the extent permitted by applicable law. You can redistribute it
 * and/or modify it under the terms of the Do What The Fuck You Want
 * To Public License, Version 2, as published by Sam Hocevar. See
 * http://sam.zoy.org/wtfpl/COPYING for more details. */
namespace SierraBravo.Xbox.Common.DTOs
{
    public class VideoGame
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int NumberOfVotes { get; set; }
        public bool IsOwned { get; set; }
    }
}
