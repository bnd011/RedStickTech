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

            [Required]
            [DataType(DataType.Text)]
            public string URL { get; set; }

            
            private string user_name { get; set; }
            private string User_name
            {
                get
                {
                    return "dummy@email.com";
                }
                set
                {
                    user_name = "dummy@email.com";
                }
            }

            private string GetScheduleQuery()
            {
                return "insert into RST_DB.schedules (`user_email`,`url`,`is_recurring`,`item`) values ('"+User_name+"','"+URL+"','0','"+ItemName+"');";
            }
            
        }

        public InputModel Input { get; set; }

        public void OnGet()
        {
            Input = new InputModel();
        }
        public void MakeASchedule()
        {
            Console.WriteLine("Testing");
            Console.WriteLine("Must've Worked");
        }

    }
}
