using Microsoft.EntityFrameworkCore;
using NevladinaOrganizacija.Areas.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NevladinaOrganizacija.Models.Context
{
    public class myContext:DbContext
    {
        public myContext(DbContextOptions<myContext> options)
            : base(options)
        { }
        public DbSet<City> Cities { get; set; }
        public DbSet<Citizenship> Citizenships { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { optionsBuilder
        //        .UseSqlServer("Server=.;Database=NevladinaOrganizacija;Trusted_Connection=False;MultipleActiveResultSets=true"); }
    }
}
