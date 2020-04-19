using System;
using System.Text.Json;
using NHibernate.Extensions.NpgSql;
using NHibernate.Mapping.Attributes;

namespace NHibernate.Extensions.UnitTest.TestDb {


    [Class(Schema = "public", Table = "json_values")]
    public class JsonValue {

        [Id(Name = nameof(Id), Column = "id", Type = "long")]
        [Generator(Class = "sequence")]
        [Param(Name = "sequence", Content = "public.test_id_seq")]
        public virtual long Id { get; set; }

        [Property(Column = "value", TypeType = typeof(JsonbType<ConnectionString>))]
        public virtual ConnectionString Value { get; set; }
    }

    public class ConnectionString {
        public string Server { get; set; }
        public int Port { get; set; }
        public string Database { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

}
