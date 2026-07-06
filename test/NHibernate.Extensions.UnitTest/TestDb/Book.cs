using NHibernate.Mapping.Attributes;
using MFetchMode = NHibernate.Mapping.Attributes.FetchMode;
using PropertyAttribute = NHibernate.Mapping.Attributes.PropertyAttribute;

namespace NHibernate.Extensions.UnitTest.TestDb;

[Class(Table = "books", Schema = "public")]
public class Book {

    [Id(Name = "Id", Column = "id", Type = "long", Generator = "trigger-identity")]
    public virtual long Id { get; set; }

    [Property(Column = "title", Type = "string")]
    public virtual string Title { get; set; }

    [ManyToOne(Column = "author_id", Fetch = MFetchMode.Join)]
    public virtual Author Author { get; set; }

    [Property(Column = "published_in", Type = "int")]
    public virtual int PublishedIn { get; set; }

    [Property(Column = "language_id", Type = "int")]
    public virtual int Language { get; set; }

}
