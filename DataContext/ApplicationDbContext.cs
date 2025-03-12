using EPlatformWebApp.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EPlatformWebApp.DataContext
{
    public class ApplicationDbContext : DbContext
    {
        #region "Constructor"
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
                
        }
        #endregion


        #region "Models"
        public DbSet<Role> Role { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<CourseCategory> CourseCategory { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<UserCourse> UserCourse { get; set; }
        public DbSet<UserFavoriteCourse> UserFavoriteCourse { get; set; }
        public DbSet<Blog> Blog { get; set; }
        public DbSet<BlogPost> BlogPost { get; set; }
        public DbSet<BlogPostAnswer> BlogPostAnswer { get; set; }
        public DbSet<Video> Video { get; set; }
        public DbSet<PostLike> PostLike { get; set; }
        public DbSet<PostAnswerLikes> PostAnswerLike { get; set; }
        public DbSet<Cert> Cert { get; set; }
        public DbSet<PDFFile> PDFFile { get; set; }
        public DbSet<CourseTest> CourseTests { get; set; }
        public DbSet<UserImage> UserImage { get; set; }



        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Creatting default values
            modelBuilder.Entity<Role>().HasData(
                new Role { ID=1, Name = "Administrator"},
                new Role { ID=2, Name = "User"}  
            );
           
            //Setting default values
            modelBuilder.Entity<User>()
            .Property(b => b.CreationDate)
            .HasDefaultValueSql("getdate()");
            modelBuilder.Entity<User>()
            .Property(b => b.RoleID)
            .HasDefaultValueSql("2");
            modelBuilder.Entity<UserCourse>()
            .Property(b => b.StartTime)
            .HasDefaultValueSql("getdate()");
            modelBuilder.Entity<UserCourse>()
            .Property(b => b.IsOnline)
            .HasDefaultValueSql("0");
            modelBuilder.Entity<UserCourse>()
            .Property(b => b.IsOppened)
            .HasDefaultValueSql("0");
            modelBuilder.Entity<BlogPost>()
            .Property(b => b.CreationDate)
            .HasDefaultValueSql("getdate()");
            modelBuilder.Entity<BlogPost>()
            .Property(b => b.CreationDate)
            .HasDefaultValueSql("getdate()");
            modelBuilder.Entity<BlogPostAnswer>()
            .Property(b => b.CreationDate)
            .HasDefaultValueSql("getdate()");

            //Skipt cicle problems related with multiple users for blog post and blog post answers
            modelBuilder.Entity<BlogPost>()
            .HasOne(e => e.User)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BlogPostAnswer>()
            .HasOne(bpa => bpa.User)
            .WithMany()
            .HasForeignKey(bpa => bpa.UserID)
            .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<UserCourse>()
            .HasOne(uc => uc.User)
            .WithMany()
            .HasForeignKey(uc => uc.UserID)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserFavoriteCourse>()
            .HasOne(uf => uf.User)
            .WithMany()
            .HasForeignKey(uf => uf.UserID)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
