using Performance;
using PerformanceClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApiDocument();
builder.Services.AddSingleton(builder.Configuration);
builder.Services.AddSingleton<IGrpcPerformanceClient>(p => new GrpcPerformanceClient(builder.Configuration["ServerUrl"]));
builder.Services.AddGrpcClient<Performance.Monitor.MonitorClient>(o =>
{
	o.Address = new Uri(builder.Configuration["ServerUrl"]);
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseOpenApi();
	app.UseSwaggerUi3();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
