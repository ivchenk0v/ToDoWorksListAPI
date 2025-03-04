using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ToDoWorksListAPI.DataAccess;
using ToDoWorksListAPI.Middleware;
using ToDoWorksListAPI.Models;
using ToDoWorksListAPI.Service;

namespace ToDoWorksListAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services
                .AddMvc(options =>
                {
                    options.InputFormatters.Add(new XmlSerializerInputFormatter(options));
                    options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
                });

            builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<User, IdentityRole>(opts => {
                opts.Password.RequiredLength = 3;   // минимальная длина
                opts.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
                opts.Password.RequireLowercase = false; // требуются ли символы в нижнем регистре
                opts.Password.RequireUppercase = false; // требуются ли символы в верхнем регистре
                opts.Password.RequireDigit = false; // требуются ли цифры
            })
                .AddEntityFrameworkStores<ApplicationContext>();


            var tokenSettings = builder.Configuration.GetSection("tokenSettings");
            var secretKey = Environment.GetEnvironmentVariable("SECRET");

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = tokenSettings["Issuer"],
                    ValidAudience = tokenSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Список дел", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            builder.Services.AddApiVersioning(setup =>
            {
                setup.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
                setup.AssumeDefaultVersionWhenUnspecified = true;
                setup.ReportApiVersions = true;
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            builder.Services.AddSingleton<IToDoList, ToDoList>();
            builder.Services.AddScoped<ILogService, LogService>();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var userManager = services.GetRequiredService<UserManager<User>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    await RoleInitializer.InitializeAsync(userManager, roleManager);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Список дел");
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<RequestLoggingMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
