using System;
using System.Linq;
using NHibernate.Extensions.UnitTest.NpgSql.Data;
using NHibernate.Linq;
using Xunit;

namespace NHibernate.Extensions.UnitTest {

    public class QueryTest : BaseTest {

        [Fact]
        public void _01_CanQueryManyToOne() {
            using (var session = factory.OpenSession()) {
                var query = session.Query<Book>().Select(b => b.Author);
                var data = query.ToList();
                Console.WriteLine(data.Count);
            }
        }

        [Fact]
        public void _02_CanQueryOneToMany() {
            using (var session = factory.OpenSession()) {
                var query = session.Query<Author>().Select(a => new {
                    a.Name,
                    BookCount = a.Books.Count()
                });
                var data = query.ToList();
                Console.WriteLine(data.Count);
            }
        }

        [Fact]
        public void _03_CanQueryReference() {
            using (var session = factory.OpenSession()) {
                var query = from book in session.Query<Book>()
                    select new {
                        BookId = book.BookId,
                        AuthorId = book.Author.AuthorId
                    };
                var data = query.ToList();
                foreach (var d in data) {
                    Console.WriteLine($"{d.AuthorId}, {d.BookId}");
                }
            }
        }

    }

}
