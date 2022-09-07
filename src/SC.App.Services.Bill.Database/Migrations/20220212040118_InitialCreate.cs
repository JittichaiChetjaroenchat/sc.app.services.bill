using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SC.App.Services.Bill.Database.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "bill_channels",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    code = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    index = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bill_channels", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "bill_configurations",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    channel_id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    current_no = table.Column<int>(type: "int", nullable: false),
                    created_by = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bill_configurations", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "bill_payment_types",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    code = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    index = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bill_payment_types", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "bill_statuses",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    code = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    index = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bill_statuses", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "parcel_statuses",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    code = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    index = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_parcel_statuses", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "payment_statuses",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    code = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    index = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_statuses", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "payment_verification_statuses",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    code = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    index = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_verification_statuses", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    channel_id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    color = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_by = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "now()"),
                    updated_by = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "bills",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    channel_id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    bill_no = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    running_no = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    bill_channel_id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    bill_status_id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_deposit = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    is_new_customer = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    remark = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    key = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_by = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "now()"),
                    updated_by = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bills", x => x.id);
                    table.ForeignKey(
                        name: "FK_bills_bill_channels_bill_channel_id",
                        column: x => x.bill_channel_id,
                        principalTable: "bill_channels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bills_bill_statuses_bill_status_id",
                        column: x => x.bill_status_id,
                        principalTable: "bill_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "bill_discounts",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    bill_id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    created_by = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "now()"),
                    updated_by = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bill_discounts", x => x.id);
                    table.ForeignKey(
                        name: "FK_bill_discounts_bills_bill_id",
                        column: x => x.bill_id,
                        principalTable: "bills",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "bill_notifications",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    bill_id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_issue_notified = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    issue_notified_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    can_notify_issue = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    is_before_cancel_notified = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    before_cancel_notified_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    can_notify_before_cancel = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    is_cancel_notified = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    cancel_notified_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    can_notify_cancel = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    is_summary_notified = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    summary_notified_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    can_notify_summary = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    created_by = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "now()"),
                    updated_by = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bill_notifications", x => x.id);
                    table.ForeignKey(
                        name: "FK_bill_notifications_bills_bill_id",
                        column: x => x.bill_id,
                        principalTable: "bills",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "bill_payments",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    bill_id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    bill_payment_type_id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    has_cod_addon = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    cod_addon_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    cod_addon_percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    has_vat = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    included_vat = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    vat_percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    created_by = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "now()"),
                    updated_by = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bill_payments", x => x.id);
                    table.ForeignKey(
                        name: "FK_bill_payments_bill_payment_types_bill_payment_type_id",
                        column: x => x.bill_payment_type_id,
                        principalTable: "bill_payment_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bill_payments_bills_bill_id",
                        column: x => x.bill_id,
                        principalTable: "bills",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "bill_recipients",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    bill_id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    customer_id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    alias_name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_by = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "now()"),
                    updated_by = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bill_recipients", x => x.id);
                    table.ForeignKey(
                        name: "FK_bill_recipients_bills_bill_id",
                        column: x => x.bill_id,
                        principalTable: "bills",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "bill_shippings",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    bill_id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_by = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "now()"),
                    updated_by = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bill_shippings", x => x.id);
                    table.ForeignKey(
                        name: "FK_bill_shippings_bills_bill_id",
                        column: x => x.bill_id,
                        principalTable: "bills",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "bill_tags",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    bill_id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tag_id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_by = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "now()"),
                    updated_by = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bill_tags", x => x.id);
                    table.ForeignKey(
                        name: "FK_bill_tags_bills_bill_id",
                        column: x => x.bill_id,
                        principalTable: "bills",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bill_tags_tags_tag_id",
                        column: x => x.tag_id,
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "parcels",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    bill_id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    parcel_status_id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    remark = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "now()"),
                    is_printed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    is_packed = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_parcels", x => x.id);
                    table.ForeignKey(
                        name: "FK_parcels_bills_bill_id",
                        column: x => x.bill_id,
                        principalTable: "bills",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_parcels_parcel_statuses_parcel_status_id",
                        column: x => x.parcel_status_id,
                        principalTable: "parcel_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    bill_id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    payment_no = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    evidence_id = table.Column<string>(type: "varchar(36)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    pay_on = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "now()"),
                    remark = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    payment_status_id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.id);
                    table.ForeignKey(
                        name: "FK_payments_bills_bill_id",
                        column: x => x.bill_id,
                        principalTable: "bills",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_payments_payment_statuses_payment_status_id",
                        column: x => x.payment_status_id,
                        principalTable: "payment_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "bill_recipient_contacts",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    bill_recipient_id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    address = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    area_id = table.Column<string>(type: "varchar(36)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    primary_phone = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    secondary_phone = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_by = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "now()"),
                    updated_by = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bill_recipient_contacts", x => x.id);
                    table.ForeignKey(
                        name: "FK_bill_recipient_contacts_bill_recipients_bill_recipient_id",
                        column: x => x.bill_recipient_id,
                        principalTable: "bill_recipients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "bill_shipping_free_rules",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    bill_shipping_id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    amount = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    enabled = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bill_shipping_free_rules", x => x.id);
                    table.ForeignKey(
                        name: "FK_bill_shipping_free_rules_bill_shippings_bill_shipping_id",
                        column: x => x.bill_shipping_id,
                        principalTable: "bill_shippings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "bill_shipping_range_rules",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    bill_shipping_id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    enabled = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bill_shipping_range_rules", x => x.id);
                    table.ForeignKey(
                        name: "FK_bill_shipping_range_rules_bill_shippings_bill_shipping_id",
                        column: x => x.bill_shipping_id,
                        principalTable: "bill_shippings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "bill_shipping_total_rules",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    bill_shipping_id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    enabled = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bill_shipping_total_rules", x => x.id);
                    table.ForeignKey(
                        name: "FK_bill_shipping_total_rules_bill_shippings_bill_shipping_id",
                        column: x => x.bill_shipping_id,
                        principalTable: "bill_shippings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "payment_verifications",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    payment_id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_proceed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    can_verifiy = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    is_unique = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    duplicate_to = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_correct_bank_account_number = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    is_correct_bank_account_name = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    is_correct_amount = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    unbalance_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    remark = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    payment_verification_status_id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_verifications", x => x.id);
                    table.ForeignKey(
                        name: "FK_payment_verifications_payments_payment_id",
                        column: x => x.payment_id,
                        principalTable: "payments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_payment_verifications_payment_verification_statuses_payment_~",
                        column: x => x.payment_verification_status_id,
                        principalTable: "payment_verification_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "bill_shipping_ranges",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    bill_shipping_range_rule_id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    begin = table.Column<int>(type: "int", nullable: false),
                    end = table.Column<int>(type: "int", nullable: false),
                    cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bill_shipping_ranges", x => x.id);
                    table.ForeignKey(
                        name: "FK_bill_shipping_ranges_bill_shipping_range_rules_bill_shipping~",
                        column: x => x.bill_shipping_range_rule_id,
                        principalTable: "bill_shipping_range_rules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "payment_verification_details",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    payment_verification_id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    source_bank_code = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    source_bank_account_type = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    source_bank_account_number = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    source_bank_account_name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    source_bank_account_display_name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    destination_bank_code = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    destination_bank_account_type = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    destination_bank_account_number = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    destination_bank_account_name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    destination_bank_account_display_name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    transaction_ref_no = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    transaction_date = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_verification_details", x => x.id);
                    table.ForeignKey(
                        name: "FK_payment_verification_details_payment_verifications_payment_v~",
                        column: x => x.payment_verification_id,
                        principalTable: "payment_verifications",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "bill_channels",
                columns: new[] { "id", "code", "description", "index" },
                values: new object[,]
                {
                    { "5413a227-1d32-c2fb-8f0c-b17b995bb9ca", "offline", "Offline", 1 },
                    { "71e7ca26-328c-0a18-7a0f-8e19d6d40a59", "facebook", "Facebook", 2 }
                });

            migrationBuilder.InsertData(
                table: "bill_payment_types",
                columns: new[] { "id", "code", "description", "index" },
                values: new object[,]
                {
                    { "3c704e83-57fc-65d5-c394-f2a3b2e71261", "prepaid", "Pre Paid", 1 },
                    { "896ce2a0-e070-722d-352c-3de9c2bfc824", "postpaid", "Post Paid", 2 }
                });

            migrationBuilder.InsertData(
                table: "bill_statuses",
                columns: new[] { "id", "code", "description", "index" },
                values: new object[,]
                {
                    { "561f110d-4996-f585-62aa-aaf9d2980787", "archived", "Archived", 7 },
                    { "51ed2d6b-1ad8-0344-d8a4-bd25fa1e57ee", "done", "Done", 6 },
                    { "0a1e8838-0324-c29d-621e-1d6f86cb71f7", "cancelled", "Cancelled", 5 },
                    { "0b2f60da-2f16-bfcc-6b15-0cfcfc7a7379", "deleted", "Deleted", 8 },
                    { "6d7d53c7-ec48-61f2-749c-09a9f284bd45", "rejected", "Rejected", 3 },
                    { "34ad59e5-3d90-713d-7a14-88eaeed707f1", "notified", "Notified", 2 },
                    { "5d2e6c7c-ab48-a037-07cb-f70d3ea25fa4", "pending", "Pending", 1 },
                    { "c521a7ed-c6f8-ae1d-e37c-a15ce3c4d881", "confirmed", "Confirmed", 4 }
                });

            migrationBuilder.InsertData(
                table: "parcel_statuses",
                columns: new[] { "id", "code", "description", "index" },
                values: new object[,]
                {
                    { "845e6ac7-bde4-52ee-7e27-4ea30c680d79", "active", "Active", 1 },
                    { "0a1e8838-0324-c29d-621e-1d6f86cb71f7", "cancelled", "Cancelled", 2 }
                });

            migrationBuilder.InsertData(
                table: "payment_statuses",
                columns: new[] { "id", "code", "description", "index" },
                values: new object[,]
                {
                    { "5d2e6c7c-ab48-a037-07cb-f70d3ea25fa4", "pending", "Pending", 1 },
                    { "6d7d53c7-ec48-61f2-749c-09a9f284bd45", "rejected", "Rejected", 2 },
                    { "1a894d3e-f35d-d0d6-d7dd-9432a1bc6470", "accepted", "Accepted", 3 }
                });

            migrationBuilder.InsertData(
                table: "payment_verification_statuses",
                columns: new[] { "id", "code", "description", "index" },
                values: new object[,]
                {
                    { "d7a457ab-7396-121e-e7ef-27325fe02a06", "incorrect_amount", "Incorrect amount", 6 },
                    { "fb69709b-ec60-ded2-0471-8697313843b2", "not_verify", "Not verify", 1 },
                    { "1d33e72c-5f37-9c3e-9242-461d0de6dd67", "unverifiable", "Unverifiable", 2 },
                    { "a7b0f124-7394-0c25-195c-7fb84e393392", "duplicate", "Duplicate", 3 },
                    { "54920ea8-96a1-a435-60c3-72ab58b8201b", "incorrect_bank_account_number", "Incorrect bank account's number", 4 },
                    { "d48c7861-fb48-a078-8b11-21f950ccad2d", "incorrect_bank_account_name", "Incorrect bank account's name", 5 },
                    { "2aa83a72-c283-d578-e7e7-be9b109b406a", "verified", "Verified", 7 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_bill_channels_code",
                table: "bill_channels",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bill_configurations_channel_id",
                table: "bill_configurations",
                column: "channel_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bill_discounts_bill_id",
                table: "bill_discounts",
                column: "bill_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bill_notifications_bill_id",
                table: "bill_notifications",
                column: "bill_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bill_payments_bill_id",
                table: "bill_payments",
                column: "bill_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bill_payments_bill_payment_type_id",
                table: "bill_payments",
                column: "bill_payment_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_bill_payment_types_code",
                table: "bill_payment_types",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bill_recipient_contacts_bill_recipient_id",
                table: "bill_recipient_contacts",
                column: "bill_recipient_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bill_recipients_bill_id",
                table: "bill_recipients",
                column: "bill_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bills_bill_channel_id",
                table: "bills",
                column: "bill_channel_id");

            migrationBuilder.CreateIndex(
                name: "IX_bills_bill_status_id",
                table: "bills",
                column: "bill_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_bills_channel_id_bill_no",
                table: "bills",
                columns: new[] { "channel_id", "bill_no" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bill_shipping_free_rules_bill_shipping_id",
                table: "bill_shipping_free_rules",
                column: "bill_shipping_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bill_shipping_range_rules_bill_shipping_id",
                table: "bill_shipping_range_rules",
                column: "bill_shipping_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bill_shipping_ranges_bill_shipping_range_rule_id",
                table: "bill_shipping_ranges",
                column: "bill_shipping_range_rule_id");

            migrationBuilder.CreateIndex(
                name: "IX_bill_shippings_bill_id",
                table: "bill_shippings",
                column: "bill_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bill_shipping_total_rules_bill_shipping_id",
                table: "bill_shipping_total_rules",
                column: "bill_shipping_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bill_statuses_code",
                table: "bill_statuses",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bill_tags_bill_id_tag_id",
                table: "bill_tags",
                columns: new[] { "bill_id", "tag_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bill_tags_tag_id",
                table: "bill_tags",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "IX_parcels_bill_id",
                table: "parcels",
                column: "bill_id");

            migrationBuilder.CreateIndex(
                name: "IX_parcels_parcel_status_id",
                table: "parcels",
                column: "parcel_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_parcel_statuses_code",
                table: "parcel_statuses",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_payments_bill_id_payment_no",
                table: "payments",
                columns: new[] { "bill_id", "payment_no" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_payments_payment_status_id",
                table: "payments",
                column: "payment_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_payment_statuses_code",
                table: "payment_statuses",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_payment_verification_details_payment_verification_id",
                table: "payment_verification_details",
                column: "payment_verification_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_payment_verifications_payment_id",
                table: "payment_verifications",
                column: "payment_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_payment_verifications_payment_verification_status_id",
                table: "payment_verifications",
                column: "payment_verification_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_payment_verification_statuses_code",
                table: "payment_verification_statuses",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tags_channel_id_name",
                table: "tags",
                columns: new[] { "channel_id", "name" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bill_configurations");

            migrationBuilder.DropTable(
                name: "bill_discounts");

            migrationBuilder.DropTable(
                name: "bill_notifications");

            migrationBuilder.DropTable(
                name: "bill_payments");

            migrationBuilder.DropTable(
                name: "bill_recipient_contacts");

            migrationBuilder.DropTable(
                name: "bill_shipping_free_rules");

            migrationBuilder.DropTable(
                name: "bill_shipping_ranges");

            migrationBuilder.DropTable(
                name: "bill_shipping_total_rules");

            migrationBuilder.DropTable(
                name: "bill_tags");

            migrationBuilder.DropTable(
                name: "parcels");

            migrationBuilder.DropTable(
                name: "payment_verification_details");

            migrationBuilder.DropTable(
                name: "bill_payment_types");

            migrationBuilder.DropTable(
                name: "bill_recipients");

            migrationBuilder.DropTable(
                name: "bill_shipping_range_rules");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "parcel_statuses");

            migrationBuilder.DropTable(
                name: "payment_verifications");

            migrationBuilder.DropTable(
                name: "bill_shippings");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "payment_verification_statuses");

            migrationBuilder.DropTable(
                name: "bills");

            migrationBuilder.DropTable(
                name: "payment_statuses");

            migrationBuilder.DropTable(
                name: "bill_channels");

            migrationBuilder.DropTable(
                name: "bill_statuses");
        }
    }
}
