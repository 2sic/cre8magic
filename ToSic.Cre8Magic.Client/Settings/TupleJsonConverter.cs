//using System.Text.Json.Serialization;
//using System.Text.Json;
//using System.Text.Json.Nodes;

//namespace ToSic.Cre8magic.Client.Settings;

//public class TupleJsonConverter : JsonConverter<(string On, string Off)>
//{
//    //private readonly string Format;
//    //public TupleJsonConverter(string format)
//    //{
//    //    Format = format;
//    //}
//    public override void Write(Utf8JsonWriter writer, (string On, string Off) date, JsonSerializerOptions options)
//    {
//        if (date.Off == null)
//            writer.WriteStringValue(date.On);
//        else
//        {
//            writer.WriteStartArray();
//            writer.WriteStringValue(date.On);
//            writer.WriteStringValue(date.Off);
//            writer.WriteEndArray();
//        }
//        // writer.WriteStringValue(date.ToString(Format));
//    }


//    public override (string On, string Off) Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//    {
//        (string On, string Off) ArrayToTuple(JsonArray jsonArray)
//        {
//            var parts = jsonArray.AsArray();
//            var len = parts.Count;
//            return (len > 0 ? parts[0]?.ToString() : null, len > 1 ? parts[1]?.ToString() : null)!;
//        }

//        var x = JsonNode.Parse(ref reader);
//        if (x == null) return (null, null)!;
//        //var type = x.GetType();
//        //if (type == typeof(string))
//        //    return (x.ToString(), null!);

//        // Clear cut case: array - usually of string
//        //if (x is JsonArray jArray)
//        //{
//        //    return ArrayToTuple(jArray);
//        //}

//        return x switch
//        {
//            // Clear cut case: array - usually of string
//            JsonArray jArray => ArrayToTuple(jArray),

//            // String value case
//            JsonValue jValue => (jValue.ToString(), null!),

//            // Object, currently not handlede
//            JsonObject jObject => (null!, null!),

//            _ => ("on-fail", "off-fail")!,

//        };
//    }
//}