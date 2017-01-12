using System;

namespace WeatherStats.Shared
{
    public static class SharedMethods
    {
        public static DateTime ToTenMinutePrecision(this DateTime source)
        {
            return new DateTime(source.Year, source.Month, source.Day, source.Hour, (source.Minute / 10) * 10, 0, 0, source.Kind);
        }
        public static string Timestamp2String(this DateTime timestamp, bool includeMs = true, bool fileNameFriendly = false)
        {
            if (fileNameFriendly == false)
            {
                if (includeMs)
                {
                    return $"{timestamp.Year:0000}-{timestamp.Month:00}-{timestamp.Day:00} {timestamp.Hour:00}:{timestamp.Minute:00}:{timestamp.Second:00}.{timestamp.Millisecond:000}";
                }

                return $"{timestamp.Year:0000}-{timestamp.Month:00}-{timestamp.Day:00} {timestamp.Hour:00}:{timestamp.Minute:00}:{timestamp.Second:00}";
            }

            if (includeMs)
            {
                return $"{timestamp.Year:0000}-{timestamp.Month:00}-{timestamp.Day:00} {timestamp.Hour:00}-{timestamp.Minute:00}-{timestamp.Second:00}.{timestamp.Millisecond:000}";
            }

            return $"{timestamp.Year:0000}-{timestamp.Month:00}-{timestamp.Day:00} {timestamp.Hour:00}-{timestamp.Minute:00}-{timestamp.Second:00}";
        }
    }
}
