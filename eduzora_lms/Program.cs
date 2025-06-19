using eduzora_lms.Data;
using eduzora_lms.Repositories.Admin;
using eduzora_lms.Repositories.Instructor;
using eduzora_lms.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Database Connection
builder.Services.AddDbContext<ApplicationDbContext>(op => {
    op.UseSqlServer(builder.Configuration.GetConnectionString("eduzora_lms_cn"));
});

// For IdentityDbContext Dependency Injection
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();


// Custom password
builder.Services.Configure<IdentityOptions>(op => {
    op.Password.RequireDigit = true;
    op.Password.RequireNonAlphanumeric = false;
    op.Password.RequireLowercase = false;
    op.Password.RequireUppercase = false;
    op.Password.RequiredLength = 3;
    op.Password.RequiredUniqueChars = 0;
    // require unique Email
    op.User.RequireUniqueEmail = true;
    // require confirm Email.
    op.SignIn.RequireConfirmedEmail = true;
    // require confirm phone no false
    op.SignIn.RequireConfirmedPhoneNumber = false;

    // Configure LifeSpan Of Token
    op.Tokens.PasswordResetTokenProvider = "Default";
});

// Set Reset-Password Token LifeSpan
builder.Services.Configure<DataProtectionTokenProviderOptions>(op => {
    op.TokenLifespan = TimeSpan.FromMinutes(5);
});


// SMTP Config / Dependency Injection
builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddSingleton<IEmailSender, SmtpEmailSender>();

// Config Session
builder.Services.AddSession( op => {
    op.IdleTimeout = TimeSpan.FromMinutes(10);
    op.Cookie.HttpOnly = true;
    op.Cookie.IsEssential = true;
});


// Custom Access Denied and redirection to Login Page
builder.Services.ConfigureApplicationCookie(op => {
    // Redirect to Login Page (UnAuthenticated User)
    op.LoginPath = "/account/login";
    // Config Access Denied Path
    op.AccessDeniedPath = "/account/access-denied";
});



// Repository Pattern Injection 
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ILevelRepository, LevelRepository>();
builder.Services.AddScoped<ILanguageRepository, LanguageRepository>();
builder.Services.AddScoped<ISettingsRepository, SettingsRepository>();
builder.Services.AddScoped<ICreateCourseRepository, CreateCourseRepository>();
builder.Services.AddScoped<IDisplayCourseRepository, DisplayCourseRepository>();



// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// Each And Every Req Run Inside this Path
app.Map("/eduzora_lms", eduzora_app =>
{
    eduzora_app.UseStaticFiles();

    eduzora_app.UseRouting();

    // Session Middleware
    eduzora_app.UseSession();

    eduzora_app.UseAuthentication();
    eduzora_app.UseAuthorization();

    eduzora_app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    });
});

app.Run();
