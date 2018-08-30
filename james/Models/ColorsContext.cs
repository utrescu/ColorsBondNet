using System;
using System.IO;
using Microsoft.Extensions.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace james.Db
{

    public class ColorsContext : DbContext
    {
        public ColorsContext() { }
        public ColorsContext(DbContextOptions<ColorsContext> options)
            : base(options)
        {

        }
        public virtual DbSet<Color> Colors { get; set; }

    }
}