using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;



namespace ShopBot.Views.Home
{
    public class GetScheduleModel : PageModel
    {

        public class OutputModel
        {
            private string getList { get; set; }
            //[Required]
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

            private string userName { get; set; }
            [Required]
            public string UserName
            {
                get
                {
                    Console.WriteLine("Output.UserName.get() stub");
                    return userName;
                }
                set
                {
                    userName = value;
                    Console.WriteLine("Output.UserName.set() stub");
                }
            }

        }

        public OutputModel Output { get; set; }
         
        public string GetHtml()
        {
            List<string[]> schedules = getDBSchedule();
            return "<div> default output </div>";
        }

        private string GetRequestSchedulesQuery()
        {
            return "select * from RST_DB.schedules where `user_email` = '" + Output.UserName + "';";
        }

        private List<string[]> getDBSchedule()
        {
            List<String[]> schedules = new();
            string ConnectionStr = "Server= rst-db-do-user-8696039-0.b.db.ondigitalocean.com;Port = 25060;Database=RST_DB;Uid=doadmin;Pwd=wwd0oli7w2rplovh;SslMode=Required;";
            MySqlConnection connect = new MySqlConnection(ConnectionStr);
            MySqlCommand getSchedule = connect.CreateCommand();
            getSchedule.CommandText = GetRequestSchedulesQuery();
            connect.Open();
            try
            {
                bool jumper = true;
                MySqlDataReader connection = getSchedule.ExecuteReader();
                while (connection.HasRows && jumper)
                {
                    connection.Read();
                    if (connection.FieldCount != 5)
                    {
                        connect.Close();
                        Console.WriteLine("Wrong number of fields: ", connection.FieldCount);
                        jumper = false;
                    }
                    else
                    {
                        string[] results = { (string)connection.GetValue(0),
                                                (string)connection.GetValue(1),
                                                (string)connection.GetValue(2),
                                                (string)connection.GetValue(3),
                                                (string)connection.GetValue(4)};
                        Console.WriteLine(results[0], results[1], results[2], results[3], results[4]);
                        schedules.Add(results);
                    }
                }
                if(schedules.Count == 0)
                {
                    Console.WriteLine("No matching Schedules!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            connect.Close();
            return schedules;
        }
        public void OnGet()
        {
            Console.WriteLine("OnGet stub?");
        }
    }
}
