using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ConnectUs.Domain.Helpers
{
    /// <summary>
    /// can be caught and handled within the application
    /// </summary>
    public class AppException : Exception
    {
        public AppException() : base() { }

        public AppException(string message) : base(message) { }

        public AppException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
