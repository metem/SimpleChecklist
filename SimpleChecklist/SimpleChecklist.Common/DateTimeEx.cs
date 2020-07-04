using System;

namespace SimpleChecklist.Common
{
    public static class DateTimeEx
    {
        public static DateTime RemoveMiliseconds(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute,
                 dateTime.Second, dateTime.Kind);
        }
    }
}