using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var defaultCORSPolicyName = "Wasm";
builder.Services.AddCors(options =>
{
    var policy = new CorsPolicyBuilder();
    policy.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();

    options.AddPolicy(name: defaultCORSPolicyName, policy.Build());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(
        options =>
        {
            options.Authority = "https://e24py9.logto.app/oidc";
            options.RequireHttpsMetadata = false;
            options.Audience = "http://localhost:5150";
        });

#region Logs

// to enable HTTP verbose logging
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("Authorization");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});

#endregion Logs

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(defaultCORSPolicyName);

app.UseAuthorization();

app.MapControllers();

app.Run();
