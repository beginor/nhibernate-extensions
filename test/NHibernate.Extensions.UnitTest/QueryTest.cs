using System;
using System.Linq;
using NHibernate.Extensions.UnitTest.TestDb;
using NUnit.Framework;

namespace NHibernate.Extensions.UnitTest {

    public class QueryTest : BaseTest {

        [Test]
        public void _01_CanQueryManyToOne() {
            using (var session = TestDbSessionFactory.OpenSession()) {
                var query = session.Query<Book>().Select(b => b.Author);
                var data = query.ToList();
                Console.WriteLine(data.Count);
            }
        }

        [Test]
        public void _02_CanQueryOneToMany() {
            using (var session = TestDbSessionFactory.OpenSession()) {
                var query = session.Query<Author>().Select(a => new {
                    Author = a,
                    Books = a.Books
                });
                var data = query.ToList();
                Console.WriteLine(data.Count);
            }
        }

        [Test]
        public void _03_CanQueryReference() {
            using (var session = TestDbSessionFactory.OpenSession()) {
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

        [Test]
        public void _04_CanSaveOneToMany() {
            using (var session = TestDbSessionFactory.OpenSession()) {
                using (var tx = session.BeginTransaction()) {
                    try {
                        var author = new Author();
                        author.Name = "beginor";
                        session.Save(author);
                        var book = new Book();
                        book.Title = "learning nhibernate";
                        book.Author = author;
                        session.Save(book);
                        session.Flush();

                        session.Delete(book);
                        session.Delete(author);
                        tx.Commit();
                    }
                    catch (Exception) {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }

    }

}
