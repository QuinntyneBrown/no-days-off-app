using System;


namespace NoDaysOffApp.Features.Core
{
    public static class DateExtensions
    {

        public static string ToWebUtcString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
        }
    }
}
