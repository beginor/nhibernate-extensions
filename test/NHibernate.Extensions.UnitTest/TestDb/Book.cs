using NHibernate.Mapping.Attributes;
using MFetchMode = NHibernate.Mapping.Attributes.FetchMode;
using PropertyAttribute = NHibernate.Mapping.Attributes.PropertyAttribute;

namespace NHibernate.Extensions.UnitTest.TestDb;

[Class(Table = "books", Schema = "public")]
public class Book {

    [Id(Name = "BookId", Column = "book_id", Type = "long", Generator = "trigger-identity")]
    public virtual long BookId { get; set; }

    [Property(Column = "title", Type = "string")]
    public virtual string Title { get; set; }

    [ManyToOne(Column = "author_id", Fetch = MFetchMode.Join)]
    public virtual Author Author { get; set; }

}
