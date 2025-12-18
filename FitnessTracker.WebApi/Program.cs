using FitnessTracker.WebApi.Extensions;
using FitnessTracker.Application;
using FitnessTracker.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Service registrations
builder.Services.AddControllers();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddMemoryCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

// Pipeline
app.UseFitnessTrackerPipeline(app.Environment);
app.MapControllers();
app.Run();
