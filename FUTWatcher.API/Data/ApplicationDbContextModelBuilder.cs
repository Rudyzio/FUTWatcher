using Microsoft.EntityFrameworkCore;

namespace FUTWatcher.API.Data
{
    public class ApplicationDbContextModelBuilder
    {
        internal static void BuildModel(ModelBuilder modelBuilder)
        {

            // Example for cascade on delete
            /*modelBuilder.Entity<Employee>()
                .HasMany<EmployeeSituationPeriod>(p => p.EmployeeSituationPeriods)
                .WithOne(p => p.Employee)
                .OnDelete(Microsoft.Data.Entity.Metadata.DeleteBehavior.Cascade);*/
        }
    }
}
