using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rocket.Chat.RestAPI.Data
{
    public class Channel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Topic { get; set; }
        public string Announcement { get; set; }
        [DeserializeAs(Name= "ts")]
        public DateTime CreationTime { get; set; }
        [DeserializeAs(Name= "lm")]
        public DateTime ModificationTime { get; set; }
        [DeserializeAs(Name= "username")]
        public string LastUser { get; set; }
        [DeserializeAs(Name= "usernames")]
        public List<string> Users { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}