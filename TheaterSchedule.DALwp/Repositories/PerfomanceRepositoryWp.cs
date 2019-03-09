﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheaterSchedule.DAL.Interfaces;
using TheaterSchedule.DAL.Models;
using WordPressPCL;

namespace TheaterSchedule.DALwp.Repositories
{
    public class TitleItem : WordPressPCL.Models.Base
    {
        [JsonProperty("rendered", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Rendered { get; set; }
    }

    public class Performance : WordPressPCL.Models.Base
    {
        [JsonProperty("title", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public TitleItem Title { get; set; }

        [JsonProperty("date", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime Date { get; set; }

        [JsonProperty("featured_media", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Featured_media { get; set; }
    }

    public class Media_detailsItem : WordPressPCL.Models.Base
    {
        [JsonProperty("sizes", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public SizesItem Sizes { get; set; }
    }

    public class SizesItem : WordPressPCL.Models.Base
    {
        [JsonProperty("full", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public FullItem Full { get; set; }
    }

    public class FullItem : WordPressPCL.Models.Base
    {
        [JsonProperty("source_url", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Source_url { get; set; }
    }

    public class Media : WordPressPCL.Models.Base
    {
        [JsonProperty("media_details", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Media_detailsItem Media_details { get; set; }
    }

    public class PerfomanceRepositoryWp : Performance, IPerfomanceRepository
    {

        private async Task<IEnumerable<PerformanceDataModel>> GetPerformanceTitlesAndImagesAsync(string languageCode)
        {
            //no localisation yet

            var client = new Repository().InitializeClient();
            var performances = await client.CustomRequest.Get<IEnumerable<Performance>>($"wp/v2/performance");
        
            List<PerformanceDataModel> performancesData = performances.Select(p => new PerformanceDataModel
            {
                PerformanceId = p.Id,
                Title = p.Title.Rendered,
                MainImageUrl = client.CustomRequest.Get<Media>($"wp/v2/media/{p.Featured_media}").Result.Media_details.Sizes.Full.Source_url
            }).ToList();

            return performancesData;
        }

        List<PerformanceDataModel> IPerfomanceRepository.GetPerformanceTitlesAndImages(string languageCode)
        {
            return GetPerformanceTitlesAndImagesAsync(languageCode).Result.ToList();
        }
    }
}