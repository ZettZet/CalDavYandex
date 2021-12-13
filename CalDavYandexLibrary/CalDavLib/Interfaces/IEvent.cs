using CalDavYandexLibrary.CalDavLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalDavYandexLibrary.CalDavLib.Interfaces
{
    public interface IEvent
    {
        /// <summary>
        /// Get the uid of the event.
        /// </summary>
        string Uid { get; set; }

        /// <summary>
        /// Get or set the start date of the event.
        /// </summary>
        DateTime Start { get; set; }

        /// <summary>
        /// Get or set the end date of the event.
        /// </summary>
        DateTime End { get; set; }

        /// <summary>
        /// Get or set the duration of the event.
        /// </summary>
        DateTime Created { get; set; }

        /// <summary>
        /// Get the last modification date of the event.
        /// </summary>
        DateTime LastModified { get; set; }

        /// <summary>
        /// Get or set the location of the event.
        /// </summary>
        string Location { get; set; }

        /// <summary>
        /// Get or set the summary of the event.
        /// </summary>
        string Summary { get; set; }

        /// <summary>
        /// Get or set conservation status of the event.
        /// </summary>
        StatusOfEvent Status { get; set; }
    }
}
