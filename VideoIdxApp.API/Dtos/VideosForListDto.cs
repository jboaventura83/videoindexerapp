using System;
using System.Collections.Generic;

namespace VideoIdxApp.API.Dtos
{
    public class VideosForListDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public string AccessToken { get; set; }
        public string ThumbnailId { get; set; }     
        public string Base64ThumbnailImage {get; set;}
        public string EmbedPlayerUrl { get; set; }   
        public string EmbedInsightsUrl { get; set; }
    }
}