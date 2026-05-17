using RaizesDoNordeste.API.Configuration;
using Microsoft.EntityFrameworkCore;
using RaizesDoNordeste.API.Filter;
using RaizesDoNordeste.Application.DependencyInjectionApplication;
using RaizesDoNordeste.Domain.Enum;
using RaizesDoNordeste.Infrastructure.DependencyInjection;
using RaizesDoNordeste.Infrastructure.Persistence;
using RaizesDoNordeste.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(opt => opt.Filters.Add(typeof(ValidationFilter)));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddDataBase(builder.Configuration)
    .AddRepositories()
    .AddServices()
    .AddFluentValidationServices()
    .AddJwtAuthentication(builder.Configuration)
    .AddSwaggerService();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<RaizesDoNordesteDbContext>();

        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }

        SeedConfiguracaoInicial(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocorreu um erro crítico ao aplicar as migrações ou realizar o seed do banco de dados.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerService();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

static void SeedConfiguracaoInicial(RaizesDoNordesteDbContext context)
{
    var adminId = Guid.Parse("6b9b39ec-8163-47f8-b3a9-e12176a3a465");
    var adminExiste = context.Usuarios.Any(u => u.Id == adminId || u.Email == "admin@raizes.com");

    if (!adminExiste)
    {
        var admin = new Usuario
        {
            Id = adminId,
            Nome = "Administrador",
            Email = "admin@raizes.com",
            SenhaHash = "oso3/m/cSQuPfOhB4XAaFp0rFpfGtbXGP5SruPm21t0=", // Hash equivalente a 'Senha@123'
            Telefone = null,
            Role = RoleUsuario.Admin,

            ConsentimentoLGPD = true,
            ConsentimentoEm = DateTime.UtcNow.AddHours(-3),
            Ativo = true,
            CriadoEm = DateTime.UtcNow.AddHours(-3),
            AtualizadoEm = DateTime.UtcNow.AddHours(-3)
        };

        context.Usuarios.Add(admin);
        context.SaveChanges();
    }
}