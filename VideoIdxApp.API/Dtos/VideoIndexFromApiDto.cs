using System;

namespace VideoIdxApp.API.Dtos
{
    public class VideoIndexFromApiDto
    {
        public string accountId { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string userName { get; set; }
        public DateTime created { get; set; }
        public string privacyMode { get; set; }
        public string state { get; set; }
        public bool isOwned { get; set; }
        public bool isEditable { get; set; }
        public bool isBase { get; set; }
        public int durationInSeconds { get; set; }
    }
}