﻿// <auto-generated />
using FUTWatcher.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace FUTWatcher.API.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20171110225035_DailyProfits")]
    partial class DailyProfits
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("FUTWatcher.API.Models.Club", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("LeagueId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("LeagueId");

                    b.ToTable("Clubs");
                });

            modelBuilder.Entity("FUTWatcher.API.Models.DailyProfit", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<long>("Profit");

                    b.HasKey("Id");

                    b.ToTable("DailyProfits");
                });

            modelBuilder.Entity("FUTWatcher.API.Models.League", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Leagues");
                });

            modelBuilder.Entity("FUTWatcher.API.Models.Nation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Nations");
                });

            modelBuilder.Entity("FUTWatcher.API.Models.PlayerType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CommonName");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.HasKey("Id");

                    b.ToTable("PlayerTypes");
                });

            modelBuilder.Entity("FUTWatcher.API.Models.PlayerVersion", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("ClubId");

                    b.Property<string>("Color");

                    b.Property<long>("Cost");

                    b.Property<bool>("IsSpecialType");

                    b.Property<long>("LeagueId");

                    b.Property<long?>("NationId");

                    b.Property<bool>("OwnedByMe");

                    b.Property<long>("PlayerTypeId");

                    b.Property<string>("Position");

                    b.Property<string>("Quality");

                    b.Property<long>("Rating");

                    b.HasKey("Id");

                    b.HasIndex("ClubId");

                    b.HasIndex("LeagueId");

                    b.HasIndex("NationId");

                    b.HasIndex("PlayerTypeId");

                    b.ToTable("PlayerVersions");
                });

            modelBuilder.Entity("FUTWatcher.API.Models.Club", b =>
                {
                    b.HasOne("FUTWatcher.API.Models.League", "League")
                        .WithMany()
                        .HasForeignKey("LeagueId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FUTWatcher.API.Models.PlayerVersion", b =>
                {
                    b.HasOne("FUTWatcher.API.Models.Club", "Club")
                        .WithMany("PlayerVersions")
                        .HasForeignKey("ClubId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FUTWatcher.API.Models.League", "League")
                        .WithMany("PlayerVersions")
                        .HasForeignKey("LeagueId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FUTWatcher.API.Models.Nation")
                        .WithMany("PlayerVersions")
                        .HasForeignKey("NationId");

                    b.HasOne("FUTWatcher.API.Models.PlayerType", "PlayerType")
                        .WithMany()
                        .HasForeignKey("PlayerTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
