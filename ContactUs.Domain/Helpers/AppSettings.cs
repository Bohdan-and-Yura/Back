using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectUs.Domain.Helpers
{
    /// <summary>
    /// Contains properties defined in the appsettings.json file.
    /// Used for accessing application settings via objects that are injected 
    /// </summary>
    public class AppSettings
    {
        public string Secret { get; set; }
    }
}
