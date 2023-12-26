using NUnit.Framework.Legacy;
using NHibernate.Extensions.UnitTest.TestDb;

namespace NHibernate.Extensions.UnitTest;

[TestFixture]
public class NameQueryTest : BaseTest {

    [OneTimeSetUp]
    public override void OneTimeSetUp() {
        base.OneTimeSetUp();
    }

    [Test]
    public void _01_CanQueryClass() {
        using (var session = TestDbSessionFactory.OpenSession()) {
            var query = session.GetNamedQuery("name_test_01");
            var books = query.List<Book>();

            ClassicAssert.True(books.Count > 0);
        }
    }

    [Test]
    public void _02_CanQueryClassWithParam() {
        using (var session = TestDbSessionFactory.OpenSession()) {
            var query = session.GetNamedQuery("name_test_02");
            query.SetInt32("authorid", 2);
            var books = query.List<Book>();
            ClassicAssert.True(books.Count > 0);
        }
    }

    [Test]
    public void _03_CanQueryScalar() {
        using (var session = TestDbSessionFactory.OpenSession()) {
            var query = session.GetNamedQuery("name_test_03");
            var booksCount = query.List<long>().First();
            ClassicAssert.True(booksCount > 0);
        }
    }

    [Test]
    public void _04_CanQueryScalarWithParam() {
        using (var session = TestDbSessionFactory.OpenSession()) {
            var query = session.GetNamedQuery("name_test_04");
            ClassicAssert.True(query.NamedParameters.Length > 0);
            query.SetInt32("authorid", 1);
            ClassicAssert.True(query.NamedParameters.Length > 0);
            var booksCount = query.List<long>().First();
            ClassicAssert.True(booksCount > 0);
        }
    }

    [Test]
    public void _05_CanQueryDynamicColumns() {
        using (var session = TestDbSessionFactory.OpenSession()) {
            var query = session.GetNamedQuery("name_test_05");
            var books = query.List();

            ClassicAssert.True(books.Count > 0);
        }
    }

}
