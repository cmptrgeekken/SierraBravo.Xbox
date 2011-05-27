/* This program is free software. It comes without any warranty, to
 * the extent permitted by applicable law. You can redistribute it
 * and/or modify it under the terms of the Do What The Fuck You Want
 * To Public License, Version 2, as published by Sam Hocevar. See
 * http://sam.zoy.org/wtfpl/COPYING for more details. */
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using SierraBravo.Xbox.Common.DTOs;
using SierraBravo.Xbox.Common.Enums;
using SierraBravo.Xbox.Repositories.Interfaces;
using SierraBravo.Xbox.Services;

namespace SierraBravo.Xbox.Tests.Services
{
    /// <summary>
    /// Unit test fixture for the video game voting service. Ensures that the appropriate
    /// business logic is in place and that the corresponding repository methods are called
    /// when required.
    /// </summary>
    [TestFixture]
    public class VideoGameVotingServiceUnitTests
    {
        private readonly IList<VideoGame> _games;
        private IVotingRepository _mockVotingRepository;
        private VideoGameVotingService _votingService;

        public VideoGameVotingServiceUnitTests()
        {
            _games = new List<VideoGame>
                         {
                             new VideoGame
                                 {
                                     Id = 1,
                                     Title = "test",
                                     NumberOfVotes = 1,
                                     IsOwned = false,
                                     GameType = VideoGameType.Xbox
                                 },
                             new VideoGame
                                 {
                                     Id = 2,
                                     Title = "test2",
                                     NumberOfVotes = 1,
                                     IsOwned = true,
                                     GameType = VideoGameType.Xbox
                                 }
                         };
        }

        [SetUp]
        public void SetUp()
        {
            _mockVotingRepository = MockRepository.GenerateMock<IVotingRepository>();
            _votingService = new VideoGameVotingService(_mockVotingRepository);

            _mockVotingRepository.Stub(mvr => mvr.GetAllGames()).Return(_games);
        }

        [Test]
        public void AddGameWithNullTitleReturnsFalseAndDoesNotCallRepoAddGame()
        {
            var success = _votingService.AddGame(null);

            Assert.IsFalse(success);
            _mockVotingRepository.AssertWasNotCalled(mvr => mvr.AddGame(null));
        }

        [Test]
        public void AddGameWithEmptyTitleReturnsFalseAndDoesNotCallRepoAddGame()
        {
            var success = _votingService.AddGame("");

            Assert.IsFalse(success);
            _mockVotingRepository.AssertWasNotCalled(mvr => mvr.AddGame(null));
        }

        [Test]
        public void AddGameWithValidTitleReturnsFalseIfTitleExists()
        {
            var success = _votingService.AddGame(_games.First().Title);

            Assert.IsFalse(success);
        }

        [Test]
        public void AddGameWithValidTitleCallsRepoAddGameIfTitleDoesNotExist()
        {
            _votingService.AddGame("newtitle");

            _mockVotingRepository.AssertWasCalled(mvr => mvr.AddGame(null), opt => opt.IgnoreArguments());
        }

        [Test]
        public void GetGameByIdReturnsNullIfGameWithIdDoesNotExist()
        {
            var game = _votingService.GetGameById(42);

            Assert.IsNull(game);
        }

        [Test]
        public void GetGameByIdReturnsCorrectGame()
        {
            var game = _votingService.GetGameById(_games.First().Id);

            Assert.AreEqual(_games.First(), game);
        }

        [Test]
        public void GetAllGamesCallsRepoGetGames()
        {
            _votingService.GetAllGames();

            _mockVotingRepository.AssertWasCalled(mvr => mvr.GetAllGames());
        }

        [Test]
        public void MarkGameAsOwnedReturnsFalseAndDoesNotCallRepoMarkOwnedIfGameNotFound()
        {
            var success = _votingService.MarkGameAsOwned(42);

            Assert.IsFalse(success);
            _mockVotingRepository.AssertWasNotCalled(mvr => mvr.MarkAsOwned(0),opt => opt.IgnoreArguments());
        }

        [Test]
        public void MarkGameAsOwnedReturnsFalseAndDoesNotCallRepoMarkOwnedIfAlreadyOwned()
        {
            var success = _votingService.MarkGameAsOwned(_games.First(g => g.IsOwned).Id);

            Assert.IsFalse(success);
            _mockVotingRepository.AssertWasNotCalled(mvr => mvr.MarkAsOwned(0), opt => opt.IgnoreArguments());
        }

        [Test]
        public void MarkGameAsOwnedCallsRepoMarkOwnedIfNotOwned()
        {
            _votingService.MarkGameAsOwned(_games.First(g => !g.IsOwned).Id);

            _mockVotingRepository.AssertWasCalled(mvr => mvr.MarkAsOwned(0), opt => opt.IgnoreArguments());
        }

        [Test]
        public void VoteForGameReturnsFalseAndDoesNotCallRepoVoteForGameIfGameNotFound()
        {
           var success = _votingService.VoteForGame(42);

            Assert.IsFalse(success);
            _mockVotingRepository.AssertWasNotCalled(mvr => mvr.AddVote(0), opt => opt.IgnoreArguments());
        }

        [Test]
        public void VoteForGameReturnsFalseAndDoesNotCallRepoVoteForGameIfGameOwned()
        {
            var success = _votingService.VoteForGame(_games.First(g => g.IsOwned).Id);

            Assert.IsFalse(success);
            _mockVotingRepository.AssertWasNotCalled(mvr => mvr.AddVote(0), opt => opt.IgnoreArguments());
        }

        [Test]
        public void VoteForGameCallsRepoVoteForGameIfGameNotOwned()
        {
            _votingService.VoteForGame(_games.First(g => !g.IsOwned).Id);

            _mockVotingRepository.AssertWasCalled(mvr => mvr.AddVote(0), opt => opt.IgnoreArguments());
        }

        [Test]
        public void ClearAllGamesCallsRepoClearGames()
        {
            _votingService.ClearAllGames();

            _mockVotingRepository.AssertWasCalled(mvr => mvr.ClearAllGames());
        }
    }
}
