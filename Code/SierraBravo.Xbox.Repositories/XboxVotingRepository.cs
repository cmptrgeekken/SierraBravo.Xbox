/* This program is free software. It comes without any warranty, to
 * the extent permitted by applicable law. You can redistribute it
 * and/or modify it under the terms of the Do What The Fuck You Want
 * To Public License, Version 2, as published by Sam Hocevar. See
 * http://sam.zoy.org/wtfpl/COPYING for more details. */
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using SierraBravo.Xbox.Common.DTOs;
using SierraBravo.Xbox.Common.Enums;
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
        private static readonly string ApiKey;


        /// <summary>
        /// Reference to the Sierra Bravo Voting Service SOAP Client
        /// </summary>
        private readonly XboxVotingServiceSoap _xboxVotingServiceClient;

        static XboxVotingRepository()
        {
            // TODO: Find a better way to initialize this key
            ApiKey = ConfigurationManager.AppSettings["XboxVotingService.APIKey"];

            if (String.IsNullOrWhiteSpace(ApiKey))
            {
                throw new ApplicationException("Application Configuration Setting 'XboxVotingService.APIKey' is not set.");
            }
        }

        /// <summary>
        /// Initialize a new voting repository instance with a new 
        /// Xbox Voting Service SOAP client
        /// </summary>
        public XboxVotingRepository()
            : this(new XboxVotingServiceSoapClient())
        {
            
        }

        /// <summary>
        /// Initialize a new voting repository instance with the provided Xbox Voting
        /// Service implementation
        /// </summary>
        /// <param name="client">Service to utilize</param>
        /// <exception cref="ArgumentException">Thrown if the API Key for the service is invalid</exception>
        public XboxVotingRepository(XboxVotingServiceSoap client)
        {
            // TODO: Decide if we should check the validity of the key
            //if( !IsValidKey(ApiKey) )
            //{
            //    throw new ArgumentException("ApiKey for XboxVotingServiceSoapClient is invalid.");
            //}

            _xboxVotingServiceClient = client;
        }

        /// <summary>
        /// Add a game to the catalog.
        /// </summary>
        /// <param name="gameTitle"></param>
        /// <returns>True if adding the game succeeded</returns>
        /// <exception cref="ArgumentException">Thrown if ApiKey is not valid</exception>
        public bool AddGame(string gameTitle)
        {
            return _xboxVotingServiceClient.AddGame(gameTitle, ApiKey);
        }

        /// <summary>
        /// Returns a list of all games currently in the database.
        /// </summary>
        /// <returns>List of all games currently in the database</returns>
        /// <exception cref="ArgumentException">Thrown if ApiKey is not valid</exception>
        public IEnumerable<VideoGame> GetAllGames()
        {
            return _xboxVotingServiceClient.GetGames(ApiKey).Select(g => new VideoGame
                                                                       {
                                                                           Id = g.Id,
                                                                           Title = g.Title,
                                                                           IsOwned = g.Status == "gotit",
                                                                           NumberOfVotes = g.Votes,
                                                                           GameType = VideoGameType.Xbox
                                                                       });
        }

        /// <summary>
        /// Adds a vote to the game with the specified ID.
        /// </summary>
        /// <param name="gameId">ID of the game to vote on</param>
        /// <returns>True if voting succeeded</returns>
        /// <exception cref="ArgumentException">Thrown if ApiKey is not valid</exception>
        public bool AddVote(int gameId)
        {
            return _xboxVotingServiceClient.AddVote(gameId, ApiKey);
        }

        /// <summary>
        /// Removes all games from the database.
        /// </summary>
        /// <returns>True of removing all games succeeded</returns>
        /// <exception cref="ArgumentException">Thrown if ApiKey is not valid</exception>
        public bool ClearAllGames()
        {
            return _xboxVotingServiceClient.ClearGames(ApiKey);
        }

        /// <summary>
        /// Sets the 'owned' status to true for the game with the specified ID
        /// </summary>
        /// <param name="gameId">ID of the game that is now owned</param>
        /// <returns>True if setting the status succeeded</returns>
        /// <exception cref="ArgumentException">Thrown if ApiKey is not valid</exception>
        public bool MarkAsOwned(int gameId)
        {
            return _xboxVotingServiceClient.SetGotIt(gameId, ApiKey);
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
