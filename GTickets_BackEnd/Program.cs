using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using User.Management.Service.Models;
using User.Management.Service.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using GTickets_BackEnd.Models;
using GTickets_BackEnd.Services.ServicesContracts;
using GTickets_BackEnd.Services.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("GTickets_BackEndContextConnection") ?? throw new InvalidOperationException("Connection string 'GTickets_BackEndContextConnection' not found.");

// Add services to the container.
var configuration = builder.Configuration;
builder.Services.AddDbContext<GTickets_BackEndContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

//For identity
builder.Services.AddIdentity<CustomUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<GTickets_BackEndContext>()
    .AddDefaultTokenProviders();
builder.Services.AddScoped < IEmailService,  EmailService>();

//Add Config for required Email
builder.Services.Configure<IdentityOptions>(
    options => options.SignIn.RequireConfirmedEmail = true
    );
builder.Services.Configure<DataProtectionTokenProviderOptions>(
    options => options.TokenLifespan = TimeSpan.FromHours(24));

//Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
})
;
// Add Email Configs
var emailConfig = configuration
                    .GetSection("EmailConfiguration")
                    .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
options.AddPolicy("AllowOrigin",
            builder => builder.WithOrigins("http://localhost:4200")
                              .AllowAnyHeader()
                              .AllowAnyMethod()));

builder.Services.AddScoped<IAuthService, AuthService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();;
app.UseCors("AllowOrigin");
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();
