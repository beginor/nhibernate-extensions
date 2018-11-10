using System;

namespace NHibernate.Extensions.UnitTest.DvdRental {

    public class Actor {

        public virtual int ActorId { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual DateTime? LastUpdate { get; set; }

    }

}
