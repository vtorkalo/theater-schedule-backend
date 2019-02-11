﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TheaterSchedule.BLL.DTO;
using TheaterSchedule.BLL.Interfaces;

namespace TheaterSchedule.Controllers
{
    [Route( "api/[controller]" )]
    [ApiController]
    public class WatchlistController : Controller
    {
        private IWatchlistService watchlistService;

        public WatchlistController( IWatchlistService watchlistService )
        {
            this.watchlistService = watchlistService;
        }

        [HttpGet( "{phoneId}/{languageCode}" )]
        public IEnumerable<WatchlistDTO> GetWatchlist(
            string phoneId, string languageCode )
        {
            return watchlistService.GetWatchlist( phoneId, languageCode );
        }

        [HttpPost( "{phoneId}/{scheduleId}" )]
        public IActionResult SaveOrDeletePerformance(
            string phoneId, int scheduleId )
        {
            watchlistService.SaveOrDeletePerformance( phoneId, scheduleId );
            return Ok();
        }
    }
}
