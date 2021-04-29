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

        public class OutputModel
        {
            private string getList { get; set; }
            [Required]
			public string GetList 
            {
                get
                {
                    Console.WriteLine("Getlist.get() stub");
                    return getList;
                }
                set
                {
                    Console.WriteLine("Getlist.set() stub");
                    getList = value;
                }
            }

            [Required]
            [DataType(DataType.Text)]
            public string URL { get; set; }

            public string UserName => "Default@email.com";

        }

        public OutputModel Output { get; set; }
         
        public string GetHtml()
        {
            return "<div> default output </div>";
        }
        public void OnGet()
        {
        }
    }
}
