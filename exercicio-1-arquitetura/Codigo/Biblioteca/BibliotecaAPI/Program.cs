using Application.Autor;

using Domain.Autor;

using Infrastructure;

using BibliotecaAPI.Filter;
using Core;
using Core.Identity.Data;
using Core.Service;

using Service;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Domain.ItemAcervo;
using Application.ItemAcervo;
using Domain.Livro;
using Application.Livro;

namespace BibliotecaAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureControllers(builder);
            ConfigureSwagger(builder);
            ConfigureLegacyArchitecture(builder);
            ConfigureCleanArchitecture(builder);
            ConfigureAutoMapper(builder);
            ConfigureIdentity(builder);
            ConfigureAuthentication(builder);

            var app = builder.Build();

            ConfigureMiddleware(app);

            app.Run();
        }

        private static void ConfigureControllers(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers(options =>
                options.Filters.Add(new HttpResponseExceptionFilter())
            );
        }

        private static void ConfigureSwagger(WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }

        private static void ConfigureLegacyArchitecture(WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IAutorService, AutorService>();
            builder.Services.AddTransient<ILivroService, LivroService>();

            builder.Services.AddDbContext<BibliotecaContext>(
                options => options.UseInMemoryDatabase("BibliotecaDatabase")
            );
        }

        private static void ConfigureCleanArchitecture(WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<Context>(
                options => options.UseInMemoryDatabase("CleanArchitectureDatabase")
            );

            builder.Services.AddScoped<IAutorRepository, AutorRepository>();

            builder.Services.AddScoped<CreateAutorUseCase>();
            builder.Services.AddScoped<UpdateAutorUseCase>();
            builder.Services.AddScoped<DeleteAutorUseCase>();
            builder.Services.AddScoped<GetAutorByIdUseCase>();
            builder.Services.AddScoped<GetAllAutoresUseCase>();
            builder.Services.AddScoped<GetAutoresPageUseCase>();

            builder.Services.AddScoped<IItemAcervoRepository, ItemAcervorepository>();

            builder.Services.AddScoped<CreateItemAcervoUseCase>();
            builder.Services.AddScoped<DeleteItemAcervoUseCase>();
            builder.Services.AddScoped<UpdateItemAcervoUseCase>();
            builder.Services.AddScoped<GetAllItemAcervoUseCase>();
            builder.Services.AddScoped<GetItemAcervoByIdUseCase>();

            builder.Services.AddScoped<ILivroRepository, LivroRepository>();
            
            builder.Services.AddScoped<UseCaseCriarLivro>();
            builder.Services.AddScoped<UseCaseEditarLivro>();
            builder.Services.AddScoped<UseCaseExcluirLivro>();
            builder.Services.AddScoped<UseCaseObterLivroPorId>();
            builder.Services.AddScoped<UseCaseListarLivros>();
            builder.Services.AddScoped<UseCaseListarLivrosPorTitulo>();
        }

        private static void ConfigureAutoMapper(WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(
                AppDomain.CurrentDomain.GetAssemblies()
            );
        }

        private static void ConfigureIdentity(WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<IdentityContext>(
                options => options.UseInMemoryDatabase("IdentityDatabase")
            );

            builder.Services
                .AddIdentityApiEndpoints<UsuarioIdentity>(options =>
                {
                    ConfigureIdentityOptions(options);
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>();
        }

        private static void ConfigureIdentityOptions(
            IdentityOptions options
        )
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;

            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;

            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
        }

        private static void ConfigureAuthentication(
            WebApplicationBuilder builder
        )
        {
            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();
        }

        private static void ConfigureMiddleware(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.MapIdentityApi<UsuarioIdentity>();

            app.MapControllers();
        }
    }
}