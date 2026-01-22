var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// TODO: Uncomment when implementing this service
// builder.Services.AddControllers();
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
// builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
// builder.Services.AddInfrastructure(builder.Configuration);
// builder.Services.AddSharedMessaging(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
// TODO: Uncomment when implementing this service
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

app.MapGet("/", () => "Communication Service - Not yet implemented");

app.Run();
