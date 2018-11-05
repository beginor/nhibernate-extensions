using System;
using System.Linq;
using Dapper;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace NHibernate.Extensions.UnitTest {

    [TestFixture]
    public class DapperTest : BaseTest {

        private ISession session;

        [OneTimeSetUp]
        public void OneTimeSetUp() {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() {
        }

        [SetUp]
        public void SetUp() {
            session = factory.OpenSession();
        }

        [TearDown]
        public void TearDown() {
        }

        [Test]
        public void CanQueryAuthors() {
            using (var session = factory.OpenSession()) {
                var conn = session.Connection;
                var authors = conn.Query<AuthorEntity>(
                    "select * from public.authors"
                );
                Assert.Greater(authors.Count(), 0);

                authors = conn.Query<AuthorEntity>(
                    "select * from public.authors where authorid = any(@Ids)",
                    new {
                        Ids = new [] { 1, 2, 3 } .AsEnumerable()
                    }
                );
                Assert.LessOrEqual(authors.Count(), 3);
            }
        }

    }

    public class AuthorEntity {
        public int AuthorId { get; set; }
        public string Name { get; set; }
    }

    public class BookEntity {
        public int BookId { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
    }

    public class TestTableEntity {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string[] Tags { get; set; }
        public virtual JToken JsonField { get; set; }
        public virtual JToken JsonbField { get; set; }
        public virtual DateTime? UpdateTime { get; set; }
        public virtual short[] Int16Arr { get; set; }
        public virtual int[] Int32Arr { get; set; }
        public virtual long[] Int64Arr { get; set; }
        public virtual float[] FloatArr { get; set; }
        public virtual double[] DoubleArr { get; set; }
        public virtual bool[] BooleanArr { get; set; }
    }
}
