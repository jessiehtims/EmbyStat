using EmbyStat.Clients.Base.Http;
using System.Collections.Generic;
using System.Linq;
using EmbyStat.Clients.Base.Converters;
using EmbyStat.Common.Extensions;
using EmbyStat.Common.Models;
using EmbyStat.Common.Models.Entities;
using EmbyStat.Common.Models.Net;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Querying;
using RestSharp;

using EmbyStat.Services.Interfaces;

namespace EmbyStat.Clients.Jellyfin.Http
{
    public class JellyfinHttpClient : BaseHttpClient, IJellyfinHttpClient
    {

        protected readonly ISettingsService _settingsService;
        public JellyfinHttpClient(IRestClient client, ISettingsService settingsService) : base(client)
        {
            _settingsService = settingsService;
        }

        public bool Ping()
        {
            return Ping("Jellyfin Server");
        }

        public MediaServerUdpBroadcast SearchServer()
        {
            return SearchServer("who is JellyfinServer?");
        
        }


        new public List<Movie> GetMovies(string parentId, int startIndex, int limit, string userId = "")
        {
           return base.GetMovies(parentId,startIndex,limit, GetAdminUserId());
        }

        new public List<BoxSet> GetBoxSet(string parentId, string userId = "")
        {
           return base.GetBoxSet(parentId, GetAdminUserId());
        }

        new public List<Show> GetShows(string parentId, string userId = "")
        {
            return base.GetShows(parentId, GetAdminUserId());
        }

        new public List<Season> GetSeasons(string parentId , string userId = "")
        {
           return base.GetSeasons(parentId, GetAdminUserId());
        }

        new public List<Episode> GetEpisodes(IEnumerable<string> parentIds, string showId, string userId = "")
        {
            return base.GetEpisodes(parentIds, showId, GetAdminUserId());
        }

        new public int GetMovieCount(string parentId, string userId = "")
        {
          return base.GetMovieCount(parentId, GetAdminUserId());
        }
        public string GetAdminUserId() {
        return _settingsService.GetUserSettings().MediaServer.UserId;
        }
    }

  
}
