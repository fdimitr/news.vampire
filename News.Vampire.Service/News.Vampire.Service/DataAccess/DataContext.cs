using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using News.Vampire.Service.Models;
using News.Vampire.Service.Models.UserManagement;

namespace News.Vampire.Service.DataAccess
{
    public sealed class DataContext : IdentityDbContext<ApplicationUser>
    {
        private string? _databaseId = null;

        public DbSet<Source> Sources { get; set; }
        public DbSet<NewsItem> NewsItem { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }


        public DataContext() : base()
        {
            Database.Migrate();
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public string GetDatabaseId()
        {
            if (string.IsNullOrWhiteSpace(_databaseId))
            {
                var result = Database.SqlQuery<long>($"SELECT system_identifier FROM pg_control_system()").ToList();
                _databaseId = result.FirstOrDefault().ToString();
            }
            return _databaseId;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Authentication
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Name = StaticUserRoles.Admin, NormalizedName = StaticUserRoles.Admin });
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Name = StaticUserRoles.User, NormalizedName = StaticUserRoles.User });

            // Application Data
            modelBuilder.Entity<Reader>().HasData(new Reader
            {
                Id = 1,
                Login = "Dmitry",
                Password = "6QvNlj77zDchLwiTrY/b/o28Cg3vvwwO7IkZrh5BqaA=",
                Role = "Admin"
            });

            modelBuilder.Entity<Group>()
                .HasMany(e => e.Sources)
                .WithOne(e => e.Group)
                .HasForeignKey(e => e.GroupId)
                .HasPrincipalKey(e => e.Id);

            modelBuilder.Entity<Group>().HasData(new Group { Id = 1, Name = "Корреспондент", IsActive = true });
            modelBuilder.Entity<Group>().HasData(new Group { Id = 2, Name = "Habr", IsActive = true });
            modelBuilder.Entity<Group>().HasData(new Group { Id = 3, Name = "News.RU", IsActive = true });
            modelBuilder.Entity<Group>().HasData(new Group { Id = 4, Name = "Новости", IsActive = true });
            modelBuilder.Entity<Group>().HasData(new Group { Id = 5, Name = "Goha", IsActive = true });

            modelBuilder.Entity<Source>().HasData(new Source
            {
                Id = 1,
                Code = "corr.events",
                GroupId = 1,
                Name = "Последние новости по разделу События в Украине",
                Url = "http://k.img.com.ua/rss/ru/events.xml"
            });

            modelBuilder.Entity<Source>().HasData(new Source
            {
                Id = 2,
                Code = "goha.games",
                GroupId = 5,
                Name = "GOHA Видеоигры",
                Url = "https://www.goha.ru/rss/videogames"
            });

            modelBuilder.Entity<Source>().HasData(new Source
            {
                Id = 4,
                Code = "corr.tech",
                GroupId = 1,
                Name = "Технологии",
                Url = "http://k.img.com.ua/rss/ru/technews.xml"
            });

            modelBuilder.Entity<Source>().HasData(new Source
            {
                Id = 5,
                Code = "corr.space",
                GroupId = 1,
                Name = "Космос",
                Url = "http://k.img.com.ua/rss/ru/space.xml"
            });

            modelBuilder.Entity<Source>().HasData(new Source
            {
                Id = 6,
                Code = "habr.develop",
                GroupId = 2,
                Name = "HABR. Все публикации в потоке Разработка",
                Url = "https://habr.com/ru/rss/flows/develop/all/?fl=rul"
            });

            modelBuilder.Entity<Source>().HasData(new Source
            {
                Id = 8,
                Code = "meduza.news",
                GroupId = 4,
                Name = "Meduza: Новости",
                Url = "https://meduza.io/rss/news"
            });

            modelBuilder.Entity<Source>().HasData(new Source
            {
                Id = 10,
                Code = "autonews",
                GroupId = 4,
                Name = "AUTO News",
                Url = "https://autonews.autoua.net/rss/"
            });

            modelBuilder.Entity<UserGroup>()
                .HasMany(e => e.Subscriptions)
                .WithOne(e => e.UserGroup)
                .HasForeignKey(e => e.UserGroupId)
                .HasPrincipalKey(e => e.Id);

            var userGroup = new UserGroup { Id = 1, UserId = 1, Name = "My Group" };
            modelBuilder.Entity<UserGroup>().HasData(userGroup);

            modelBuilder.Entity<Subscription>().HasData(new Subscription { Id = 1, SourceId = 1, UserId = 1, UserGroupId = 1 });
            modelBuilder.Entity<Subscription>().HasData(new Subscription { Id = 3, SourceId = 6, UserId = 1, UserGroupId = 1 });
            modelBuilder.Entity<Subscription>().HasData(new Subscription { Id = 2, SourceId = 8, UserId = 1, UserGroupId = 1 });
        }

    }
}
