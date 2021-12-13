using CalDavYandexLibrary.CalDavLib.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples
{
    internal class ExampleUpdateEvent
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

            var firstEvent = currentCalendar.Events.First();
            //Getting the desired event

            currentCalendar.UpdateEvent(firstEvent, summary: "new name");
            //Update event

            var results = client.SaveChangesAsync(currentCalendar).Result;
            //Save all changes and take results of conservation
        }
    }
}
