using NHibernate.Mapping.Attributes;
using MFetchMode = NHibernate.Mapping.Attributes.FetchMode;

namespace NHibernate.Extensions.UnitTest.NpgSql.Data {

    [Class(Table = "books", Schema = "public")]
    public class Book {

        [Id(Name = "BookId", Column = "bookid", Type = "int")]
        [Generator(Class = "native")]
        public virtual int BookId { get; set; }

        [Property(Column = "title", Type = "string")]
        public virtual string Title { get; set; }

        [ManyToOne(Column = "authorid", Fetch = MFetchMode.Join)]
        public virtual Author Author { get; set; }

    }

}
