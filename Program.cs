using EPlatformWebApp.Controllers.Utils;
using EPlatformWebApp.DataContext;
using EPlatformWebApp.DTO.Utils;
using EPlatformWebApp.Model;
using EPlatformWebApp.Model.Utils;
using EPlatformWebApp.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var key = builder.Configuration.GetSection("JWT:Key").Value!;
var bKey = Encoding.UTF8.GetBytes(key);
//Change this value for production when it is ready to be deployed
String angularAppUrl = "https://localhost:44451";


// Add services to the container.
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServerConnection")));
builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(ModelMapper));
builder.Services.AddScoped<Repository<User>>();
builder.Services.AddScoped<Repository<Course>>();
builder.Services.AddScoped<Repository<CourseCategory>>();
builder.Services.AddScoped<Repository<Video>>();
builder.Services.AddScoped<Repository<Blog>>();
builder.Services.AddScoped<Repository<BlogPost>>();
builder.Services.AddScoped<Repository<BlogPostAnswer>>();
builder.Services.AddScoped<Repository<PostLike>>();
builder.Services.AddScoped<Repository<PostAnswerLikes>>();
builder.Services.AddScoped<Repository<UserFavoriteCourse>>();
builder.Services.AddScoped<Repository<UserCourse>>();
builder.Services.AddScoped<Repository<CourseTest>>();
builder.Services.AddScoped<Repository<PDFFile>>();
builder.Services.AddScoped<JWTHandler>();
builder.Services.AddScoped<Repository<UserImage>>();
builder.Services.AddScoped<Repository<Cert>>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(bKey),
            ValidateIssuer = false,
            ValidateAudience = false,
            // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
            ClockSkew = TimeSpan.Zero

        };
    });

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    }
    );
});


builder.Services.AddCors(options => options.AddPolicy("AngularApp", policy =>
{
    policy.WithOrigins("https://localhost:44451", "https://cayudi-001-site1.anytempurl.com")
          .AllowAnyMethod()
          .AllowAnyHeader();
}));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else 
{
    app.UseSwagger();
    app.UseSwaggerUI();
    angularAppUrl = "https://localhost:44451";
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AngularApp");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
