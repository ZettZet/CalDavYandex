using CalDavYandexLibrary.CalDav.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples
{
    public class ExampleCreateEvent
    {

        public static void Main()
        {
            var login = "your login";
            var pass = "your password";
            var uri = new Uri("url to yandex caldav server");

            var client = new Client(uri, login, pass);

            var allCalendars = client.GetCalendarsAsync().Result;
            //allCalendars  -  Collection of all calendars of this user

            var currentCalendar = allCalendars.First();

            currentCalendar.CreateEvent("Test", DateTime.Now, DateTime.Now.AddHours(1), "location");
            //Create event

            var results = client.SaveChangesAsync(currentCalendar).Result;
            //Save all changes and take results of conservation
        }

    }
}

