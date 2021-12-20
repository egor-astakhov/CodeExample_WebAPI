using System;

namespace TitanApi.Infrastructure
{
    public interface IDateTimeProvider
    {
        public DateTime Since { get; }

        public DateTime Now { get; }
    }
}