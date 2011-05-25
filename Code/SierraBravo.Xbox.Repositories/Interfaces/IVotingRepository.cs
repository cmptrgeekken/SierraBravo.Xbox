/* This program is free software. It comes without any warranty, to
 * the extent permitted by applicable law. You can redistribute it
 * and/or modify it under the terms of the Do What The Fuck You Want
 * To Public License, Version 2, as published by Sam Hocevar. See
 * http://sam.zoy.org/wtfpl/COPYING for more details. */
using System.Collections.Generic;
using SierraBravo.Xbox.Common.DTOs;

namespace SierraBravo.Xbox.Repositories.Interfaces
{
    public interface IVotingRepository
    {
        /// <summary>
        /// Add a game to the catalog.
        /// </summary>
        /// <param name="gameTitle"></param>
        /// <returns>True if adding the game succeeded</returns>
        bool AddGame(string gameTitle);

        /// <summary>
        /// Returns a list of all games currently in the database.
        /// </summary>
        /// <returns>List of all games currently in the database</returns>
        IEnumerable<VideoGame> GetAllGames();

        /// <summary>
        /// Adds a vote to the game with the specified ID.
        /// </summary>
        /// <param name="gameId">ID of the game to vote on</param>
        /// <returns>True if voting succeeded</returns>
        bool AddVote(int gameId);

        /// <summary>
        /// Removes all games from the database.
        /// </summary>
        /// <returns>True of removing all games succeeded</returns>
        bool ClearAllGames();

        /// <summary>
        /// Sets the 'owned' status to true for the game with the specified ID
        /// </summary>
        /// <param name="gameId">ID of the game that is now owned</param>
        /// <returns>True if setting the status succeeded</returns>
        bool MarkAsOwned(int gameId);
    }
}
