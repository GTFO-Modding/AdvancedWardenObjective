using AWO.WEE.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AWO.API
{
    public static class JsonAPI
    {
        public static JsonConverter EventDataConverter => new ExternalEventDataConverter();
    }
}
