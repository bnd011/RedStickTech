using System;
using System.Collections.Generic;

#nullable disable

namespace ShopBot
{
    public partial class FailedLogin
    {
        public string UserEmail { get; set; }
        public int FailedNum { get; set; }
        public DateTime TimeOfTry { get; set; }
    }
}
