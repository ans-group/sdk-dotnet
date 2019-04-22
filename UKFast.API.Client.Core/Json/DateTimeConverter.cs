using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace UKFast.API.Client.Json
{
    public class DateTimeConverter : IsoDateTimeConverter
    {
        public DateTimeConverter()
        {
            base.DateTimeFormat = "yyyy-MM-ddThh:mm:ss+00:00";
        }
    }
}
