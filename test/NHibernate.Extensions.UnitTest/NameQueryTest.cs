using System.Linq;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Extensions.UnitTest.NpgSql.Data;

using Xunit;

namespace NHibernate.Extensions.UnitTest {

    public class NameQueryTest : BaseTest {

        [Fact]
        public void _01_CanQueryClass() {
            using (var session = factory.OpenSession()) {
                var query = session.GetNamedQuery("name_test_01");
                var books = query.List<Book>();

                Assert.True(books.Count > 0);
            }
        }

        [Fact]
        public void _02_CanQueryClassWithParam() {
            using (var session = factory.OpenSession()) {
                var query = session.GetNamedQuery("name_test_02");
                query.SetInt32("authorid", 2);
                var books = query.List<Book>();
                Assert.True(books.Count > 0);
            }
        }

        [Fact]
        public void _03_CanQueryScalar() {
            using (var session = factory.OpenSession()) {
                var query = session.GetNamedQuery("name_test_03");
                var booksCount = query.List<long>().First();
                Assert.True(booksCount > 0);
            }
        }

        [Fact]
        public void _04_CanQueryScalarWithParam() {
            using (var session = factory.OpenSession()) {
                var query = session.GetNamedQuery("name_test_04");
                Assert.True(query.NamedParameters.Length > 0);
                query.SetInt32("authorid", 1);
                Assert.True(query.NamedParameters.Length > 0);
                var booksCount = query.List<long>().First();
                Assert.True(booksCount > 0);
            }
        }

        [Fact]
        public void _05_CanQueryDynamicColumns() {
            using (var session = factory.OpenSession()) {
                var query = session.GetNamedQuery("name_test_05");
                var books = query.List();

                Assert.True(books.Count > 0);
            }
        }


    }

}
