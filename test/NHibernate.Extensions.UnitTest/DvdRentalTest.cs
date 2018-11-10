using System;
using System.Linq;
using NHibernate.Linq;
using NHibernate.Extensions.UnitTest.DvdRental;
using NHibernate.Extensions.UnitTest.TestDb;
using NUnit.Framework;

namespace NHibernate.Extensions.UnitTest {

    [TestFixture]
    public class DvdRentalTest : BaseTest {

        private ISession OpenSession() {
            return DvdRentalSessionFactory.OpenSession();
        }

        [Test]
        public void _01_CanSetupSessionFactory() {
            Assert.IsNotNull(DvdRentalSessionFactory);
            Assert.IsNotNull(TestDbSessionFactory);

            var session1 = DvdRentalSessionFactory.OpenSession();
            var connStr = session1.Connection.ConnectionString;
            Console.WriteLine(connStr);
            session1.Close();

            var session2 = TestDbSessionFactory.OpenSession();
            var connStr2 = session2.Connection.ConnectionString;
            Console.WriteLine(connStr2);
            session2.Close();

            Assert.AreNotEqual(connStr, connStr2);
        }

        [Test]
        public void _02_CanQueryAuthors() {
            var factory = DvdRentalSessionFactory;
            using (var session = factory.OpenSession()) {
                var actors = session.Query<Actor>().ToList();
                Assert.IsNotEmpty(actors);
            }
        }

        [Test]
        public void _03_CanInsertUpdateDeleteAuthors() {
            using (var session = OpenSession()) {
                var author = new Actor {
                    FirstName = "Simon",
                    LastName = "Zhang",
                    LastUpdate = DateTime.Now
                };
                session.Save(author);
                session.Flush();
                Assert.Greater(author.ActorId, 0);
                session.Delete(author);
                session.Flush();
            }
        }

    }

}
