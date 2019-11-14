﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using EmbyStat.Clients.Emby.Http.Model;
using EmbyStat.Common;
using EmbyStat.Common.Net;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.IO;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Querying;
using Newtonsoft.Json.Linq;
using NLog;
using RestSharp;
using ServerInfo = EmbyStat.Common.Models.Entities.ServerInfo;

namespace EmbyStat.Clients.Emby.Http
{
    public class EmbyClient : IEmbyClient
    {
        private Device Device { get; set; }
        private string ClientName { get; set; }
        private string DeviceName => Device.DeviceName;
        private string ApplicationVersion { get; set; }
        private string DeviceId => Device.DeviceId;
        private string AccessToken { get; set; }
        private Guid CurrentUserId { get; set; }
        private string AuthorizationScheme { get; set; }

        private readonly Logger _logger;
        private readonly Dictionary<string, string> _httpHeaders;
        private readonly IRestClient _client;

        public EmbyClient(IRestClient client)
        {
            _client = client.UseSerializer(new JsonNetSerializer());
            _logger = LogManager.GetCurrentClassLogger();
            _httpHeaders = new Dictionary<string, string>();
            Device = new Device();
        }

        public void SetDeviceInfo(string clientName, string authorizationScheme, string applicationVersion, string deviceId)
        {
            ClientName = clientName;
            AuthorizationScheme = authorizationScheme;
            ApplicationVersion = applicationVersion;

            Device = new Device
            {
                DeviceId = deviceId,
                DeviceName = clientName
            };
            ResetHttpHeaders();
        }

        public Task<AuthenticationResult> AuthenticateUserAsync(string username, string password, string url)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            _client.BaseUrl = new Uri(url);
            ResetHttpHeaders();

            return CallAuthenticateUserApi(username, password);
        }

        private async Task<AuthenticationResult> CallAuthenticateUserApi(string username, string password)
        {
            var request = new RestRequest("Users/AuthenticateByName", Method.POST)
                .AddQueryParameter("Username", username)
                .AddQueryParameter("Pw", password);

            _logger.Info($"{Constants.LogPrefix.EmbyClient}\tAuthenticating user {username} on Emby server on {_client.BaseUrl}");
            var result = await ExecuteCall<AuthenticationResult>(request);

            if (result != null)
            {
                SetAuthenticationInfo(result.AccessToken, result.User.Id);
            }

            return result;
        }

        public Task<List<PluginInfo>> GetInstalledPluginsAsync()
        {
            var request = new RestRequest("Plugins", Method.GET);
            return ExecuteCall<List<PluginInfo>>(request);
        }

        public Task<ServerInfo> GetServerInfoAsync()
        {
            var request = new RestRequest("System/Info", Method.GET);
            return ExecuteCall<ServerInfo>(request);
        }

        public Task<List<FileSystemEntryInfo>> GetLocalDrivesAsync()
        {
            var request = new RestRequest("Environment/Drives", Method.GET);
            return ExecuteCall<List<FileSystemEntryInfo>>(request);
        }

        public Task<JArray> GetEmbyUsersAsync()
        {
            var request = new RestRequest("Users", Method.GET);
            return ExecuteCall<JArray>(request);
        }

        public Task<JObject> GetEmbyDevicesAsync()
        {
            var request = new RestRequest("Devices", Method.GET);
            return ExecuteCall<JObject>(request);
        }

        public async Task<string> PingEmbyAsync(string embyAddress)
        {
            var pingClient = new RestClient(embyAddress + "/emby");
            var request = new RestRequest("System/Ping", Method.POST) { Timeout = 5000 };
            foreach (var httpHeader in _httpHeaders)
            {
                request.AddHeader(httpHeader.Key, httpHeader.Value);
            }
            
            try
            {
                var result = await pingClient.ExecuteTaskAsync(request);
                return result.Content;
            }
            catch (Exception)
            {
                return "Ping failed";
            }
        }

        public Task<QueryResult<BaseItemDto>> GetItemsAsync(ItemQuery query)
        {
            var request = new RestRequest($"Items", Method.GET);
            request.AddItemQueryAsParameters(query);
            return ExecuteCall<QueryResult<BaseItemDto>>(request);
        }

        public Task<BaseItemDto> GetPersonByNameAsync(string personName)
        {
            var request = new RestRequest($"persons/{personName}", Method.GET);
            request.AddItemQueryAsParameters(new ItemQuery { Fields = new[] { ItemFields.PremiereDate } });
            return ExecuteCall<BaseItemDto>(request);
        }

        public async Task<QueryResult<BaseItemDto>> GetMediaFoldersAsync()
        {
            var request = new RestRequest("Library/MediaFolders", Method.GET);
            var result = await ExecuteCall<QueryResult<BaseItemDto>>(request);
            return result;
        }

        private async Task<T> ExecuteCall<T>(IRestRequest request)
        {
            foreach (var httpHeader in _httpHeaders)
            {
                request.AddHeader(httpHeader.Key, httpHeader.Value);
            }
            
            var result = await _client.ExecuteTaskAsync<T>(request);
            return result.Data;
        }

        private void SetAuthenticationInfo(string accessToken, Guid userId)
        {
            CurrentUserId = userId;
            AccessToken = accessToken;
            ResetHttpHeaders();
        }

        private void ResetHttpHeaders()
        {
            if (string.IsNullOrEmpty(AccessToken))
            {
                _httpHeaders.Remove("X-MediaBrowser-Token");
            }
            else
            {
                _httpHeaders["X-MediaBrowser-Token"] = AccessToken;
            }

            if (string.IsNullOrEmpty(AuthorizationParameter))
            {
                _httpHeaders.Remove("X-Emby-Authorization");
            }
            else
            {
                _httpHeaders["X-Emby-Authorization"] = $"{AuthorizationScheme} {AuthorizationParameter}";
            }
        }

        private string AuthorizationParameter
        {
            get
            {
                if (string.IsNullOrEmpty(ClientName) && string.IsNullOrEmpty(DeviceId) && string.IsNullOrEmpty(DeviceName))
                {
                    return string.Empty;
                }

                var header = $"Client=\"other\", DeviceId=\"{DeviceId}\", Device=\"{DeviceName}\", Version=\"{ApplicationVersion}\"";

                if (CurrentUserId != Guid.Empty)
                {
                    header += $", Emby UserId=\"{CurrentUserId}\"";
                }

                return header;
            }
        }

        public void SetAddressAndUser(string url, string token, string userId)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            _client.BaseUrl = new Uri(url);
            AccessToken = token;
            CurrentUserId = new Guid(userId);
            ResetHttpHeaders();
        }
    }
}
