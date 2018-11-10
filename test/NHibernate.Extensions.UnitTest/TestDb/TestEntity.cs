using System;
using NHibernate.Extensions.NpgSql;
using NHibernate.Mapping.Attributes;
using Newtonsoft.Json.Linq;

namespace NHibernate.Extensions.UnitTest.TestDb {

    [Class(Table = "test_table", Schema = "public")]
    public class TestEntity {

        [Id(Name = nameof(Id), Column = "id", Type = "long")]
        [Generator(Class = "sequence")]
        [Param(Name = "sequence", Content = "public.test_table_id_seq")]
        public virtual long Id { get; set; }

        [Property(Column = "name", Type = "string", Length = 32)]
        public virtual string Name { get; set; }

        [Property(Column = "tags", TypeType = typeof(ArrayType<string>))]
        public virtual string[] Tags { get; set; }

        [Property(Column = "json_field", TypeType = typeof(JsonType))]
        public virtual JToken JsonField { get; set; }

        [Property(Column = "jsonb_field", TypeType = typeof(JsonbType))]
        public virtual JToken JsonbField { get; set; }

        [Property(Column = "update_time", Type = "datetime")]
        public virtual DateTime? UpdateTime { get; set; }

        [Property(Column = "int16_arr", TypeType = typeof(ArrayType<short>))]
        public virtual short[] Int16Arr { get; set; }

        [Property(Column = "int32_arr", TypeType = typeof(Int32ArrayType))]
        public virtual int[] Int32Arr { get; set; }

        [Property(Column = "int64_arr", TypeType = typeof(Int64ArrayType))]
        public virtual long[] Int64Arr { get; set; }

        [Property(Column = "real_arr", TypeType = typeof(FloatArrayType))]
        public virtual float[] FloatArr { get; set; }

        [Property(Column = "double_arr", TypeType = typeof(DoubleArrayType))]
        public virtual double[] DoubleArr { get; set; }

        [Property(Column = "bool_arr", TypeType = typeof(BooleanArrayType))]
        public virtual bool[] BooleanArr { get; set; }

    }

}
