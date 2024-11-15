using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMediaApp.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class CrearTablaNotificaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProveedorAutenticacion",
                columns: table => new
                {
                    ProveedorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreProveedor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Proveedo__61266BB9537BCCCD", x => x.ProveedorID);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    UsuarioID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreUsuario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Contraseña = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FotoPerfil = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Biografia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ubicacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Intereses = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    EsPremium = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Usuarios__2B3DE79858DC9012", x => x.UsuarioID);
                });

            migrationBuilder.CreateTable(
                name: "Amistades",
                columns: table => new
                {
                    AmistadID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioID = table.Column<int>(type: "int", nullable: true),
                    AmigoID = table.Column<int>(type: "int", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FechaSolicitud = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    FechaAceptacion = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Amistade__8668448B2952CBB0", x => x.AmistadID);
                    table.ForeignKey(
                        name: "FK__Amistades__Amigo__5BE2A6F2",
                        column: x => x.AmigoID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID");
                    table.ForeignKey(
                        name: "FK__Amistades__Usuar__5AEE82B9",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID");
                });

            migrationBuilder.CreateTable(
                name: "AutenticacionSocial",
                columns: table => new
                {
                    AutenticacionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioID = table.Column<int>(type: "int", nullable: true),
                    ProveedorID = table.Column<int>(type: "int", nullable: true),
                    Token = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FechaAutenticacion = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Autentic__3E8C6FA3004D9DAE", x => x.AutenticacionID);
                    table.ForeignKey(
                        name: "FK__Autentica__Prove__534D60F1",
                        column: x => x.ProveedorID,
                        principalTable: "ProveedorAutenticacion",
                        principalColumn: "ProveedorID");
                    table.ForeignKey(
                        name: "FK__Autentica__Usuar__52593CB8",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID");
                });

            migrationBuilder.CreateTable(
                name: "Eventos",
                columns: table => new
                {
                    EventoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioID = table.Column<int>(type: "int", nullable: true),
                    Titulo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaEvento = table.Column<DateTime>(type: "datetime", nullable: true),
                    Ubicacion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Eventos__1EEB5901A310BA9D", x => x.EventoID);
                    table.ForeignKey(
                        name: "FK__Eventos__Usuario__778AC167",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID");
                });

            migrationBuilder.CreateTable(
                name: "Mensajes",
                columns: table => new
                {
                    MensajeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmisorID = table.Column<int>(type: "int", nullable: true),
                    ReceptorID = table.Column<int>(type: "int", nullable: true),
                    MensajeTexto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaEnvio = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    EsLeido = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Mensajes__FEA0557F6F54DB16", x => x.MensajeID);
                    table.ForeignKey(
                        name: "FK__Mensajes__Emisor__6D0D32F4",
                        column: x => x.EmisorID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID");
                    table.ForeignKey(
                        name: "FK__Mensajes__Recept__6E01572D",
                        column: x => x.ReceptorID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID");
                });

            migrationBuilder.CreateTable(
                name: "Notificacione",
                columns: table => new
                {
                    NotificacionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioID = table.Column<int>(type: "int", nullable: true),
                    Tipo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fecha = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    EsLeida = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Notifica__BCC120C4A59726B8", x => x.NotificacionID);
                    table.ForeignKey(
                        name: "FK__Notificac__Usuar__72C60C4A",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID");
                });

            migrationBuilder.CreateTable(
                name: "Pagos",
                columns: table => new
                {
                    PagoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioID = table.Column<int>(type: "int", nullable: true),
                    Monto = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    FechaPago = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    Estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Pagos__F00B6158AB8AC44C", x => x.PagoID);
                    table.ForeignKey(
                        name: "FK__Pagos__UsuarioID__00200768",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID");
                });

            migrationBuilder.CreateTable(
                name: "Publicaciones",
                columns: table => new
                {
                    PublicacionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioID = table.Column<int>(type: "int", nullable: true),
                    Texto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Imagen = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Enlace = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FechaPublicacion = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Publicac__10DF15AA45ECF1BE", x => x.PublicacionID);
                    table.ForeignKey(
                        name: "FK__Publicaci__Usuar__60A75C0F",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID");
                });

            migrationBuilder.CreateTable(
                name: "RecuperacionContrasena",
                columns: table => new
                {
                    RecuperacionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioID = table.Column<int>(type: "int", nullable: true),
                    Token = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FechaSolicitud = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    FechaExpiracion = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Recupera__32BA9F434B131F7F", x => x.RecuperacionID);
                    table.ForeignKey(
                        name: "FK__Recuperac__Usuar__571DF1D5",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID");
                });

            migrationBuilder.CreateTable(
                name: "InvitadosEvento",
                columns: table => new
                {
                    InvitadoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventoID = table.Column<int>(type: "int", nullable: true),
                    UsuarioID = table.Column<int>(type: "int", nullable: true),
                    Confirmacion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Invitado__5144C1941D875E94", x => x.InvitadoID);
                    table.ForeignKey(
                        name: "FK__Invitados__Event__7B5B524B",
                        column: x => x.EventoID,
                        principalTable: "Eventos",
                        principalColumn: "EventoID");
                    table.ForeignKey(
                        name: "FK__Invitados__Usuar__7C4F7684",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID");
                });

            migrationBuilder.CreateTable(
                name: "Comentarios",
                columns: table => new
                {
                    ComentarioID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublicacionID = table.Column<int>(type: "int", nullable: true),
                    UsuarioID = table.Column<int>(type: "int", nullable: true),
                    Texto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaComentario = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Comentar__F184495802EA4568", x => x.ComentarioID);
                    table.ForeignKey(
                        name: "FK__Comentari__Publi__6477ECF3",
                        column: x => x.PublicacionID,
                        principalTable: "Publicaciones",
                        principalColumn: "PublicacionID");
                    table.ForeignKey(
                        name: "FK__Comentari__Usuar__656C112C",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID");
                });

            migrationBuilder.CreateTable(
                name: "Reacciones",
                columns: table => new
                {
                    ReaccionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublicacionID = table.Column<int>(type: "int", nullable: true),
                    UsuarioID = table.Column<int>(type: "int", nullable: true),
                    TipoReaccion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Reaccion__EAF7E4CA620954E5", x => x.ReaccionID);
                    table.ForeignKey(
                        name: "FK__Reaccione__Publi__693CA210",
                        column: x => x.PublicacionID,
                        principalTable: "Publicaciones",
                        principalColumn: "PublicacionID");
                    table.ForeignKey(
                        name: "FK__Reaccione__Usuar__6A30C649",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Amistades_AmigoID",
                table: "Amistades",
                column: "AmigoID");

            migrationBuilder.CreateIndex(
                name: "IX_Amistades_UsuarioID",
                table: "Amistades",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_AutenticacionSocial_ProveedorID",
                table: "AutenticacionSocial",
                column: "ProveedorID");

            migrationBuilder.CreateIndex(
                name: "IX_AutenticacionSocial_UsuarioID",
                table: "AutenticacionSocial",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_PublicacionID",
                table: "Comentarios",
                column: "PublicacionID");

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_UsuarioID",
                table: "Comentarios",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_UsuarioID",
                table: "Eventos",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_InvitadosEvento_EventoID",
                table: "InvitadosEvento",
                column: "EventoID");

            migrationBuilder.CreateIndex(
                name: "IX_InvitadosEvento_UsuarioID",
                table: "InvitadosEvento",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Mensajes_EmisorID",
                table: "Mensajes",
                column: "EmisorID");

            migrationBuilder.CreateIndex(
                name: "IX_Mensajes_ReceptorID",
                table: "Mensajes",
                column: "ReceptorID");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_UsuarioID",
                table: "Notificacione",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_UsuarioID",
                table: "Pagos",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "UQ__Proveedo__C20DF553C1C4B278",
                table: "ProveedorAutenticacion",
                column: "NombreProveedor",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Publicaciones_UsuarioID",
                table: "Publicaciones",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Reacciones_PublicacionID",
                table: "Reacciones",
                column: "PublicacionID");

            migrationBuilder.CreateIndex(
                name: "IX_Reacciones_UsuarioID",
                table: "Reacciones",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_RecuperacionContrasena_UsuarioID",
                table: "RecuperacionContrasena",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "UQ__Usuarios__6B0F5AE0E5711640",
                table: "Usuarios",
                column: "NombreUsuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Usuarios__A9D105349BFFA09A",
                table: "Usuarios",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Amistades");

            migrationBuilder.DropTable(
                name: "AutenticacionSocial");

            migrationBuilder.DropTable(
                name: "Comentarios");

            migrationBuilder.DropTable(
                name: "InvitadosEvento");

            migrationBuilder.DropTable(
                name: "Mensajes");

            migrationBuilder.DropTable(
                name: "Notificacione");

            migrationBuilder.DropTable(
                name: "Pagos");

            migrationBuilder.DropTable(
                name: "Reacciones");

            migrationBuilder.DropTable(
                name: "RecuperacionContrasena");

            migrationBuilder.DropTable(
                name: "ProveedorAutenticacion");

            migrationBuilder.DropTable(
                name: "Eventos");

            migrationBuilder.DropTable(
                name: "Publicaciones");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
