using Autofac;
using Microsoft.EntityFrameworkCore;
namespace TesteDigitas.Infrastructure.EntityFrameworkDataAccess
{
    public class Module : Autofac.Module
    {
        //comentei em 11/09/24 -- sergio
        //public string ConnectionString { get; set; }

        //protected override void Load(ContainerBuilder builder)
        //{
            //var optionsBuilder = new DbContextOptionsBuilder<DbContext>();
            //optionsBuilder.UseNpgsql(ConnectionString);
            //optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            //optionsBuilder.EnableSensitiveDataLogging(true);

            //builder.RegisterType<Context>()
            //  .WithParameter(new TypedParameter(typeof(DbContextOptions), optionsBuilder.Options))
            //  .InstancePerLifetimeScope();

            //builder.RegisterAssemblyTypes(typeof(InfrastructureException).Assembly)
            //    .Where(type => type.Namespace.Contains("EntityFrameworkDataAccess"))
            //    .AsImplementedInterfaces()
            //    .InstancePerLifetimeScope();
        //}
    }
}
