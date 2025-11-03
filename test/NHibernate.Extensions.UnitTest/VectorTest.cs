using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using Npgsql;
using Pgvector;

using NHibernate.Extensions.Pgvector;
using NHibernate.Extensions.UnitTest.TestDb;

namespace NHibernate.Extensions.UnitTest;

[TestFixture]
public class VectorTest : BaseTest {

    [Test]
    public void _01_Can_QueryVector() {
        using var session = OpenTestDbSession();
        var query = session.Query<ChunkEmbeddingEntity>().OrderBy(e => e.Id);
        var data = query.ToList();
        Assert.That(data.Count, Is.GreaterThanOrEqualTo(0));
    }

    [Test]
    public void _02_Can_QueryVector_distance() {
        var target = new Vector(new float[] { 1, 2, 0 });
        using var session = OpenTestDbSession();
        var query = session.Query<ChunkEmbeddingEntity>().Select(x => new {
            x.Id,
            x.Text,
            x.Embedding,
            x.CreateTime
        }).OrderBy(x => x.Embedding.L1Distance(target)).Take(3);
        var data = query.ToList();
        Assert.That(data.Count, Is.GreaterThanOrEqualTo(3));
    }

    [Test]
    public void _03_CanQueryDistanceWithNpgsql() {
        var target = new Vector(new float[] { 1, 2, 0 });
        using var session = OpenTestDbSession();
        var conn = session.Connection as NpgsqlConnection;
        var sql = """
                  select text, (embedding <=> @target) as distance
                  from public.chunk_embeddings
                  order by distance
                  limit 3;
                  """;
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        cmd.Parameters.AddWithValue("target", target);
        var reader = cmd.ExecuteReader();
        while (reader.Read()) {
            Console.WriteLine(reader["text"]);
        }
    }

    [Test]
    public async Task _02_Can_SaveVector() {
        // using var session = OpenTestDbSession();
        var input = "hello, world!";
        var embedding = new float[3] { 1.0F, 2.0F, 3.0F };

        var entity = new ChunkEmbeddingEntity {
            Text = input,
            CreateTime = DateTime.Now,
            Embedding = new Vector(embedding)
        };
        using var session = OpenTestDbSession();
        await session.SaveAsync(entity);
        await session.FlushAsync();
        session.Clear();
    }

}
