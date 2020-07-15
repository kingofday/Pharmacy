﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Pharmacy.DataAccess.Ef;

namespace Pharmacy.DataAccess.Ef.Migrations.AppDb
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20200518112528_AddNewPassword2User")]
    partial class AddNewPassword2User
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("DrugVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Pharmacy.Domain.Address", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AddressDetails")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<byte>("AddressType")
                        .HasColumnType("tinyint");

                    b.Property<DateTime>("InsertDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("InsertDateSh")
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<bool>("IsDefault")
                        .HasColumnType("bit");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("ReciverName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AddressId");

                    b.HasIndex("UserId");

                    b.ToTable("Address","Base");
                });

            modelBuilder.Entity("Pharmacy.Domain.BankAccount", b =>
                {
                    b.Property<int>("BankAccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccountNumber")
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20);

                    b.Property<byte>("BankName")
                        .HasColumnType("tinyint");

                    b.Property<string>("CardNumber")
                        .IsRequired()
                        .HasColumnType("varchar(19)")
                        .HasMaxLength(19);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<DateTime>("InsertDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("InsertDateSh")
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModifyDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifyDateSh")
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<string>("Shaba")
                        .HasColumnType("varchar(24)")
                        .HasMaxLength(24);

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("BankAccountId");

                    b.HasIndex("UserId");

                    b.ToTable("BankAccount","Base");
                });

            modelBuilder.Entity("Pharmacy.Domain.DeliveryProvider", b =>
                {
                    b.Property<int>("DeliveryProviderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte>("DeliveryType")
                        .HasColumnType("tinyint");

                    b.Property<string>("InquiryUrl")
                        .HasColumnType("varchar(150)")
                        .HasMaxLength(150);

                    b.Property<DateTime>("InsertDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("InsertDateSh")
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("OrderUrl")
                        .IsRequired()
                        .HasColumnType("varchar(150)")
                        .HasMaxLength(150);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("varchar(25)")
                        .HasMaxLength(25);

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("varchar(25)")
                        .HasMaxLength(25);

                    b.HasKey("DeliveryProviderId");

                    b.ToTable("DeliveryProvider","Order");
                });

            modelBuilder.Entity("Pharmacy.Domain.DeliveryTime", b =>
                {
                    b.Property<int>("DeliveryTimeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("DeliverySpan")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPublicHoliday")
                        .HasColumnType("bit");

                    b.HasKey("DeliveryTimeId");

                    b.ToTable("DeliveryTime","Base");
                });

            modelBuilder.Entity("Pharmacy.Domain.Discount", b =>
                {
                    b.Property<int>("DiscountId")
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

                    b.Property<int>("MaxPrice")
                        .HasColumnType("int");

                    b.Property<DateTime>("ModifyDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifyDateSh")
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<float>("Percent")
                        .HasColumnType("real");

                    b.Property<int>("StoreId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ValidFromDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("ValidFromDateSh")
                        .IsRequired()
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<DateTime>("ValidToDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("ValidToDateSh")
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.HasKey("DiscountId");

                    b.HasIndex("StoreId");

                    b.ToTable("Discount","Order");
                });

            modelBuilder.Entity("Pharmacy.Domain.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DeliverTrackingId")
                        .HasColumnType("varchar(32)")
                        .HasMaxLength(32);

                    b.Property<int>("DeliveryProviderId")
                        .HasColumnType("int");

                    b.Property<int>("DeliveryTime")
                        .HasColumnType("int");

                    b.Property<int?>("DiscountId")
                        .HasColumnType("int");

                    b.Property<int>("DiscountPrice")
                        .HasColumnType("int");

                    b.Property<int>("FromAddressId")
                        .HasColumnType("int");

                    b.Property<DateTime>("InsertDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("InsertDateSh")
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModifyDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifyDateSh")
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<string>("OrderComment")
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.Property<int>("OrderStatus")
                        .HasColumnType("int");

                    b.Property<DateTime?>("PreparationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("StoreId")
                        .HasColumnType("int");

                    b.Property<int>("ToAddressId")
                        .HasColumnType("int");

                    b.Property<int>("TotalPrice")
                        .HasColumnType("int");

                    b.Property<int>("TotalPriceAfterDiscount")
                        .HasColumnType("int");

                    b.Property<string>("UserComment")
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("OrderId");

                    b.HasIndex("FromAddressId");

                    b.HasIndex("StoreId");

                    b.HasIndex("ToAddressId");

                    b.HasIndex("UserId");

                    b.ToTable("Order","Order");
                });

            modelBuilder.Entity("Pharmacy.Domain.OrderDetail", b =>
                {
                    b.Property<int>("OrderDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<int>("DiscountPrice")
                        .HasColumnType("int");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<int>("DrugId")
                        .HasColumnType("int");

                    b.Property<int>("TotalPrice")
                        .HasColumnType("int");

                    b.HasKey("OrderDetailId");

                    b.HasIndex("OrderId");

                    b.HasIndex("DrugId");

                    b.ToTable("OrderDetail","Order");
                });

            modelBuilder.Entity("Pharmacy.Domain.Payment", b =>
                {
                    b.Property<int>("PaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("InsertDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("InsertDateSh")
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModifyDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifyDateSh")
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("PaymentGatewayId")
                        .HasColumnType("int");

                    b.Property<int>("PaymentStatus")
                        .HasColumnType("int");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<string>("TransactionId")
                        .HasColumnType("varchar(36)")
                        .HasMaxLength(36);

                    b.HasKey("PaymentId");

                    b.HasIndex("OrderId");

                    b.HasIndex("PaymentGatewayId");

                    b.ToTable("Payment","Payment");
                });

            modelBuilder.Entity("Pharmacy.Domain.PaymentGateway", b =>
                {
                    b.Property<int>("PaymentGatewayId")
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

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("MerchantId")
                        .IsRequired()
                        .HasColumnType("varchar(36)")
                        .HasMaxLength(36);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(25)")
                        .HasMaxLength(25);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("varchar(25)")
                        .HasMaxLength(25);

                    b.Property<string>("PostBackUrl")
                        .IsRequired()
                        .HasColumnType("varchar(150)")
                        .HasMaxLength(150);

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("varchar(150)")
                        .HasMaxLength(150);

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("varchar(25)")
                        .HasMaxLength(25);

                    b.Property<string>("VerifyUrl")
                        .IsRequired()
                        .HasColumnType("varchar(150)")
                        .HasMaxLength(150);

                    b.HasKey("PaymentGatewayId");

                    b.ToTable("PaymentGateway","Payment");
                });

            modelBuilder.Entity("Pharmacy.Domain.Drug", b =>
                {
                    b.Property<int>("DrugId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<float?>("DiscountPercent")
                        .HasColumnType("real");

                    b.Property<DateTime>("InsertDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("InsertDateSh")
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("LikeCount")
                        .HasColumnType("int");

                    b.Property<int>("MaxOrderCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("ModifyDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifyDateSh")
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(35)")
                        .HasMaxLength(35);

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<int?>("DrugCategoryId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int>("StoreId")
                        .HasColumnType("int");

                    b.Property<string>("UniqueId")
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20);

                    b.HasKey("DrugId");

                    b.HasIndex("DrugCategoryId");

                    b.HasIndex("StoreId");

                    b.ToTable("Drug","Store");
                });

            modelBuilder.Entity("Pharmacy.Domain.DrugAsset", b =>
                {
                    b.Property<int>("DrugAssetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CdnFileUrl")
                        .HasColumnType("varchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("CdnThumbnailUrl")
                        .HasColumnType("varchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("Extention")
                        .IsRequired()
                        .HasColumnType("varchar(5)")
                        .HasMaxLength(5);

                    b.Property<byte>("FileType")
                        .HasColumnType("tinyint");

                    b.Property<string>("FileUrl")
                        .HasColumnType("varchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<DateTime>("InsertDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("InsertDateSh")
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<string>("Name")
                        .HasColumnType("varchar(35)")
                        .HasMaxLength(35);

                    b.Property<int>("DrugId")
                        .HasColumnType("int");

                    b.Property<string>("ThumbnailUrl")
                        .HasColumnType("varchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<string>("UniqueId")
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20);

                    b.HasKey("DrugAssetId");

                    b.HasIndex("DrugId");

                    b.ToTable("DrugAsset","Store");
                });

            modelBuilder.Entity("Pharmacy.Domain.DrugCategory", b =>
                {
                    b.Property<int>("DrugCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("InsertDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("InsertDateSh")
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<DateTime>("ModifyDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifyDateSh")
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.HasKey("DrugCategoryId");

                    b.HasIndex("ParentId");

                    b.ToTable("DrugCategory","Base");
                });

            modelBuilder.Entity("Pharmacy.Domain.DrugTag", b =>
                {
                    b.Property<int>("DrugTagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("InsertDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("InsertDateSh")
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<int>("DrugId")
                        .HasColumnType("int");

                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.HasKey("DrugTagId");

                    b.HasIndex("DrugId");

                    b.HasIndex("TagId");

                    b.ToTable("DrugTag","Store");
                });

            modelBuilder.Entity("Pharmacy.Domain.Store", b =>
                {
                    b.Property<int>("StoreId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AddressId")
                        .HasColumnType("int");

                    b.Property<int?>("FolowerCount")
                        .HasColumnType("int");

                    b.Property<int?>("FolowingCount")
                        .HasColumnType("int");

                    b.Property<string>("FullName")
                        .HasColumnType("varchar(40)")
                        .HasMaxLength(40);

                    b.Property<DateTime>("InsertDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("InsertDateSh")
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastCrawlTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModifyDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifyDateSh")
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<int?>("PostCount")
                        .HasColumnType("int");

                    b.Property<int?>("PreparationDay")
                        .HasColumnType("int");

                    b.Property<int?>("DrugCount")
                        .HasColumnType("int");

                    b.Property<string>("ProfilePictureUrl")
                        .HasColumnType("varchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<string>("PharmacyUrl")
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100);

                    b.Property<byte>("StoreStatus")
                        .HasColumnType("tinyint");

                    b.Property<byte>("StoreType")
                        .HasColumnType("tinyint");

                    b.Property<string>("TelegramUrl")
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("UniqueId")
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20);

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Username")
                        .HasColumnType("varchar(25)")
                        .HasMaxLength(25);

                    b.Property<string>("WhatsAppUrl")
                        .HasColumnType("varchar(150)")
                        .HasMaxLength(150);

                    b.HasKey("StoreId");

                    b.HasIndex("UserId");

                    b.ToTable("Store","Store");
                });

            modelBuilder.Entity("Pharmacy.Domain.Tag", b =>
                {
                    b.Property<int>("TagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("InsertDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("InsertDateSh")
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.HasKey("TagId");

                    b.HasIndex("Title")
                        .IsUnique()
                        .HasName("IX_Title");

                    b.ToTable("Tag","Base");
                });

            modelBuilder.Entity("Pharmacy.Domain.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime>("InsertDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("InsertDateSh")
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastLoginDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastLoginDateSh")
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<long>("MobileNumber")
                        .HasColumnType("bigint");

                    b.Property<bool>("MustChangePassword")
                        .HasColumnType("bit");

                    b.Property<string>("NewPassword")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("TelegramId")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50);

                    b.Property<byte>("UserStatus")
                        .HasColumnType("tinyint");

                    b.HasKey("UserId");

                    b.HasIndex("MobileNumber")
                        .IsUnique()
                        .HasName("IX_MobileNumber");

                    b.ToTable("User","Base");
                });

            modelBuilder.Entity("Pharmacy.Domain.UserAttachment", b =>
                {
                    b.Property<int>("UserAttachmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Extention")
                        .IsRequired()
                        .HasColumnType("varchar(5)")
                        .HasMaxLength(5);

                    b.Property<byte>("FileType")
                        .HasColumnType("tinyint");

                    b.Property<DateTime>("InsertDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("InsertDateSh")
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(16)")
                        .HasMaxLength(16);

                    b.Property<int>("Size")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("varchar(150)")
                        .HasMaxLength(150);

                    b.Property<int>("UserAttachmentType")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserAttachmentId");

                    b.HasIndex("UserId");

                    b.ToTable("UserAttachment","Base");
                });

            modelBuilder.Entity("Pharmacy.Domain.Address", b =>
                {
                    b.HasOne("Pharmacy.Domain.User", "User")
                        .WithMany("Addresses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Pharmacy.Domain.BankAccount", b =>
                {
                    b.HasOne("Pharmacy.Domain.User", "User")
                        .WithMany("bankAccounts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Pharmacy.Domain.Discount", b =>
                {
                    b.HasOne("Pharmacy.Domain.Store", "Store")
                        .WithMany()
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Pharmacy.Domain.Order", b =>
                {
                    b.HasOne("Pharmacy.Domain.Address", "FromAddress")
                        .WithMany()
                        .HasForeignKey("FromAddressId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Pharmacy.Domain.Store", "Store")
                        .WithMany()
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Pharmacy.Domain.Address", "ToAddress")
                        .WithMany()
                        .HasForeignKey("ToAddressId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Pharmacy.Domain.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Pharmacy.Domain.OrderDetail", b =>
                {
                    b.HasOne("Pharmacy.Domain.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Pharmacy.Domain.Drug", "Drug")
                        .WithMany()
                        .HasForeignKey("DrugId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Pharmacy.Domain.Payment", b =>
                {
                    b.HasOne("Pharmacy.Domain.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pharmacy.Domain.PaymentGateway", "PaymentGateway")
                        .WithMany("Payments")
                        .HasForeignKey("PaymentGatewayId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Pharmacy.Domain.Drug", b =>
                {
                    b.HasOne("Pharmacy.Domain.DrugCategory", "DrugCategory")
                        .WithMany()
                        .HasForeignKey("DrugCategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Pharmacy.Domain.Store", "Store")
                        .WithMany("Drugs")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Pharmacy.Domain.DrugAsset", b =>
                {
                    b.HasOne("Pharmacy.Domain.Drug", "Drug")
                        .WithMany("DrugAssets")
                        .HasForeignKey("DrugId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Pharmacy.Domain.DrugCategory", b =>
                {
                    b.HasOne("Pharmacy.Domain.DrugCategory", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Pharmacy.Domain.DrugTag", b =>
                {
                    b.HasOne("Pharmacy.Domain.Drug", "Drug")
                        .WithMany()
                        .HasForeignKey("DrugId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pharmacy.Domain.Tag", "Tag")
                        .WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Pharmacy.Domain.Store", b =>
                {
                    b.HasOne("Pharmacy.Domain.User", "User")
                        .WithMany("Stores")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Pharmacy.Domain.UserAttachment", b =>
                {
                    b.HasOne("Pharmacy.Domain.User", "User")
                        .WithMany("UserAttachments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
