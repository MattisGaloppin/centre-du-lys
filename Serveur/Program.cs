using Microsoft.Extensions.Options;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Serveur.Model.Data; // Namespace pour UserDAO
using Serveur.Model.DTO;
using Serveur.Model.Managers; // Namespace pour UserManager

var builder = WebApplication.CreateBuilder(args);

// Ajouter les services nécessaires
builder.Services.AddControllers(); // Permet d'utiliser les controllers
builder.Services.AddEndpointsApiExplorer(); // Nécessaire pour Swagger
builder.Services.AddSwaggerGen(); // Ajout de Swagger pour la documentation

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("ConnectionStrings"));

// Injection de dépendances
builder.Services.AddScoped<IDatabase, SQLiteService>(provider =>
{
    var config = provider.GetRequiredService<IOptions<DatabaseSettings>>().Value;
    return new SQLiteService(config.DefaultDatabase);
});

// Configuration des dépendances
builder.Services.AddScoped<UserDAO>(); // Ajout de UserDAO avec son interface
builder.Services.AddScoped<UserManager>(); // Injection de UserManager

var app = builder.Build();

// Activer Swagger en environnement de développement
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Utilisateurs v1");
        c.RoutePrefix = string.Empty; // Swagger accessible à la racine
    });
}

// Middleware
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers(); // Mapper les controllers
app.Run();
