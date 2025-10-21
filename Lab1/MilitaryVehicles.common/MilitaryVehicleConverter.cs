using System.Text.Json;
using System.Text.Json.Serialization;
using MilitaryVehicles.common;

public class MilitaryVehicleConverter : JsonConverter<MilitaryVehicle>
{
    public override MilitaryVehicle Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var jsonDoc = JsonDocument.ParseValue(ref reader);
        var root = jsonDoc.RootElement;

        if (!root.TryGetProperty("Type", out var typeProperty))
        {
            throw new JsonException("Поле 'Type' не знайдено");
        }

        string type = typeProperty.GetString();

        return type switch
        {
            "Tank" => JsonSerializer.Deserialize<Tank>(root.GetRawText(), options),
            "Helicopter" => JsonSerializer.Deserialize<Helicopter>(root.GetRawText(), options),
            "Destroyer" => JsonSerializer.Deserialize<Destroyer>(root.GetRawText(), options),
            _ => throw new JsonException($"Невідомий тип: {type}")
        };
    }

    public override void Write(Utf8JsonWriter writer, MilitaryVehicle value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (object)value, value.GetType(), options);
    }
}