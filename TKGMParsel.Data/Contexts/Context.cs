using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKGMParsel.Data.Entities;

namespace TKGMParsel.Data.Contexts
{
    public class Context:DbContext
    {

        public DbSet<City> City { get; set; }
        public DbSet<District> District { get; set; }
        public DbSet<Street> Street { get; set; }
        public DbSet<Parcel> Parsel { get; set; }
        public Context(DbContextOptions<Context> options) : base(options) { }
       

    }
}
