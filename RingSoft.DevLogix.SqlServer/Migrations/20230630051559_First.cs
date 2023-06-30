using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class First : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdvancedFinds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Table = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FromFormula = table.Column<string>(type: "ntext", nullable: true),
                    RefreshRate = table.Column<byte>(type: "tinyint", nullable: true),
                    RefreshValue = table.Column<int>(type: "integer", nullable: true),
                    RefreshCondition = table.Column<byte>(type: "tinyint", nullable: true),
                    YellowAlert = table.Column<int>(type: "integer", nullable: true),
                    RedAlert = table.Column<int>(type: "integer", nullable: true),
                    Disabled = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvancedFinds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DevLogixCharts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevLogixCharts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ErrorPriorities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorPriorities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ErrorStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Rights = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LaborParts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MinutesCost = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    Comment = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaborParts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaterialParts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Cost = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    Comment = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialParts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecordLocks",
                columns: table => new
                {
                    Table = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PrimaryKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LockDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    User = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordLocks", x => new { x.Table, x.PrimaryKey });
                });

            migrationBuilder.CreateTable(
                name: "SystemMaster",
                columns: table => new
                {
                    OrganizationName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemMaster", x => x.OrganizationName);
                });

            migrationBuilder.CreateTable(
                name: "SystemPreferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemPreferences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestingTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BaseTemplateId = table.Column<int>(type: "integer", nullable: true),
                    Notes = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestingTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestingTemplates_TestingTemplates_BaseTemplateId",
                        column: x => x.BaseTemplateId,
                        principalTable: "TestingTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TimeZone",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HourToGMT = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeZone", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserTracker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RefreshInterval = table.Column<int>(type: "integer", nullable: false),
                    RefreshType = table.Column<byte>(type: "tinyint", nullable: false),
                    RedMinutes = table.Column<decimal>(type: "numeric(38,17)", nullable: true),
                    YellowMinutes = table.Column<decimal>(type: "numeric(38,17)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTracker", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AdvancedFindColumns",
                columns: table => new
                {
                    AdvancedFindId = table.Column<int>(type: "integer", nullable: false),
                    ColumnId = table.Column<int>(type: "integer", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FieldName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PrimaryTableName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PrimaryFieldName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Path = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Caption = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PercentWidth = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    Formula = table.Column<string>(type: "ntext", nullable: true),
                    FieldDataType = table.Column<byte>(type: "tinyint", nullable: false),
                    DecimalFormatType = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvancedFindColumns", x => new { x.AdvancedFindId, x.ColumnId });
                    table.ForeignKey(
                        name: "FK_AdvancedFindColumns_AdvancedFinds_AdvancedFindId",
                        column: x => x.AdvancedFindId,
                        principalTable: "AdvancedFinds",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AdvancedFindFilters",
                columns: table => new
                {
                    AdvancedFindId = table.Column<int>(type: "integer", nullable: false),
                    FilterId = table.Column<int>(type: "integer", nullable: false),
                    LeftParentheses = table.Column<byte>(type: "tinyint", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FieldName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PrimaryTableName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PrimaryFieldName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Path = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Operand = table.Column<byte>(type: "tinyint", nullable: false),
                    SearchForValue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Formula = table.Column<string>(type: "ntext", nullable: true),
                    FormulaDataType = table.Column<byte>(type: "tinyint", nullable: false),
                    FormulaDisplayValue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SearchForAdvancedFindId = table.Column<int>(type: "integer", nullable: true),
                    CustomDate = table.Column<bool>(type: "bit", nullable: false),
                    RightParentheses = table.Column<byte>(type: "tinyint", nullable: false),
                    EndLogic = table.Column<byte>(type: "tinyint", nullable: false),
                    DateFilterType = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvancedFindFilters", x => new { x.AdvancedFindId, x.FilterId });
                    table.ForeignKey(
                        name: "FK_AdvancedFindFilters_AdvancedFinds_AdvancedFindId",
                        column: x => x.AdvancedFindId,
                        principalTable: "AdvancedFinds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdvancedFindFilters_AdvancedFinds_SearchForAdvancedFindId",
                        column: x => x.SearchForAdvancedFindId,
                        principalTable: "AdvancedFinds",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DevLogixChartsBars",
                columns: table => new
                {
                    ChartId = table.Column<int>(type: "integer", nullable: false),
                    BarId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AdvancedFindId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevLogixChartsBars", x => new { x.ChartId, x.BarId });
                    table.ForeignKey(
                        name: "FK_DevLogixChartsBars_DevLogixCharts_ChartId",
                        column: x => x.ChartId,
                        principalTable: "DevLogixCharts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ErrorFixStatusId = table.Column<int>(type: "integer", nullable: true),
                    ErrorPassStatusId = table.Column<int>(type: "integer", nullable: true),
                    ErrorFailStatusId = table.Column<int>(type: "integer", nullable: true),
                    FixText = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PassText = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FailText = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FtpAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FtpUsername = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FtpPassword = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Notes = table.Column<string>(type: "ntext", nullable: true),
                    ReleaseLevel = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_ErrorStatuses_ErrorFailStatusId",
                        column: x => x.ErrorFailStatusId,
                        principalTable: "ErrorStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Departments_ErrorStatuses_ErrorFixStatusId",
                        column: x => x.ErrorFixStatusId,
                        principalTable: "ErrorStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Departments_ErrorStatuses_ErrorPassStatusId",
                        column: x => x.ErrorPassStatusId,
                        principalTable: "ErrorStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SystemPreferencesHolidays",
                columns: table => new
                {
                    SystemPreferencesId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemPreferencesHolidays", x => new { x.SystemPreferencesId, x.Date });
                    table.ForeignKey(
                        name: "FK_SystemPreferencesHolidays_SystemPreferences_SystemPreferencesId",
                        column: x => x.SystemPreferencesId,
                        principalTable: "SystemPreferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestingTemplateItems",
                columns: table => new
                {
                    TestingTemplateId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestingTemplateItems", x => new { x.TestingTemplateId, x.Description });
                    table.ForeignKey(
                        name: "FK_TestingTemplateItems_TestingTemplates_TestingTemplateId",
                        column: x => x.TestingTemplateId,
                        principalTable: "TestingTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    InstallerFileName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ArchivePath = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AppGuid = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreateDepartmentId = table.Column<int>(type: "integer", nullable: false),
                    ArchiveDepartmentId = table.Column<int>(type: "integer", nullable: true),
                    Notes = table.Column<string>(type: "ntext", nullable: true),
                    Price = table.Column<decimal>(type: "numeric(38,17)", nullable: true),
                    Revenue = table.Column<decimal>(type: "numeric(38,17)", nullable: true),
                    Cost = table.Column<decimal>(type: "numeric(38,17)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Departments_ArchiveDepartmentId",
                        column: x => x.ArchiveDepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Products_Departments_CreateDepartmentId",
                        column: x => x.CreateDepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DepartmentId = table.Column<int>(type: "integer", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Rights = table.Column<string>(type: "ntext", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Notes = table.Column<string>(type: "ntext", nullable: true),
                    DefaultChartId = table.Column<int>(type: "integer", nullable: true),
                    ClockDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    SupervisorId = table.Column<int>(type: "integer", nullable: true),
                    HourlyRate = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    BillableProjectsMinutesSpent = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    NonBillableProjectsMinutesSpent = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    ErrorsMinutesSpent = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    TestingOutlinesMinutesSpent = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    ClockOutReason = table.Column<byte>(type: "tinyint", nullable: false),
                    OtherClockOutReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_DevLogixCharts_DefaultChartId",
                        column: x => x.DefaultChartId,
                        principalTable: "DevLogixCharts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Users_SupervisorId",
                        column: x => x.SupervisorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductVersions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Notes = table.Column<string>(type: "ntext", nullable: true),
                    ArchiveDateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DepartmentId = table.Column<int>(type: "integer", nullable: true),
                    VersionDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVersions_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductVersions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ManagerId = table.Column<int>(type: "integer", nullable: false),
                    ContractCost = table.Column<decimal>(type: "numeric(38,17)", nullable: true),
                    Deadline = table.Column<DateTime>(type: "datetime", nullable: false),
                    OriginalDeadline = table.Column<DateTime>(type: "datetime", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: true),
                    SundayMinutes = table.Column<decimal>(type: "numeric(38,17)", nullable: true),
                    MondayMinutes = table.Column<decimal>(type: "numeric(38,17)", nullable: true),
                    TuesdayMinutes = table.Column<decimal>(type: "numeric(38,17)", nullable: true),
                    WednesdayMinutes = table.Column<decimal>(type: "numeric(38,17)", nullable: true),
                    ThursdayMinutes = table.Column<decimal>(type: "numeric(38,17)", nullable: true),
                    FridayMinutes = table.Column<decimal>(type: "numeric(38,17)", nullable: true),
                    SaturdayMinutes = table.Column<decimal>(type: "numeric(38,17)", nullable: true),
                    Notes = table.Column<string>(type: "ntext", nullable: true),
                    IsBillable = table.Column<bool>(type: "bit", nullable: false),
                    EstimatedMinutes = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    EstimatedCost = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    MinutesSpent = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    Cost = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Projects_Users_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Territory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SalespersonId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Territory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Territory_Users_SalespersonId",
                        column: x => x.SalespersonId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestingOutlines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "integer", nullable: false),
                    AssignedToUserId = table.Column<int>(type: "integer", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    PercentComplete = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    MinutesSpent = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    TotalCost = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    Notes = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestingOutlines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestingOutlines_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestingOutlines_Users_AssignedToUserId",
                        column: x => x.AssignedToUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestingOutlines_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsersGroups",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    GroupId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersGroups", x => new { x.UserId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_UsersGroups_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersGroups_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsersTimeOff",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    RowId = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersTimeOff", x => new { x.UserId, x.RowId });
                    table.ForeignKey(
                        name: "FK_UsersTimeOff_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserTrackerUsers",
                columns: table => new
                {
                    UserTrackerId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTrackerUsers", x => new { x.UserTrackerId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserTrackerUsers_UserTracker_UserTrackerId",
                        column: x => x.UserTrackerId,
                        principalTable: "UserTracker",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTrackerUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductVersionDepartments",
                columns: table => new
                {
                    VersionId = table.Column<int>(type: "integer", nullable: false),
                    DepartmentId = table.Column<int>(type: "integer", nullable: false),
                    ReleaseDateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVersionDepartments", x => new { x.VersionId, x.DepartmentId });
                    table.ForeignKey(
                        name: "FK_ProductVersionDepartments_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductVersionDepartments_ProductVersions_VersionId",
                        column: x => x.VersionId,
                        principalTable: "ProductVersions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    Cost = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    IsCostEdited = table.Column<bool>(type: "bit", nullable: false),
                    ActualCost = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    Notes = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectMaterials_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    MinutesCost = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    MinutesEdited = table.Column<bool>(type: "bit", nullable: false),
                    EstimatedCost = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    MinutesSpent = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    Cost = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    PercentComplete = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    HourlyRate = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    Notes = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectTasks_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectTasks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectUsers",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    MinutesSpent = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    Cost = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    IsStandard = table.Column<bool>(type: "bit", nullable: false),
                    SundayMinutes = table.Column<decimal>(type: "numeric(38,17)", nullable: true),
                    MondayMinutes = table.Column<decimal>(type: "numeric(38,17)", nullable: true),
                    TuesdayMinutes = table.Column<decimal>(type: "numeric(38,17)", nullable: true),
                    WednesdayMinutes = table.Column<decimal>(type: "numeric(38,17)", nullable: true),
                    ThursdayMinutes = table.Column<decimal>(type: "numeric(38,17)", nullable: true),
                    FridayMinutes = table.Column<decimal>(type: "numeric(38,17)", nullable: true),
                    SaturdayMinutes = table.Column<decimal>(type: "numeric(38,17)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUsers", x => new { x.ProjectId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ProjectUsers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ContactName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContactTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Region = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TimeZoneId = table.Column<int>(type: "integer", nullable: false),
                    TerritoryId = table.Column<int>(type: "integer", nullable: false),
                    SupportMinutesPurchased = table.Column<decimal>(type: "numeric(38,17)", nullable: true),
                    SupportMinutesSpent = table.Column<decimal>(type: "numeric(38,17)", nullable: true),
                    SupportCost = table.Column<decimal>(type: "numeric(38,17)", nullable: true),
                    Notes = table.Column<string>(type: "ntext", nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    WebAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TotalSales = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    TotalCost = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    MinutesCost = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customer_Territory_TerritoryId",
                        column: x => x.TerritoryId,
                        principalTable: "Territory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customer_TimeZone_TimeZoneId",
                        column: x => x.TimeZoneId,
                        principalTable: "TimeZone",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customer_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Errors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ErrorId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ErrorDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ErrorStatusId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    ErrorPriorityId = table.Column<int>(type: "integer", nullable: false),
                    FoundVersionId = table.Column<int>(type: "integer", nullable: false),
                    FoundByUserId = table.Column<int>(type: "integer", nullable: true),
                    FixedVersionId = table.Column<int>(type: "integer", nullable: true),
                    AssignedDeveloperId = table.Column<int>(type: "integer", nullable: true),
                    AssignedTesterId = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "ntext", nullable: false),
                    Resolution = table.Column<string>(type: "ntext", nullable: true),
                    MinutesSpent = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    Cost = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    TestingOutlineId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Errors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Errors_ErrorPriorities_ErrorPriorityId",
                        column: x => x.ErrorPriorityId,
                        principalTable: "ErrorPriorities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Errors_ErrorStatuses_ErrorStatusId",
                        column: x => x.ErrorStatusId,
                        principalTable: "ErrorStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Errors_ProductVersions_FixedVersionId",
                        column: x => x.FixedVersionId,
                        principalTable: "ProductVersions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Errors_ProductVersions_FoundVersionId",
                        column: x => x.FoundVersionId,
                        principalTable: "ProductVersions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Errors_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Errors_TestingOutlines_TestingOutlineId",
                        column: x => x.TestingOutlineId,
                        principalTable: "TestingOutlines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Errors_Users_AssignedDeveloperId",
                        column: x => x.AssignedDeveloperId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Errors_Users_AssignedTesterId",
                        column: x => x.AssignedTesterId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Errors_Users_FoundByUserId",
                        column: x => x.FoundByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TestingOutlineCosts",
                columns: table => new
                {
                    TestingOutlineId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    TimeSpent = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    Cost = table.Column<decimal>(type: "numeric(38,17)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestingOutlineCosts", x => new { x.TestingOutlineId, x.UserId });
                    table.ForeignKey(
                        name: "FK_TestingOutlineCosts_TestingOutlines_TestingOutlineId",
                        column: x => x.TestingOutlineId,
                        principalTable: "TestingOutlines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestingOutlineCosts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestingOutlineDetails",
                columns: table => new
                {
                    TestingOutlineId = table.Column<int>(type: "integer", nullable: false),
                    DetailId = table.Column<int>(type: "integer", nullable: false),
                    Step = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false),
                    CompletedVersionId = table.Column<int>(type: "integer", nullable: true),
                    TestingTemplateId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestingOutlineDetails", x => new { x.TestingOutlineId, x.DetailId });
                    table.ForeignKey(
                        name: "FK_TestingOutlineDetails_ProductVersions_CompletedVersionId",
                        column: x => x.CompletedVersionId,
                        principalTable: "ProductVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestingOutlineDetails_TestingOutlines_TestingOutlineId",
                        column: x => x.TestingOutlineId,
                        principalTable: "TestingOutlines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestingOutlineDetails_TestingTemplates_TestingTemplateId",
                        column: x => x.TestingTemplateId,
                        principalTable: "TestingTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestingOutlineTemplates",
                columns: table => new
                {
                    TestingOutlineId = table.Column<int>(type: "integer", nullable: false),
                    TestingTemplateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestingOutlineTemplates", x => new { x.TestingOutlineId, x.TestingTemplateId });
                    table.ForeignKey(
                        name: "FK_TestingOutlineTemplates_TestingOutlines_TestingOutlineId",
                        column: x => x.TestingOutlineId,
                        principalTable: "TestingOutlines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestingOutlineTemplates_TestingTemplates_TestingTemplateId",
                        column: x => x.TestingTemplateId,
                        principalTable: "TestingTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectMaterialHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectMaterialId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    Cost = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMaterialHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectMaterialHistory_ProjectMaterials_ProjectMaterialId",
                        column: x => x.ProjectMaterialId,
                        principalTable: "ProjectMaterials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectMaterialHistory_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectMaterialParts",
                columns: table => new
                {
                    ProjectMaterialId = table.Column<int>(type: "integer", nullable: false),
                    DetailId = table.Column<int>(type: "integer", nullable: false),
                    LineType = table.Column<byte>(type: "tinyint", nullable: false),
                    RowId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ParentRowId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CommentCrLf = table.Column<bool>(type: "bit", nullable: true),
                    MaterialPartId = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Quantity = table.Column<decimal>(type: "numeric(38,17)", nullable: true),
                    Cost = table.Column<decimal>(type: "numeric(38,17)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMaterialParts", x => new { x.ProjectMaterialId, x.DetailId });
                    table.ForeignKey(
                        name: "FK_ProjectMaterialParts_MaterialParts_MaterialPartId",
                        column: x => x.MaterialPartId,
                        principalTable: "MaterialParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectMaterialParts_ProjectMaterials_ProjectMaterialId",
                        column: x => x.ProjectMaterialId,
                        principalTable: "ProjectMaterials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTaskDependency",
                columns: table => new
                {
                    ProjectTaskId = table.Column<int>(type: "integer", nullable: false),
                    DependsOnProjectTaskId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTaskDependency", x => new { x.ProjectTaskId, x.DependsOnProjectTaskId });
                    table.ForeignKey(
                        name: "FK_ProjectTaskDependency_ProjectTasks_DependsOnProjectTaskId",
                        column: x => x.DependsOnProjectTaskId,
                        principalTable: "ProjectTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectTaskDependency_ProjectTasks_ProjectTaskId",
                        column: x => x.ProjectTaskId,
                        principalTable: "ProjectTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTaskLaborParts",
                columns: table => new
                {
                    ProjectTaskId = table.Column<int>(type: "integer", nullable: false),
                    DetailId = table.Column<int>(type: "integer", nullable: false),
                    LineType = table.Column<byte>(type: "tinyint", nullable: false),
                    RowId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ParentRowId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CommentCrLf = table.Column<bool>(type: "bit", nullable: true),
                    LaborPartId = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Quantity = table.Column<decimal>(type: "numeric(38,17)", nullable: true),
                    MinutesCost = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    Complete = table.Column<bool>(type: "bit", nullable: false),
                    PercentComplete = table.Column<decimal>(type: "numeric(38,17)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTaskLaborParts", x => new { x.ProjectTaskId, x.DetailId });
                    table.ForeignKey(
                        name: "FK_ProjectTaskLaborParts_LaborParts_LaborPartId",
                        column: x => x.LaborPartId,
                        principalTable: "LaborParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectTaskLaborParts_ProjectTasks_ProjectTaskId",
                        column: x => x.ProjectTaskId,
                        principalTable: "ProjectTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerComputer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OperatingSystem = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Speed = table.Column<decimal>(type: "numeric(38,17)", nullable: true),
                    ScreenResolution = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RamSize = table.Column<int>(type: "integer", nullable: true),
                    HardDriveSize = table.Column<int>(type: "integer", nullable: true),
                    HardDriveFree = table.Column<int>(type: "integer", nullable: true),
                    InternetSpeed = table.Column<int>(type: "integer", nullable: true),
                    DatabasePlatform = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Printer = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Notes = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerComputer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerComputer_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerProduct",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerProduct", x => new { x.CustomerId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_CustomerProduct_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerProduct_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ShippedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ContactName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContactTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Region = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SubTotal = table.Column<decimal>(type: "numeric(38,17)", nullable: true),
                    TotalDiscount = table.Column<decimal>(type: "numeric(38,17)", nullable: true),
                    Freight = table.Column<decimal>(type: "numeric(38,17)", nullable: true),
                    Total = table.Column<decimal>(type: "numeric(38,17)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ErrorDevelopers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ErrorId = table.Column<int>(type: "integer", nullable: false),
                    DeveloperId = table.Column<int>(type: "integer", nullable: false),
                    DateFixed = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorDevelopers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ErrorDevelopers_Errors_ErrorId",
                        column: x => x.ErrorId,
                        principalTable: "Errors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ErrorDevelopers_Users_DeveloperId",
                        column: x => x.DeveloperId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ErrorTesters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ErrorId = table.Column<int>(type: "integer", nullable: false),
                    TesterId = table.Column<int>(type: "integer", nullable: false),
                    NewStatusId = table.Column<int>(type: "integer", nullable: false),
                    DateChanged = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorTesters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ErrorTesters_ErrorStatuses_NewStatusId",
                        column: x => x.NewStatusId,
                        principalTable: "ErrorStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ErrorTesters_Errors_ErrorId",
                        column: x => x.ErrorId,
                        principalTable: "Errors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ErrorTesters_Users_TesterId",
                        column: x => x.TesterId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ErrorUsers",
                columns: table => new
                {
                    ErrorId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    MinutesSpent = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    Cost = table.Column<decimal>(type: "numeric(38,17)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorUsers", x => new { x.ErrorId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ErrorUsers_Errors_ErrorId",
                        column: x => x.ErrorId,
                        principalTable: "Errors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ErrorUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TimeClocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PunchInDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    PunchOutDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    MinutesSpent = table.Column<decimal>(type: "numeric(38,17)", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ErrorId = table.Column<int>(type: "integer", nullable: true),
                    ProjectTaskId = table.Column<int>(type: "integer", nullable: true),
                    TestingOutlineId = table.Column<int>(type: "integer", nullable: true),
                    CustomerId = table.Column<int>(type: "integer", nullable: true),
                    Notes = table.Column<string>(type: "ntext", nullable: true),
                    AreDatesEdited = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeClocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeClocks_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TimeClocks_Errors_ErrorId",
                        column: x => x.ErrorId,
                        principalTable: "Errors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TimeClocks_ProjectTasks_ProjectTaskId",
                        column: x => x.ProjectTaskId,
                        principalTable: "ProjectTasks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TimeClocks_TestingOutlines_TestingOutlineId",
                        column: x => x.TestingOutlineId,
                        principalTable: "TestingOutlines",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TimeClocks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderDetail",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    DetailId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    ExtendedPrice = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    Discount = table.Column<decimal>(type: "numeric(38,17)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetail", x => new { x.OrderId, x.DetailId });
                    table.ForeignKey(
                        name: "FK_OrderDetail_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdvancedFindFilters_SearchForAdvancedFindId",
                table: "AdvancedFindFilters",
                column: "SearchForAdvancedFindId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_TerritoryId",
                table: "Customer",
                column: "TerritoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_TimeZoneId",
                table: "Customer",
                column: "TimeZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_UserId",
                table: "Customer",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerComputer_CustomerId",
                table: "CustomerComputer",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProduct_ProductId",
                table: "CustomerProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ErrorFailStatusId",
                table: "Departments",
                column: "ErrorFailStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ErrorFixStatusId",
                table: "Departments",
                column: "ErrorFixStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ErrorPassStatusId",
                table: "Departments",
                column: "ErrorPassStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ErrorDevelopers_DeveloperId",
                table: "ErrorDevelopers",
                column: "DeveloperId");

            migrationBuilder.CreateIndex(
                name: "IX_ErrorDevelopers_ErrorId",
                table: "ErrorDevelopers",
                column: "ErrorId");

            migrationBuilder.CreateIndex(
                name: "IX_Errors_AssignedDeveloperId",
                table: "Errors",
                column: "AssignedDeveloperId");

            migrationBuilder.CreateIndex(
                name: "IX_Errors_AssignedTesterId",
                table: "Errors",
                column: "AssignedTesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Errors_ErrorPriorityId",
                table: "Errors",
                column: "ErrorPriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_Errors_ErrorStatusId",
                table: "Errors",
                column: "ErrorStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Errors_FixedVersionId",
                table: "Errors",
                column: "FixedVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Errors_FoundByUserId",
                table: "Errors",
                column: "FoundByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Errors_FoundVersionId",
                table: "Errors",
                column: "FoundVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Errors_ProductId",
                table: "Errors",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Errors_TestingOutlineId",
                table: "Errors",
                column: "TestingOutlineId");

            migrationBuilder.CreateIndex(
                name: "IX_ErrorTesters_ErrorId",
                table: "ErrorTesters",
                column: "ErrorId");

            migrationBuilder.CreateIndex(
                name: "IX_ErrorTesters_NewStatusId",
                table: "ErrorTesters",
                column: "NewStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ErrorTesters_TesterId",
                table: "ErrorTesters",
                column: "TesterId");

            migrationBuilder.CreateIndex(
                name: "IX_ErrorUsers_UserId",
                table: "ErrorUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CustomerId",
                table: "Order",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_ProductId",
                table: "OrderDetail",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ArchiveDepartmentId",
                table: "Products",
                column: "ArchiveDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CreateDepartmentId",
                table: "Products",
                column: "CreateDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVersionDepartments_DepartmentId",
                table: "ProductVersionDepartments",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVersions_DepartmentId",
                table: "ProductVersions",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVersions_ProductId",
                table: "ProductVersions",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMaterialHistory_ProjectMaterialId",
                table: "ProjectMaterialHistory",
                column: "ProjectMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMaterialHistory_UserId",
                table: "ProjectMaterialHistory",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMaterialParts_MaterialPartId",
                table: "ProjectMaterialParts",
                column: "MaterialPartId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMaterials_ProjectId",
                table: "ProjectMaterials",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ManagerId",
                table: "Projects",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProductId",
                table: "Projects",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTaskDependency_DependsOnProjectTaskId",
                table: "ProjectTaskDependency",
                column: "DependsOnProjectTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTaskLaborParts_LaborPartId",
                table: "ProjectTaskLaborParts",
                column: "LaborPartId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_ProjectId",
                table: "ProjectTasks",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_UserId",
                table: "ProjectTasks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUsers_UserId",
                table: "ProjectUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Territory_SalespersonId",
                table: "Territory",
                column: "SalespersonId");

            migrationBuilder.CreateIndex(
                name: "IX_TestingOutlineCosts_UserId",
                table: "TestingOutlineCosts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TestingOutlineDetails_CompletedVersionId",
                table: "TestingOutlineDetails",
                column: "CompletedVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_TestingOutlineDetails_TestingTemplateId",
                table: "TestingOutlineDetails",
                column: "TestingTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_TestingOutlines_AssignedToUserId",
                table: "TestingOutlines",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TestingOutlines_CreatedByUserId",
                table: "TestingOutlines",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TestingOutlines_ProductId",
                table: "TestingOutlines",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TestingOutlineTemplates_TestingTemplateId",
                table: "TestingOutlineTemplates",
                column: "TestingTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_TestingTemplates_BaseTemplateId",
                table: "TestingTemplates",
                column: "BaseTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeClocks_CustomerId",
                table: "TimeClocks",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeClocks_ErrorId",
                table: "TimeClocks",
                column: "ErrorId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeClocks_ProjectTaskId",
                table: "TimeClocks",
                column: "ProjectTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeClocks_TestingOutlineId",
                table: "TimeClocks",
                column: "TestingOutlineId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeClocks_UserId",
                table: "TimeClocks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DefaultChartId",
                table: "Users",
                column: "DefaultChartId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DepartmentId",
                table: "Users",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_SupervisorId",
                table: "Users",
                column: "SupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersGroups_GroupId",
                table: "UsersGroups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTrackerUsers_UserId",
                table: "UserTrackerUsers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvancedFindColumns");

            migrationBuilder.DropTable(
                name: "AdvancedFindFilters");

            migrationBuilder.DropTable(
                name: "CustomerComputer");

            migrationBuilder.DropTable(
                name: "CustomerProduct");

            migrationBuilder.DropTable(
                name: "DevLogixChartsBars");

            migrationBuilder.DropTable(
                name: "ErrorDevelopers");

            migrationBuilder.DropTable(
                name: "ErrorTesters");

            migrationBuilder.DropTable(
                name: "ErrorUsers");

            migrationBuilder.DropTable(
                name: "OrderDetail");

            migrationBuilder.DropTable(
                name: "ProductVersionDepartments");

            migrationBuilder.DropTable(
                name: "ProjectMaterialHistory");

            migrationBuilder.DropTable(
                name: "ProjectMaterialParts");

            migrationBuilder.DropTable(
                name: "ProjectTaskDependency");

            migrationBuilder.DropTable(
                name: "ProjectTaskLaborParts");

            migrationBuilder.DropTable(
                name: "ProjectUsers");

            migrationBuilder.DropTable(
                name: "RecordLocks");

            migrationBuilder.DropTable(
                name: "SystemMaster");

            migrationBuilder.DropTable(
                name: "SystemPreferencesHolidays");

            migrationBuilder.DropTable(
                name: "TestingOutlineCosts");

            migrationBuilder.DropTable(
                name: "TestingOutlineDetails");

            migrationBuilder.DropTable(
                name: "TestingOutlineTemplates");

            migrationBuilder.DropTable(
                name: "TestingTemplateItems");

            migrationBuilder.DropTable(
                name: "TimeClocks");

            migrationBuilder.DropTable(
                name: "UsersGroups");

            migrationBuilder.DropTable(
                name: "UsersTimeOff");

            migrationBuilder.DropTable(
                name: "UserTrackerUsers");

            migrationBuilder.DropTable(
                name: "AdvancedFinds");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "MaterialParts");

            migrationBuilder.DropTable(
                name: "ProjectMaterials");

            migrationBuilder.DropTable(
                name: "LaborParts");

            migrationBuilder.DropTable(
                name: "SystemPreferences");

            migrationBuilder.DropTable(
                name: "TestingTemplates");

            migrationBuilder.DropTable(
                name: "Errors");

            migrationBuilder.DropTable(
                name: "ProjectTasks");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "UserTracker");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "ErrorPriorities");

            migrationBuilder.DropTable(
                name: "ProductVersions");

            migrationBuilder.DropTable(
                name: "TestingOutlines");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Territory");

            migrationBuilder.DropTable(
                name: "TimeZone");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "DevLogixCharts");

            migrationBuilder.DropTable(
                name: "ErrorStatuses");
        }
    }
}
