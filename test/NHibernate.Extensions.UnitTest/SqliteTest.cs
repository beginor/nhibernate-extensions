namespace NHibernate.Extensions.UnitTest;

[TestFixture]
public class SqliteTest {

    [Test]
    public void _01_CanGetScheme() {
        var connStr = "Data Source=./test_db.db3;Foreign Keys=True;";
        var conn = new Microsoft.Data.Sqlite.SqliteConnection(connStr);
        conn.Open();
        Assert.That(conn, Is.Not.Null);
        var schema = conn.GetSchema();
        Assert.That(schema, Is.Not.Null);
        conn.Close();
    }

}
