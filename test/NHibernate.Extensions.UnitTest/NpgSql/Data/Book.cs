namespace NHibernate.Extensions.UnitTest.NpgSql.Data {

    public class Book {

        public virtual int BookId { get; set; }

        public virtual string Title { get; set; }

        public virtual Author Author { get; set; }

    }

}
