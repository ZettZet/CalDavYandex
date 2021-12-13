using CalDavYandexLibrary.CalDavLib.Objects;
using System;
using System.Linq;

namespace Examples
{
    public class ExampleGetCalendars
    {
        public static void Main()
        {
            var login = "your login";
            var pass = "your password";
            var uri = new Uri("url to yandex caldav server");

            var client = new Client(uri, login, pass);

            var allCalendars = client.GetCalendarsAsync();
            //allCalendars  -  Collection of all calendars of this user
        }
       
    }
}

