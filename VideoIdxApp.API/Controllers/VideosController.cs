using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using VideoIdxApp.API.Dtos;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace VideoIdxApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class VideosController : ControllerBase
    {
        private readonly IConfiguration _config;
        public VideosController(IConfiguration config)
        {
            _config = config;

        }

        [HttpGet("auth", Name = "GetAccessTokenVideoIndexer")]
        public async Task<IActionResult> GetAccessTokenVideoIndexer()
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _config.GetSection("AppSettings:SubscriptionKeyApi").Value);

            // Request parameters
            queryString["allowEdit"] = "False";
            var uri = "https://api.videoindexer.ai/auth/"+ _config.GetSection("AppSettings:LocationApi").Value +
                        "/Accounts/"+ _config.GetSection("AppSettings:AccountIdApi").Value  +"/AccessToken?" + queryString;

            var response = await client.GetAsync(uri);

            return Ok(await response.Content.ReadAsStringAsync());
        }
        
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVideo(string id)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            var accessToken = await this.GetToken();
            accessToken = accessToken.Replace("\"","");

            // Request parameters
            queryString["language"] = "pt-BR";
            queryString["reTranslate"] = "False";
            queryString["accessToken"] = accessToken;
            var uri = "https://api.videoindexer.ai/"+ _config.GetSection("AppSettings:LocationApi").Value +
                "/Accounts/"+ _config.GetSection("AppSettings:AccountIdApi").Value  +"/Videos/"+ id + "/Index?" + queryString;

            var response = await client.GetAsync(uri);

            VideoIndexFromApiDto videoFromApi = JsonConvert.DeserializeObject<VideoIndexFromApiDto>(await response.Content.ReadAsStringAsync()); 

            var videoAccessToken = await this.GetVideoAccessToken(videoFromApi.id);
            videoAccessToken = videoAccessToken.Replace("\"","");

            if(videoFromApi != null)
            {
               var videoToReturn = new VideosForListDto{
                   Id = videoFromApi.id,
                   Name = videoFromApi.name,
                   AccessToken = videoAccessToken,
                   ThumbnailId = "",
                   State = videoFromApi.state,
                   EmbedPlayerUrl = "https://www.videoindexer.ai/embed/player/" + videoFromApi.accountId + "/"+ videoFromApi.id +"/?locale=pt&accessToken="+videoAccessToken,
                   EmbedInsightsUrl = "https://www.videoindexer.ai/embed/insights/" + videoFromApi.accountId + "/"+ videoFromApi.id +"/?locale=pt&accessToken="+videoAccessToken
               };
               return Ok(videoToReturn);     
            }

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetVideos()
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            var accessToken = await this.GetToken();
            accessToken = accessToken.Replace("\"","");

            // Request parameters
            queryString["pageSize"] = "25";
            queryString["skip"] = "0";
            var uri = "https://api.videoindexer.ai/"+ _config.GetSection("AppSettings:LocationApi").Value +"/Accounts/"+
                _config.GetSection("AppSettings:AccountIdApi").Value  +"/Videos?accessToken="+ accessToken +"&" + queryString;

            var response = await client.GetAsync(uri);   

            VideosFromApiDto videosForList = JsonConvert.DeserializeObject<VideosFromApiDto>(await response.Content.ReadAsStringAsync()); 

            if(videosForList.results.Any())
            {
                var videosToReturn = new List<VideosForListDto>();
                foreach (var video in videosForList.results)
                {
                    var videoDto = new VideosForListDto{
                        Id = video.id,
                        Name = video.name,
                        State = video.state,
                        ThumbnailId = video.thumbnailId,
                        Base64ThumbnailImage = await this.GetVideoThumbnail(video.id, video.thumbnailId),
                        AccessToken = "",
                        EmbedPlayerUrl = "",
                        EmbedInsightsUrl = ""
                    };
                    videosToReturn.Add(videoDto);
                }
                return Ok(videosToReturn);
            }      

            return NoContent();
        }



        private async Task<string> GetToken()
        {
             var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _config.GetSection("AppSettings:SubscriptionKeyApi").Value);

            // Request parameters
            queryString["allowEdit"] = "False";
            var uri = "https://api.videoindexer.ai/auth/"+ _config.GetSection("AppSettings:LocationApi").Value +
                        "/Accounts/"+ _config.GetSection("AppSettings:AccountIdApi").Value  +"/AccessToken?" + queryString;

            var response = await client.GetAsync(uri);

            return await response.Content.ReadAsStringAsync();
        }

        private async Task<string> GetVideoAccessToken(string videoId)
        {
             var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _config.GetSection("AppSettings:SubscriptionKeyApi").Value);

            // Request parameters
            queryString["allowEdit"] = "True";
            var uri = "https://api.videoindexer.ai/auth/"+ _config.GetSection("AppSettings:LocationApi").Value +
                        "/Accounts/"+ _config.GetSection("AppSettings:AccountIdApi").Value  +
                        "/Videos/" + videoId + "/AccessToken?" + queryString;

            var response = await client.GetAsync(uri);

            return await response.Content.ReadAsStringAsync();
        }



        private async Task<string> GetVideoThumbnail(string videoId, string thumbnailId)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            var accessToken = await this.GetToken();
            accessToken = accessToken.Replace("\"","");


            // Request parameters
            queryString["format"] = "Base64";
            queryString["accessToken"] = accessToken;
            var uri = "https://api.videoindexer.ai/"+ _config.GetSection("AppSettings:LocationApi").Value + 
                "/Accounts/"+ _config.GetSection("AppSettings:AccountIdApi").Value  +"/Videos/" + videoId +"/Thumbnails/"+ thumbnailId +"?" + queryString;

            var response = await client.GetAsync(uri);

            return await response.Content.ReadAsStringAsync();
        }
    }
}