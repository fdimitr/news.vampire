﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using News.Vampire.Service.DataAccess;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace News.Vampire.Service.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("News.Vampire.Service.Models.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.HasKey("Id");

                    b.ToTable("Groups");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsActive = true,
                            Name = "Корреспондент"
                        },
                        new
                        {
                            Id = 2,
                            IsActive = true,
                            Name = "Habr"
                        },
                        new
                        {
                            Id = 3,
                            IsActive = true,
                            Name = "News.RU"
                        },
                        new
                        {
                            Id = 4,
                            IsActive = true,
                            Name = "Новости"
                        });
                });

            modelBuilder.Entity("News.Vampire.Service.Models.NewsItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<List<string>>("Author")
                        .HasMaxLength(256)
                        .HasColumnType("jsonb");

                    b.Property<List<string>>("Category")
                        .HasMaxLength(512)
                        .HasColumnType("jsonb");

                    b.Property<string>("Description")
                        .HasMaxLength(4096)
                        .HasColumnType("character varying(4096)");

                    b.Property<string>("ExternalId")
                        .HasColumnType("text");

                    b.Property<long>("PublicationDate")
                        .HasColumnType("bigint");

                    b.Property<int>("SourceId")
                        .HasColumnType("integer");

                    b.Property<long>("TimeStamp")
                        .HasColumnType("bigint");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<List<string>>("Url")
                        .HasMaxLength(2048)
                        .HasColumnType("jsonb");

                    b.HasKey("Id");

                    b.HasIndex("SourceId");

                    b.ToTable("NewsItem");
                });

            modelBuilder.Entity("News.Vampire.Service.Models.Source", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<int>("GroupId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<long>("NextLoadedTime")
                        .HasColumnType("bigint");

                    b.Property<int>("Sort")
                        .HasColumnType("integer");

                    b.Property<int>("UpdateFrequencyMinutes")
                        .HasColumnType("integer");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("Sources");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            GroupId = 1,
                            Name = "Последние новости по разделу События в Украине",
                            NextLoadedTime = 0L,
                            Sort = 0,
                            UpdateFrequencyMinutes = 0,
                            Url = "http://k.img.com.ua/rss/ru/events.xml"
                        },
                        new
                        {
                            Id = 2,
                            GroupId = 1,
                            Name = "Автомобили",
                            NextLoadedTime = 0L,
                            Sort = 0,
                            UpdateFrequencyMinutes = 0,
                            Url = "http://k.img.com.ua/rss/ru/motors.xml"
                        },
                        new
                        {
                            Id = 3,
                            GroupId = 1,
                            Name = "Новости кино",
                            NextLoadedTime = 0L,
                            Sort = 0,
                            UpdateFrequencyMinutes = 0,
                            Url = "http://k.img.com.ua/rss/ru/cinema.xml"
                        },
                        new
                        {
                            Id = 4,
                            GroupId = 1,
                            Name = "Технологии",
                            NextLoadedTime = 0L,
                            Sort = 0,
                            UpdateFrequencyMinutes = 0,
                            Url = "http://k.img.com.ua/rss/ru/technews.xml"
                        },
                        new
                        {
                            Id = 5,
                            GroupId = 1,
                            Name = "Космос",
                            NextLoadedTime = 0L,
                            Sort = 0,
                            UpdateFrequencyMinutes = 0,
                            Url = "http://k.img.com.ua/rss/ru/space.xml"
                        },
                        new
                        {
                            Id = 6,
                            GroupId = 2,
                            Name = "HABR. Все публикации в потоке Разработка",
                            NextLoadedTime = 0L,
                            Sort = 0,
                            UpdateFrequencyMinutes = 0,
                            Url = "https://habr.com/ru/rss/flows/develop/all/?fl=rul"
                        },
                        new
                        {
                            Id = 8,
                            GroupId = 4,
                            Name = "Meduza: Новости",
                            NextLoadedTime = 0L,
                            Sort = 0,
                            UpdateFrequencyMinutes = 0,
                            Url = "https://meduza.io/rss/news"
                        },
                        new
                        {
                            Id = 10,
                            GroupId = 4,
                            Name = "AUTO News",
                            NextLoadedTime = 0L,
                            Sort = 0,
                            UpdateFrequencyMinutes = 0,
                            Url = "https://autonews.autoua.net/rss/"
                        });
                });

            modelBuilder.Entity("News.Vampire.Service.Models.Subscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<long?>("LastLoadedTime")
                        .HasColumnType("bigint");

                    b.Property<int>("SourceId")
                        .HasColumnType("integer");

                    b.Property<int>("UserGroupId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SourceId");

                    b.HasIndex("UserGroupId");

                    b.HasIndex("UserId");

                    b.ToTable("Subscriptions");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            SourceId = 1,
                            UserGroupId = 1,
                            UserId = 1
                        },
                        new
                        {
                            Id = 3,
                            SourceId = 6,
                            UserGroupId = 1,
                            UserId = 1
                        },
                        new
                        {
                            Id = 2,
                            SourceId = 8,
                            UserGroupId = 1,
                            UserId = 1
                        });
                });

            modelBuilder.Entity("News.Vampire.Service.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("character varying(16)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Login = "Dmitry",
                            Password = "6QvNlj77zDchLwiTrY/b/o28Cg3vvwwO7IkZrh5BqaA=",
                            Role = "admin"
                        });
                });

            modelBuilder.Entity("News.Vampire.Service.Models.UserGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("UserGroups");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "My Group",
                            UserId = 1
                        });
                });

            modelBuilder.Entity("News.Vampire.Service.Models.NewsItem", b =>
                {
                    b.HasOne("News.Vampire.Service.Models.Source", "Source")
                        .WithMany("News")
                        .HasForeignKey("SourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Source");
                });

            modelBuilder.Entity("News.Vampire.Service.Models.Source", b =>
                {
                    b.HasOne("News.Vampire.Service.Models.Group", "Group")
                        .WithMany("Sources")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");
                });

            modelBuilder.Entity("News.Vampire.Service.Models.Subscription", b =>
                {
                    b.HasOne("News.Vampire.Service.Models.Source", "Source")
                        .WithMany()
                        .HasForeignKey("SourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("News.Vampire.Service.Models.UserGroup", "UserGroup")
                        .WithMany("Subscriptions")
                        .HasForeignKey("UserGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("News.Vampire.Service.Models.User", "User")
                        .WithMany("Subscriptions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Source");

                    b.Navigation("User");

                    b.Navigation("UserGroup");
                });

            modelBuilder.Entity("News.Vampire.Service.Models.Group", b =>
                {
                    b.Navigation("Sources");
                });

            modelBuilder.Entity("News.Vampire.Service.Models.Source", b =>
                {
                    b.Navigation("News");
                });

            modelBuilder.Entity("News.Vampire.Service.Models.User", b =>
                {
                    b.Navigation("Subscriptions");
                });

            modelBuilder.Entity("News.Vampire.Service.Models.UserGroup", b =>
                {
                    b.Navigation("Subscriptions");
                });
#pragma warning restore 612, 618
        }
    }
}
