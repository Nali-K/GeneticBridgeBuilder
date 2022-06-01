using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace GenenticAlgorithmBlazor.Server.Models
{
    public class DataContext:DbContext
    {
        public DbSet<StoredText> StoredTexts { get;set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
        /* override void OnConfiguring(DbContextOptions<Cont> optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }*/
        /*protected void OnConfiguring(DbContextOptionsBuilder<Context> optionsBuilder):base(optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Integrated Security=true;");
            //(@"data source=(localdb)\MSSQLLocalDB;Database=EFEasyExample;Trusted_Connection=True;");
        }*/
    }
}