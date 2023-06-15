using ArcTicketSystem.API.Data;
using ArcTicketSystem.API.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TICKETS", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
    { new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer"}
            },
        new string[] {}
    }
    });
});


builder.Services.AddDbContext<TicketContext>(options => options.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings:DefaultConnection").Value));

builder.Services.AddScoped<ITicketRepository, TicketRepository>();


var TokenValidationParameters = new TokenValidationParameters
{
    ValidIssuer = "https://fbi-demo.com",
    ValidAudience = "https://fbi-demo.com",
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SXkSqsKyNUyvGbnHs7ke2NCq8zQzNLW7mPmHbnZZ")),
    ClockSkew = TimeSpan.Zero // remove delay of token when expire
};

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(cfg =>
    {
        cfg.TokenValidationParameters = TokenValidationParameters;
    });

//builder.Services.AddSwaggerGen();

//builder.Services.AddDbContext<TicketContext>(options => options.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings:DefaultConnection").Value));

//builder.Services.AddScoped<ITicketRepository, TicketRepository>();



builder.Services.AddAuthorization(cfg =>
{
    cfg.AddPolicy("Admin", policy => policy.RequireClaim("type", "Admin"));
    cfg.AddPolicy("User", policy => policy.RequireClaim("type", "User", "Admin"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files")),
    RequestPath = "/Upload/Files"
});


//Enable directory browsing
app.UseDirectoryBrowser(new DirectoryBrowserOptions
{
    FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files")),
    RequestPath = "/Upload/Files"
});


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
