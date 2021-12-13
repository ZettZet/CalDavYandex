using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalDavYandexLibrary.CalDavLib.Interfaces
{
    public interface IDeserializer
    {
        /// <summary>
        /// Deserialize calendar from string
        /// </summary>
        /// <returns>Object of calendar</returns>
        ICalendar DeserializeCalendar(string source);

        /// <summary>
        /// Deserialize Event from string
        /// </summary>
        /// <returns>Object of Event</returns>
        IEvent DeserializeEvent(string source);
    }
}
