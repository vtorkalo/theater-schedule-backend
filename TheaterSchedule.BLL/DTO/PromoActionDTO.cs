﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TheaterSchedule.BLL.DTO
{
    public class PromoActionDTO
    {
        public string PromoActionName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}