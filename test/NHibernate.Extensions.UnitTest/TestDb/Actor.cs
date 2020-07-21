using System;
using NHibernate.Mapping.Attributes;

namespace NHibernate.Extensions.UnitTest.TestDb {

    [Class(Schema = "public", Table = "actor")]
    public class Actor {

        [Id(Name = "ActorId", Column = "actor_id", Type = "long", Generator = "trigger-identity")]
        public virtual long ActorId { get; set; }

        [Property(Column = "first_name", Type="string", Length = 45)]
        public virtual string FirstName { get; set; }

        [Property(Column = "last_name", Type="string", Length = 45)]
        public virtual string LastName { get; set; }

        [Property(Column = "last_update", Type = "datetime")]
        public virtual DateTime? LastUpdate { get; set; }

    }

}
