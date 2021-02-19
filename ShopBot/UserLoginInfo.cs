using System;
using System.Collections.Generic;

#nullable disable

namespace ShopBot
{
    public partial class UserLoginInfo
    {
        public string UserEmail { get; set; }
        public string Verify { get; set; }
        public string Salt { get; set; }
        public bool? EmailVerified { get; set; }
    }
}
