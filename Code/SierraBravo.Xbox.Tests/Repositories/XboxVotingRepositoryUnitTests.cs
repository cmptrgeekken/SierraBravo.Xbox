/* This program is free software. It comes without any warranty, to
 * the extent permitted by applicable law. You can redistribute it
 * and/or modify it under the terms of the Do What The Fuck You Want
 * To Public License, Version 2, as published by Sam Hocevar. See
 * http://sam.zoy.org/wtfpl/COPYING for more details. */
using NUnit.Framework;
using Rhino.Mocks;
using SierraBravo.Xbox.Repositories;
using SierraBravo.Xbox.Repositories.XboxVotingService;

namespace SierraBravo.Xbox.Tests.Repositories
{
    /// <summary>
    /// Basic test fixture for the Xbox voting repository. This repository
    /// cannot be too in-depth as there is no testing API key.
    /// </summary>
    [TestFixture]
    public class XboxVotingRepositoryUnitTests
    {
        private XboxVotingServiceSoap _mockVotingService;
        private XboxVotingRepository _xboxVotingRepository;

        [SetUp]
        public void SetUp()
        {
            var mockRepository = new MockRepository();
            _mockVotingService = mockRepository.DynamicMock<XboxVotingServiceSoap>();
            _xboxVotingRepository = new XboxVotingRepository(_mockVotingService);
        }

        [Test]
        public void AddingGameByTitleCallsClientAddGame()
        {
            _mockVotingService.Expect(xvs => xvs.AddGame(Arg<string>.Is.Anything, Arg<string>.Is.Anything)).Return(true);
            _mockVotingService.Replay();

            _xboxVotingRepository.AddGame("test");

            _mockVotingService.VerifyAllExpectations();
        }

        [Test]
        public void GettingAllGamesCallsClientGetGames()
        {
            _mockVotingService.Expect(xvs => xvs.GetGames(Arg<string>.Is.Anything)).Return(new XboxGame[0]);
            _mockVotingService.Replay();

            _xboxVotingRepository.GetAllGames();

            _mockVotingService.VerifyAllExpectations();
        }

        [Test]
        public void ClearingAllGamesCallsClientClearGames()
        {
            _mockVotingService.Expect(xvs => xvs.ClearGames(Arg<string>.Is.Anything)).Return(true);
            _mockVotingService.Replay();

            _xboxVotingRepository.ClearAllGames();

            _mockVotingService.VerifyAllExpectations();
        }

        [Test]
        public void SettingGameAsOwnedCallsClientSetGotIt()
        {
            _mockVotingService.Expect(xvs => xvs.SetGotIt(Arg<int>.Is.Anything, Arg<string>.Is.Anything)).Return(true);
            _mockVotingService.Replay();

            _xboxVotingRepository.MarkAsOwned(123);

            _mockVotingService.VerifyAllExpectations();
        }
    }
}
