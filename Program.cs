using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using negare_kanban_api.Data;
using negare_kanban_api.Services.AuthService;
using negare_kanban_api.Services.BoardListService;
using negare_kanban_api.Services.BoardService;
using negare_kanban_api.Services.CardService;
using negare_kanban_api.Services.TokenService;
using negare_kanban_api.Services.UserService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// TODO - Adicionar descrição
// Se quiser usar database in memory, descomentar essa parte:
// builder.Services.AddDbContext<DataContext>(opt => 
//     opt.UseInMemoryDatabase("NegareKanban")
// );

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var conn = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(conn));

// Configure JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JwtSecurityToken:Audience"],
            ValidIssuer = builder.Configuration["JwtSecurityToken:Issuer"],
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSecurityToken:Key"] ?? string.Empty))
        };
    });

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

if(allowedOrigins != null)
{
    // Add services to the container.
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("corsPolicy", policy =>
        {
            policy.WithOrigins(allowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
    });
}

// Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBoardService, BoardService>();
builder.Services.AddScoped<IBoardListService, BoardListService>();
builder.Services.AddScoped<ICardService, CardService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("corsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
