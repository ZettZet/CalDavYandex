using CalDavYandexLibrary.CalDavLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalDavYandexLibrary.CalDavLib.Models
{
    internal class Calendar : ICalendar
    {
        public string Uid { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public string Owner { get; set; }

        public DateTime LastModified { get; set; }

        public string ProductId { get; set; }
        public string SyncToken { get; set; }

        public string Color { get; set; }

        public IList<IEvent> Events { get; set; }

        public bool LocalChanges { get; set; }
        public string Ctag { get; set; }
        public Calendar()
        {
            LastModified = DateTime.Now;
            Events = new List<IEvent>();
        }

        public void CreateEvent(string summary, DateTime start, DateTime end = default, string location = null)
        {
            var targetEvent = new Event();

            targetEvent.Uid = Guid.NewGuid().ToString();
            targetEvent.Summary = summary;
            targetEvent.Start = start;
            if (!end.Equals(default))
            {
                targetEvent.End = end;
            }
            else
            {
                targetEvent.End = start;
            }
            targetEvent.Location = location;
            targetEvent.LastModified = DateTime.Now;

            Events.Add(targetEvent);
            LocalChanges = true;

        }

        public void UpdateEvent(IEvent calendarEvent, string summary = null, DateTime start = default, DateTime end = default, string location = null)
        {
            if (Events.Where(x => x.Equals(calendarEvent)).Count() != 0)
            {
                var items = Events.Where(x => x.Equals(calendarEvent));

                foreach (var item in items)
                {
                    if (!string.IsNullOrEmpty(summary))
                    {
                        item.Summary = summary;
                    }
                    if (!start.Equals(default))
                    {
                        item.Start = start;
                    }
                    if (!end.Equals(default))
                    {
                        item.End = end;
                    }
                    if (!string.IsNullOrEmpty(location))
                    {
                        item.Location = location;
                    }

                    item.Status = StatusOfEvent.Updated;
                }

                LocalChanges = true;
            }
        }

        public void DeleteEvent(IEvent calendarEvent)
        {
            if (Events.Where(x => x.Equals(calendarEvent)).Count() != 0)
            {
                var items = Events.Where(x => x.Equals(calendarEvent));

                foreach (var item in items)
                {
                    item.Status = StatusOfEvent.Deleted;
                }

                LocalChanges = true;
            }
        }
    }
}
