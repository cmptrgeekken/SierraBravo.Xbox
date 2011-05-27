/* This program is free software. It comes without any warranty, to
 * the extent permitted by applicable law. You can redistribute it
 * and/or modify it under the terms of the Do What The Fuck You Want
 * To Public License, Version 2, as published by Sam Hocevar. See
 * http://sam.zoy.org/wtfpl/COPYING for more details. */
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using SierraBravo.Xbox.Repositories;
using SierraBravo.Xbox.Repositories.XboxVotingService;

namespace SierraBravo.Xbox.Tests.Repositories
{
    /// <summary>
    /// Basic unit test fixture for the Xbox voting repository. Ensures
    /// that the appropriate web service routines are called and that 
    /// conversion to the VideoGame DTO is handled properly
    /// </summary>
    [TestFixture]
    public class XboxVotingRepositoryUnitTests
    {
        /// <summary>
        /// Mock interface to the web service
        /// </summary>
        private XboxVotingServiceSoap _mockVotingService;
        
        /// <summary>
        /// Voting repository that is being tested
        /// </summary>
        private XboxVotingRepository _xboxVotingRepository;
        
        /// <summary>
        /// Initialize variables with each run to ensure that
        /// the mock state does not impact tests.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _mockVotingService = MockRepository.GenerateMock<XboxVotingServiceSoap>();
            _xboxVotingRepository = new XboxVotingRepository(_mockVotingService);
        }

        /// <summary>
        /// Verifies that adding a game calls the appropriate service methods.
        /// </summary>
        [Test]
        public void AddingGameByTitleCallsClientAddGame()
        {
            _xboxVotingRepository.AddGame("test");

            _mockVotingService.AssertWasCalled(mvs => mvs.AddGame(null,null), opt => opt.IgnoreArguments());
        }

        /// <summary>
        /// Verifies that the IsOwned parameter of the VideoGame structure is set to true only if 
        /// the returned value from the service call is 'gotit'
        /// </summary>
        [Test]
        public void GettingAllGamesSetsIsOwnedStatusToTrueWhenStatusEqualsGotItAndFalseOtherwise()
        {
            _mockVotingService.Stub(xvs => xvs.GetGames(Arg<string>.Is.Anything)).Return(new []
                                                                                              {
                                                                                                  new XboxGame
                                                                                                      {
                                                                                                          Id = 1,
                                                                                                          Status = "gotit",
                                                                                                          Title = "test",
                                                                                                          Votes = 1
                                                                                                      },
                                                                                                  new XboxGame
                                                                                                      {
                                                                                                          Id = 2,
                                                                                                          Status = "dontgotit",
                                                                                                          Title = "test2",
                                                                                                          Votes = 1
                                                                                                      },
                                                                                                  new XboxGame
                                                                                                      {
                                                                                                          Id = 3,
                                                                                                          Status = "notowned",
                                                                                                          Title = "test3",
                                                                                                          Votes = 1
                                                                                                      }
                                                                                              });

            var games = _xboxVotingRepository.GetAllGames();

            Assert.AreEqual(3,games.Count());
            Assert.IsTrue(games.First().IsOwned);
            Assert.IsFalse(games.ElementAt(1).IsOwned);
            Assert.IsFalse(games.ElementAt(2).IsOwned);
        }

        /// <summary>
        /// Ensures that retrieving all games calls the appropriate service method
        /// </summary>
        [Test]
        public void GettingAllGamesCallsClientGetGames()
        {
            _mockVotingService.Stub(mvs => mvs.GetGames(Arg<string>.Is.Anything)).Return(new XboxGame[0]);

            _xboxVotingRepository.GetAllGames();

            _mockVotingService.AssertWasCalled(mvs => mvs.GetGames(null),opt => opt.IgnoreArguments());
        }


        /// <summary>
        /// Ensures that clearing all games calls the appropriate service method
        /// </summary>
        [Test]
        public void ClearingAllGamesCallsClientClearGames()
        {
            _xboxVotingRepository.ClearAllGames();

            _mockVotingService.AssertWasCalled(xvs => xvs.ClearGames(Arg<string>.Is.Anything),opt => opt.IgnoreArguments());
        }

        /// <summary>
        /// Ensures that marking a game as owned calls the appropriate service method
        /// </summary>
        [Test]
        public void SettingGameAsOwnedCallsClientSetGotIt()
        {
            _mockVotingService.Expect(xvs => xvs.SetGotIt(Arg<int>.Is.Anything, Arg<string>.Is.Anything)).Return(true);
            _mockVotingService.Replay();

            _xboxVotingRepository.MarkAsOwned(1);

            _mockVotingService.VerifyAllExpectations();
        }
    }
}
