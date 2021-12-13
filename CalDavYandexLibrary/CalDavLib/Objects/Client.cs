using CalDavYandexLibrary.CalDavLib.Interfaces;
using CalDavYandexLibrary.CalDavLib.Models;
using CalDavYandexLibrary.CalDavLib.Objects.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CalDavYandexLibrary.CalDavLib.Objects
{
    public class Client : IClient
    {
        private HttpClient httpClient = new();
        private Serializer serializer = new();
        private Deserializer deserializer = new();

        public Client(Uri targetUri, string login, string password)
        {
            Uri = targetUri;
            Username = login;
            Password = password;

            SetAuthorization(Username, Password);
        }

        public string Username { get; }

        public string Password { get; }

        public Uri Uri { get; }

        public bool TryConnecting()
        {
            var request = new HttpRequestMessage();
            request.Method = HttpMethod.Get;
            request.RequestUri = Uri;

            var responce = httpClient.Send(request);

            return 200 <= (int)responce?.StatusCode && (int)responce?.StatusCode <= 299;
        }

        public async Task<IEnumerable<ICalendar>> GetCalendarsAsync()
        {
            var request = new HttpRequestMessage();
            request.Method = new HttpMethod("PROPFIND");
            request.RequestUri = Uri;

            var result = await httpClient.SendAsync(request);

            if (200 > (int)result?.StatusCode || (int)result?.StatusCode > 299)
            {
                return null;
            }

            try
            {
                var data = await result.Content.ReadAsByteArrayAsync();

                var encoding = result.Content.Headers.ContentType?.CharSet is null ?
                    Encoding.UTF8 :
                    Encoding.GetEncoding(result.Content.Headers.ContentType?.CharSet);

                var dataAsString = encoding.GetString(data);

                var document = XDocument.Parse(dataAsString);

                var resources = document.Root
                    .Elements()
                    .Where(x => x.Name.LocalName.Equals("response", StringComparison.OrdinalIgnoreCase))
                    .Select(ParseResource)
                    .ToList();


                var calendars = new List<ICalendar>();

                foreach (var resource in resources.Where(x => x.Contains("events-") && !x.Contains(".ics")))
                {
                    var calendar = await GetCalendarWithUriAsync(resource);
                    if (calendar == null)
                    {
                        continue;
                    }

                    calendars.Add(calendar);
                }

                return calendars;
            }
            catch
            {
                return null;
            }
        }

        public async Task<ICalendar> GetCalendarWithUriAsync(string uri)
        {
            var request = new HttpRequestMessage();
            request.Method = new HttpMethod("PROPFIND");
            request.RequestUri = new Uri(new Uri("https://caldav.yandex.ru/"), uri);

            var result = await httpClient.SendAsync(request);

            if (200 > (int)result?.StatusCode || (int)result?.StatusCode > 299)
            {
                return null;
            }

            var data = await result.Content.ReadAsStringAsync();

            Calendar calendar = deserializer.DeserializeCalendar(data) as Calendar;

            return calendar;
        }

        public async Task<IList<SaveStatus>> SaveChangesAsync(ICalendar calendar)
        {
            var results = new List<SaveStatus>();

            if (calendar.LocalChanges)
            {
                foreach (var item in calendar.Events)
                {
                    switch (item.Status)
                    {
                        case StatusOfEvent.Created:
                        case StatusOfEvent.Updated:
                            results.Add(await SaveEventToServerAsync(item, GetEventUri(calendar, item)));
                            item.Status = StatusOfEvent.Complete;
                            break;
                        case StatusOfEvent.Deleted:
                            results.Add(await DeleteEventFromServerAsync(item, GetEventUri(calendar, item)));
                            item.Status = StatusOfEvent.Complete;
                            break;

                    }
                }

                calendar.LocalChanges = false;
            }

            return results;
        }

        #region additional methods
        void SetAuthorization(string username, string password)
        {
            var value = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", value);
        }
        string ParseResource(XElement element)
        {
            var uri = element
                .Elements()
                .FirstOrDefault(x => x.Name.LocalName.Equals("href", StringComparison.OrdinalIgnoreCase))?.Value;

            return uri;
        }

        async Task<SaveStatus> SaveEventToServerAsync(IEvent targetEvent, string uri)
        {
            try
            {
                var eventAsString = serializer.SerializeEventToString(targetEvent);

                var request = new HttpRequestMessage();
                request.RequestUri = new Uri(Uri, uri);
                request.Method = HttpMethod.Put;
                request.Content = new StringContent(eventAsString, Encoding.UTF8);

                var result = await httpClient.SendAsync(request);

                return new SaveStatus(result.StatusCode.ToString(), (int)result.StatusCode, targetEvent);
            }
            catch (Exception ex)
            {
                return new SaveStatus(ex.Message, 500, targetEvent);
            }
        }

        async Task<SaveStatus> DeleteEventFromServerAsync(IEvent targetEvent, string uri)
        {
            try
            {
                var request = new HttpRequestMessage();
                request.RequestUri = new Uri(Uri, uri);
                request.Method = HttpMethod.Delete;

                var result = await httpClient.SendAsync(request);

                return new SaveStatus(result.StatusCode.ToString(), (int)result.StatusCode, targetEvent);
            }
            catch (Exception ex)
            {
                return new SaveStatus(ex.Message, 500, targetEvent);
            }
        }

        string GetEventUri(ICalendar calendar, IEvent targetEvent)
        {
            return $"{calendar.Uid}{targetEvent.Uid}.ics";
        }
        #endregion
    }
}
