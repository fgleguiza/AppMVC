
using Microsoft.EntityFrameworkCore;
using app.Models;

namespace app.Context
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuario { get; set; }
    }
    
}
