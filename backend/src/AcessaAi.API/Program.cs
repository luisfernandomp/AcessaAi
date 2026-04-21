using AcessaAi.Infrastructure.Configurations;
using AcessaAi.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddOpenApi();

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(opt =>
    {
        opt.Password.RequireDigit = true;
        opt.Password.RequiredLength = 8;
        opt.Password.RequireUppercase = true;
        opt.User.RequireUniqueEmail = true;
        opt.SignIn.RequireConfirmedEmail = true;
    })
    .AddEntityFrameworkStores<AcessaAiDbContext>()    
    .AddDefaultTokenProviders();    

var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");

builder.Services.Configure<JwtSettings>(jwtSettingsSection);

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRouting();
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
