using Microsoft.Extensions.Options;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Serveur.Model.Data; // Namespace pour UserDAO
using Serveur.Model.DTO;
using Serveur.Model.Managers;
using DatabaseSettings = Serveur.Model.Data.DatabaseSettings; // Namespace pour UserManager

var builder = WebApplication.CreateBuilder(args);

// Ajouter les services n�cessaires
builder.Services.AddControllers(); // Permet d'utiliser les controllers
builder.Services.AddEndpointsApiExplorer(); // N�cessaire pour Swagger
builder.Services.AddSwaggerGen(); // Ajout de Swagger pour la documentation

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("ConnectionStrings"));


// Injection de d�pendances
builder.Services.AddScoped<IDatabase, SQLiteService>(provider =>
{
    var config = provider.GetRequiredService<IOptions<DatabaseSettings>>().Value;
    return new SQLiteService(config.DefaultConnection);
});

// Configuration des d�pendances
// Configuration des d�pendances
builder.Services.AddScoped<UserDAO>(provider =>
{
    var database = provider.GetRequiredService<IDatabase>();
    var logger = provider.GetRequiredService<ILogger<UserDAO>>();
    return new UserDAO(database);
});
builder.Services.AddScoped<UserManager>(); // Injection de UserManager

var app = builder.Build();

// Activer Swagger en environnement de d�veloppement
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Utilisateurs v1");
        c.RoutePrefix = string.Empty; // Swagger accessible � la racine
    });
}

// Middleware
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers(); // Mapper les controllers
app.Run();
