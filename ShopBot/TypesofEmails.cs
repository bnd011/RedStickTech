//This file is a temporary file to store the code for emails until their proper avenues have been set up

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentEmail.Core;

namespace ShopBot
{
    public class TypesofEmails
    {
        public async void ScheduleCreated()
        {
            var Item = "Placeholder";
            var Quantity = "0";
            var email = await Email
                        .From("redsticktechshopbot@gmail.com")
                        .To("email@email")
                        .Subject("A new schedule has been registered with your account")
                        .Body("A purchase schedule has been created to buy " + Quantity + " " + Item + ". This purchase will occure during the time frame you placed on the website")
                        .SendAsync();
        }

        public async void PurchaseMade()
        {
            var Item = "Placeholder";
            var Quantity = "0";
            var email = await Email
                        .From("redsticktechshopbot@gmail.com")
                        .To("email@email")
                        .Subject("A new purchase has been made on your account")
                        .Body("A purchase schedule buying " + Quantity + " " + Item + " has been made. Please check the relevant website accounts to verify your item has been bought on there. Thank you for shopping with Shopbot!")
                        .SendAsync();
        }

        public async void UpcomingPurchase()
        {
            var Item = "Placeholder";
            var Quantity = "0";
            var Days = "2";
            var email = await Email
                        .From("redsticktechshopbot@gmail.com")
                        .To("email@email")
                        .Subject("An automated purchase will occure soon")
                        .Body("A purchase schedule buying " + Quantity + " " + Item + " will occure in " + Days + ". Please ensure your accounts are remembered on the website the purchase will be made on and the payment information is correct so there are no problems with your order")
                        .SendAsync();
        }
    }
}
