﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using tes3mp_verifier.Data;

namespace tes3mp_verifier.Data.Migrations
{
    [DbContext(typeof(VerifierContext))]
    [Migration("20201106152426_UserSearchNicknameAssign")]
    partial class UserSearchNicknameAssign
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("tes3mp_verifier.Data.Models.ApiKey", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("text");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("GameServerId")
                        .HasColumnType("integer");

                    b.HasKey("Key");

                    b.HasIndex("GameServerId")
                        .IsUnique();

                    b.ToTable("ApiKeys");
                });

            modelBuilder.Entity("tes3mp_verifier.Data.Models.GameServer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("OwnerId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("GameServers");
                });

            modelBuilder.Entity("tes3mp_verifier.Data.Models.Login", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("GameServerId")
                        .HasColumnType("integer");

                    b.Property<string>("Ip")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("GameServerId");

                    b.HasIndex("UserId");

                    b.ToTable("Logins");
                });

            modelBuilder.Entity("tes3mp_verifier.Data.Models.LoginKey", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Key", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("LoginKeys");
                });

            modelBuilder.Entity("tes3mp_verifier.Data.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<string>("SearchNickname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Nickname")
                        .IsUnique();

                    b.HasIndex("PhoneNumber")
                        .IsUnique();

                    b.HasIndex("SearchNickname")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("tes3mp_verifier.Data.Models.UserSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UserSettings");
                });

            modelBuilder.Entity("tes3mp_verifier.Data.Models.Verification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("Confirmed")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Verifications");
                });

            modelBuilder.Entity("tes3mp_verifier.Data.Models.ApiKey", b =>
                {
                    b.HasOne("tes3mp_verifier.Data.Models.GameServer", "GameServer")
                        .WithOne("ApiKey")
                        .HasForeignKey("tes3mp_verifier.Data.Models.ApiKey", "GameServerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("tes3mp_verifier.Data.Models.GameServer", b =>
                {
                    b.HasOne("tes3mp_verifier.Data.Models.User", "Owner")
                        .WithMany("GameServers")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("tes3mp_verifier.Data.Models.Login", b =>
                {
                    b.HasOne("tes3mp_verifier.Data.Models.GameServer", "GameServer")
                        .WithMany("Logins")
                        .HasForeignKey("GameServerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("tes3mp_verifier.Data.Models.User", "User")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("tes3mp_verifier.Data.Models.LoginKey", b =>
                {
                    b.HasOne("tes3mp_verifier.Data.Models.User", "User")
                        .WithMany("LoginKeys")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("tes3mp_verifier.Data.Models.UserSettings", b =>
                {
                    b.HasOne("tes3mp_verifier.Data.Models.User", "User")
                        .WithOne("Settings")
                        .HasForeignKey("tes3mp_verifier.Data.Models.UserSettings", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("tes3mp_verifier.Data.Models.Verification", b =>
                {
                    b.HasOne("tes3mp_verifier.Data.Models.User", "User")
                        .WithOne("Verification")
                        .HasForeignKey("tes3mp_verifier.Data.Models.Verification", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
