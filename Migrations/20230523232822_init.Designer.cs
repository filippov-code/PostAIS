﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PostAIS.Database;

#nullable disable

namespace PostAIS.Migrations
{
    [DbContext(typeof(PostAisDbContext))]
    [Migration("20230523232822_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PostAIS.Models.PackageToReceive", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("PackageType")
                        .HasColumnType("int");

                    b.Property<string>("ReceiverTelephoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SenderAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SenderFio")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PackagesToReceive");
                });

            modelBuilder.Entity("PostAIS.Models.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("PostAIS.Models.RegistrationCode", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Code")
                        .HasColumnType("int");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("RegistrationCodes");
                });

            modelBuilder.Entity("PostAIS.Models.Service", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<int>("Code")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OperationType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("Services");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Service");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("PostAIS.Models.ShoppingCartItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ProductPurchaseServiceId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("ProductPurchaseServiceId");

                    b.ToTable("ShoppingCartItem");
                });

            modelBuilder.Entity("PostAIS.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MiddleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PassportNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PassportSeries")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RegisteringEmployeeId")
                        .HasColumnType("int");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("TelephoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RegisteringEmployeeId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PostAIS.Models.ProductPurchaseService", b =>
                {
                    b.HasBaseType("PostAIS.Models.Service");

                    b.HasDiscriminator().HasValue("ProductPurchaseService");
                });

            modelBuilder.Entity("PostAIS.Models.ReceivePackageService", b =>
                {
                    b.HasBaseType("PostAIS.Models.Service");

                    b.Property<Guid>("PackageToReceiveId")
                        .HasColumnType("uniqueidentifier");

                    b.HasIndex("PackageToReceiveId");

                    b.HasDiscriminator().HasValue("ReceivePackageService");
                });

            modelBuilder.Entity("PostAIS.Models.SendPackageService", b =>
                {
                    b.HasBaseType("PostAIS.Models.Service");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Cost")
                        .HasColumnType("float");

                    b.Property<int>("DeliveryType")
                        .HasColumnType("int");

                    b.Property<double>("Height")
                        .HasColumnType("float");

                    b.Property<double>("Length")
                        .HasColumnType("float");

                    b.Property<int>("PackageType")
                        .HasColumnType("int");

                    b.Property<string>("RecipientFIO")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Weight")
                        .HasColumnType("float");

                    b.Property<double>("Width")
                        .HasColumnType("float");

                    b.HasDiscriminator().HasValue("SendPackageService");
                });

            modelBuilder.Entity("PostAIS.Models.RegistrationCode", b =>
                {
                    b.HasOne("PostAIS.Models.User", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("PostAIS.Models.Service", b =>
                {
                    b.HasOne("PostAIS.Models.User", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("PostAIS.Models.ShoppingCartItem", b =>
                {
                    b.HasOne("PostAIS.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PostAIS.Models.ProductPurchaseService", null)
                        .WithMany("ShoppingCart")
                        .HasForeignKey("ProductPurchaseServiceId");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("PostAIS.Models.User", b =>
                {
                    b.HasOne("PostAIS.Models.User", "RegisteringEmployee")
                        .WithMany()
                        .HasForeignKey("RegisteringEmployeeId");

                    b.Navigation("RegisteringEmployee");
                });

            modelBuilder.Entity("PostAIS.Models.ReceivePackageService", b =>
                {
                    b.HasOne("PostAIS.Models.PackageToReceive", "PackageToReceive")
                        .WithMany()
                        .HasForeignKey("PackageToReceiveId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PackageToReceive");
                });

            modelBuilder.Entity("PostAIS.Models.ProductPurchaseService", b =>
                {
                    b.Navigation("ShoppingCart");
                });
#pragma warning restore 612, 618
        }
    }
}
