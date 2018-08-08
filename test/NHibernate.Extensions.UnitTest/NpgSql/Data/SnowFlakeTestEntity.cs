using System;
using Newtonsoft.Json.Linq;
using NHibernate.Mapping.Attributes;


namespace NHibernate.Extensions.UnitTest.NpgSql.Data {

    [Class(Schema = "public", Table = "snow_flake_test")]
    public class SnowFlakeTestEntity {

        // [Id(Column = "id", Type = "long")]
        // [NaturalId()]
        // [Generator(Class = "select")]
        public virtual long Id { get; set; }

        // [Property(Column = "name", Type = "string")]
        public virtual string Name { get; set; }

    }

}
