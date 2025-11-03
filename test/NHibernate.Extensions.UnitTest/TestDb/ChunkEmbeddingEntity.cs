using NHibernate.Mapping.Attributes;
using PropertyAttribute = NHibernate.Mapping.Attributes.PropertyAttribute;
using Pgvector;

namespace NHibernate.Extensions.UnitTest.TestDb;

[Class(Schema = "public", Table = "chunk_embeddings")]
public class ChunkEmbeddingEntity {
    [Id(Name = nameof(Id), Column = "id", Type = "long", Generator = "trigger-identity")]
    public virtual long Id { get; set; }
    [Property(Name = nameof(Text), Column = "text", Type = "string", Length = 8192)]
    public virtual string Text { get; set; }
    [Property(Name = nameof(CreateTime), Column = "created_at", Type = "timestamp")]
    public virtual DateTime CreateTime { get; set; }
    [Property(Name = nameof(Embedding), Column = "embedding", Type = "vector")]
    public virtual Vector Embedding { get; set; }

}
