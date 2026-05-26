using AcessaAi.API.Extensions;
using AcessaAi.API.Middlewares;
using AcessaAi.Domain.Usuarios.Entities;
using AcessaAi.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AcessaAi.Infrastructure.Configurations;

var builder = WebApplication.CreateBuilder(args);

var urls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS");
if (string.IsNullOrWhiteSpace(urls))
{
    urls = builder.Environment.IsDevelopment()
        ? "http://localhost:5000"
        : "http://0.0.0.0:5000";
}
builder.WebHost.UseUrls(urls);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddHttpsRedirection(options =>
    {
        options.HttpsPort = 5001;
    });
}

builder.Services.AddServicesExtensions(builder.Configuration);

builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(policy =>
    {
        var allowedOrigins = builder.Configuration
            .GetSection("AllowedOrigins")
            .Get<string[]>() ?? ["http://localhost:4200"];

        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers(opt =>
{
    opt.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
});

builder.Services.AddOpenApiWithJwt();
builder.Services.AddS3(builder.Configuration);

builder.Services
    .AddIdentity<Usuario, IdentityRole<int>>(opt =>
    {
        opt.Password.RequireDigit = false;
        opt.Password.RequiredLength = 1;
        opt.Password.RequireUppercase = false;
        opt.User.RequireUniqueEmail = true;
        opt.SignIn.RequireConfirmedEmail = false;
    })
    .AddEntityFrameworkStores<AcessaAiDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");

builder.Services.Configure<JwtSettings>(jwtSettingsSection);

builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IOptions<JwtSettings>>().Value);

builder.Services
    .AddAuthentication(opt =>
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettingsSection["Issuer"],
            ValidAudience = jwtSettingsSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettingsSection["SecretKey"]!)),
            ClockSkew = TimeSpan.Zero
        };
    });

var app = builder.Build();

var enableSwagger = app.Environment.IsDevelopment()
    || app.Configuration.GetValue<bool>("EnableSwagger");

if (enableSwagger)
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.RoutePrefix = "api/swagger";
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseRouting();
app.UseCors();

app.UseForwardedHeaders();

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandler();

app.MapControllers();

app.Run();
