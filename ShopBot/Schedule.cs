using System;
using System.Collections.Generic;

#nullable disable

namespace ShopBot
{
    public partial class Schedule
    {
        public int ScheduleIdn { get; set; }
        public string UserEmail { get; set; }
        public ulong? Archived { get; set; }
        public string Url { get; set; }
        public ulong? IsRecurring { get; set; }
        public int? Frequency { get; set; }
        public int? WantOption { get; set; }
        public int? PriceLimit { get; set; }
        public DateTime? ExpireDate { get; set; }
        public float CurrentPrice { get; set; }
        public string Item { get; set; }
    }
}
