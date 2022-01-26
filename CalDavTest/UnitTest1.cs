using CalDavYandexLibrary.CalDav.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace CalDavTest
{
    [TestClass]
    public class UnitTest1
    {
        static string login = "devehher@yandex.ru"; //your login for calendar
        static string pass = "jptyvznqpamfpetn"; //your password for calendar
        static Uri uri = new Uri("https://caldav.yandex.ru/calendars/devehher@yandex.ru/");

        static Client cli = new Client(uri, login, pass);


        [TestMethod]
        public void GetCalendarTest()
        {
            var cal = cli.GetCalendarsAsync().Result.First();

            Assert.IsNotNull(cal);

            Assert.IsNotNull(cal.Uid);
            Assert.IsFalse(string.IsNullOrEmpty(cal.Uid));

            Assert.IsNotNull(cal.DisplayName);
            Assert.IsFalse(string.IsNullOrEmpty(cal.DisplayName));

            Assert.IsNotNull(cal.SyncToken);
            Assert.IsFalse(string.IsNullOrEmpty(cal.SyncToken));

            Assert.IsNotNull(cal.Ctag);
            Assert.IsFalse(string.IsNullOrEmpty(cal.Ctag));
        }

        [TestMethod]
        public void CreateEventTest()
        {
            var cal = cli.GetCalendarsAsync().Result.First();

            Assert.IsTrue(cal.Events.Count == 0); //calendar must be empty

            var start = DateTime.Now;

            cal.CreateEvent("Test 1", start);
            var result = cli.SaveChangesAsync(cal).Result;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() == 1);
            Assert.IsTrue(result.All(x => x.StatusCode == 201));


            cal.CreateEvent("Пример 2", start, start);
            result = cli.SaveChangesAsync(cal).Result;
            Assert.IsTrue(cal.Events.Count == 2);
            Assert.IsTrue(result.All(x => x.StatusCode == 201));

            cal.CreateEvent("Test 2", start, start.AddHours(1));
            result = cli.SaveChangesAsync(cal).Result;
            Assert.IsTrue(cal.Events.Count == 3);
            Assert.IsTrue(result.All(x => x.StatusCode == 201));

            cal.CreateEvent("Пример 2", start, start.AddHours(2), "first location");
            result = cli.SaveChangesAsync(cal).Result;
            Assert.IsTrue(cal.Events.Count == 4);
            Assert.IsTrue(result.All(x => x.StatusCode == 201));

            cal.CreateEvent("Test 3", start, start.AddHours(3), "locations");
            result = cli.SaveChangesAsync(cal).Result;
            Assert.IsTrue(cal.Events.Count == 5);
            Assert.IsTrue(result.All(x => x.StatusCode == 201));

            cal.CreateEvent("Пример 3", start, start.AddHours(3), "место");
            result = cli.SaveChangesAsync(cal).Result;
            Assert.IsTrue(cal.Events.Count == 6);
            Assert.IsTrue(result.All(x => x.StatusCode == 201));


            foreach (var x in cal.Events)
            {
                cal.DeleteEvent(x);
            }
            cli.SaveChangesAsync(cal).Wait();
        }

        [TestMethod]
        public void ReadEventTest()
        {
            var cal = cli.GetCalendarsAsync().Result.First();

            Assert.IsTrue(cal.Events.Count == 0); //calendar must be empty

            var start = DateTime.Now;

            cal.CreateEvent("Test 1", start);
            cal.CreateEvent("Пример 1", start, start);
            cal.CreateEvent("Test 2", start, start.AddHours(1));
            cal.CreateEvent("Пример 2", start, start.AddHours(2), "first location");
            cal.CreateEvent("Test 3", start, start.AddHours(3), "locations");
            cal.CreateEvent("Пример 3", start, start.AddHours(3), "место");
            cli.SaveChangesAsync(cal).Wait();


            var newCal = cli.GetCalendarsAsync().Result.First();

            Assert.IsTrue(newCal.Events.Count == 6);
            Assert.IsTrue(newCal.Events.Where(x => x.Summary == "Test 1").Count() == 1);
            Assert.IsTrue(newCal.Events.Where(x => x.Summary == "Пример 1").Count() == 1);
            Assert.IsTrue(newCal.Events.Where(x => x.Summary == "Test 2").Count() == 1);
            Assert.IsTrue(newCal.Events.Where(x => x.Summary == "Пример 2").Count() == 1);
            Assert.IsTrue(newCal.Events.Where(x => x.Summary == "Test 3").Count() == 1);
            Assert.IsTrue(newCal.Events.Where(x => x.Summary == "Пример 3").Count() == 1);


            Assert.IsTrue(newCal.Events.Where(x => x.Summary == "Пример 2").First().Location == "first location");
            Assert.IsTrue(newCal.Events.Where(x => x.Summary == "Test 3").First().Location == "locations");
            Assert.IsTrue(newCal.Events.Where(x => x.Summary == "Пример 3").First().Location == "место");


            foreach (var x in cal.Events)
            {
                cal.DeleteEvent(x);
            }
            cli.SaveChangesAsync(cal).Wait();
        }

        [TestMethod]
        public void UpdateEventTest()
        {
            var cal = cli.GetCalendarsAsync().Result.First();

            Assert.IsTrue(cal.Events.Count == 0); //calendar must be empty

            var start = DateTime.Now;

            cal.CreateEvent("Test 1", start);
            cal.CreateEvent("Пример 1", start, start);
            cal.CreateEvent("Test 2", start, start.AddHours(1));
            cal.CreateEvent("Пример 2", start, start.AddHours(2), "first location");
            cal.CreateEvent("Test 3", start, start.AddHours(3), "locations");
            cal.CreateEvent("Пример 3", start, start.AddHours(3), "место");
            cli.SaveChangesAsync(cal).Wait();


            var newCal = cli.GetCalendarsAsync().Result.First();

            Assert.IsTrue(newCal.Events.Count == 6);

            newCal.UpdateEvent(newCal.Events.Where(x => x.Summary == "Test 1").First(), "REQ 1");
            newCal.UpdateEvent(newCal.Events.Where(x => x.Summary == "Пример 1").First(), "REQ 2", start.AddHours(1));
            newCal.UpdateEvent(newCal.Events.Where(x => x.Summary == "Test 2").First(), "REQ 3", location: "new location");
            newCal.UpdateEvent(newCal.Events.Where(x => x.Summary == "Пример 2").First(), "REQ 4", start.AddHours(1), location: "new locations");
            cli.SaveChangesAsync(cal).Wait();

            Assert.IsTrue(newCal.Events.Where(x => x.Summary == "REQ 1").Count() == 1);
            Assert.IsTrue(newCal.Events.Where(x => x.Summary == "REQ 2").Count() == 1);
            Assert.IsTrue(newCal.Events.Where(x => x.Summary == "REQ 3").Count() == 1);
            Assert.IsTrue(newCal.Events.Where(x => x.Summary == "REQ 4").Count() == 1);

            Assert.IsTrue(newCal.Events.Where(x => x.Summary == "REQ 2").First().Start == start.AddHours(1));
            Assert.IsTrue(newCal.Events.Where(x => x.Summary == "REQ 3").First().Location == "new location");
            Assert.IsTrue(newCal.Events.Where(x => x.Summary == "REQ 4").First().Start == start.AddHours(1));
            Assert.IsTrue(newCal.Events.Where(x => x.Summary == "REQ 4").First().Location == "new locations");


            foreach (var x in cal.Events)
            {
                cal.DeleteEvent(x);
            }
            cli.SaveChangesAsync(cal).Wait();

        }

        [TestMethod]
        public void RemoveEventsTest()
        {
            var cal = cli.GetCalendarsAsync().Result.First();

            Assert.IsTrue(cal.Events.Count == 0); //calendar must be empty

            var start = DateTime.Now;

            cal.CreateEvent("Test 1", start);
            cal.CreateEvent("Пример 1", start, start);
            cal.CreateEvent("Test 2", start, start.AddHours(1));
            cal.CreateEvent("Пример 2", start, start.AddHours(2), "first location");
            cal.CreateEvent("Test 3", start, start.AddHours(3), "locations");
            cal.CreateEvent("Пример 3", start, start.AddHours(3), "место");
            cli.SaveChangesAsync(cal).Wait();

            Assert.IsTrue(cal.Events.Count == 6);

            foreach (var x in cal.Events)
            {
                cal.DeleteEvent(x);
            }

            cli.SaveChangesAsync(cal).Wait();


            var newCal = cli.GetCalendarsAsync().Result.First();

            Assert.IsTrue(cal.Events.Count == 0);
        }
    }
}
