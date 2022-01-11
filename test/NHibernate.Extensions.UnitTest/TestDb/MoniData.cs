using NHibernate.Mapping.Attributes;
using PropertyAttribute = NHibernate.Mapping.Attributes.PropertyAttribute;

namespace NHibernate.Extensions.UnitTest.TestDb;

/// <summary>Table, moni_data</summary>
[Class(Schema = "public", Table = "moni_data")]
public partial class MoniData  {

    /// <summary>CompositeId for moni_time,station_id,item_id</summary>
    [CompositeId]
    [KeyProperty(Name = "MoniTime", Column = "moni_time", Type = "datetime" )]
    [KeyProperty(Name = "StationId", Column = "station_id", Type = "long" )]
    [KeyProperty(Name = "ItemId", Column = "item_id", Type = "long" )]
    protected virtual int CompositeId => GetHashCode();

    /// <summary>moni_time, timestamp</summary>
    public virtual DateTime MoniTime { get; set; }

    /// <summary>station_id, int8</summary>
    public virtual long StationId { get; set; }

    /// <summary>item_id, int8</summary>
    public virtual long ItemId { get; set; }

    /// <summary>value, numeric</summary>
    [Property(Name = "Value", Column = "value", Type = "decimal", NotNull = false)]
    public virtual decimal? Value { get; set; }

    /// <summary>description, varchar</summary>
    [Property(Name = "Description", Column = "description", Type = "string", NotNull = false, Length = 64)]
    public virtual string Description { get; set; }

    public override int GetHashCode() {
        unchecked {
            int hashCode;
            hashCode = MoniTime.GetHashCode();
            hashCode = (hashCode * 397) ^ StationId.GetHashCode();
            hashCode = (hashCode * 397) ^ ItemId.GetHashCode();
            return hashCode;
        }
    }

    public override bool Equals(object obj) {
        if (ReferenceEquals(null, obj)) {
            return false;
        }
        if (ReferenceEquals(this, obj)) {
            return true;
        }
        if (obj.GetType() != this.GetType()) {
            return false;
        }
        return Equals((MoniData)obj);
    }

    public virtual bool Equals(MoniData other) {
        if (other == null){
            return false;
        }
        return this.GetHashCode() == other.GetHashCode();
    }
}
