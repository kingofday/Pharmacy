﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Pharmacy.Notifier.DataAccess.Ef;

namespace Pharmacy.Notifier.DataAccess.Ef.Migrations
{
    [DbContext(typeof(NotifierDbContext))]
    [Migration("20200513065951_AddApplicationIdToEventMapper")]
    partial class AddApplicationIdToEventMapper
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Pharmacy.Domain.Application", b =>
                {
                    b.Property<int>("ApplicationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("InsertDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("InsertDateSh")
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<Guid>("Token")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ValidIp")
                        .IsRequired()
                        .HasColumnType("varchar(150)")
                        .HasMaxLength(150);

                    b.HasKey("ApplicationId");

                    b.ToTable("Application","Notifier");
                });

            modelBuilder.Entity("Pharmacy.Domain.EventMapper", b =>
                {
                    b.Property<int>("EventMapperId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ApplicationId")
                        .HasColumnType("int");

                    b.Property<DateTime>("InsertDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("InsertDateSh")
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<string>("NotifyStrategy")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100);

                    b.Property<byte>("Type")
                        .HasColumnType("tinyint");

                    b.HasKey("EventMapperId");

                    b.HasIndex("ApplicationId");

                    b.ToTable("EventMapper","Notifier");
                });

            modelBuilder.Entity("Pharmacy.Domain.Notification", b =>
                {
                    b.Property<int>("NotificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ApplicationId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(2000)")
                        .HasMaxLength(2000);

                    b.Property<string>("ExtraData")
                        .HasColumnType("varchar(150)")
                        .HasMaxLength(150);

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime>("InsertDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("InsertDateSh")
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<bool>("IsLock")
                        .HasColumnType("bit");

                    b.Property<string>("Receiver")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime>("SendDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("SendStatus")
                        .HasColumnType("nvarchar(25)")
                        .HasMaxLength(25);

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<byte>("TryCount")
                        .HasColumnType("tinyint");

                    b.Property<byte>("Type")
                        .HasColumnType("tinyint");

                    b.HasKey("NotificationId");

                    b.HasIndex("ApplicationId");

                    b.ToTable("Notification","Notifier");
                });

            modelBuilder.Entity("Pharmacy.Domain.EventMapper", b =>
                {
                    b.HasOne("Pharmacy.Domain.Application", "Application")
                        .WithMany()
                        .HasForeignKey("ApplicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Pharmacy.Domain.Notification", b =>
                {
                    b.HasOne("Pharmacy.Domain.Application", "Application")
                        .WithMany("Notifications")
                        .HasForeignKey("ApplicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
