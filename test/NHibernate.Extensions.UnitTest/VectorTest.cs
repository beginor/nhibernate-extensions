using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using NHibernate.Extensions.Npgsql.UserTypes;
using Npgsql;
using Pgvector;

using NHibernate.Extensions.Pgvector;
using NHibernate.Extensions.Pgvector.UserTypes;
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
    public async Task _02_Can_SaveVector() {
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

    [Test]
    public void _03_CanQueryDistanceWithSql() {
        var target = new Vector(new float[] { 1, 2, 0 });
        using var session = OpenTestDbSession();
        var sql = """
            select id, text, created_at, embedding
            from public.chunk_embeddings
            order by (embedding <=> :target)
            limit 3;
        """;
        var query = session.CreateSQLQuery(sql);
        query.AddEntity(typeof(ChunkEmbeddingEntity))
             .SetParameter("target", target);
        var data = query.List<ChunkEmbeddingEntity>();
        foreach (var item in data) {
            // Console.WriteLine($"{item.Id}, {item.Text}, {item.CreateTime}, {item.Embedding}");
            Console.WriteLine($"{item.GetType()}");
        }
        Assert.That(data.Count, Is.GreaterThanOrEqualTo(3));
    }

    [Test, Ignore("Is anyone who use hql with NHibernate?")]
    public void _04_CanQueryDistanceWithHql() {
        var target = new Vector(new float[] { 1, 2, 0 });
        using var session = OpenTestDbSession();
        var query = session.CreateQuery("from ChunkEmbeddingEntity e order by cosine_distance(e.Embedding, :target")
            .SetParameter("target", target)
            .SetFirstResult(0)
            .SetMaxResults(3);
        var data = query.List<ChunkEmbeddingEntity>();
        Assert.That(data.Count, Is.GreaterThanOrEqualTo(3));
    }

    [Test]
    public void _05_CanQueryDistanceWithLinq() {
        var target = new Vector(new float[] { 1, 2, 0 });
        using var session = OpenTestDbSession();
        var query = session.Query<ChunkEmbeddingEntity>().Select(x => new {
            x.Id,
            x.Text,
            x.Embedding,
            x.CreateTime,
            Distance = x.Embedding.L1Distance(target)
        }).Skip(0).Take(3);
        var data = query.ToList();
        Assert.That(data.Count, Is.GreaterThanOrEqualTo(3));

        var query2 = session.Query<ChunkEmbeddingEntity>()
            .OrderBy(x => x.Embedding.CosineDistance(target))
            .Select(x => new {
                x.Id,
                x.Text,
                x.CreateTime
            }).Skip(0).Take(3);
        var data2 = query2.ToList();
        Assert.That(data2.Count, Is.GreaterThanOrEqualTo(3));
    }

}
