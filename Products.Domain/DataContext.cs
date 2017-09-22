namespace Products.Domain
{
    using System.Data.Entity;

    public class DataContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        #region Constructor
        public DataContext() : base("DefaultConnection")
        {

        }
        #endregion

    }
}
