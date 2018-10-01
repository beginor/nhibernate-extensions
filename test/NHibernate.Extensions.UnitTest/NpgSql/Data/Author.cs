using System.Collections.Generic;
using NHibernate.Mapping.Attributes;

namespace NHibernate.Extensions.UnitTest.NpgSql.Data {

    [Class(Table = "authors", Schema = "public")]
    public class Author {

        [Id(Column = "authorid", Type = "int", Name = "AuthorId")]
        [Generator(Class = "native")]
        public virtual int AuthorId { get; set; }

        [Property(Column = "name", Type = "string")]
        public virtual string Name { get; set; }

        [Bag(Table = "books", Inverse = true)]
        [Key(Column = "authorid")]
        [OneToMany(ClassType = typeof(Book))]
        public virtual ICollection<Book> Books { get; set; }

    }

}
