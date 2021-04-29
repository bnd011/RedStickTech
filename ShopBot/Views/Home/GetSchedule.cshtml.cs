using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;



namespace ShopBot.Views.Home
{
    public class GetScheduleModel : PageModel
    {

        public class InputModel
        {
            [Required]
			public string GetList { get; set; }

            [Required]
            [DataType(DataType.Text)]
            public string URL { get; set; }

            public string UserName { get; set; }

        }

        public InputModel Input { get; set; }
         
        public void OnGet()
        {
        }
    }
}
