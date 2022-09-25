using SistemasSalgados.Bootstrapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("SistemaSalgadosDbContext");

builder.Services.ConfigContext(connectionString);
builder.Services.ConfigHealthCheck(connectionString);
builder.Services.ConfigIoc();
builder.Services.ConfigSwagger();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.StartSwagger();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.StartHealthCheck();
}

app.UseCors(options =>
{
    options.AllowAnyOrigin();
    options.AllowAnyHeader();

    options.WithMethods("POST", "GET", "PUT", "DELETE");
});

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
