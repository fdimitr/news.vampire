using Microsoft.EntityFrameworkCore;
using News.Vampire.Service.Models;
using System;

namespace News.Vampire.Service.DataAccess
{
    public class DataContext : DbContext
    {
        public virtual DbSet<Source> Sources { get; set; }
        public virtual DbSet<NewsItem> NewsItem { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Subscription> Subscriptions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserGroup> UserGroups { get; set; }

        private bool UnitTestMode { get; }

        public DataContext() : base()
        {
            Database.Migrate();
        }

        public DataContext(DbContextOptions<DataContext> options, bool unitTestMode = false) : base(options)
        {
            UnitTestMode = unitTestMode;
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (UnitTestMode) return;

            // hashed "newspassword"
            modelBuilder.Entity<User>().HasData(new User { Id = 1, Login = "Dmitry", Password = "6QvNlj77zDchLwiTrY/b/o28Cg3vvwwO7IkZrh5BqaA=" }); 

            modelBuilder.Entity<Group>().HasData(new Group { Id = 1, Name = "Корреспондент", isActive = true });
            modelBuilder.Entity<Group>().HasData(new Group { Id = 2, Name = "Habr", isActive = true });
            modelBuilder.Entity<Group>().HasData(new Group { Id = 3, Name = "News.RU", isActive = true });
            modelBuilder.Entity<Group>().HasData(new Group { Id = 4, Name = "Новости", isActive = true });

            modelBuilder.Entity<Source>().HasData(new Source
            {
                Id = 1,
                GroupId = 1,
                Name = "Последние новости по разделу События в Украине",
                Url = "http://k.img.com.ua/rss/ru/events.xml"
            });

            modelBuilder.Entity<Source>().HasData(new Source
            {
                Id = 2,
                GroupId = 1,
                Name = "Автомобили",
                Url = "http://k.img.com.ua/rss/ru/motors.xml"
            });

            modelBuilder.Entity<Source>().HasData(new Source
            {
                Id = 3,
                GroupId = 1,
                Name = "Новости кино",
                Url = "http://k.img.com.ua/rss/ru/cinema.xml"
            });

            modelBuilder.Entity<Source>().HasData(new Source
            {
                Id = 4,
                GroupId = 1,
                Name = "Технологии",
                Url = "http://k.img.com.ua/rss/ru/technews.xml"
            });

            modelBuilder.Entity<Source>().HasData(new Source
            {
                Id = 5,
                GroupId = 1,
                Name = "Космос",
                Url = "http://k.img.com.ua/rss/ru/space.xml"
            });

            modelBuilder.Entity<Source>().HasData(new Source
            {
                Id = 6,
                GroupId = 2,
                Name = "HABR. Все публикации в потоке Разработка",
                Url = "https://habr.com/ru/rss/flows/develop/all/?fl=rul"
            });

            modelBuilder.Entity<Source>().HasData(new Source
            {
                Id = 7,
                GroupId = 3,
                Name = "NEWSru.com :: Мнения",
                Url = "https://rss.newsru.com/blog"
            });

            modelBuilder.Entity<Source>().HasData(new Source
            {
                Id = 8,
                GroupId = 4,
                Name = "Meduza: Новости",
                Url = "https://meduza.io/rss/news"
            });

            modelBuilder.Entity<Source>().HasData(new Source
            {
                Id = 10,
                GroupId = 4,
                Name = "AUTO News",
                Url = "https://autonews.autoua.net/rss/"
            });
            modelBuilder.Entity<UserGroup>().HasData(new UserGroup { Id = 1, Name = "My Group"});

            modelBuilder.Entity<Subscription>().HasData(new Subscription { Id = 1, SourceId = 1, UserId = 1, UserGroupId = 1 });
            modelBuilder.Entity<Subscription>().HasData(new Subscription { Id = 2, SourceId = 2, UserId = 1, UserGroupId = 1 });
            modelBuilder.Entity<Subscription>().HasData(new Subscription { Id = 3, SourceId = 6, UserId = 1 });
            modelBuilder.Entity<Subscription>().HasData(new Subscription { Id = 4, SourceId = 7, UserId = 1 });
        }

    }
}
