﻿using System.Collections.Generic;

namespace TheaterSchedule.BLL.DTO
{
    public class PerformanceDetailsWpDTO: PerformanceDetailsBaseDTO
    {
        public string MainImage { get; set; }
        public IEnumerable<string> GalleryImage { get; set; }
    }
}


