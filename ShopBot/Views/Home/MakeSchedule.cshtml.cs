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

            
           
            public string UserName{ get; set; }

            private string GetMakeScheduleQuery()
            {
                return "insert into RST_DB.schedules (`user_email`,`url`,`is_recurring`,`item`) values ('"+UserName+"','"+URL+"','0','"+ItemName+"');";
            }
            
        }
        [BindProperty]
        public InputModel Input { get; set; }

        public MakeScheduleModel()
        {
            Input = new InputModel();
             
        }

        

    }
}
