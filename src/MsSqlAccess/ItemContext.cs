using DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsSqlAccess
{
    public class ItemContext : DbContext
    {
#pragma warning disable CS8618
        public ItemContext(DbContextOptions<ItemContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
              .Entity<Item>()
              .Property(e => e.TimeStamp)
             .HasConversion(v => DateTime.SpecifyKind(v, DateTimeKind.Utc), v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
        }

        public DbSet<Item> Items { get; set; }
#pragma warning restore
    }

}
