using ExtendedWardenEvents.WEE.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ExtendedWardenEvents.API
{
    public static class JsonAPI
    {
        public static JsonConverter EventDataConverter => new ExternalEventDataConverter();
    }
}
