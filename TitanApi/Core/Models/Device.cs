﻿namespace TitanApi.Core.Models
{
    public class Device : IPersistent
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

    }
}
