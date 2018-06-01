using System;

namespace NHibernate.Extensions.UnitTest.NpgSql.Data {

    public class TestEntity {

        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string[] Tags { get; set; }

        public virtual string JsonField { get; set; }

        public virtual string JsonbField { get; set; }

        public virtual DateTime UpdateTime { get; set; }

        public virtual short[] Int16Arr { get; set; }

        public virtual int[] Int32Arr { get; set; }

        public virtual long[] Int64Arr { get; set; }

        public virtual float[] FloatArr { get; set; }

        public virtual double[] DoubleArr { get; set; }

        public virtual bool[] BooleanArr { get; set; }

    }

}
