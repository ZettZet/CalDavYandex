using CalDavYandexLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalDavYandexLibrary.CalDav.Interfaces;

namespace CalDavYandexLibrary.CalDav.Objects.Serialization
{
    public class Serializer : ISerializer
    {
        public string SerializeEventToString(IEvent targetEvent)
        {
            var result = new StringBuilder("BEGIN:VCALENDAR\r\nBEGIN:VEVENT\r\n");

            result.Append($"DTEND:{GetTimeInString(targetEvent.End.ToUniversalTime())}\r\n");
            result.Append($"DTSTART:{GetTimeInString(targetEvent.Start.ToUniversalTime())}\r\n");
            result.Append($"CREATED:{GetTimeInString(targetEvent.Created.ToUniversalTime())}\r\n");
            result.Append($"LAST-MODIFIED:{GetTimeInString(targetEvent.LastModified.ToUniversalTime())}\r\n");
            result.Append($"SEQUENCE:0\r\n");
            result.Append($"SUMMARY:{targetEvent.Summary}\r\n");
            result.Append($"UID:{targetEvent.Uid}\r\n");
            result.Append($"LOCATION:{targetEvent.Location}\r\n");
            result.Append($"END:VEVENT\r\nEND:VCALENDAR\r\n");

            return result.ToString();
        }

        string GetTimeInString(DateTime dt)
        {
            string month = dt.Month.ToString();
            if(dt.Month < 10)
            {
                month = "0" + month;
            }

            string day = dt.Day.ToString();
            if (dt.Day < 10)
            {
                day = "0" + day;
            }

            string hour = dt.Hour.ToString();
            if (dt.Hour < 10)
            {
                hour = "0" + hour;
            }

            string minute = dt.Minute.ToString();
            if (dt.Minute < 10)
            {
                minute = "0" + minute;
            }

            string second = dt.Second.ToString();
            if (dt.Second < 10)
            {
                second = "0" + second;
            }

            return $"{dt.Year}{month}{day}T{hour}{minute}{second}Z";
        }
    }
}
