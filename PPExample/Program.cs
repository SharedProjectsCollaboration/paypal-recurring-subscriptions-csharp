using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PayPalRecurring;

namespace PPExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // pass in your business API credentials along with the purchasers information
            // this is the minimum info that is needed
            PPRecurring pp = new PPRecurring(true, "kevinr_1246254684_biz_api1.gmail.com", 
                "1246254691", "AFcWxV21C7fd0v3bYYYRCpSSRl31AI57VGs7wtKAIIlqiF-XIrCOKrxR",
                10.00, "Visa", "1001234543", 1, 2012, "Test", "test", "test@testerrrr.net");
            // you can add more details to the PPRecurring client by using its public accessors
            // for example, add a description to the account purchase
            pp.description = "Super Deluxe Package";
            // send the request to pp
            pp.sendSubscriptionRequest();
            Console.WriteLine(pp.lastResponse);
        }
    }
}
