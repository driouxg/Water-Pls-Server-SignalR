﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SignalRTest.DataAccess;

namespace SignalRTest.Migrations
{
    [DbContext(typeof(WaterDbContext))]
    [Migration("20190215233027_AddedIdentityRolesDbSettoDB")]
    partial class AddedIdentityRolesDbSettoDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedName");

                    b.HasKey("Id");

                    b.ToTable("IdentityRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.ToTable("IdentityUserClaims");
                });

            modelBuilder.Entity("SignalRTest.Domain.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Email");

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail");

                    b.Property<string>("NormalizedUserName");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName");

                    b.Property<int?>("userLoginId");

                    b.HasKey("Id");

                    b.HasIndex("userLoginId");

                    b.ToTable("IdManagementUsers");
                });

            modelBuilder.Entity("SignalRTest.Domain.Dto.AddressDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("cityName");

                    b.Property<string>("route");

                    b.Property<string>("stateName");

                    b.Property<string>("streetName");

                    b.Property<int>("streetNumber");

                    b.Property<int>("zipcode");

                    b.HasKey("Id");

                    b.ToTable("AddressDto");
                });

            modelBuilder.Entity("SignalRTest.Domain.Dto.GeoCoordinatesDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("latitude");

                    b.Property<double>("longitude");

                    b.HasKey("Id");

                    b.ToTable("GeoCoordinatesDto");
                });

            modelBuilder.Entity("SignalRTest.Domain.Dto.UserDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Username")
                        .IsRequired();

                    b.Property<int?>("addressDtoId");

                    b.Property<string>("clientConnection");

                    b.Property<string>("connectionStatus");

                    b.Property<string>("email");

                    b.Property<string>("firstName");

                    b.Property<int?>("geoCoordinatesDtoId");

                    b.Property<string>("lastName");

                    b.HasKey("Id");

                    b.HasIndex("addressDtoId");

                    b.HasIndex("geoCoordinatesDtoId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SignalRTest.Domain.Dto.UserLoginDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("email")
                        .IsRequired();

                    b.Property<string>("password")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<bool>("rememberMe");

                    b.Property<string>("username")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("UserLoginDto");
                });

            modelBuilder.Entity("SignalRTest.Domain.ApplicationUser", b =>
                {
                    b.HasOne("SignalRTest.Domain.Dto.UserLoginDto", "userLogin")
                        .WithMany()
                        .HasForeignKey("userLoginId");
                });

            modelBuilder.Entity("SignalRTest.Domain.Dto.UserDto", b =>
                {
                    b.HasOne("SignalRTest.Domain.Dto.AddressDto", "addressDto")
                        .WithMany()
                        .HasForeignKey("addressDtoId");

                    b.HasOne("SignalRTest.Domain.Dto.GeoCoordinatesDto", "geoCoordinatesDto")
                        .WithMany()
                        .HasForeignKey("geoCoordinatesDtoId");
                });
#pragma warning restore 612, 618
        }
    }
}