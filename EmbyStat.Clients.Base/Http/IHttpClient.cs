using System.Collections.Generic;
using EmbyStat.Common.Models;
using EmbyStat.Common.Models.Entities;
using EmbyStat.Common.Models.Net;
using MediaBrowser.Model.IO;
using MediaBrowser.Model.Querying;
using Newtonsoft.Json.Linq;

namespace EmbyStat.Clients.Base.Http
{
    public interface IHttpClient
    {
        void SetDeviceInfo(string deviceName, string authorizationScheme, string applicationVersion, string deviceId);
        string BaseUrl { get; set; }
        string ApiKey { get; set; }
        List<PluginInfo> GetInstalledPlugins();
        ServerInfo GetServerInfo();
        List<FileSystemEntryInfo> GetLocalDrives();
        JArray GetUsers();
        JObject GetDevices();
        bool Ping();
        MediaServerUdpBroadcast SearchServer();
        List<Movie> GetMovies(string parentId, int startIndex, int limit, string userId = "");
        List<BoxSet> GetBoxSet(string parentId, string userId = "");
        List<Show> GetShows(string libraryId, string userId = "");
        List<Season> GetSeasons(string parentId, string userId = "");
        List<Episode> GetEpisodes(IEnumerable<string> parentIds, string showId, string userId = "");
        int GetMovieCount(string parentId, string userId = "");
        Person GetPersonByName(string personName);
        QueryResult<BaseItemDto> GetMediaFolders();
    }
}
