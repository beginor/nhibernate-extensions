using System;
using NHibernate.Mapping.Attributes;

namespace NHibernate.Extensions.UnitTest.DvdRental {

    [Class(Schema = "public", Table = "actor")]
    public class Actor {

        [Id(Name = "ActorId", Column = "actor_id", Type = "int")]
        [Generator(Class = "sequence")]
        [Param(Name = "sequence", Content = "public.actor_actor_id_seq")]
        public virtual int ActorId { get; set; }

        [Property(Column = "first_name", Type="string", Length = 45)]
        public virtual string FirstName { get; set; }

        [Property(Column = "last_name", Type="string", Length = 45)]
        public virtual string LastName { get; set; }

        [Property(Column = "last_update", Type = "datetime")]
        public virtual DateTime? LastUpdate { get; set; }

    }

}
