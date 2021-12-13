using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalDavYandexLibrary.CalDavLib.Interfaces
{
    internal interface ICalendar
    {
        /// <summary>
        /// Get the uid of the calendar.
        /// </summary>
        string Uid { get; set; }

        /// <summary>
        /// Get the display name of the calendar.
        /// </summary>
        string DisplayName { get; set; }

        /// <summary>
        /// Get the description of the calendar.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Get the owner of the calendar.
        /// </summary>
        string Owner { get; set; }

        /// <summary>
        /// Get date of last modification of the calendar.
        /// </summary>
        DateTime LastModified { get; set; }

        /// <summary>
        /// Get the product id of the calendar.
        /// </summary>
        string ProductId { get; set; }

        /// <summary>
        /// Get SyncToken of the calendar.
        /// </summary>
        string SyncToken { get; set; }

        /// <summary>
        /// Get the owner of the calendar.
        /// </summary>
        string Ctag { get; set; }

        /// <summary>
        /// Get the color of the calendar.
        /// </summary>
        string Color { get; set; }

        /// <summary>
        /// Get all events in the calendar.
        /// </summary>
        ///
        /// The events may not represent the state on the server since local changes may not be synchronized.
        /// With <see cref="LocalChanges" /> a calendar can be checked whether it contains any changes.
        ///
        /// To synchronize the calendar use <see cref="Client.SaveChangesAsync" />.
        IList<IEvent> Events { get; set; }

        /// <summary>
        /// Check if the calendar has local changes which are not synchronized yet.
        /// </summary>
        ///
        /// To synchronize local changes with the server use <see cref="Client.SaveChangesAsync" />.
        bool LocalChanges { get; set; }

        /// <summary>
        /// Create a new event in the calendar.
        /// </summary>
        ///
        /// If no end is set the event will default to one hour duration.
        ///
        /// Attention: The event will not be synchronized until <see cref="Client.SaveChangesAsync" /> is called.
        ///
        /// <param name="summary">Summary text of the event</param>
        /// <param name="start">Start date and time of the event</param>
        /// <param name="end">End date and time of the event</param>
        /// <param name="location">Location of the event</param>
        /// <returns>New created event</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="summary" /> is null</exception>
        void CreateEvent(string summary, DateTime start, DateTime end = default(DateTime), string location = null);

        /// <summary>
        /// Update a new event in the calendar.
        /// </summary>
        ///
        /// If no end is set the event will default to one hour duration.
        ///
        /// Attention: The event will not be synchronized until <see cref="Client.SaveChangesAsync" /> is called.
        ///
        /// <param name="summary">Summary text of the event</param>
        /// <param name="start">Start date and time of the event</param>
        /// <param name="end">End date and time of the event</param>
        /// <param name="location">Location of the event</param>
        /// <returns>New created event</returns>
        public void UpdateEvent(IEvent calendarEvent, string summary = null, DateTime start = default, DateTime end = default, string location = null);

        /// <summary>
        /// Delete a given event from the calendar.
        /// </summary>
        ///
        /// Attention: The event will not be synchronized (deleted on the server) until <see cref="SaveChangesAsync" /> is called.
        ///
        /// <param name="calendarEvent">Event to delete from the calendar</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="calendarEvent"/> is null</exception>
        void DeleteEvent(IEvent calendarEvent);
    }
}
