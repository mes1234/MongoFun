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


        public DbSet<Item> Items { get; set; }
#pragma warning restore
    }
}
