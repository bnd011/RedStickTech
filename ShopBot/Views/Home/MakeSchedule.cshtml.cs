using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShopBot.Views.Home
{
    public class MakeScheduleModel : PageModel
    {
        
        public class InputModel
        {
            [Required]
            public string ItemName { get; set; }
            
            [Required]
            [DataType(DataType.Date)]
            public string OrderDate { get; set; }

            [Required]
            
            public int Quantity { get; set; }

            
        }

        public InputModel Input { get; set; }

        public void OnGet()
        {
        }
    }
}
