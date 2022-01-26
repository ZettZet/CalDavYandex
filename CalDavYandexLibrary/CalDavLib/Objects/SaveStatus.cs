using CalDavYandexLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalDavYandexLibrary.CalDav.Objects
{
    public class SaveStatus
    {
        /// <summary>
        /// Get message about request.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Get StatusCode of response.
        /// </summary>
        public int StatusCode { get; }

        /// <summary>
        /// Get object of event which try to save, update or delete.
        /// </summary>
        public IEvent TargetEvent { get; }

        public SaveStatus(string message, int statusCode, IEvent targetEvent)
        {
            Message = message;
            StatusCode = statusCode;
            TargetEvent = targetEvent;
        }
    }
}
