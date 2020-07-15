using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pharmacy.DataAccess.Ef.Migrations.AppDb
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Base");

            migrationBuilder.EnsureSchema(
                name: "Order");

            migrationBuilder.EnsureSchema(
                name: "Payment");

            migrationBuilder.EnsureSchema(
                name: "Store");

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
                name: "Notification",
                schema: "Base",
                columns: table => new
                {
                    NotificationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(nullable: false),
                    Type = table.Column<byte>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    SendDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    SendStatus = table.Column<string>(maxLength: 25, nullable: true),
                    Receiver = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    FullName = table.Column<string>(maxLength: 50, nullable: true),
                    ExtraData = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true),
                    Content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.NotificationId);
                });

            migrationBuilder.CreateTable(
                name: "DrugCategory",
                schema: "Base",
                columns: table => new
                {
                    DrugCategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(nullable: true),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    ModifyDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    ModifyDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    Name = table.Column<string>(maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugCategory", x => x.DrugCategoryId);
                    table.ForeignKey(
                        name: "FK_DrugCategory_DrugCategory_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Base",
                        principalTable: "DrugCategory",
                        principalColumn: "DrugCategoryId",
                        onDelete: ReferentialAction.Restrict);
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
                    Title = table.Column<string>(maxLength: 30, nullable: false)
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
                    MobileNumber = table.Column<long>(nullable: false),
                    UserStatus = table.Column<byte>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsRecoveredPassword = table.Column<bool>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    LastLoginDateMi = table.Column<DateTime>(nullable: true),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    LastLoginDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    Password = table.Column<string>(type: "char(25)", maxLength: 25, nullable: false),
                    FullName = table.Column<string>(maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    TelegramId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryProvider",
                schema: "Order",
                columns: table => new
                {
                    DeliveryProviderId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    Username = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false),
                    Password = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    OrderUrl = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    InquiryUrl = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryProvider", x => x.DeliveryProviderId);
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
                    PostBackUrl = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentGateway", x => x.PaymentGatewayId);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                schema: "Base",
                columns: table => new
                {
                    AddressId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(nullable: false),
                    AddressType = table.Column<byte>(nullable: false),
                    IsDefault = table.Column<bool>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    ReciverName = table.Column<string>(nullable: true),
                    AddressDetails = table.Column<string>(maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.AddressId);
                    table.ForeignKey(
                        name: "FK_Address_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Base",
                        principalTable: "User",
                        principalColumn: "UserId",
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
                    UserId = table.Column<Guid>(nullable: false),
                    FileType = table.Column<byte>(nullable: false),
                    UserAttachmentType = table.Column<int>(nullable: false),
                    Size = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    Extention = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false),
                    Name = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false),
                    Url = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
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
                name: "Store",
                schema: "Store",
                columns: table => new
                {
                    StoreId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(nullable: false),
                    AddressId = table.Column<int>(nullable: true),
                    StoreType = table.Column<byte>(nullable: false),
                    StoreStatus = table.Column<byte>(nullable: false),
                    PreparationDay = table.Column<int>(nullable: true),
                    DrugCount = table.Column<int>(nullable: true),
                    PostCount = table.Column<int>(nullable: true),
                    FolowerCount = table.Column<int>(nullable: true),
                    FolowingCount = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastCrawlTime = table.Column<DateTime>(nullable: true),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    ModifyDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    ModifyDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    UniqueId = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    PharmacyUrl = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    TelegramUrl = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    WhatsAppUrl = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Store", x => x.StoreId);
                    table.ForeignKey(
                        name: "FK_Store_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Base",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Discount",
                schema: "Order",
                columns: table => new
                {
                    DiscountId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreId = table.Column<int>(nullable: false),
                    Percent = table.Column<float>(nullable: false),
                    MaxPrice = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    ValidFromDateMi = table.Column<DateTime>(nullable: false),
                    ValidToDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    ModifyDateMi = table.Column<DateTime>(nullable: false),
                    ValidFromDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: false),
                    ValidToDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    ModifyDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discount", x => x.DiscountId);
                    table.ForeignKey(
                        name: "FK_Discount_Store_StoreId",
                        column: x => x.StoreId,
                        principalSchema: "Store",
                        principalTable: "Store",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                schema: "Order",
                columns: table => new
                {
                    OrderId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreId = table.Column<int>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    FromAddressId = table.Column<int>(nullable: false),
                    ToAddressId = table.Column<int>(nullable: false),
                    DeliveryProviderId = table.Column<int>(nullable: false),
                    DeliveryTime = table.Column<int>(nullable: false),
                    DiscountId = table.Column<int>(nullable: true),
                    DiscountPrice = table.Column<int>(nullable: false),
                    TotalPrice = table.Column<int>(nullable: false),
                    TotalPriceAfterDiscount = table.Column<int>(nullable: false),
                    DeliveryType = table.Column<byte>(nullable: false),
                    OrderStatus = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    PreparationDate = table.Column<DateTime>(nullable: true),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    ModifyDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    ModifyDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    DeliverTrackingId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true),
                    OrderComment = table.Column<string>(maxLength: 150, nullable: true),
                    UserComment = table.Column<string>(maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Order_Address_FromAddressId",
                        column: x => x.FromAddressId,
                        principalSchema: "Base",
                        principalTable: "Address",
                        principalColumn: "AddressId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_Store_StoreId",
                        column: x => x.StoreId,
                        principalSchema: "Store",
                        principalTable: "Store",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_Address_ToAddressId",
                        column: x => x.ToAddressId,
                        principalSchema: "Base",
                        principalTable: "Address",
                        principalColumn: "AddressId",
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
                name: "Drug",
                schema: "Store",
                columns: table => new
                {
                    DrugId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreId = table.Column<int>(nullable: false),
                    DrugCategoryId = table.Column<int>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    LikeCount = table.Column<int>(nullable: false),
                    MaxOrderCount = table.Column<int>(nullable: false),
                    DiscountPercent = table.Column<float>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    ModifyDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    ModifyDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    UniqueId = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drug", x => x.DrugId);
                    table.ForeignKey(
                        name: "FK_Drug_DrugCategory_DrugCategoryId",
                        column: x => x.DrugCategoryId,
                        principalSchema: "Base",
                        principalTable: "DrugCategory",
                        principalColumn: "DrugCategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Drug_Store_StoreId",
                        column: x => x.StoreId,
                        principalSchema: "Store",
                        principalTable: "Store",
                        principalColumn: "StoreId",
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payment_PaymentGateway_PaymentGatewayId",
                        column: x => x.PaymentGatewayId,
                        principalSchema: "Payment",
                        principalTable: "PaymentGateway",
                        principalColumn: "PaymentGatewayId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetail",
                schema: "Order",
                columns: table => new
                {
                    OrderDetailId = table.Column<int>(nullable: false)
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
                    table.PrimaryKey("PK_OrderDetail", x => x.OrderDetailId);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Order_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "Order",
                        principalTable: "Order",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Drug_DrugId",
                        column: x => x.DrugId,
                        principalSchema: "Store",
                        principalTable: "Drug",
                        principalColumn: "DrugId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DrugAsset",
                schema: "Store",
                columns: table => new
                {
                    DrugAssetId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DrugId = table.Column<int>(nullable: false),
                    FileType = table.Column<byte>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    Extention = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false),
                    UniqueId = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    Name = table.Column<string>(type: "varchar(35)", maxLength: 35, nullable: true),
                    CdnFileUrl = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    CdnThumbnailUrl = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    FileUrl = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true),
                    ThumbnailUrl = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugAsset", x => x.DrugAssetId);
                    table.ForeignKey(
                        name: "FK_DrugAsset_Drug_DrugId",
                        column: x => x.DrugId,
                        principalSchema: "Store",
                        principalTable: "Drug",
                        principalColumn: "DrugId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DrugTag",
                schema: "Store",
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
                        principalSchema: "Store",
                        principalTable: "Drug",
                        principalColumn: "DrugId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DrugTag_Tag_TagId",
                        column: x => x.TagId,
                        principalSchema: "Base",
                        principalTable: "Tag",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_UserId",
                schema: "Base",
                table: "Address",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccount_UserId",
                schema: "Base",
                table: "BankAccount",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugCategory_ParentId",
                schema: "Base",
                table: "DrugCategory",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Title",
                schema: "Base",
                table: "Tag",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MobileNumber",
                schema: "Base",
                table: "User",
                column: "MobileNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAttachment_UserId",
                schema: "Base",
                table: "UserAttachment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Discount_StoreId",
                schema: "Order",
                table: "Discount",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_FromAddressId",
                schema: "Order",
                table: "Order",
                column: "FromAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_StoreId",
                schema: "Order",
                table: "Order",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ToAddressId",
                schema: "Order",
                table: "Order",
                column: "ToAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserId",
                schema: "Order",
                table: "Order",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_OrderId",
                schema: "Order",
                table: "OrderDetail",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_DrugId",
                schema: "Order",
                table: "OrderDetail",
                column: "DrugId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Drug_DrugCategoryId",
                schema: "Store",
                table: "Drug",
                column: "DrugCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Drug_StoreId",
                schema: "Store",
                table: "Drug",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugAsset_DrugId",
                schema: "Store",
                table: "DrugAsset",
                column: "DrugId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugTag_DrugId",
                schema: "Store",
                table: "DrugTag",
                column: "DrugId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugTag_TagId",
                schema: "Store",
                table: "DrugTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Store_UserId",
                schema: "Store",
                table: "Store",
                column: "UserId");
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
                name: "Notification",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "UserAttachment",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "DeliveryProvider",
                schema: "Order");

            migrationBuilder.DropTable(
                name: "Discount",
                schema: "Order");

            migrationBuilder.DropTable(
                name: "OrderDetail",
                schema: "Order");

            migrationBuilder.DropTable(
                name: "Payment",
                schema: "Payment");

            migrationBuilder.DropTable(
                name: "DrugAsset",
                schema: "Store");

            migrationBuilder.DropTable(
                name: "DrugTag",
                schema: "Store");

            migrationBuilder.DropTable(
                name: "Order",
                schema: "Order");

            migrationBuilder.DropTable(
                name: "PaymentGateway",
                schema: "Payment");

            migrationBuilder.DropTable(
                name: "Drug",
                schema: "Store");

            migrationBuilder.DropTable(
                name: "Tag",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "Address",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "DrugCategory",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "Store",
                schema: "Store");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Base");
        }
    }
}
