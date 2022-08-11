using System.Data;
using NHibernate.Extensions.UnitTest.TestDb;
using Npgsql;
using NpgsqlTypes;

namespace NHibernate.Extensions.UnitTest;

[TestFixture]
public class NpgTimeTests : BaseTest {

    [OneTimeSetUp]
    public override void OneTimeSetUp() {
        base.OneTimeSetUp();
    }

    [Test]
    public void _01_CanQueryTimes() {
        using var session = OpenTestDbSession();
        var data = session.Query<NpgTime>().ToList();
        foreach (var item in data) {
            Console.WriteLine(item);
        }
    }

    [Test]
    public void _02_CanSaveTimes() {
        var localTime = DateTime.Now;
        var utcTime = DateTime.UtcNow;
        Console.WriteLine($"local time: {localTime}, utc time: {utcTime}");
        var entity = new NpgTime {
            // LocalTime = DateTime.SpecifyKind(localTime, DateTimeKind.Utc),
            LocalTime = localTime,
            UtcTime = utcTime
        };
        using var session = OpenTestDbSession();
        session.Save(entity);
        session.Flush();
    }

    [Test]
    public void _03_CanSaveRawTime() {
        var localTime = DateTime.Now;
        var utcTime = DateTime.UtcNow;
        using var session = OpenTestDbSession();
        using var conn = session.Connection as NpgsqlConnection;
        // conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "insert into public.npg_times(local_time, utc_time) values(:p0, :p1) returning id;";
        var p0 = cmd.CreateParameter();
        p0.ParameterName = ":p0";
        p0.Value = localTime;
        p0.DbType = DbType.DateTime;
        p0.NpgsqlDbType = NpgsqlDbType.Timestamp;
        cmd.Parameters.Add(p0);

        var p1 = cmd.CreateParameter();
        p1.ParameterName = ":p1";
        p1.Value = utcTime;
        p1.DbType = DbType.DateTime;
        p1.NpgsqlDbType = NpgsqlDbType.TimestampTz;
        cmd.Parameters.Add(p1);

        var result = cmd.ExecuteNonQuery();
        Console.WriteLine($"result {result}");

    }

}
