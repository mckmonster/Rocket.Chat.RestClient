using System;
using System.Collections.Generic;
using System.Text;

namespace Rocket.Chat.RestAPI.Data
{
    internal class ChannelsResult
    {
        public List<Channel> Channels { get; set; }

        public bool Success { get; set; }
    }
}
