using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequiredLength = 10;
    })
    .AddRoles<IdentityRole>()
    .AddDapperStores(options =>
    {
        options.ConnectionString = Environment.GetEnvironmentVariable("ENVIRONMENT_VARIABLE_SQL") ?? builder.Configuration.GetConnectionString("azure");
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// these 2 endpoints could be merged
app.MapGroup("/account").MapIdentityApi<IdentityUser>();
app.MapGet("/account/checkAccessToken", [Authorize] () => Results.Content("{\"authorized\": true}", "application/json"));
app.MapControllers();

app.Run();