using System.Collections.Generic;
using SierraBravo.Xbox.Common.DTOs;

namespace SierraBravo.Xbox.Services.Interfaces
{
    public interface IVideoGameVotingService
    {
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
        bool AddGame(string gameTitle);

        /// <summary>
        /// Returns the game with the provided ID.
        /// </summary>
        /// <remarks>As the number of games in the database increases,
        /// the performance of this method will decrease. This is due to
        /// the lack of index-based searching in the SOAP client.</remarks>
        /// <param name="gameId">ID of the game to return</param>
        /// <returns>The game with the provided ID, or null if not found</returns>
        VideoGame GetGameById(int gameId);

        /// <summary>
        /// Returns a list of all games in the repository.
        /// </summary>
        /// <returns>List of all games</returns>
        IEnumerable<VideoGame> GetAllGames();

        /// <summary>
        /// Returns a list of all games, limited by whether
        /// or not it is currently owned.
        /// </summary>
        /// <param name="isOwned">Owned status of the game</param>
        /// <returns>List of games that match the criteria</returns>
        IEnumerable<VideoGame> GetAllGames(bool isOwned);

        /// <summary>
        /// Marks a game with the specified ID as owned.
        /// </summary>
        /// <param name="gameId">ID of the game to mark as owned</param>
        /// <returns>True if the game was marked as owned</returns>
        bool MarkGameAsOwned(int gameId);

        /// <summary>
        /// Casts a vote for a game, as long as it is not
        /// currently owned.
        /// </summary>
        /// <param name="gameId">ID of the game to vote for</param>
        /// <returns>True if voting succeeded</returns>
        bool VoteForGame(int gameId);

        /// <summary>
        /// Deletes all games from the repository
        /// </summary>
        /// <returns>True if successfully cleared</returns>
        bool ClearAllGames();
    }
}