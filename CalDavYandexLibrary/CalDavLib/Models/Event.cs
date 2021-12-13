using CalDavYandexLibrary.CalDavLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalDavYandexLibrary.CalDavLib.Models
{
    internal class Event : IEvent
    {
        public Event()
        {
            Created = DateTime.Now;
            Status = StatusOfEvent.Created;
        }

        public string Uid { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastModified { get; set; }

        public string Location { get; set; }
        public string Summary { get; set; }
        public StatusOfEvent Status { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is Event))
            {
                return false;
            }
            if ((obj as Event) is null)
            {
                return false;
            }

            var ev = obj as Event;

            return this.Summary.Equals(ev.Summary, StringComparison.OrdinalIgnoreCase) &&
                this.Uid.Equals(ev.Uid) &&
                this.Start.Equals(ev.Start) &&
                this.End.Equals(ev.End);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Uid, Summary, Start, End, Location, Created);
        }
    }
}
