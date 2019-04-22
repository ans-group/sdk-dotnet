using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace UKFast.API.Client.Json
{
    public class DateConverter : IsoDateTimeConverter
    {
        public DateConverter()
        {
            base.DateTimeFormat = "yyyy-MM-dd";
        }
    }
}
