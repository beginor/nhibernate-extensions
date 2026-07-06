using NHibernate.Mapping.Attributes;
using PropertyAttribute = NHibernate.Mapping.Attributes.PropertyAttribute;

namespace NHibernate.Extensions.UnitTest.TestDb;

[Class(Table = "authors", Schema = "public")]
public class Author {

    [Id(Column = "id", Type = "long", Name = "AuthorId", Generator = "trigger-identity")]
    public virtual long AuthorId { get; set; }

    [Property(Column = "first_name", Type = "string")]
    public virtual string FirstName { get; set; }

    [Property(Column = "last_name", Type = "string")]
    public virtual string LastName { get; set; }

    [Bag(Table = "books", Inverse = true)]
    [Key(Column = "author_id")]
    [OneToMany(ClassType = typeof(Book))]
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();

}
