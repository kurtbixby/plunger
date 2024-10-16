﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Plunger.Data;

#nullable disable

namespace Plunger.Data.Migrations
{
    [DbContext(typeof(PlungerDbContext))]
    [Migration("20240801223235_NewStart")]
    partial class NewStart
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GamePlatform", b =>
                {
                    b.Property<int>("GameId")
                        .HasColumnType("integer");

                    b.Property<int>("PlatformsId")
                        .HasColumnType("integer");

                    b.HasKey("GameId", "PlatformsId");

                    b.HasIndex("PlatformsId");

                    b.ToTable("GamePlatform");
                });

            modelBuilder.Entity("GameRegion", b =>
                {
                    b.Property<int>("GameId")
                        .HasColumnType("integer");

                    b.Property<int>("RegionsId")
                        .HasColumnType("integer");

                    b.HasKey("GameId", "RegionsId");

                    b.HasIndex("RegionsId");

                    b.ToTable("GameRegion");
                });

            modelBuilder.Entity("Plunger.Data.DbModels.Collection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Collections");
                });

            modelBuilder.Entity("Plunger.Data.DbModels.CollectionGame", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CollectionId")
                        .HasColumnType("integer");

                    b.Property<int>("GameId")
                        .HasColumnType("integer");

                    b.Property<int>("Physicality")
                        .HasColumnType("integer");

                    b.Property<int>("PlatformId")
                        .HasColumnType("integer");

                    b.Property<int>("Region")
                        .HasColumnType("integer");

                    b.Property<int>("RegionId")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset?>("TimeAcquired")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("TimeAdded")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("VersionId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CollectionId");

                    b.HasIndex("GameId");

                    b.HasIndex("PlatformId");

                    b.ToTable("CollectionGames");
                });

            modelBuilder.Entity("Plunger.Data.DbModels.Cover", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<Guid>("Checksum")
                        .HasColumnType("uuid");

                    b.Property<int>("GameId")
                        .HasColumnType("integer");

                    b.Property<int>("Height")
                        .HasColumnType("integer");

                    b.Property<string>("ImageId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Width")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("GameId")
                        .IsUnique();

                    b.ToTable("Covers");
                });

            modelBuilder.Entity("Plunger.Data.DbModels.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<Guid>("Checksum")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("FirstReleased")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Plunger.Data.DbModels.GameList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Unordered")
                        .HasColumnType("boolean");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<Guid>("VersionId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("GameLists");
                });

            modelBuilder.Entity("Plunger.Data.DbModels.GameListEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("GameId")
                        .HasColumnType("integer");

                    b.Property<int>("GameListId")
                        .HasColumnType("integer");

                    b.Property<int>("Number")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("GameListId");

                    b.ToTable("GameListEntries");
                });

            modelBuilder.Entity("Plunger.Data.DbModels.GameStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Completed")
                        .HasColumnType("boolean");

                    b.Property<int>("GameId")
                        .HasColumnType("integer");

                    b.Property<int>("PlayState")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("UserId");

                    b.ToTable("GameStatuses");
                });

            modelBuilder.Entity("Plunger.Data.DbModels.Platform", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Abbreviation")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("AltName")
                        .HasColumnType("text");

                    b.Property<Guid>("Checksum")
                        .HasColumnType("uuid");

                    b.Property<int?>("Generation")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Platforms");
                });

            modelBuilder.Entity("Plunger.Data.DbModels.PlayStateChange", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("GameStatusId")
                        .HasColumnType("integer");

                    b.Property<int>("NewState")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("GameStatusId");

                    b.ToTable("PlayStateChanges");
                });

            modelBuilder.Entity("Plunger.Data.DbModels.Region", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Regions");
                });

            modelBuilder.Entity("Plunger.Data.DbModels.ReleaseDate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<Guid>("Checksum")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("DateFormat")
                        .HasColumnType("integer");

                    b.Property<int?>("GameId")
                        .HasColumnType("integer");

                    b.Property<int>("PlatformId")
                        .HasColumnType("integer");

                    b.Property<int>("Region")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("PlatformId");

                    b.ToTable("ReleaseDate");
                });

            modelBuilder.Entity("Plunger.Data.DbModels.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GamePlatform", b =>
                {
                    b.HasOne("Plunger.Data.DbModels.Game", null)
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Plunger.Data.DbModels.Platform", null)
                        .WithMany()
                        .HasForeignKey("PlatformsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GameRegion", b =>
                {
                    b.HasOne("Plunger.Data.DbModels.Game", null)
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Plunger.Data.DbModels.Region", null)
                        .WithMany()
                        .HasForeignKey("RegionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Plunger.Data.DbModels.Collection", b =>
                {
                    b.HasOne("Plunger.Data.DbModels.User", "User")
                        .WithOne("Collection")
                        .HasForeignKey("Plunger.Data.DbModels.Collection", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Plunger.Data.DbModels.CollectionGame", b =>
                {
                    b.HasOne("Plunger.Data.DbModels.Collection", "Collection")
                        .WithMany("Games")
                        .HasForeignKey("CollectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Plunger.Data.DbModels.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Plunger.Data.DbModels.Platform", "Platform")
                        .WithMany()
                        .HasForeignKey("PlatformId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Collection");

                    b.Navigation("Game");

                    b.Navigation("Platform");
                });

            modelBuilder.Entity("Plunger.Data.DbModels.Cover", b =>
                {
                    b.HasOne("Plunger.Data.DbModels.Game", "Game")
                        .WithOne("Cover")
                        .HasForeignKey("Plunger.Data.DbModels.Cover", "GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("Plunger.Data.DbModels.GameList", b =>
                {
                    b.HasOne("Plunger.Data.DbModels.User", "User")
                        .WithMany("GameLists")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Plunger.Data.DbModels.GameListEntry", b =>
                {
                    b.HasOne("Plunger.Data.DbModels.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Plunger.Data.DbModels.GameList", "GameList")
                        .WithMany("GameListEntries")
                        .HasForeignKey("GameListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("GameList");
                });

            modelBuilder.Entity("Plunger.Data.DbModels.GameStatus", b =>
                {
                    b.HasOne("Plunger.Data.DbModels.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Plunger.Data.DbModels.User", "User")
                        .WithMany("GameStatuses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Plunger.Data.DbModels.PlayStateChange", b =>
                {
                    b.HasOne("Plunger.Data.DbModels.GameStatus", "GameStatus")
                        .WithMany("PlayStateChanges")
                        .HasForeignKey("GameStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GameStatus");
                });

            modelBuilder.Entity("Plunger.Data.DbModels.ReleaseDate", b =>
                {
                    b.HasOne("Plunger.Data.DbModels.Game", null)
                        .WithMany("ReleaseDates")
                        .HasForeignKey("GameId");

                    b.HasOne("Plunger.Data.DbModels.Platform", "Platform")
                        .WithMany()
                        .HasForeignKey("PlatformId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Platform");
                });

            modelBuilder.Entity("Plunger.Data.DbModels.Collection", b =>
                {
                    b.Navigation("Games");
                });

            modelBuilder.Entity("Plunger.Data.DbModels.Game", b =>
                {
                    b.Navigation("Cover");

                    b.Navigation("ReleaseDates");
                });

            modelBuilder.Entity("Plunger.Data.DbModels.GameList", b =>
                {
                    b.Navigation("GameListEntries");
                });

            modelBuilder.Entity("Plunger.Data.DbModels.GameStatus", b =>
                {
                    b.Navigation("PlayStateChanges");
                });

            modelBuilder.Entity("Plunger.Data.DbModels.User", b =>
                {
                    b.Navigation("Collection")
                        .IsRequired();

                    b.Navigation("GameLists");

                    b.Navigation("GameStatuses");
                });
#pragma warning restore 612, 618
        }
    }
}