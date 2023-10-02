using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlyPan.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AUDITORIA",
                columns: table => new
                {
                    id_auditoria = table.Column<int>(type: "int", nullable: true),
                    usuario = table.Column<int>(type: "int", nullable: true),
                    accion = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    tabla = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    sq = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    fecha = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "CATEGORIA",
                columns: table => new
                {
                    id_categoria = table.Column<int>(type: "int", nullable: false),
                    receta = table.Column<int>(type: "int", nullable: true),
                    categoria = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CATEGORIA", x => x.id_categoria);
                });

            migrationBuilder.CreateTable(
                name: "ESTADO",
                columns: table => new
                {
                    id_estado = table.Column<int>(type: "int", nullable: false),
                    nombre_estado = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    descripcion_estado = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESTADO", x => x.id_estado);
                });

            migrationBuilder.CreateTable(
                name: "ETIQUETA",
                columns: table => new
                {
                    id_etiqueta = table.Column<int>(type: "int", nullable: false),
                    receta = table.Column<int>(type: "int", nullable: true),
                    etiqueta = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ETIQUETA", x => x.id_etiqueta);
                });

            migrationBuilder.CreateTable(
                name: "INGREDIENTE",
                columns: table => new
                {
                    id_ingrediente = table.Column<int>(type: "int", nullable: false),
                    receta = table.Column<int>(type: "int", nullable: true),
                    ingrediente = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INGREDIENTE", x => x.id_ingrediente);
                });

            migrationBuilder.CreateTable(
                name: "ROL",
                columns: table => new
                {
                    id_rol = table.Column<int>(type: "int", nullable: false),
                    nombre_rol = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROL", x => x.id_rol);
                });

            migrationBuilder.CreateTable(
                name: "RECETA_INGREDIENTE",
                columns: table => new
                {
                    id_lista = table.Column<int>(type: "int", nullable: false),
                    ingrediente = table.Column<int>(type: "int", nullable: true),
                    cantidad = table.Column<decimal>(type: "decimal(3,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LISTA", x => x.id_lista);
                    table.ForeignKey(
                        name: "FK_Ingrediente_Lista",
                        column: x => x.ingrediente,
                        principalTable: "INGREDIENTE",
                        principalColumn: "id_ingrediente");
                });

            migrationBuilder.CreateTable(
                name: "USUARIO",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rol = table.Column<int>(type: "int", nullable: false, defaultValueSql: "((1))"),
                    fecha_inscrito = table.Column<DateTime>(type: "date", nullable: true),
                    foto = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    nombre = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    correo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    contraseña = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    estado = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIO", x => x.id_usuario);
                    table.ForeignKey(
                        name: "FK_usuario_estado",
                        column: x => x.estado,
                        principalTable: "ESTADO",
                        principalColumn: "id_estado");
                    table.ForeignKey(
                        name: "FK_usuario_rol",
                        column: x => x.rol,
                        principalTable: "ROL",
                        principalColumn: "id_rol");
                });

            migrationBuilder.CreateTable(
                name: "RECETA",
                columns: table => new
                {
                    id_receta = table.Column<int>(type: "int", nullable: false),
                    categoria = table.Column<int>(type: "int", nullable: true),
                    etiqueta = table.Column<int>(type: "int", nullable: true),
                    lista = table.Column<int>(type: "int", nullable: true),
                    titulo_plato = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    instrucciones = table.Column<string>(type: "text", nullable: true),
                    foto = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "date", nullable: true),
                    estado = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RECETA", x => x.id_receta);
                    table.ForeignKey(
                        name: "FK_receta_categoria",
                        column: x => x.categoria,
                        principalTable: "CATEGORIA",
                        principalColumn: "id_categoria");
                    table.ForeignKey(
                        name: "FK_receta_etiqueta",
                        column: x => x.etiqueta,
                        principalTable: "ETIQUETA",
                        principalColumn: "id_etiqueta");
                    table.ForeignKey(
                        name: "FK_receta_lista",
                        column: x => x.lista,
                        principalTable: "RECETA_INGREDIENTE",
                        principalColumn: "id_lista");
                });

            migrationBuilder.CreateTable(
                name: "DONACION",
                columns: table => new
                {
                    id_donacion = table.Column<int>(type: "int", nullable: false),
                    donador = table.Column<int>(type: "int", nullable: true),
                    monto = table.Column<decimal>(type: "decimal(4,2)", nullable: true),
                    fecha_donacion = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DONACION", x => x.id_donacion);
                    table.ForeignKey(
                        name: "FK_donacion_donador",
                        column: x => x.donador,
                        principalTable: "USUARIO",
                        principalColumn: "id_usuario");
                });

            migrationBuilder.CreateTable(
                name: "SEGUIR_USUARIO",
                columns: table => new
                {
                    id_seguir = table.Column<int>(type: "int", nullable: false),
                    seguido = table.Column<int>(type: "int", nullable: true),
                    seguidor = table.Column<int>(type: "int", nullable: true),
                    fecha_seguimiento = table.Column<DateTime>(type: "date", nullable: true),
                    seguidores_chef = table.Column<int>(type: "int", nullable: true),
                    estado = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEGUIR_USUARIO", x => x.id_seguir);
                    table.ForeignKey(
                        name: "FK_seguir_estado",
                        column: x => x.estado,
                        principalTable: "ESTADO",
                        principalColumn: "id_estado");
                    table.ForeignKey(
                        name: "FK_seguir_seguido",
                        column: x => x.seguido,
                        principalTable: "USUARIO",
                        principalColumn: "id_usuario");
                    table.ForeignKey(
                        name: "FK_seguir_seguidor",
                        column: x => x.seguidor,
                        principalTable: "USUARIO",
                        principalColumn: "id_usuario");
                });

            migrationBuilder.CreateTable(
                name: "SOLICITUD_ROL",
                columns: table => new
                {
                    id_solicitud = table.Column<int>(type: "int", nullable: false),
                    usuario_solicitud = table.Column<int>(type: "int", nullable: true),
                    rol_solicitado = table.Column<int>(type: "int", nullable: true),
                    estado_solicitud = table.Column<int>(type: "int", nullable: true),
                    fecha_solicitud = table.Column<DateTime>(type: "date", nullable: true),
                    comentario = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    fecha_aprovacion = table.Column<DateTime>(type: "date", nullable: true),
                    usuario_aprovador = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SOLICITUD_ROLES", x => x.id_solicitud);
                    table.ForeignKey(
                        name: "FK_solicitud_estado",
                        column: x => x.estado_solicitud,
                        principalTable: "ESTADO",
                        principalColumn: "id_estado");
                    table.ForeignKey(
                        name: "FK_solicitud_rol",
                        column: x => x.rol_solicitado,
                        principalTable: "ROL",
                        principalColumn: "id_rol");
                    table.ForeignKey(
                        name: "FK_solicitud_usuario",
                        column: x => x.usuario_solicitud,
                        principalTable: "USUARIO",
                        principalColumn: "id_usuario");
                    table.ForeignKey(
                        name: "FK_solicitud_usuario_aprovador",
                        column: x => x.usuario_aprovador,
                        principalTable: "USUARIO",
                        principalColumn: "id_usuario");
                });

            migrationBuilder.CreateTable(
                name: "COMENTARIO",
                columns: table => new
                {
                    id_interaccion = table.Column<int>(type: "int", nullable: false),
                    usuario = table.Column<int>(type: "int", nullable: true),
                    receta = table.Column<int>(type: "int", nullable: true),
                    fecha_interacion = table.Column<DateTime>(type: "date", nullable: true),
                    comentario = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    estado = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMENTARIO", x => x.id_interaccion);
                    table.ForeignKey(
                        name: "FK_comentario_estado",
                        column: x => x.estado,
                        principalTable: "ESTADO",
                        principalColumn: "id_estado");
                    table.ForeignKey(
                        name: "FK_comentario_receta",
                        column: x => x.receta,
                        principalTable: "RECETA",
                        principalColumn: "id_receta");
                    table.ForeignKey(
                        name: "FK_comentario_usuario",
                        column: x => x.usuario,
                        principalTable: "USUARIO",
                        principalColumn: "id_usuario");
                });

            migrationBuilder.CreateTable(
                name: "RECETA_CHEF",
                columns: table => new
                {
                    id_actuacion = table.Column<int>(type: "int", nullable: false),
                    chef = table.Column<int>(type: "int", nullable: true),
                    receta = table.Column<int>(type: "int", nullable: true),
                    fecha_actualizacion = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RECETA_CHEF", x => x.id_actuacion);
                    table.ForeignKey(
                        name: "FK_rc_chef",
                        column: x => x.chef,
                        principalTable: "USUARIO",
                        principalColumn: "id_usuario");
                    table.ForeignKey(
                        name: "FK_rc_receta",
                        column: x => x.receta,
                        principalTable: "RECETA",
                        principalColumn: "id_receta");
                });

            migrationBuilder.CreateTable(
                name: "REPLICA_USUARIO",
                columns: table => new
                {
                    id_replica = table.Column<int>(type: "int", nullable: false),
                    usuario = table.Column<int>(type: "int", nullable: true),
                    receta = table.Column<int>(type: "int", nullable: true),
                    comentario = table.Column<string>(type: "text", nullable: true),
                    fecha_consulta = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REPLICA_USUARIO", x => x.id_replica);
                    table.ForeignKey(
                        name: "FK_ru_receta",
                        column: x => x.receta,
                        principalTable: "RECETA",
                        principalColumn: "id_receta");
                    table.ForeignKey(
                        name: "FK_ru_usuario",
                        column: x => x.usuario,
                        principalTable: "USUARIO",
                        principalColumn: "id_usuario");
                });

            migrationBuilder.CreateTable(
                name: "VALORACION",
                columns: table => new
                {
                    id_interaccion = table.Column<int>(type: "int", nullable: false),
                    usuario = table.Column<int>(type: "int", nullable: true),
                    receta = table.Column<int>(type: "int", nullable: true),
                    fecha_interacion = table.Column<DateTime>(type: "date", nullable: true),
                    valoracion = table.Column<int>(type: "int", nullable: true),
                    estado = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VALORACION", x => x.id_interaccion);
                    table.ForeignKey(
                        name: "FK_valoracion_estado",
                        column: x => x.estado,
                        principalTable: "ESTADO",
                        principalColumn: "id_estado");
                    table.ForeignKey(
                        name: "FK_valoracion_receta",
                        column: x => x.receta,
                        principalTable: "RECETA",
                        principalColumn: "id_receta");
                    table.ForeignKey(
                        name: "FK_valoracion_usuario",
                        column: x => x.usuario,
                        principalTable: "USUARIO",
                        principalColumn: "id_usuario");
                });

            migrationBuilder.CreateIndex(
                name: "IX_COMENTARIO_estado",
                table: "COMENTARIO",
                column: "estado");

            migrationBuilder.CreateIndex(
                name: "IX_COMENTARIO_receta",
                table: "COMENTARIO",
                column: "receta");

            migrationBuilder.CreateIndex(
                name: "IX_COMENTARIO_usuario",
                table: "COMENTARIO",
                column: "usuario");

            migrationBuilder.CreateIndex(
                name: "IX_DONACION_donador",
                table: "DONACION",
                column: "donador");

            migrationBuilder.CreateIndex(
                name: "IX_RECETA_categoria",
                table: "RECETA",
                column: "categoria");

            migrationBuilder.CreateIndex(
                name: "IX_RECETA_etiqueta",
                table: "RECETA",
                column: "etiqueta");

            migrationBuilder.CreateIndex(
                name: "IX_RECETA_lista",
                table: "RECETA",
                column: "lista");

            migrationBuilder.CreateIndex(
                name: "IX_RECETA_CHEF_chef",
                table: "RECETA_CHEF",
                column: "chef");

            migrationBuilder.CreateIndex(
                name: "IX_RECETA_CHEF_receta",
                table: "RECETA_CHEF",
                column: "receta");

            migrationBuilder.CreateIndex(
                name: "IX_RECETA_INGREDIENTE_ingrediente",
                table: "RECETA_INGREDIENTE",
                column: "ingrediente");

            migrationBuilder.CreateIndex(
                name: "IX_REPLICA_USUARIO_receta",
                table: "REPLICA_USUARIO",
                column: "receta");

            migrationBuilder.CreateIndex(
                name: "IX_REPLICA_USUARIO_usuario",
                table: "REPLICA_USUARIO",
                column: "usuario");

            migrationBuilder.CreateIndex(
                name: "IX_SEGUIR_USUARIO_estado",
                table: "SEGUIR_USUARIO",
                column: "estado");

            migrationBuilder.CreateIndex(
                name: "IX_SEGUIR_USUARIO_seguido",
                table: "SEGUIR_USUARIO",
                column: "seguido");

            migrationBuilder.CreateIndex(
                name: "IX_SEGUIR_USUARIO_seguidor",
                table: "SEGUIR_USUARIO",
                column: "seguidor");

            migrationBuilder.CreateIndex(
                name: "IX_SOLICITUD_ROL_estado_solicitud",
                table: "SOLICITUD_ROL",
                column: "estado_solicitud");

            migrationBuilder.CreateIndex(
                name: "IX_SOLICITUD_ROL_rol_solicitado",
                table: "SOLICITUD_ROL",
                column: "rol_solicitado");

            migrationBuilder.CreateIndex(
                name: "IX_SOLICITUD_ROL_usuario_aprovador",
                table: "SOLICITUD_ROL",
                column: "usuario_aprovador");

            migrationBuilder.CreateIndex(
                name: "IX_SOLICITUD_ROL_usuario_solicitud",
                table: "SOLICITUD_ROL",
                column: "usuario_solicitud");

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO_estado",
                table: "USUARIO",
                column: "estado");

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO_rol",
                table: "USUARIO",
                column: "rol");

            migrationBuilder.CreateIndex(
                name: "IX_VALORACION_estado",
                table: "VALORACION",
                column: "estado");

            migrationBuilder.CreateIndex(
                name: "IX_VALORACION_receta",
                table: "VALORACION",
                column: "receta");

            migrationBuilder.CreateIndex(
                name: "IX_VALORACION_usuario",
                table: "VALORACION",
                column: "usuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AUDITORIA");

            migrationBuilder.DropTable(
                name: "COMENTARIO");

            migrationBuilder.DropTable(
                name: "DONACION");

            migrationBuilder.DropTable(
                name: "RECETA_CHEF");

            migrationBuilder.DropTable(
                name: "REPLICA_USUARIO");

            migrationBuilder.DropTable(
                name: "SEGUIR_USUARIO");

            migrationBuilder.DropTable(
                name: "SOLICITUD_ROL");

            migrationBuilder.DropTable(
                name: "VALORACION");

            migrationBuilder.DropTable(
                name: "RECETA");

            migrationBuilder.DropTable(
                name: "USUARIO");

            migrationBuilder.DropTable(
                name: "CATEGORIA");

            migrationBuilder.DropTable(
                name: "ETIQUETA");

            migrationBuilder.DropTable(
                name: "RECETA_INGREDIENTE");

            migrationBuilder.DropTable(
                name: "ESTADO");

            migrationBuilder.DropTable(
                name: "ROL");

            migrationBuilder.DropTable(
                name: "INGREDIENTE");
        }
    }
}
