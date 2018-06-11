using RestSharp;
using Rocket.Chat.RestAPI.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.Chat.RestAPI
{
    public partial class RocketChatConnection
    {
        public Task<List<Channel>> GetChannelsAsync(uint count = 0, uint offset = 0) => Task.Run(() =>
        {
            return GetChannels(offset);
        });
        public List<Channel> GetChannels(uint count = 0, uint offset = 0)
        {
            Assert.ThrowIfVersionLowerThan(0, 49, 0);

            var request = new RestRequest("api/v1/channels.list", Method.GET)
            {
                RequestFormat = DataFormat.Json
            };

            if (count > 0)
                request.AddQueryParameter("count", count.ToString());

            request.AddHeader("X-Auth-Token", Authoring.AuthToken);
            request.AddHeader("X-User-Id", Authoring.UserId);

            if (count == 0)
            {
                List<Channel> channels = new List<Channel>();
                bool stop = false;
                offset = 0;
                do
                {
                    if (offset > 0)
                        request.AddQueryParameter("offset", offset.ToString());

                    var response = Client.Execute<ChannelsResult>(request);
                    if (!response.IsSuccessful)
                    {
                        throw new Exception(response.ErrorMessage, response.ErrorException);
                    }
                    var channelsResult = response.Data.Channels;
                    channels.AddRange(channelsResult);
                    if (channelsResult.Count < 50)
                        stop = true;
                    offset += 50;
                } while (!stop);

                return channels;
            }
            else
            {
                if (offset > 0)
                    request.AddQueryParameter("offset", offset.ToString());

                var response = Client.Execute<ChannelsResult>(request);
                if (!response.IsSuccessful)
                {
                    throw new Exception(response.ErrorMessage, response.ErrorException);
                }
                return response.Data.Channels;
            }
        }

        public Task<Channel> GetChannelAsync(string name) => Task.Run(() =>
        {
            return GetChannel(name);
        });
        public Channel GetChannel(string name)
        {
            Assert.ThrowIfVersionLowerThan(0, 48, 0);

            var request = new RestRequest($"api/v1/channels.info?roomName={name}", Method.GET)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddHeader("X-Auth-Token", Authoring.AuthToken);
            request.AddHeader("X-User-Id", Authoring.UserId);

            var response = Client.Execute<RequestResult<Channel>>(request);
            if (!response.IsSuccessful)
            {
                if (response.Data.ErrorType.Equals(""))
                {
                    throw new Exception($"couldn't find {name} channel");
                }
                else
                {
                    throw new Exception(response.Data.Error);
                }
            }

            return response.Data.Channel;
        }
    }
}
