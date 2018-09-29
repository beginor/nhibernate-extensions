using System.Collections.Generic;

namespace NHibernate.Extensions.UnitTest.NpgSql.Data {

    public class Author {

        public virtual int AuthorId { get; set; }

        public virtual string Name { get; set; }

        public virtual ICollection<Book> Books { get; set; }

    }

}
