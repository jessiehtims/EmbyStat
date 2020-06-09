using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmbyStat.Clients.Base;
using EmbyStat.Clients.Base.Http;
using EmbyStat.Common;
using EmbyStat.Common.Enums;
using EmbyStat.Common.Models.Entities;
using EmbyStat.Common.Models.Settings;
using EmbyStat.Repositories.Interfaces;
using EmbyStat.Services;
using EmbyStat.Services.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;
using ServerInfo = EmbyStat.Common.Models.Entities.ServerInfo;

namespace Tests.Unit.Services
{
    public class JellyFinServiceTests
    {
        private Mock<IMovieRepository> _movieRepositoryMock;
        private Mock<IShowRepository> _showRepositoryMock;
        private Mock<IMediaServerRepository> _embyRepositoryMock;
        private Mock<ISessionService> _sessionServiceMock;
        private readonly Mock<ISettingsService> _settingsServiceMock;
        private Mock<IHttpClient> _httpClientMock;

        public JellyFinServiceTests()
        {
            _settingsServiceMock = new Mock<ISettingsService>();
            _settingsServiceMock.Setup(x => x.GetUserSettings()).Returns(new UserSettings
            {
                MediaServer = new MediaServerSettings
                {
                    ApiKey = "123",
                    ServerAddress = "localhost",
                    ServerPort = 1,
                    ServerProtocol = ConnectionProtocol.Http,
                }
            });

            _settingsServiceMock.Setup(x => x.GetAppSettings()).Returns(new AppSettings()
            {
                Version = "0.0.0.0"
            });
        }
       [Fact]
       public void Jellyfin_service_calls_for_items_should_include_admin_userid() {
            _httpClientMock = new Mock<IHttpClient>();
            _embyRepositoryMock = new Mock<IMediaServerRepository>();
            _movieRepositoryMock = new Mock<IMovieRepository>();
            _showRepositoryMock = new Mock<IShowRepository>();
            _embyRepositoryMock = new Mock<IMediaServerRepository>();
            _sessionServiceMock = new Mock<ISessionService>();


       }
    }
}