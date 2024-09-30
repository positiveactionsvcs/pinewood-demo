using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Infrastructure;
using PinewoodDemo.Data;
using Microsoft.EntityFrameworkCore.Storage;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Configure the database connection.
services.AddDbContext<PinewoodContext>(o =>
{
    o.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Integrated Security=true;Initial Catalog=PinewoodDemo");
});

// Configure MVC.
services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

var app = builder.Build();

// Create the database on (localdb)\MSSQLLocalDB if it doesn't exist.
using (IServiceScope serviceScope = app.Services.CreateScope())
{
    PinewoodContext context = serviceScope.ServiceProvider.GetRequiredService<PinewoodContext>();
    context.Database.Migrate();
    context.Database.EnsureCreated();
    RelationalDatabaseCreator databaseCreator = (RelationalDatabaseCreator)context.Database.GetService<IDatabaseCreator>();

    using (DbCommand command = context.Database.GetDbConnection().CreateCommand())
    {
        command.CommandText = "SELECT CASE WHEN OBJECT_ID('dbo.Customers', 'U') IS NOT NULL THEN 1 ELSE 0 END";
        context.Database.OpenConnection();

        bool tableExists = (int?)command.ExecuteScalar() == 1;

        if (!tableExists)
            databaseCreator.CreateTables();
    }
}

// Configure the HTTP request pipeline.
app.UseRouting();
app.MapControllers();
app.MapControllerRoute("default", "{controller=Home}/{action=get}");

app.Run();