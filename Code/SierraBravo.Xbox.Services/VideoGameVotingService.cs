/* This program is free software. It comes without any warranty, to
 * the extent permitted by applicable law. You can redistribute it
 * and/or modify it under the terms of the Do What The Fuck You Want
 * To Public License, Version 2, as published by Sam Hocevar. See
 * http://sam.zoy.org/wtfpl/COPYING for more details. */
using System;
using System.Collections.Generic;
using System.Linq;
using SierraBravo.Xbox.Common.DTOs;
using SierraBravo.Xbox.Repositories;
using SierraBravo.Xbox.Repositories.Interfaces;
using SierraBravo.Xbox.Services.Interfaces;

namespace SierraBravo.Xbox.Services
{
    public class VideoGameVotingService : IVideoGameVotingService
    {
        /// <summary>
        /// As there is no concept of transactions with the web service routine,
        /// to ensure that the business logic is followed, all transactions that
        /// may change the state of the database will be wrapped in lock statements.
        /// </summary>
        /// <remarks>Double-checked locking is not used for performance reasons.</remarks>
        private static readonly object LockObj = new object();

        private readonly IVotingRepository _votingRepository;
        
        public VideoGameVotingService(IVotingRepository votingRepository)
        {
            _votingRepository = votingRepository;
        }


        /// <summary>
        /// Adds the game with the provided title to the repository,
        /// as long as it does not currently exist.
        /// </summary>
        /// <remarks>As the number of games in the database increases,
        /// the performance of this method will decrease. This is due 
        /// to the limitations of the SOAP client, which does not allow 
        /// for database searching.</remarks>
        /// <param name="gameTitle">Title of game to add</param>
        /// <returns>True if the title does not exist and it is added successfully</returns>
        public bool AddGame(string gameTitle)
        {
            // Do not allow empty titles.
            if (String.IsNullOrEmpty(gameTitle)) return false;

            var success = false;
            lock (LockObj)
            {
                var allGames = GetAllGames();

                if (!allGames.Any(g => g.Title == gameTitle))
                {
                    success = _votingRepository.AddGame(gameTitle);
                }
            }
            return success;
        }

        /// <summary>
        /// Returns the game with the provided ID.
        /// </summary>
        /// <remarks>As the number of games in the database increases,
        /// the performance of this method will decrease. This is due to
        /// the lack of index-based searching in the SOAP client.</remarks>
        /// <param name="gameId">ID of the game to return</param>
        /// <returns>The game with the provided ID, or null if not found</returns>
        public VideoGame GetGameById(int gameId)
        {
            var allGames = GetAllGames();

            return allGames.FirstOrDefault(g => g.Id == gameId);
        }


        /// <summary>
        /// Returns a list of all games in the repository.
        /// </summary>
        /// <returns>List of all games</returns>
        public IEnumerable<VideoGame> GetAllGames()
        {
            return _votingRepository.GetAllGames();
        }

        /// <summary>
        /// Returns a list of all games, limited by whether
        /// or not it is currently owned.
        /// </summary>
        /// <param name="isOwned">Owned status of the game</param>
        /// <returns>List of games that match the criteria</returns>
        public IEnumerable<VideoGame> GetAllGames(bool isOwned)
        {
            return GetAllGames().Where(g => g.IsOwned == isOwned);
        }

        /// <summary>
        /// Marks a game with the specified ID as owned.
        /// </summary>
        /// <param name="gameId">ID of the game to mark as owned</param>
        /// <returns>True if the game was marked as owned</returns>
        public bool MarkGameAsOwned(int gameId)
        {
            var success = false;
            lock (LockObj)
            {
                var game = GetGameById(gameId);

                if (game != null && game.IsOwned == false)
                {
                    success = _votingRepository.MarkAsOwned(gameId);
                }
            }
            return success;
        }

        /// <summary>
        /// Casts a vote for a game, as long as it is not
        /// currently owned.
        /// </summary>
        /// <param name="gameId">ID of the game to vote for</param>
        /// <returns>True if voting succeeded</returns>
        public bool VoteForGame(int gameId)
        {
            var success = false;
            lock(LockObj)
            {
                var game = GetGameById(gameId);
            
                if( game != null && game.IsOwned == false)
                {
                    success = _votingRepository.AddVote(gameId);
                }
            }
            return success;
        }

        /// <summary>
        /// Deletes all games from the repository
        /// </summary>
        /// <returns>True if successfully cleared</returns>
        public bool ClearAllGames()
        {
            return _votingRepository.ClearAllGames();
        }
    }
}
