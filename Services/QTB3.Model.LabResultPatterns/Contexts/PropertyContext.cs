using Microsoft.EntityFrameworkCore;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;

namespace QTB3.Model.LabResultPatterns.Contexts
{
    public class PropertyContext: DbContext
    {
        public PropertyContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Uom> Uoms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);
            // informing EF about the private fields here allows
            // the Entity to be readonly elsewhere in the app
            // https://docs.microsoft.com/en-us/ef/core/modeling/backing-field
            // https://csharp.christiannagel.com/2016/11/07/efcorefields/
            modelbuilder.Entity<Uom>().Property(u => u.Id).HasField("_id");
            modelbuilder.Entity<Uom>().Property(u => u.Name).HasField("_name");
            modelbuilder.Entity<Uom>().Property(u => u.Description).HasField("_description");
            modelbuilder.Entity<Uom>()
                .HasIndex(u => u.Name)
                .IsUnique();
        }
    }
}