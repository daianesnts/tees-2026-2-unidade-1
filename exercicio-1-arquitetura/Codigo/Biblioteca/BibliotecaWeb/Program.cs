using BibliotecaWeb.Filter;
using BibliotecaWeb.Helpers;

using Application.Autor;

using Domain.Autor;

using Infrastructure;

using Application.Editora;
using Application.ItemAcervo;
using Application.Livro;

using Domain.Editora;
using Domain.ItemAcervo;
using Domain.Livro;

using Core.Identity.Data;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add<CustomExceptionFilter>();
            });
            builder.Services.AddRazorPages();
            builder.Services.AddAutoMapper(
                AppDomain.CurrentDomain.GetAssemblies()
            );


            builder.Services.AddTransient<IEmailSender, EmailSender>();

            builder.Services.AddDbContext<Context>(
                options => options.UseInMemoryDatabase(
                    "BibliotecaDatabase"
                )
            );

            builder.Services.AddScoped<
                IAutorRepository,
                AutorRepository
            >();


            builder.Services.AddScoped<IAutorRepository, AutorRepository>();
            builder.Services.AddScoped<IEditoraRepository, EditoraRepository>();
            builder.Services.AddScoped<IItemAcervoRepository, ItemAcervoRepository>();
            builder.Services.AddScoped<ILivroRepository, LivroRepository>();

            builder.Services.AddScoped<ICriarAutor, UseCaseCriarAutor>();
            builder.Services.AddScoped<IEditarAutor, UseCaseEditarAutor>();
            builder.Services.AddScoped<IExcluirAutor, UseCaseExcluirAutor>();
            builder.Services.AddScoped<IObterAutorPorId, UseCaseObterAutorPorId>();
            builder.Services.AddScoped<IListarAutores, UseCaseListarAutores>();
            builder.Services.AddScoped<IGetAutoresPage, GetAutoresPageUseCase>();

            builder.Services.AddScoped<UseCaseCriarEditora>();
            builder.Services.AddScoped<UseCaseEditarEditora>();
            builder.Services.AddScoped<UseCaseExcluirEditora>();
            builder.Services.AddScoped<UseCaseListarEditoras>();
            builder.Services.AddScoped<UseCaseObterEditoraPorId>();
            builder.Services.AddScoped<UseCaseBuscarEditoraPorNome>();

            builder.Services.AddScoped<UseCaseCriarItemAcervo>();
            builder.Services.AddScoped<UseCaseEditarItemAcervo>();
            builder.Services.AddScoped<UseCaseExcluirItemAcervo>();
            builder.Services.AddScoped<UseCaseListarItemAcervo>();
            builder.Services.AddScoped<UseCaseObterItemAcervoPorId>();

            builder.Services.AddScoped<UseCaseCriarLivro>();
            builder.Services.AddScoped<UseCaseEditarLivro>();
            builder.Services.AddScoped<UseCaseExcluirLivro>();
            builder.Services.AddScoped<UseCaseListarLivros>();
            builder.Services.AddScoped<UseCaseObterLivroPorId>();
            builder.Services.AddScoped<UseCaseListarLivrosPorTitulo>();


            builder.Services.AddDbContext<IdentityContext>(
                options => options.UseInMemoryDatabase(
                    "IdentityDatabase"
                )
            );

            builder.Services
                .AddDefaultIdentity<UsuarioIdentity>(
                    options =>
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

                        options.Lockout.DefaultLockoutTimeSpan =
                            TimeSpan.FromMinutes(5);
                        options.Lockout.MaxFailedAccessAttempts = 5;
                        options.Lockout.AllowedForNewUsers = true;
                    }
                )
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>();

            builder.Services.Configure<
                DataProtectionTokenProviderOptions
            >(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(2);
            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "BibliotecaCookieName";

                options.Cookie.HttpOnly = true;

                options.ExpireTimeSpan =
                    TimeSpan.FromMinutes(60);

                options.ReturnUrlParameter =
                    CookieAuthenticationDefaults.ReturnUrlParameter;

                options.SlidingExpiration = true;
            });

            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout =
                    TimeSpan.FromSeconds(10);

                options.Cookie.HttpOnly = true;

                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");

                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSession();

            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );


            app.Run();
        }
    }
}
