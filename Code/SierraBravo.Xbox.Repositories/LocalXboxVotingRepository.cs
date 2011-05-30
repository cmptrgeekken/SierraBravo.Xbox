/* This program is free software. It comes without any warranty, to
 * the extent permitted by applicable law. You can redistribute it
 * and/or modify it under the terms of the Do What The Fuck You Want
 * To Public License, Version 2, as published by Sam Hocevar. See
 * http://sam.zoy.org/wtfpl/COPYING for more details. */
using System;
using System.Collections.Generic;
using SierraBravo.Xbox.Common.DTOs;
using SierraBravo.Xbox.Repositories.Interfaces;

namespace SierraBravo.Xbox.Repositories
{
    /// <summary>
    /// This class serves as a very basic repository for storing
    /// information in local, temporary storage. If the server is
    /// restarted, or the website has to be recompiled, the storage
    /// will be cleared.
    /// 
    /// This repository was developed to serve as a way of continuing
    /// development while not connected to the internet.
    /// </summary>
    public class LocalXboxVotingRepository : IVotingRepository
    {
        private static readonly object LockObj = new object();

        /// <summary>
        /// Even though this local "database" would allow for ID-based lookups, 
        /// the functionality of this repository has been limited to only what 
        /// the web service this is based on is capable of.
        /// </summary>
        private static readonly Dictionary<int,VideoGame> AllGames = new Dictionary<int,VideoGame>();

        private static int _currentId = 1;
        
        /// <summary>
        /// Resets the games dictionary to contain the games in the provided array
        /// </summary>
        /// <param name="videoGames">Array to replace the dictionary with</param>
        public static void ResetDictionary(IEnumerable<VideoGame> videoGames)
        {
            AllGames.Clear();
            foreach(var game in videoGames)
            {
                AllGames.Add(game.Id,game);
                _currentId = Math.Max(_currentId, game.Id+1);
            }
        }
        
        #region Implementation of IVotingRepository

        /// <summary>
        /// Add a game to the catalog.
        /// </summary>
        /// <param name="gameTitle"></param>
        /// <returns>True if adding the game succeeded</returns>
        public bool AddGame(string gameTitle)
        {
            lock(LockObj)
            {
                var gameId = _currentId++;

                var game = new VideoGame
                {
                    Id = gameId,
                    IsOwned = false,
                    NumberOfVotes = 1,
                    Title = gameTitle
                };
                AllGames.Add(gameId, game);
            }

            return true;
        }

        /// <summary>
        /// Returns a list of all games currently in the database.
        /// </summary>
        /// <returns>List of all games currently in the database</returns>
        public IEnumerable<VideoGame> GetAllGames()
        {
            return AllGames.Values;
        }

        /// <summary>
        /// Adds a vote to the game with the specified ID.
        /// </summary>
        /// <param name="gameId">ID of the game to vote on</param>
        /// <returns>True if voting succeeded</returns>
        public bool AddVote(int gameId)
        {
            if (AllGames[gameId] != null)
            {
                lock (LockObj)
                {
                    if (AllGames[gameId] != null)
                    {
                        AllGames[gameId].NumberOfVotes++;

                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Removes all games from the database.
        /// </summary>
        /// <returns>True if removing all games succeeded</returns>
        public bool ClearAllGames()
        {
            lock(LockObj)
            {
                AllGames.Clear();
                _currentId = 1;
            }
            return true;
        }

        /// <summary>
        /// Sets the 'owned' status to true for the game with the specified ID
        /// </summary>
        /// <param name="gameId">ID of the game that is now owned</param>
        /// <returns>True if setting the status succeeded</returns>
        public bool MarkAsOwned(int gameId)
        {
            if(AllGames[gameId] != null)
            {
                lock (LockObj)
                {
                    if(AllGames[gameId] != null)
                    {
                        AllGames[gameId].IsOwned = true;

                        return true;
                    }
                }
            }
            return false;
        }

        #endregion
    }
}
