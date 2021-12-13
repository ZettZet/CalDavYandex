using CalDavYandexLibrary.CalDavLib.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalDavYandexLibrary.CalDavLib.Interfaces
{
    internal interface IClient
    {
        /// <summary>
        /// Get the username to authenticate with.
        /// </summary>
        string Username { get; }

        /// <summary>
        /// Get the password to authenticate with.
        /// </summary>
        string Password { get; }

        /// <summary>
        /// Get the uri of the server to connect to.
        /// </summary>
        Uri Uri { get; }

        /// <summary>
        /// Try connect to the server by Uri
        /// </summary>
        /// <returns>State of trying</returns>
        bool TryConnecting();

        /// <summary>
        /// Get all calendars available for the current user (or unauthenticated).
        /// </summary>
        /// <returns>Collection of available calendars</returns>
        Task<IEnumerable<ICalendar>> GetCalendarsAsync();

        /// <summary>
        /// Save all local changes to the server.
        /// </summary>
        /// <returns>True if all changes could be applied, otherwise false.</returns>
        Task<IList<SaveStatus>> SaveChangesAsync(ICalendar calendar);
    }
}
