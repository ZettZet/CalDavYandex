using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalDavYandexLibrary.CalDavLib.Interfaces
{
    public interface ISerializer
    {
        /// <summary>
        /// Serialize Event to string
        /// </summary>
        /// <returns>Event as string</returns>
        string SerializeEventToString(IEvent targetEvent);
    }
}
