using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace NHibernate.Extensions.UnitTest.Sqlite;

public class Author {
    public virtual int AuthorId { get; set; }
    public virtual string Name { get; set; }
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}

public class AuthorMappingSqlite : ClassMapping<Author> {

    public AuthorMappingSqlite() {
        // Schema("main");
        Table("authors");
        Id(e => e.AuthorId, m => {
            m.Column("id");
            m.Type(NHibernateUtil.Int32);
            m.Generator(Generators.Identity);
        });
        Property(e => e.Name, m => {
            m.Column("name");
            m.Type(NHibernateUtil.String);
            m.Length(16);
            m.NotNullable(true);
        });
        Bag(p => p.Books, map => {
            map.Key(k => k.Column("author_id"));
            map.Inverse(true);
            map.Cascade(Cascade.DeleteOrphans);
        }, c => {
            c.OneToMany();
        });
    }

}
