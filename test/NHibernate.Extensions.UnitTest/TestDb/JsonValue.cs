using System.Text.Json.Serialization;
using NHibernate.Extensions.Npgsql.UserTypes;
using NHibernate.Mapping.Attributes;
using PropertyAttribute = NHibernate.Mapping.Attributes.PropertyAttribute;

namespace NHibernate.Extensions.UnitTest.TestDb;


[Class(Schema = "public", Table = "json_values")]
public class JsonValue {

    [Id(Name = nameof(Id), Column = "id", Type = "long", Generator = "trigger-identity")]
    public virtual long Id { get; set; }

    [Property(Column = "value", TypeType = typeof(JsonbType<ConnectionString>))]
    public virtual ConnectionString Value { get; set; }
}

public class ConnectionString {
    [JsonPropertyName("server")]
    public string Server { get; set; }
    [JsonPropertyName("port")]
    public int Port { get; set; }
    [JsonPropertyName("database")]
    public string Database { get; set; }
    [JsonPropertyName("username")]
    public string Username { get; set; }
    [JsonPropertyName("password")]
    public string Password { get; set; }
}
