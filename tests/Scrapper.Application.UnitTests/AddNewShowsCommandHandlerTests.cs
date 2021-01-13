using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Scraper.Application.Commands;
using Scraper.Clients.TvMaze;
using Scraper.Clients.TvMaze.Models;
using Scraper.Domain;

namespace Scraper.Application.UnitTests
{
    [TestFixture]
    public class AddNewShowsCommandHandlerTests
    {
        private AddNewShowsCommandHandler _addNewShowsCommandHandler;

        private Mock<ITvMazeApiClient> _mockApiClient;
        private Mock<IShowRepository> _mockShowRepository;

        [SetUp]
        public void Setup()
        {
            _mockApiClient = new Mock<ITvMazeApiClient>();
            _mockShowRepository = new Mock<IShowRepository>();
            
            _addNewShowsCommandHandler = new AddNewShowsCommandHandler(_mockApiClient.Object, _mockShowRepository.Object);
        }

        [TestCase(-1, 3)]
        [TestCase(0, 3)]
        [TestCase(1, 2)]
        [TestCase(2, 1)]
        [TestCase(3, 0)]
        public async Task Handle_AddNewShowsCommand_OnlyNewShowsAdded(long lastAddedShowId, int expectedNewShows)
        {
            _mockApiClient.Setup(x => x.GetShowIdsAsync(0, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new long[] {1, 2, 3});

            _mockApiClient.Setup(x => x.GetShowAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .Returns<long, CancellationToken>((id, _) => Task.FromResult(result: new Show
                {
                    Id = id,
                    Name = id.ToString(),
                    Embedded = new Embedded { Cast = new [] { new Cast() }}
                }));

            _mockShowRepository.Setup(x => x.GetLastAddedShowIdAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(lastAddedShowId);

            await _addNewShowsCommandHandler.Handle(new AddNewShowsCommand(), CancellationToken.None);

            _mockShowRepository.Verify(x => x.GetLastAddedShowIdAsync(It.IsAny<CancellationToken>()), Times.Once);
            _mockShowRepository.Verify(x => x.AddAsync(It.IsAny<Domain.Models.Show>(), It.IsAny<CancellationToken>()), Times.Exactly(expectedNewShows));
        }

        [Test]
        public async Task Handle_AddNewShowsCommand_CastOrderedByBirthday()
        {
            _mockApiClient.Setup(x => x.GetShowIdsAsync(0, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new long[] { 1 });

            _mockApiClient.Setup(x => x.GetShowAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .Returns<long, CancellationToken>((id, _) => Task.FromResult(new Show
                {
                    Id = id,
                    Name = id.ToString(),
                    Embedded = new Embedded
                    {
                        Cast = new[]
                        {
                            new Cast
                            {
                                Person = new Person
                                {
                                    Id = 1,
                                    Birthday = DateTime.Parse("2020-01-01")
                                }
                            },
                            new Cast
                            {
                                Person = new Person
                                {
                                    Id = 2,
                                    Birthday = DateTime.Parse("2021-01-01")
                                }
                            },
                            new Cast
                            {
                                Person = new Person
                                {
                                    Id = 3,
                                    Birthday = DateTime.Parse("2019-01-01")
                                }
                            },
                        }
                    }
                }));

            _mockShowRepository.Setup(x => x.GetLastAddedShowIdAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            await _addNewShowsCommandHandler.Handle(new AddNewShowsCommand(), CancellationToken.None);

            _mockShowRepository.Verify(x => x.AddAsync(It.Is<Domain.Models.Show>(show => show.Cast.First().Id == 2), It.IsAny<CancellationToken>()));
        }
    }
}