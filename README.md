This is a simple library for working with Yandex Calendar using the CalDAV protocol

It allows you to create/Edit/Delete calendar events

The library has no additional dependencies

Files with examples of using the library:
ExampleGetCalendarByUrl.cs - getting a calendar by URL
ExampleGetCalendars.cs - getting all calendars
ExampleCreateEvent.cs - creating an event and saving it
ExampleUpdateEvent.cs - updating the event and saving changes
ExampleDeleteEvent.cs - deleting an event and saving changes

Before starting work, you need to get a key from the calendar server:

1) Open the Account Management page in Yandex ID. (https://passport.yandex.ru/profile/)
2) In the Passwords and Authorization section, select Enable application passwords. Confirm the action.
3) Click Create a new password.
4) Select the Calendar application type and come up with a password name.
5) Click Create. The application password will be displayed in a pop-up window

your server credentials:
login = your yandex login + @yandex.ru
password = password generated in the algorithm above