﻿using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using TheaterSchedule.BLL.DTO;
using TheaterSchedule.BLL.Interfaces;
using TheaterSchedule.DAL.Interfaces;
using System.Linq;
using TheaterSchedule.BLL.Helpers;
using System.Threading.Tasks;

namespace TheaterSchedule.BLL.Services
{
    public class WishlistService : IWishlistService
    {
        private ITheaterScheduleUnitOfWork theaterScheduleUnitOfWork;
        private IWishlistRepository WishlistRepository;
        private IAccountRepository accountRepository;
        private IMemoryCache memoryCache;
        private IPerfomanceRepository perfomanceRepository;

        public WishlistService(
            ITheaterScheduleUnitOfWork theaterScheduleUnitOfWork,
            IWishlistRepository WishlistRepository,
            IAccountRepository accountRepository,
            IMemoryCache memoryCache,
            IPerfomanceRepository perfomanceRepository)
        {
            this.theaterScheduleUnitOfWork = theaterScheduleUnitOfWork;
            this.WishlistRepository = WishlistRepository;
            this.accountRepository = accountRepository;
            this.memoryCache = memoryCache;
            this.perfomanceRepository = perfomanceRepository;
        }

        private string GetKey(string languageCode)
        {
            string memoryCacheKey = Constants.PerformancesCacheKey + languageCode;
            return memoryCacheKey;
        }

        public IEnumerable<WishlistDTO> LoadWishlist(string AccountId, string languageCode)
        {
            var cacheProvider = new CacheProvider(memoryCache);

            var performancesWp = cacheProvider.GetAndSave(
                () => GetKey(languageCode),
                () => perfomanceRepository.GetPerformanceTitlesAndImages(languageCode));

            var perfIds = WishlistRepository.GetPerformanceIdsInWishlist(AccountId, languageCode);

            var result = from perfId in perfIds
                         join perfWp in performancesWp
                            on perfId equals perfWp.PerformanceId
                         select new WishlistDTO
                         {
                             PerformanceId = perfId,
                             MainImage = perfWp.MainImageUrl,
                             Title = perfWp.Title
                         };

            return result;

        }

        public async Task SaveOrDeletePerformance(string AccountId, int performanceId)
        {
            Entities.Models.Wishlist performance = await
                 WishlistRepository.GetPerformanceByPhoneIdAndPerformanceId(
                    AccountId, performanceId);

            if (performance == null)
            {
                performance = new Entities.Models.Wishlist()
                {
                    AccountId = int.Parse(AccountId),
                    PerformanceId = performanceId
                };
                WishlistRepository.Add(performance);
            }
            else
            {
                WishlistRepository.Remove(performance);
            }

            theaterScheduleUnitOfWork.Save();
        }
    }
}