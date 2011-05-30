/* This program is free software. It comes without any warranty, to
 * the extent permitted by applicable law. You can redistribute it
 * and/or modify it under the terms of the Do What The Fuck You Want
 * To Public License, Version 2, as published by Sam Hocevar. See
 * http://sam.zoy.org/wtfpl/COPYING for more details. */
using System;
using System.Collections.Generic;
using System.Linq;
using SierraBravo.Xbox.Common.DTOs;
using SierraBravo.Xbox.Repositories.Interfaces;
using SierraBravo.Xbox.Repositories.XboxVotingService;

namespace SierraBravo.Xbox.Repositories
{
    /// <summary>
    /// Wrapper for the XboxVotingServiceSoapClient web service. Implements
    /// </summary>
    public class XboxVotingRepository : IVotingRepository
    {
        /// <summary>
        /// String containing the API key necessary for connecting to the voting service.
        /// </summary>
        private readonly string _apiKey;


        /// <summary>
        /// Reference to the Sierra Bravo Voting Service SOAP Client
        /// </summary>
        private readonly XboxVotingServiceSoap _xboxVotingServiceClient;


        /// <summary>
        /// Initialize a new voting repository instance with the provided Xbox Voting
        /// Service implementation and API key
        /// </summary>
        /// <param name="client">Service to utilize</param>
        /// <param name="apiKey">API key to use for this instance</param>
        /// <exception cref="ArgumentException">Thrown if the API Key for the service is invalid</exception>
        public XboxVotingRepository(XboxVotingServiceSoap client,string apiKey)
        {
            _xboxVotingServiceClient = client;
            _apiKey = apiKey;
        }

        /// <summary>
        /// Add a game to the catalog.
        /// </summary>
        /// <param name="gameTitle"></param>
        /// <returns>True if adding the game succeeded</returns>
        /// <exception cref="ArgumentException">Thrown if _apiKey is not valid</exception>
        public bool AddGame(string gameTitle)
        {
            return _xboxVotingServiceClient.AddGame(gameTitle, _apiKey);
        }

        /// <summary>
        /// Returns a list of all games currently in the database.
        /// </summary>
        /// <returns>List of all games currently in the database</returns>
        /// <exception cref="ArgumentException">Thrown if _apiKey is not valid</exception>
        public IEnumerable<VideoGame> GetAllGames()
        {
            return _xboxVotingServiceClient.GetGames(_apiKey).Select(g => new VideoGame
                                                                       {
                                                                           Id = g.Id,
                                                                           Title = g.Title,
                                                                           IsOwned = g.Status == "gotit",
                                                                           NumberOfVotes = g.Votes
                                                                       });
        }

        /// <summary>
        /// Adds a vote to the game with the specified ID.
        /// </summary>
        /// <param name="gameId">ID of the game to vote on</param>
        /// <returns>True if voting succeeded</returns>
        /// <exception cref="ArgumentException">Thrown if _apiKey is not valid</exception>
        public bool AddVote(int gameId)
        {
            return _xboxVotingServiceClient.AddVote(gameId, _apiKey);
        }

        /// <summary>
        /// Removes all games from the database.
        /// </summary>
        /// <returns>True if removing all games succeeded</returns>
        /// <exception cref="ArgumentException">Thrown if _apiKey is not valid</exception>
        public bool ClearAllGames()
        {
            return _xboxVotingServiceClient.ClearGames(_apiKey);
        }

        /// <summary>
        /// Sets the 'owned' status to true for the game with the specified ID
        /// </summary>
        /// <param name="gameId">ID of the game that is now owned</param>
        /// <returns>True if setting the status succeeded</returns>
        /// <exception cref="ArgumentException">Thrown if _apiKey is not valid</exception>
        public bool MarkAsOwned(int gameId)
        {
            return _xboxVotingServiceClient.SetGotIt(gameId, _apiKey);
        }

        /// <summary>
        /// Determines if the provided key is valid.
        /// </summary>
        /// <returns>True if the key is valid</returns>
        protected bool IsValidKey(string apiKey)
        {
            return _xboxVotingServiceClient.CheckKey(apiKey);
        }
    }
}
