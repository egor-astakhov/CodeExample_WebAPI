using System;

namespace TitanApi.Infrastructure
{
    public class DateTimeProvider : IDateTimeProvider
    {
        //It will be set on first access to provider (not on server start)
        private readonly DateTime _since = DateTime.Now;

        public DateTime Since => _since;

        public DateTime Now => DateTime.Now;
    }
}
