using CalDavYandexLibrary.CalDavLib.Interfaces;
using CalDavYandexLibrary.CalDavLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CalDavYandexLibrary.CalDavLib.Objects.Serialization
{
    internal class Deserializer : IDeserializer
    {
        public ICalendar DeserializeCalendar(string source)
        {
            try
            {
                ICalendar calendar = new Calendar();

                var document = XDocument.Parse(source);

                var elements = document.Root
                    .Elements()
                    .First()
                    .Elements()
                    .ToArray();

                calendar.Uid = elements
                    .Where(x => x.ToString().Contains("<href", StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault()?.Value ?? "";
                var otherElements = elements[1]
                    .Elements()
                    .First()
                    .Elements()
                    .ToArray();


                calendar.DisplayName = otherElements
                    .Where(x => x.ToString().Contains("<D:displayname", StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault()?.Value ?? "";
                calendar.Color = otherElements
                    .Where(x => x.ToString().Contains("<calendar-color", StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault()?.Value ?? "";
                calendar.Owner = otherElements
                    .Where(x => x.ToString().Contains("<D:owner", StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault()?.Value ?? "";
                calendar.SyncToken = otherElements
                    .Where(x => x.ToString().Contains("<D:sync-token", StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault()?.Value ?? "";
                calendar.Ctag = otherElements
                    .Where(x => x.ToString().Contains("<getctag", StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault()?.Value ?? "";

                if (document.Root.Elements().Count() > 1)
                {
                    var data = document.Root.Elements().Skip(1);

                    foreach (var resp in data)
                    {
                        var eventAsString = resp.Elements()
                            .ToArray()[1]
                            .Elements()
                            .First()
                            .Elements()
                            .ToArray()[3]
                            .ToString()
                            .Replace("<C:calendar-data xmlns:C=\"urn:ietf:params:xml:ns:caldav\">", "")
                            .Replace("</C:calendar-data>", "");

                        calendar.Events.Add(DeserializeEvent(eventAsString));
                    }

                }

                return calendar;
            }
            catch
            {
                return null;
            }
        }

        public IEvent DeserializeEvent(string source)
        {
            var items = source
                .Split("\r\n", StringSplitOptions.RemoveEmptyEntries)
                .SkipWhile(x => !x.Equals("BEGIN:VEVENT"))
                .ToArray();

            IEvent targetEvent = new Event();

            targetEvent.Start = GetDateTime(items
                .Where(x => x.Contains("DTSTART:"))
                .FirstOrDefault()
                ?.Replace("DTSTART:", ""));
            targetEvent.End = GetDateTime(items
                .Where(x => x.Contains("DTEND:"))
                .FirstOrDefault()
                ?.Replace("DTEND:", ""));
            targetEvent.Summary = items
                .Where(x => x.Contains("SUMMARY:"))
                .FirstOrDefault()
                ?.Replace("SUMMARY:", "") ?? "";
            targetEvent.Uid = items
                .Where(x => x.Contains("UID:"))
                .FirstOrDefault()
                ?.Replace("UID:", "") ?? "";
            targetEvent.Created = GetDateTime(items
                .Where(x => x.Contains("CREATED:"))
                .FirstOrDefault()
                ?.Replace("CREATED:", ""));
            targetEvent.LastModified = GetDateTime(items
                .Where(x => x.Contains("LAST-MODIFIED:"))
                .FirstOrDefault()
                ?.Replace("LAST-MODIFIED:", ""));
            targetEvent.Location = items
                .Where(x => x.Contains("LOCATION:"))
                .FirstOrDefault()
                ?.Replace("LOCATION:", "");

            return targetEvent;
        }

        DateTime GetDateTime(string time)
        {
            if (string.IsNullOrEmpty(time))
            {
                return DateTime.Now;
            }

            int year = int.Parse(time.Substring(0, 4));
            int month = int.Parse(time.Substring(4, 2));
            int day = int.Parse(time.Substring(6, 2));

            int hour = int.Parse(time.Substring(9, 2));
            int minutes = int.Parse(time.Substring(11, 2));
            int seconds = int.Parse(time.Substring(13, 2));

            return new DateTime(year, month, day, hour + 3, minutes, seconds);
        }
    }
}
