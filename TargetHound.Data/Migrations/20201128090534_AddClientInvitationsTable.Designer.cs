﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TargetHound.Data;

namespace TargetHound.Data.Migrations
{
    [DbContext(typeof(TargetHoundContext))]
    [Migration("20201128090534_AddClientInvitationsTable")]
    partial class AddClientInvitationsTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("TargetHound.DataModels.ApplicationRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("TargetHound.DataModels.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ClientId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ClientId1")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("ClientId")
                        .IsUnique()
                        .HasFilter("[ClientId] IS NOT NULL");

                    b.HasIndex("ClientId1");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("TargetHound.DataModels.Borehole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CollarId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CollarId1")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ContractorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProjectId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TargetId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CollarId");

                    b.HasIndex("CollarId1");

                    b.HasIndex("ContractorId");

                    b.HasIndex("ProjectId");

                    b.ToTable("Boreholes");
                });

            modelBuilder.Entity("TargetHound.DataModels.Client", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AdminId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("TargetHound.DataModels.ClientContractor", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CientId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ContractorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("CientId");

                    b.HasIndex("ContractorId");

                    b.ToTable("ClientsContractors");
                });

            modelBuilder.Entity("TargetHound.DataModels.ClientInvitation", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ClientId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("ClientInvitations");
                });

            modelBuilder.Entity("TargetHound.DataModels.Collar", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Azimuth")
                        .HasColumnType("float");

                    b.Property<double>("Depth")
                        .HasColumnType("float");

                    b.Property<double>("Dip")
                        .HasColumnType("float");

                    b.Property<double>("Easting")
                        .HasColumnType("float");

                    b.Property<double>("Elevation")
                        .HasColumnType("float");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<double>("Northing")
                        .HasColumnType("float");

                    b.Property<string>("ProjectId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Collars");
                });

            modelBuilder.Entity("TargetHound.DataModels.Contractor", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("Id");

                    b.ToTable("Contractors");
                });

            modelBuilder.Entity("TargetHound.DataModels.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("TargetHound.DataModels.Dogleg", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BoreholeId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("DoglegAngle")
                        .HasColumnType("float");

                    b.Property<double>("DoglegSeverity")
                        .HasColumnType("float");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<double>("RatioFactor")
                        .HasColumnType("float");

                    b.Property<double>("ToolFace")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("BoreholeId");

                    b.ToTable("Doglegs");
                });

            modelBuilder.Entity("TargetHound.DataModels.DrillRig", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ContractorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("ContractorId");

                    b.ToTable("DrillRigs");
                });

            modelBuilder.Entity("TargetHound.DataModels.Project", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AdminId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ClientId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("CountryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<double>("MagneticDeclination")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.HasIndex("ClientId");

                    b.HasIndex("CountryId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("TargetHound.DataModels.ProjectContractor", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ContractorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ProjectId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ContractorId");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectsContractors");
                });

            modelBuilder.Entity("TargetHound.DataModels.SurveyPoint", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Azimuth")
                        .HasColumnType("float");

                    b.Property<string>("BoreholeId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BoreholeId1")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Depth")
                        .HasColumnType("float");

                    b.Property<double>("Dip")
                        .HasColumnType("float");

                    b.Property<double>("Easting")
                        .HasColumnType("float");

                    b.Property<double>("Elevation")
                        .HasColumnType("float");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int?>("MagneticField")
                        .HasColumnType("int");

                    b.Property<double>("Northing")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("BoreholeId");

                    b.HasIndex("BoreholeId1");

                    b.ToTable("SurveyPoints");
                });

            modelBuilder.Entity("TargetHound.DataModels.Target", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Azimuth")
                        .HasColumnType("float");

                    b.Property<string>("BoreholeId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Depth")
                        .HasColumnType("float");

                    b.Property<double>("Dip")
                        .HasColumnType("float");

                    b.Property<double>("Easting")
                        .HasColumnType("float");

                    b.Property<double>("Elevation")
                        .HasColumnType("float");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<double>("Northing")
                        .HasColumnType("float");

                    b.Property<string>("ProjectId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("BoreholeId")
                        .IsUnique()
                        .HasFilter("[BoreholeId] IS NOT NULL");

                    b.HasIndex("ProjectId");

                    b.ToTable("Targets");
                });

            modelBuilder.Entity("TargetHound.DataModels.UserProject", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<string>("ProjectId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("ProjectId");

                    b.ToTable("UsersProjects");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("TargetHound.DataModels.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("TargetHound.DataModels.ApplicationUser", null)
                        .WithMany("Claims")
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TargetHound.DataModels.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("TargetHound.DataModels.ApplicationUser", null)
                        .WithMany("Logins")
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TargetHound.DataModels.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("TargetHound.DataModels.ApplicationUser", null)
                        .WithMany("Roles")
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TargetHound.DataModels.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TargetHound.DataModels.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("TargetHound.DataModels.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TargetHound.DataModels.ApplicationUser", b =>
                {
                    b.HasOne("TargetHound.DataModels.Client", "Client")
                        .WithOne("Admin")
                        .HasForeignKey("TargetHound.DataModels.ApplicationUser", "ClientId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("TargetHound.DataModels.Client", null)
                        .WithMany("Users")
                        .HasForeignKey("ClientId1");

                    b.Navigation("Client");
                });

            modelBuilder.Entity("TargetHound.DataModels.Borehole", b =>
                {
                    b.HasOne("TargetHound.DataModels.Collar", "Collar")
                        .WithMany()
                        .HasForeignKey("CollarId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("TargetHound.DataModels.Collar", null)
                        .WithMany("Boreholes")
                        .HasForeignKey("CollarId1");

                    b.HasOne("TargetHound.DataModels.Contractor", "Contractor")
                        .WithMany("Boreholes")
                        .HasForeignKey("ContractorId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TargetHound.DataModels.Project", "Project")
                        .WithMany("Boreholes")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Collar");

                    b.Navigation("Contractor");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("TargetHound.DataModels.ClientContractor", b =>
                {
                    b.HasOne("TargetHound.DataModels.Client", "Client")
                        .WithMany("ClientContractors")
                        .HasForeignKey("CientId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TargetHound.DataModels.Contractor", "Contractor")
                        .WithMany("ClientContractors")
                        .HasForeignKey("ContractorId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Client");

                    b.Navigation("Contractor");
                });

            modelBuilder.Entity("TargetHound.DataModels.ClientInvitation", b =>
                {
                    b.HasOne("TargetHound.DataModels.Client", "Client")
                        .WithMany("ClientInvitations")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Client");
                });

            modelBuilder.Entity("TargetHound.DataModels.Collar", b =>
                {
                    b.HasOne("TargetHound.DataModels.Project", "Project")
                        .WithMany("Collars")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Project");
                });

            modelBuilder.Entity("TargetHound.DataModels.Dogleg", b =>
                {
                    b.HasOne("TargetHound.DataModels.Borehole", "Borehole")
                        .WithMany("Doglegs")
                        .HasForeignKey("BoreholeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Borehole");
                });

            modelBuilder.Entity("TargetHound.DataModels.DrillRig", b =>
                {
                    b.HasOne("TargetHound.DataModels.Contractor", "Contractor")
                        .WithMany("DrillRigs")
                        .HasForeignKey("ContractorId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Contractor");
                });

            modelBuilder.Entity("TargetHound.DataModels.Project", b =>
                {
                    b.HasOne("TargetHound.DataModels.ApplicationUser", "Admin")
                        .WithMany()
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TargetHound.DataModels.Client", "Client")
                        .WithMany("Projects")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TargetHound.DataModels.Country", "Country")
                        .WithMany("Projects")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Admin");

                    b.Navigation("Client");

                    b.Navigation("Country");
                });

            modelBuilder.Entity("TargetHound.DataModels.ProjectContractor", b =>
                {
                    b.HasOne("TargetHound.DataModels.Contractor", "Contractor")
                        .WithMany()
                        .HasForeignKey("ContractorId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TargetHound.DataModels.Project", "Project")
                        .WithMany("ProjectContractors")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Contractor");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("TargetHound.DataModels.SurveyPoint", b =>
                {
                    b.HasOne("TargetHound.DataModels.Borehole", "Borehole")
                        .WithMany()
                        .HasForeignKey("BoreholeId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("TargetHound.DataModels.Borehole", null)
                        .WithMany("SurveyPoints")
                        .HasForeignKey("BoreholeId1");

                    b.Navigation("Borehole");
                });

            modelBuilder.Entity("TargetHound.DataModels.Target", b =>
                {
                    b.HasOne("TargetHound.DataModels.Borehole", "Borehole")
                        .WithOne("Targets")
                        .HasForeignKey("TargetHound.DataModels.Target", "BoreholeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TargetHound.DataModels.Project", "Project")
                        .WithMany("Targets")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Borehole");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("TargetHound.DataModels.UserProject", b =>
                {
                    b.HasOne("TargetHound.DataModels.ApplicationUser", "ApplicationUser")
                        .WithMany("UserProjects")
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TargetHound.DataModels.Project", "Project")
                        .WithMany("ProjectUsers")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("ApplicationUser");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("TargetHound.DataModels.ApplicationUser", b =>
                {
                    b.Navigation("Claims");

                    b.Navigation("Logins");

                    b.Navigation("Roles");

                    b.Navigation("UserProjects");
                });

            modelBuilder.Entity("TargetHound.DataModels.Borehole", b =>
                {
                    b.Navigation("Doglegs");

                    b.Navigation("SurveyPoints");

                    b.Navigation("Targets");
                });

            modelBuilder.Entity("TargetHound.DataModels.Client", b =>
                {
                    b.Navigation("Admin");

                    b.Navigation("ClientContractors");

                    b.Navigation("ClientInvitations");

                    b.Navigation("Projects");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("TargetHound.DataModels.Collar", b =>
                {
                    b.Navigation("Boreholes");
                });

            modelBuilder.Entity("TargetHound.DataModels.Contractor", b =>
                {
                    b.Navigation("Boreholes");

                    b.Navigation("ClientContractors");

                    b.Navigation("DrillRigs");
                });

            modelBuilder.Entity("TargetHound.DataModels.Country", b =>
                {
                    b.Navigation("Projects");
                });

            modelBuilder.Entity("TargetHound.DataModels.Project", b =>
                {
                    b.Navigation("Boreholes");

                    b.Navigation("Collars");

                    b.Navigation("ProjectContractors");

                    b.Navigation("ProjectUsers");

                    b.Navigation("Targets");
                });
#pragma warning restore 612, 618
        }
    }
}
