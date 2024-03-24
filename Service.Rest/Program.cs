using Application.Services.Identities;
using Domain.Core;
using IdentityMicroservice.ModelFactory;
using Infrastructure.Data.EntityFramework;
using Service.Rest.Exntensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.ConfigureStartupConfig<JwtConfig>(builder.Configuration.GetSection(nameof(JwtConfig)));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddCreditAuthentication();

builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

builder.Services.AddScoped(typeof(IClientService), typeof(ClientService));
builder.Services.AddScoped(typeof(IClientRegistrationService), typeof(ClientRegistrationService));
builder.Services.AddScoped(typeof(IClientRoleService), typeof(ClientRoleService));
builder.Services.AddScoped(typeof(IPermissionService), typeof(PermissionService));
builder.Services.AddScoped(typeof(ITokenFactoryService), typeof(TokenFactoryService));
builder.Services.AddScoped(typeof(ITokenValidatorService), typeof(TokenValidatorService));
builder.Services.AddScoped(typeof(IEncryptionService), typeof(EncryptionService));
builder.Services.AddScoped(typeof(IClientRoleModelFactory), typeof(ClientRoleModelFactory));
builder.Services.AddScoped(typeof(IClientModelFactory), typeof(ClientModelFactory));
builder.Services.AddScoped(typeof(IPermissionModelFactory), typeof(PermissionModelFactory));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRepositories(builder.Configuration);
builder.Services.AddCors();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(option =>
{
    option.AllowAnyHeader()
    .AllowAnyOrigin()
    .AllowAnyMethod();
});

app.UseAuthorization();

app.MapControllers();

app.Run();
