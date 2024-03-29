using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace NHibernate.Extensions.UnitTest.Sqlite;

public class Book {
    public virtual int BookId { get; set; }
    public virtual string Title { get; set; }
    public virtual Author Author { get; set; }
}

public class BookMappingSqlite : ClassMapping<Book> {

    public BookMappingSqlite() {
        Table("books");
        Id(e => e.BookId, map => {
            map.Column("id");
            map.Type(NHibernateUtil.Int32);
            map.Generator(Generators.Identity);
        });
        Property(p => p.Title, map => {
            map.Column("title");
            map.Type(NHibernateUtil.String);
        });
        ManyToOne(p => p.Author, map => {
            map.Column("author_id");
            map.Fetch(FetchKind.Join);
            map.ForeignKey("fk_books_author_id");
        });
    }

}
