using droeftoeters_api.Data;
using droeftoeters_api.Interfaces;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add config support and logging
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Logging.ClearProviders().AddConsole();

//Adds the data repositories
builder.Services.AddTransient<IDataService, SqlDataService>();
builder.Services.AddTransient<IProcedureData, ProcedureData>();
builder.Services.AddTransient<IProcedureItemData, ProcedureItemData>();

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
app.MapGroup("/account").MapIdentityApi<IdentityUser>();
app.MapControllers();

app.Run();