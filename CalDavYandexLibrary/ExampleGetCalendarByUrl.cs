using CalDavYandexLibrary.CalDavLib.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples
{
    internal class ExampleGetCalendarByUrl
    {
        public static void Main()
        {
            var login = "your login";
            var pass = "your password";
            var uri = new Uri("url to yandex caldav server");

            var client = new Client(uri, login, pass);

            var calendar = client.GetCalendarWithUriAsync("url to calendar");
            //Calendar  -  getting calendar by url
        }
    }
}
