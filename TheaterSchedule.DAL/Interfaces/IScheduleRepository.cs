﻿using System;
using System.Collections.Generic;
using Entities.Models;
using TheaterSchedule.DAL.Models;

namespace TheaterSchedule.DAL.Interfaces
{
    public interface IScheduleRepository 
    {
        IEnumerable<ScheduleDataModel> GetListPerformancesByDateRange(
            string phoneId, string languageCode, 
            DateTime? startDate, DateTime? endDate);
    }
}
