using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movura.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokensTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Estatus",
                columns: table => new
                {
                    id_estatus = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    modulo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estatus", x => x.id_estatus);
                });

            migrationBuilder.CreateTable(
                name: "Marcas",
                columns: table => new
                {
                    id_marca = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marcas", x => x.id_marca);
                });

            migrationBuilder.CreateTable(
                name: "Parkings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Grupo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdminNombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminCorreo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AltaISO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parkings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    id_rol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.id_rol);
                });

            migrationBuilder.CreateTable(
                name: "Comercios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParkingId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comercios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comercios_Estatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Estatus",
                        principalColumn: "id_estatus",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comercios_Parkings_ParkingId",
                        column: x => x.ParkingId,
                        principalTable: "Parkings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParkingConfigs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TotalSpaces = table.Column<int>(type: "int", nullable: false),
                    TarifaBase = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostoHora = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FraccionMin = table.Column<int>(type: "int", nullable: false),
                    CostoFraccion = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GraciaMin = table.Column<int>(type: "int", nullable: false),
                    HoraCorte = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParkingId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParkingConfigs_Parkings_ParkingId",
                        column: x => x.ParkingId,
                        principalTable: "Parkings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComercioEmails",
                columns: table => new
                {
                    id_rel = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_comercio = table.Column<int>(type: "int", nullable: false),
                    correo = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComercioEmails", x => x.id_rel);
                    table.ForeignKey(
                        name: "FK_ComercioEmails_Comercios_id_comercio",
                        column: x => x.id_comercio,
                        principalTable: "Comercios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    apellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    correo = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    telefono = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    id_rol = table.Column<int>(type: "int", nullable: false),
                    id_estatus = table.Column<int>(type: "int", nullable: false),
                    first_login = table.Column<bool>(type: "bit", nullable: false),
                    fecha_registro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ComercioId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.id_usuario);
                    table.ForeignKey(
                        name: "FK_Usuarios_Comercios_ComercioId",
                        column: x => x.ComercioId,
                        principalTable: "Comercios",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Usuarios_Estatus_id_estatus",
                        column: x => x.id_estatus,
                        principalTable: "Estatus",
                        principalColumn: "id_estatus",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles_id_rol",
                        column: x => x.id_rol,
                        principalTable: "Roles",
                        principalColumn: "id_rol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Accesos",
                columns: table => new
                {
                    id_acceso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_usuario = table.Column<int>(type: "int", nullable: true),
                    hora_entrada = table.Column<DateTime>(type: "datetime2", nullable: false),
                    hora_salida = table.Column<DateTime>(type: "datetime2", nullable: true),
                    id_estatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accesos", x => x.id_acceso);
                    table.ForeignKey(
                        name: "FK_Accesos_Estatus_id_estatus",
                        column: x => x.id_estatus,
                        principalTable: "Estatus",
                        principalColumn: "id_estatus",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Accesos_Usuarios_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "Usuarios",
                        principalColumn: "id_usuario");
                });

            migrationBuilder.CreateTable(
                name: "Contrasenas",
                columns: table => new
                {
                    id_contrasena = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_usuario = table.Column<int>(type: "int", nullable: false),
                    hash = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    salt = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    activa = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contrasenas", x => x.id_contrasena);
                    table.ForeignKey(
                        name: "FK_Contrasenas_Usuarios_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "Usuarios",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    id_log = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_usuario = table.Column<int>(type: "int", nullable: true),
                    accion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ip_address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.id_log);
                    table.ForeignKey(
                        name: "FK_Logs_Usuarios_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "Usuarios",
                        principalColumn: "id_usuario");
                });

            migrationBuilder.CreateTable(
                name: "Notificaciones",
                columns: table => new
                {
                    id_notificacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_usuario = table.Column<int>(type: "int", nullable: true),
                    canal = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    mensaje = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    fecha_envio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    estatus_envio = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificaciones", x => x.id_notificacion);
                    table.ForeignKey(
                        name: "FK_Notificaciones_Usuarios_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "Usuarios",
                        principalColumn: "id_usuario");
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Revoked = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevokedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReplacedByToken = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Usuarios_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuarios",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tarjetas",
                columns: table => new
                {
                    id_tarjeta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_usuario = table.Column<int>(type: "int", nullable: false),
                    numero_enmascarado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    expiracion = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    token_seguro = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    fecha_registro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    marca = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MarcaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarjetas", x => x.id_tarjeta);
                    table.ForeignKey(
                        name: "FK_Tarjetas_Marcas_MarcaId",
                        column: x => x.MarcaId,
                        principalTable: "Marcas",
                        principalColumn: "id_marca");
                    table.ForeignKey(
                        name: "FK_Tarjetas_Usuarios_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "Usuarios",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CodigosQR",
                columns: table => new
                {
                    id_qr = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    codigo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    id_usuario = table.Column<int>(type: "int", nullable: true),
                    id_comercio = table.Column<int>(type: "int", nullable: true),
                    id_acceso = table.Column<int>(type: "int", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_expiracion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    id_estatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodigosQR", x => x.id_qr);
                    table.ForeignKey(
                        name: "FK_CodigosQR_Accesos_id_acceso",
                        column: x => x.id_acceso,
                        principalTable: "Accesos",
                        principalColumn: "id_acceso");
                    table.ForeignKey(
                        name: "FK_CodigosQR_Comercios_id_comercio",
                        column: x => x.id_comercio,
                        principalTable: "Comercios",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CodigosQR_Estatus_id_estatus",
                        column: x => x.id_estatus,
                        principalTable: "Estatus",
                        principalColumn: "id_estatus",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CodigosQR_Usuarios_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "Usuarios",
                        principalColumn: "id_usuario");
                });

            migrationBuilder.CreateTable(
                name: "Excesos",
                columns: table => new
                {
                    id_exceso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_acceso = table.Column<int>(type: "int", nullable: false),
                    hora_inicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    hora_fin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Excesos", x => x.id_exceso);
                    table.ForeignKey(
                        name: "FK_Excesos_Accesos_id_acceso",
                        column: x => x.id_acceso,
                        principalTable: "Accesos",
                        principalColumn: "id_acceso",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    AccesoId = table.Column<int>(type: "int", nullable: true),
                    ParkingId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    Plate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExitTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaEmision = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Accesos_AccesoId",
                        column: x => x.AccesoId,
                        principalTable: "Accesos",
                        principalColumn: "id_acceso");
                    table.ForeignKey(
                        name: "FK_Tickets_Estatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Estatus",
                        principalColumn: "id_estatus",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_Parkings_ParkingId",
                        column: x => x.ParkingId,
                        principalTable: "Parkings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tickets_Usuarios_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuarios",
                        principalColumn: "id_usuario");
                });

            migrationBuilder.CreateTable(
                name: "Pagos",
                columns: table => new
                {
                    id_pago = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_ticket = table.Column<int>(type: "int", nullable: false),
                    id_usuario = table.Column<int>(type: "int", nullable: true),
                    id_acceso = table.Column<int>(type: "int", nullable: true),
                    monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    metodo_pago = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    fecha_pago = table.Column<DateTime>(type: "datetime2", nullable: false),
                    id_estatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagos", x => x.id_pago);
                    table.ForeignKey(
                        name: "FK_Pagos_Accesos_id_acceso",
                        column: x => x.id_acceso,
                        principalTable: "Accesos",
                        principalColumn: "id_acceso");
                    table.ForeignKey(
                        name: "FK_Pagos_Estatus_id_estatus",
                        column: x => x.id_estatus,
                        principalTable: "Estatus",
                        principalColumn: "id_estatus",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pagos_Tickets_id_ticket",
                        column: x => x.id_ticket,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pagos_Usuarios_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "Usuarios",
                        principalColumn: "id_usuario");
                });

            migrationBuilder.CreateTable(
                name: "Transacciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PagoId = table.Column<int>(type: "int", nullable: true),
                    ParkingId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AccesoId = table.Column<int>(type: "int", nullable: true),
                    QRId = table.Column<int>(type: "int", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    TicketId = table.Column<int>(type: "int", nullable: true),
                    ComercioId = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transacciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transacciones_Accesos_AccesoId",
                        column: x => x.AccesoId,
                        principalTable: "Accesos",
                        principalColumn: "id_acceso");
                    table.ForeignKey(
                        name: "FK_Transacciones_CodigosQR_QRId",
                        column: x => x.QRId,
                        principalTable: "CodigosQR",
                        principalColumn: "id_qr");
                    table.ForeignKey(
                        name: "FK_Transacciones_Comercios_ComercioId",
                        column: x => x.ComercioId,
                        principalTable: "Comercios",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transacciones_Estatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Estatus",
                        principalColumn: "id_estatus",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transacciones_Pagos_PagoId",
                        column: x => x.PagoId,
                        principalTable: "Pagos",
                        principalColumn: "id_pago");
                    table.ForeignKey(
                        name: "FK_Transacciones_Parkings_ParkingId",
                        column: x => x.ParkingId,
                        principalTable: "Parkings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transacciones_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transacciones_Usuarios_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuarios",
                        principalColumn: "id_usuario");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accesos_hora_entrada",
                table: "Accesos",
                column: "hora_entrada");

            migrationBuilder.CreateIndex(
                name: "IX_Accesos_id_estatus",
                table: "Accesos",
                column: "id_estatus");

            migrationBuilder.CreateIndex(
                name: "IX_Accesos_id_usuario",
                table: "Accesos",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_CodigosQR_codigo",
                table: "CodigosQR",
                column: "codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CodigosQR_id_acceso",
                table: "CodigosQR",
                column: "id_acceso");

            migrationBuilder.CreateIndex(
                name: "IX_CodigosQR_id_comercio",
                table: "CodigosQR",
                column: "id_comercio");

            migrationBuilder.CreateIndex(
                name: "IX_CodigosQR_id_estatus",
                table: "CodigosQR",
                column: "id_estatus");

            migrationBuilder.CreateIndex(
                name: "IX_CodigosQR_id_usuario",
                table: "CodigosQR",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_ComercioEmails_id_comercio",
                table: "ComercioEmails",
                column: "id_comercio");

            migrationBuilder.CreateIndex(
                name: "IX_Comercios_ParkingId",
                table: "Comercios",
                column: "ParkingId");

            migrationBuilder.CreateIndex(
                name: "IX_Comercios_StatusId",
                table: "Comercios",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Contrasenas_id_usuario",
                table: "Contrasenas",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Excesos_id_acceso",
                table: "Excesos",
                column: "id_acceso");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_id_usuario",
                table: "Logs",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_id_usuario",
                table: "Notificaciones",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_fecha_pago",
                table: "Pagos",
                column: "fecha_pago");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_id_acceso",
                table: "Pagos",
                column: "id_acceso");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_id_estatus",
                table: "Pagos",
                column: "id_estatus");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_id_ticket",
                table: "Pagos",
                column: "id_ticket");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_id_usuario",
                table: "Pagos",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingConfigs_ParkingId",
                table: "ParkingConfigs",
                column: "ParkingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tarjetas_id_usuario",
                table: "Tarjetas",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Tarjetas_MarcaId",
                table: "Tarjetas",
                column: "MarcaId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_AccesoId",
                table: "Tickets",
                column: "AccesoId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_FechaEmision",
                table: "Tickets",
                column: "FechaEmision");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ParkingId",
                table: "Tickets",
                column: "ParkingId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_StatusId",
                table: "Tickets",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_UserId",
                table: "Tickets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_AccesoId",
                table: "Transacciones",
                column: "AccesoId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_ComercioId",
                table: "Transacciones",
                column: "ComercioId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_CreatedAt",
                table: "Transacciones",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_PagoId",
                table: "Transacciones",
                column: "PagoId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_ParkingId",
                table: "Transacciones",
                column: "ParkingId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_QRId",
                table: "Transacciones",
                column: "QRId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_StatusId",
                table: "Transacciones",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_TicketId",
                table: "Transacciones",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_UserId",
                table: "Transacciones",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_ComercioId",
                table: "Usuarios",
                column: "ComercioId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_correo",
                table: "Usuarios",
                column: "correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_id_estatus",
                table: "Usuarios",
                column: "id_estatus");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_id_rol",
                table: "Usuarios",
                column: "id_rol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComercioEmails");

            migrationBuilder.DropTable(
                name: "Contrasenas");

            migrationBuilder.DropTable(
                name: "Excesos");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "Notificaciones");

            migrationBuilder.DropTable(
                name: "ParkingConfigs");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Tarjetas");

            migrationBuilder.DropTable(
                name: "Transacciones");

            migrationBuilder.DropTable(
                name: "Marcas");

            migrationBuilder.DropTable(
                name: "CodigosQR");

            migrationBuilder.DropTable(
                name: "Pagos");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Accesos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Comercios");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Estatus");

            migrationBuilder.DropTable(
                name: "Parkings");
        }
    }
}
