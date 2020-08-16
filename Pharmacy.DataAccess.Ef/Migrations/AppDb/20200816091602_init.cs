using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pharmacy.DataAccess.Ef.Migrations.AppDb
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Base");

            migrationBuilder.EnsureSchema(
                name: "Drug");

            migrationBuilder.EnsureSchema(
                name: "Order");

            migrationBuilder.EnsureSchema(
                name: "Payment");

            migrationBuilder.CreateTable(
                name: "DeliveryTime",
                schema: "Base",
                columns: table => new
                {
                    DeliveryTimeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeliverySpan = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsPublicHoliday = table.Column<bool>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryTime", x => x.DeliveryTimeId);
                });

            migrationBuilder.CreateTable(
                name: "Province",
                schema: "Base",
                columns: table => new
                {
                    ProvinceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(nullable: false),
                    DefaultLatitude = table.Column<double>(nullable: false),
                    DefaulLongitude = table.Column<double>(nullable: false),
                    nvarchar70 = table.Column<string>(name: "nvarchar(70)", maxLength: 70, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Province", x => x.ProvinceId);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                schema: "Base",
                columns: table => new
                {
                    TagId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    nvarchar30 = table.Column<string>(name: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.TagId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "Base",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    IsConfirmed = table.Column<bool>(nullable: false),
                    MobileConfirmCode = table.Column<int>(nullable: true),
                    MobileNumber = table.Column<long>(nullable: false),
                    UserStatus = table.Column<byte>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    MustChangePassword = table.Column<bool>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    LastLoginDateMi = table.Column<DateTime>(nullable: true),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    LastLoginDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    Password = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    NewPassword = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    FullName = table.Column<string>(maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "DrugCategory",
                schema: "Drug",
                columns: table => new
                {
                    DrugCategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(nullable: true),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    ModifyDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    ModifyDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    OrderPriority = table.Column<int>(nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugCategory", x => x.DrugCategoryId);
                    table.ForeignKey(
                        name: "FK_DrugCategory_DrugCategory_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Drug",
                        principalTable: "DrugCategory",
                        principalColumn: "DrugCategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Unit",
                schema: "Drug",
                columns: table => new
                {
                    UnitId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unit", x => x.UnitId);
                });

            migrationBuilder.CreateTable(
                name: "PaymentGateway",
                schema: "Payment",
                columns: table => new
                {
                    PaymentGatewayId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Username = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false),
                    Password = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false),
                    MerchantId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false),
                    Url = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    PostBackUrl = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    VerifyUrl = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentGateway", x => x.PaymentGatewayId);
                });

            migrationBuilder.CreateTable(
                name: "City",
                schema: "Base",
                columns: table => new
                {
                    CityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProvinceId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    nvarchar70 = table.Column<string>(name: "nvarchar(70)", maxLength: 70, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.CityId);
                    table.ForeignKey(
                        name: "FK_City_Province_ProvinceId",
                        column: x => x.ProvinceId,
                        principalSchema: "Base",
                        principalTable: "Province",
                        principalColumn: "ProvinceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BankAccount",
                schema: "Base",
                columns: table => new
                {
                    BankAccountId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankName = table.Column<byte>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    ModifyDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    ModifyDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    AccountNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    CardNumber = table.Column<string>(type: "varchar(19)", maxLength: 19, nullable: false),
                    Shaba = table.Column<string>(type: "varchar(24)", maxLength: 24, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccount", x => x.BankAccountId);
                    table.ForeignKey(
                        name: "FK_BankAccount_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Base",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserAttachment",
                schema: "Base",
                columns: table => new
                {
                    UserAttachmentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileType = table.Column<byte>(nullable: false),
                    AttachmentType = table.Column<byte>(nullable: false),
                    Size = table.Column<int>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    Extention = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false),
                    OriginalName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    FileName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    Url = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    PhysicalPath = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAttachment", x => x.UserAttachmentId);
                    table.ForeignKey(
                        name: "FK_UserAttachment_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Base",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DrugStore",
                schema: "Drug",
                columns: table => new
                {
                    DrugStoreId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    AddressId = table.Column<int>(nullable: false),
                    Status = table.Column<byte>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    ModifyDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    ModifyDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugStore", x => x.DrugStoreId);
                    table.ForeignKey(
                        name: "FK_DrugStore_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Base",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Prescription",
                schema: "Order",
                columns: table => new
                {
                    PrescriptionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<byte>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescription", x => x.PrescriptionId);
                    table.ForeignKey(
                        name: "FK_Prescription_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Base",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Drug",
                schema: "Drug",
                columns: table => new
                {
                    DrugId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<int>(nullable: false),
                    DiscountPrice = table.Column<int>(nullable: false),
                    UnitId = table.Column<int>(nullable: false),
                    DrugCategoryId = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    NeedPrescription = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    ModifyDateMi = table.Column<DateTime>(nullable: false),
                    ViewCount = table.Column<int>(nullable: false),
                    LikeCount = table.Column<int>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    ModifyDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    UniqueId = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    NameFa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ShortDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drug", x => x.DrugId);
                    table.ForeignKey(
                        name: "FK_Drug_DrugCategory_DrugCategoryId",
                        column: x => x.DrugCategoryId,
                        principalSchema: "Drug",
                        principalTable: "DrugCategory",
                        principalColumn: "DrugCategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Drug_Unit_UnitId",
                        column: x => x.UnitId,
                        principalSchema: "Drug",
                        principalTable: "Unit",
                        principalColumn: "UnitId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "District",
                schema: "Base",
                columns: table => new
                {
                    DistrictId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    nvarchar70 = table.Column<string>(name: "nvarchar(70)", maxLength: 70, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_District", x => x.DistrictId);
                    table.ForeignKey(
                        name: "FK_District_City_CityId",
                        column: x => x.CityId,
                        principalSchema: "Base",
                        principalTable: "City",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DrugStoreAsset",
                schema: "Drug",
                columns: table => new
                {
                    DrugStoreAssetId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileType = table.Column<byte>(nullable: false),
                    AttachmentType = table.Column<byte>(nullable: false),
                    Size = table.Column<int>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    Extention = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false),
                    OriginalName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    FileName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    Url = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    PhysicalPath = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    DrugStoreId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugStoreAsset", x => x.DrugStoreAssetId);
                    table.ForeignKey(
                        name: "FK_DrugStoreAsset_DrugStore_DrugStoreId",
                        column: x => x.DrugStoreId,
                        principalSchema: "Drug",
                        principalTable: "DrugStore",
                        principalColumn: "DrugStoreId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PrescriptionAttachment",
                schema: "Order",
                columns: table => new
                {
                    PrescriptionAttachmentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileType = table.Column<byte>(nullable: false),
                    AttachmentType = table.Column<byte>(nullable: false),
                    Size = table.Column<int>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    Extention = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false),
                    OriginalName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    FileName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    Url = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    PhysicalPath = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    PrescriptionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrescriptionAttachment", x => x.PrescriptionAttachmentId);
                    table.ForeignKey(
                        name: "FK_PrescriptionAttachment_Prescription_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalSchema: "Order",
                        principalTable: "Prescription",
                        principalColumn: "PrescriptionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TempBasket",
                schema: "Order",
                columns: table => new
                {
                    TempBasketId = table.Column<Guid>(nullable: false),
                    PrescriptionId = table.Column<int>(nullable: true),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempBasket", x => x.TempBasketId);
                    table.ForeignKey(
                        name: "FK_TempBasket_Prescription_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalSchema: "Order",
                        principalTable: "Prescription",
                        principalColumn: "PrescriptionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DrugAsset",
                schema: "Drug",
                columns: table => new
                {
                    DrugAssetId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileType = table.Column<byte>(nullable: false),
                    AttachmentType = table.Column<byte>(nullable: false),
                    Size = table.Column<int>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    Extention = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false),
                    OriginalName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    FileName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    Url = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    PhysicalPath = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    DrugId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugAsset", x => x.DrugAssetId);
                    table.ForeignKey(
                        name: "FK_DrugAsset_Drug_DrugId",
                        column: x => x.DrugId,
                        principalSchema: "Drug",
                        principalTable: "Drug",
                        principalColumn: "DrugId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DrugComment",
                schema: "Drug",
                columns: table => new
                {
                    DrugCommentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(nullable: false),
                    DrugId = table.Column<int>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Score = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugComment", x => x.DrugCommentId);
                    table.ForeignKey(
                        name: "FK_DrugComment_Drug_DrugId",
                        column: x => x.DrugId,
                        principalSchema: "Drug",
                        principalTable: "Drug",
                        principalColumn: "DrugId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DrugComment_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Base",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DrugProperty",
                schema: "Drug",
                columns: table => new
                {
                    DrugPropertyId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Value = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DrugId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugProperty", x => x.DrugPropertyId);
                    table.ForeignKey(
                        name: "FK_DrugProperty_Drug_DrugId",
                        column: x => x.DrugId,
                        principalSchema: "Drug",
                        principalTable: "Drug",
                        principalColumn: "DrugId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DrugTag",
                schema: "Drug",
                columns: table => new
                {
                    DrugTagId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DrugId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugTag", x => x.DrugTagId);
                    table.ForeignKey(
                        name: "FK_DrugTag_Drug_DrugId",
                        column: x => x.DrugId,
                        principalSchema: "Drug",
                        principalTable: "Drug",
                        principalColumn: "DrugId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DrugTag_Tag_TagId",
                        column: x => x.TagId,
                        principalSchema: "Base",
                        principalTable: "Tag",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DrugStoreAddress",
                schema: "Base",
                columns: table => new
                {
                    DrugStoreAddressId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DistrictId = table.Column<int>(nullable: true),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    Details = table.Column<string>(maxLength: 250, nullable: false),
                    DrugStoreId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugStoreAddress", x => x.DrugStoreAddressId);
                    table.ForeignKey(
                        name: "FK_DrugStoreAddress_District_DistrictId",
                        column: x => x.DistrictId,
                        principalSchema: "Base",
                        principalTable: "District",
                        principalColumn: "DistrictId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DrugStoreAddress_DrugStore_DrugStoreId",
                        column: x => x.DrugStoreId,
                        principalSchema: "Drug",
                        principalTable: "DrugStore",
                        principalColumn: "DrugStoreId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserAddress",
                schema: "Base",
                columns: table => new
                {
                    UserAddressId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DistrictId = table.Column<int>(nullable: true),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    Details = table.Column<string>(maxLength: 250, nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    IsDefault = table.Column<bool>(nullable: false),
                    Fullname = table.Column<string>(nullable: true),
                    MobileNumber = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAddress", x => x.UserAddressId);
                    table.ForeignKey(
                        name: "FK_UserAddress_District_DistrictId",
                        column: x => x.DistrictId,
                        principalSchema: "Base",
                        principalTable: "District",
                        principalColumn: "DistrictId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserAddress_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Base",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TempBasketItem",
                schema: "Order",
                columns: table => new
                {
                    TempBasketItemId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TempBasketId = table.Column<Guid>(nullable: false),
                    DrugId = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    TotalPrice = table.Column<int>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempBasketItem", x => x.TempBasketItemId);
                    table.ForeignKey(
                        name: "FK_TempBasketItem_Drug_DrugId",
                        column: x => x.DrugId,
                        principalSchema: "Drug",
                        principalTable: "Drug",
                        principalColumn: "DrugId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TempBasketItem_TempBasket_TempBasketId",
                        column: x => x.TempBasketId,
                        principalSchema: "Order",
                        principalTable: "TempBasket",
                        principalColumn: "TempBasketId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                schema: "Order",
                columns: table => new
                {
                    OrderId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeliveryType = table.Column<byte>(nullable: false),
                    DeliveryAgentName = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    TempBasketId = table.Column<Guid>(nullable: true),
                    DrugStoreId = table.Column<int>(nullable: false),
                    AddressId = table.Column<int>(nullable: false),
                    DeliveryTime = table.Column<int>(nullable: false),
                    TotalDiscountPrice = table.Column<int>(nullable: false),
                    DeliveryPrice = table.Column<int>(nullable: false),
                    TotalItemsPrice = table.Column<int>(nullable: false),
                    TotalPriceWithoutDiscount = table.Column<int>(nullable: false),
                    TotalPrice = table.Column<int>(nullable: false),
                    OrderStatus = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    PreparationDate = table.Column<DateTime>(nullable: true),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    ModifyDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    ModifyDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    Comment = table.Column<string>(maxLength: 150, nullable: true),
                    ExtraInfoJson = table.Column<string>(maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Order_UserAddress_AddressId",
                        column: x => x.AddressId,
                        principalSchema: "Base",
                        principalTable: "UserAddress",
                        principalColumn: "UserAddressId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_TempBasket_TempBasketId",
                        column: x => x.TempBasketId,
                        principalSchema: "Order",
                        principalTable: "TempBasket",
                        principalColumn: "TempBasketId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Base",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderDrugStore",
                schema: "Order",
                columns: table => new
                {
                    OrderDrugStoreId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(nullable: false),
                    OrderId = table.Column<int>(nullable: false),
                    DrugStoreId = table.Column<int>(nullable: false),
                    DeliveryPrice = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDrugStore", x => x.OrderDrugStoreId);
                    table.ForeignKey(
                        name: "FK_OrderDrugStore_DrugStore_DrugStoreId",
                        column: x => x.DrugStoreId,
                        principalSchema: "Drug",
                        principalTable: "DrugStore",
                        principalColumn: "DrugStoreId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderDrugStore_Order_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "Order",
                        principalTable: "Order",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItem",
                schema: "Order",
                columns: table => new
                {
                    OrderItemId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(nullable: false),
                    DrugId = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    DiscountPrice = table.Column<int>(nullable: false),
                    TotalPrice = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_OrderItem_Drug_DrugId",
                        column: x => x.DrugId,
                        principalSchema: "Drug",
                        principalTable: "Drug",
                        principalColumn: "DrugId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItem_Order_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "Order",
                        principalTable: "Order",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                schema: "Payment",
                columns: table => new
                {
                    PaymentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentGatewayId = table.Column<int>(nullable: false),
                    OrderId = table.Column<int>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    PaymentStatus = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    ModifyDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    ModifyDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    Title = table.Column<string>(maxLength: 150, nullable: true),
                    TransactionId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payment_Order_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "Order",
                        principalTable: "Order",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payment_PaymentGateway_PaymentGatewayId",
                        column: x => x.PaymentGatewayId,
                        principalSchema: "Payment",
                        principalTable: "PaymentGateway",
                        principalColumn: "PaymentGatewayId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccount_UserId",
                schema: "Base",
                table: "BankAccount",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_City_ProvinceId",
                schema: "Base",
                table: "City",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_District_CityId",
                schema: "Base",
                table: "District",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugStoreAddress_DistrictId",
                schema: "Base",
                table: "DrugStoreAddress",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugStoreAddress_DrugStoreId",
                schema: "Base",
                table: "DrugStoreAddress",
                column: "DrugStoreId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Title",
                schema: "Base",
                table: "Tag",
                column: "nvarchar(30)",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MobileNumber",
                schema: "Base",
                table: "User",
                column: "MobileNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAddress_DistrictId",
                schema: "Base",
                table: "UserAddress",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddress_UserId",
                schema: "Base",
                table: "UserAddress",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAttachment_UserId",
                schema: "Base",
                table: "UserAttachment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Drug_DrugCategoryId",
                schema: "Drug",
                table: "Drug",
                column: "DrugCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_NameEn",
                schema: "Drug",
                table: "Drug",
                column: "NameEn");

            migrationBuilder.CreateIndex(
                name: "IX_NameFa",
                schema: "Drug",
                table: "Drug",
                column: "NameFa");

            migrationBuilder.CreateIndex(
                name: "IX_Drug_UnitId",
                schema: "Drug",
                table: "Drug",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugAsset_DrugId",
                schema: "Drug",
                table: "DrugAsset",
                column: "DrugId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugCategory_ParentId",
                schema: "Drug",
                table: "DrugCategory",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugComment_DrugId",
                schema: "Drug",
                table: "DrugComment",
                column: "DrugId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugComment_UserId",
                schema: "Drug",
                table: "DrugComment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugProperty_DrugId",
                schema: "Drug",
                table: "DrugProperty",
                column: "DrugId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugStore_UserId",
                schema: "Drug",
                table: "DrugStore",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugStoreAsset_DrugStoreId",
                schema: "Drug",
                table: "DrugStoreAsset",
                column: "DrugStoreId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugTag_DrugId",
                schema: "Drug",
                table: "DrugTag",
                column: "DrugId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugTag_TagId",
                schema: "Drug",
                table: "DrugTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_AddressId",
                schema: "Order",
                table: "Order",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_TempBasketId",
                schema: "Order",
                table: "Order",
                column: "TempBasketId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserId",
                schema: "Order",
                table: "Order",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDrugStore_DrugStoreId",
                schema: "Order",
                table: "OrderDrugStore",
                column: "DrugStoreId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDrugStore_OrderId",
                schema: "Order",
                table: "OrderDrugStore",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDrugStore",
                schema: "Order",
                table: "OrderDrugStore",
                columns: new[] { "OrderDrugStoreId", "DrugStoreId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_DrugId",
                schema: "Order",
                table: "OrderItem",
                column: "DrugId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_OrderId",
                schema: "Order",
                table: "OrderItem",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescription_UserId",
                schema: "Order",
                table: "Prescription",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionAttachment_PrescriptionId",
                schema: "Order",
                table: "PrescriptionAttachment",
                column: "PrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_TempBasket_PrescriptionId",
                schema: "Order",
                table: "TempBasket",
                column: "PrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_TempBasketItem_DrugId",
                schema: "Order",
                table: "TempBasketItem",
                column: "DrugId");

            migrationBuilder.CreateIndex(
                name: "IX_TempBasketItem_TempBasketId",
                schema: "Order",
                table: "TempBasketItem",
                column: "TempBasketId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_OrderId",
                schema: "Payment",
                table: "Payment",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_PaymentGatewayId",
                schema: "Payment",
                table: "Payment",
                column: "PaymentGatewayId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankAccount",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "DeliveryTime",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "DrugStoreAddress",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "UserAttachment",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "DrugAsset",
                schema: "Drug");

            migrationBuilder.DropTable(
                name: "DrugComment",
                schema: "Drug");

            migrationBuilder.DropTable(
                name: "DrugProperty",
                schema: "Drug");

            migrationBuilder.DropTable(
                name: "DrugStoreAsset",
                schema: "Drug");

            migrationBuilder.DropTable(
                name: "DrugTag",
                schema: "Drug");

            migrationBuilder.DropTable(
                name: "OrderDrugStore",
                schema: "Order");

            migrationBuilder.DropTable(
                name: "OrderItem",
                schema: "Order");

            migrationBuilder.DropTable(
                name: "PrescriptionAttachment",
                schema: "Order");

            migrationBuilder.DropTable(
                name: "TempBasketItem",
                schema: "Order");

            migrationBuilder.DropTable(
                name: "Payment",
                schema: "Payment");

            migrationBuilder.DropTable(
                name: "Tag",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "DrugStore",
                schema: "Drug");

            migrationBuilder.DropTable(
                name: "Drug",
                schema: "Drug");

            migrationBuilder.DropTable(
                name: "Order",
                schema: "Order");

            migrationBuilder.DropTable(
                name: "PaymentGateway",
                schema: "Payment");

            migrationBuilder.DropTable(
                name: "DrugCategory",
                schema: "Drug");

            migrationBuilder.DropTable(
                name: "Unit",
                schema: "Drug");

            migrationBuilder.DropTable(
                name: "UserAddress",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "TempBasket",
                schema: "Order");

            migrationBuilder.DropTable(
                name: "District",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "Prescription",
                schema: "Order");

            migrationBuilder.DropTable(
                name: "City",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "Province",
                schema: "Base");
        }
    }
}
