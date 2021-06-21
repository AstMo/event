using PartyMaker.Configuration.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PartyMaker.Configuration.Models
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string ConnectionString { get; set; }

        public bool EnableLog { get; set; }
    }
}
