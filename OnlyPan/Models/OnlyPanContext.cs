using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OnlyPan.Models;

public partial class OnlyPanContext : DbContext
{
    public OnlyPanContext()
    {
    }

    public OnlyPanContext(DbContextOptions<OnlyPanContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Auditorium> Auditoria { get; set; }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<Comentario> Comentarios { get; set; }

    public virtual DbSet<Donacion> Donacions { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<Etiquetum> Etiqueta { get; set; }

    public virtual DbSet<Ingrediente> Ingredientes { get; set; }

    public virtual DbSet<RecetaChef> RecetaChefs { get; set; }

    public virtual DbSet<RecetaIngrediente> RecetaIngredientes { get; set; }

    public virtual DbSet<Recetum> Receta { get; set; }

    public virtual DbSet<ReplicaUsuario> ReplicaUsuarios { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<SeguirUsuario> SeguirUsuarios { get; set; }

    public virtual DbSet<SolicitudRol> SolicitudRols { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Valoracion> Valoracions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:OnlyPanContext");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auditorium>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("AUDITORIA");

            entity.Property(e => e.Accion)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("accion");
            entity.Property(e => e.Fecha)
                .HasColumnType("date")
                .HasColumnName("fecha");
            entity.Property(e => e.IdAuditoria).HasColumnName("id_auditoria");
            entity.Property(e => e.Sq)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("sq");
            entity.Property(e => e.Tabla)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("tabla");
            entity.Property(e => e.Usuario).HasColumnName("usuario");
        });

        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.HasKey(e => e.IdCategoria);

            entity.ToTable("CATEGORIA");

            entity.Property(e => e.IdCategoria)
                .ValueGeneratedNever()
                .HasColumnName("id_categoria");
            entity.Property(e => e.Categoria)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("categoria");
            entity.Property(e => e.Receta).HasColumnName("receta");
        });

        modelBuilder.Entity<Comentario>(entity =>
        {
            entity.HasKey(e => e.IdInteraccion);

            entity.ToTable("COMENTARIO");

            entity.Property(e => e.IdInteraccion)
                .ValueGeneratedNever()
                .HasColumnName("id_interaccion");
            entity.Property(e => e.Comentario1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("comentario");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.FechaInteracion)
                .HasColumnType("date")
                .HasColumnName("fecha_interacion");
            entity.Property(e => e.Receta).HasColumnName("receta");
            entity.Property(e => e.Usuario).HasColumnName("usuario");

            entity.HasOne(d => d.EstadoNavigation).WithMany(p => p.Comentarios)
                .HasForeignKey(d => d.Estado)
                .HasConstraintName("FK_comentario_estado");

            entity.HasOne(d => d.RecetaNavigation).WithMany(p => p.Comentarios)
                .HasForeignKey(d => d.Receta)
                .HasConstraintName("FK_comentario_receta");

            entity.HasOne(d => d.UsuarioNavigation).WithMany(p => p.Comentarios)
                .HasForeignKey(d => d.Usuario)
                .HasConstraintName("FK_comentario_usuario");
        });

        modelBuilder.Entity<Donacion>(entity =>
        {
            entity.HasKey(e => e.IdDonacion);

            entity.ToTable("DONACION");

            entity.Property(e => e.IdDonacion)
                .ValueGeneratedNever()
                .HasColumnName("id_donacion");
            entity.Property(e => e.Donador).HasColumnName("donador");
            entity.Property(e => e.FechaDonacion)
                .HasColumnType("date")
                .HasColumnName("fecha_donacion");
            entity.Property(e => e.Monto)
                .HasColumnType("decimal(4, 2)")
                .HasColumnName("monto");

            entity.HasOne(d => d.DonadorNavigation).WithMany(p => p.Donacions)
                .HasForeignKey(d => d.Donador)
                .HasConstraintName("FK_donacion_donador");
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.HasKey(e => e.IdEstado);

            entity.ToTable("ESTADO");

            entity.Property(e => e.IdEstado)
                .ValueGeneratedNever()
                .HasColumnName("id_estado");
            entity.Property(e => e.DescripcionEstado)
                .HasColumnType("text")
                .HasColumnName("descripcion_estado");
            entity.Property(e => e.NombreEstado)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_estado");
        });

        modelBuilder.Entity<Etiquetum>(entity =>
        {
            entity.HasKey(e => e.IdEtiqueta);

            entity.ToTable("ETIQUETA");

            entity.Property(e => e.IdEtiqueta)
                .ValueGeneratedNever()
                .HasColumnName("id_etiqueta");
            entity.Property(e => e.Etiqueta)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("etiqueta");
            entity.Property(e => e.Receta).HasColumnName("receta");
        });

        modelBuilder.Entity<Ingrediente>(entity =>
        {
            entity.HasKey(e => e.IdIngrediente);

            entity.ToTable("INGREDIENTE");

            entity.Property(e => e.IdIngrediente)
                .ValueGeneratedNever()
                .HasColumnName("id_ingrediente");
            entity.Property(e => e.Ingrediente1)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("ingrediente");
            entity.Property(e => e.Receta).HasColumnName("receta");
        });

        modelBuilder.Entity<RecetaChef>(entity =>
        {
            entity.HasKey(e => e.IdActuacion);

            entity.ToTable("RECETA_CHEF");

            entity.Property(e => e.IdActuacion)
                .ValueGeneratedNever()
                .HasColumnName("id_actuacion");
            entity.Property(e => e.Chef).HasColumnName("chef");
            entity.Property(e => e.FechaActualizacion)
                .HasColumnType("date")
                .HasColumnName("fecha_actualizacion");
            entity.Property(e => e.Receta).HasColumnName("receta");

            entity.HasOne(d => d.ChefNavigation).WithMany(p => p.RecetaChefs)
                .HasForeignKey(d => d.Chef)
                .HasConstraintName("FK_rc_chef");

            entity.HasOne(d => d.RecetaNavigation).WithMany(p => p.RecetaChefs)
                .HasForeignKey(d => d.Receta)
                .HasConstraintName("FK_rc_receta");
        });

        modelBuilder.Entity<RecetaIngrediente>(entity =>
        {
            entity.HasKey(e => e.IdLista).HasName("PK_LISTA");

            entity.ToTable("RECETA_INGREDIENTE");

            entity.Property(e => e.IdLista)
                .ValueGeneratedNever()
                .HasColumnName("id_lista");
            entity.Property(e => e.Cantidad)
                .HasColumnType("decimal(3, 2)")
                .HasColumnName("cantidad");
            entity.Property(e => e.Ingrediente).HasColumnName("ingrediente");

            entity.HasOne(d => d.IngredienteNavigation).WithMany(p => p.RecetaIngredientes)
                .HasForeignKey(d => d.Ingrediente)
                .HasConstraintName("FK_Ingrediente_Lista");
        });

        modelBuilder.Entity<Recetum>(entity =>
        {
            entity.HasKey(e => e.IdReceta);

            entity.ToTable("RECETA");

            entity.Property(e => e.IdReceta)
                .ValueGeneratedNever()
                .HasColumnName("id_receta");
            entity.Property(e => e.Categoria).HasColumnName("categoria");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Etiqueta).HasColumnName("etiqueta");
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("date")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.Foto)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("foto");
            entity.Property(e => e.Instrucciones)
                .HasColumnType("text")
                .HasColumnName("instrucciones");
            entity.Property(e => e.Lista).HasColumnName("lista");
            entity.Property(e => e.TituloPlato)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("titulo_plato");

            entity.HasOne(d => d.CategoriaNavigation).WithMany(p => p.RecetaNavigation)
                .HasForeignKey(d => d.Categoria)
                .HasConstraintName("FK_receta_categoria");

            entity.HasOne(d => d.EtiquetaNavigation).WithMany(p => p.RecetaNavigation)
                .HasForeignKey(d => d.Etiqueta)
                .HasConstraintName("FK_receta_etiqueta");

            entity.HasOne(d => d.ListaNavigation).WithMany(p => p.Receta)
                .HasForeignKey(d => d.Lista)
                .HasConstraintName("FK_receta_lista");
        });

        modelBuilder.Entity<ReplicaUsuario>(entity =>
        {
            entity.HasKey(e => e.IdReplica);

            entity.ToTable("REPLICA_USUARIO");

            entity.Property(e => e.IdReplica)
                .ValueGeneratedNever()
                .HasColumnName("id_replica");
            entity.Property(e => e.Comentario)
                .HasColumnType("text")
                .HasColumnName("comentario");
            entity.Property(e => e.FechaConsulta)
                .HasColumnType("date")
                .HasColumnName("fecha_consulta");
            entity.Property(e => e.Receta).HasColumnName("receta");
            entity.Property(e => e.Usuario).HasColumnName("usuario");

            entity.HasOne(d => d.RecetaNavigation).WithMany(p => p.ReplicaUsuarios)
                .HasForeignKey(d => d.Receta)
                .HasConstraintName("FK_ru_receta");

            entity.HasOne(d => d.UsuarioNavigation).WithMany(p => p.ReplicaUsuarios)
                .HasForeignKey(d => d.Usuario)
                .HasConstraintName("FK_ru_usuario");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol);

            entity.ToTable("ROL");

            entity.Property(e => e.IdRol)
                .ValueGeneratedNever()
                .HasColumnName("id_rol");
            entity.Property(e => e.NombreRol)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("nombre_rol");
        });

        modelBuilder.Entity<SeguirUsuario>(entity =>
        {
            entity.HasKey(e => e.IdSeguir);

            entity.ToTable("SEGUIR_USUARIO");

            entity.Property(e => e.IdSeguir)
                .ValueGeneratedNever()
                .HasColumnName("id_seguir");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.FechaSeguimiento)
                .HasColumnType("date")
                .HasColumnName("fecha_seguimiento");
            entity.Property(e => e.Seguido).HasColumnName("seguido");
            entity.Property(e => e.Seguidor).HasColumnName("seguidor");
            entity.Property(e => e.SeguidoresChef).HasColumnName("seguidores_chef");

            entity.HasOne(d => d.EstadoNavigation).WithMany(p => p.SeguirUsuarios)
                .HasForeignKey(d => d.Estado)
                .HasConstraintName("FK_seguir_estado");

            entity.HasOne(d => d.SeguidoNavigation).WithMany(p => p.SeguirUsuarioSeguidoNavigations)
                .HasForeignKey(d => d.Seguido)
                .HasConstraintName("FK_seguir_seguido");

            entity.HasOne(d => d.SeguidorNavigation).WithMany(p => p.SeguirUsuarioSeguidorNavigations)
                .HasForeignKey(d => d.Seguidor)
                .HasConstraintName("FK_seguir_seguidor");
        });

        modelBuilder.Entity<SolicitudRol>(entity =>
        {
            entity.HasKey(e => e.IdSolicitud).HasName("PK_SOLICITUD_ROLES");

            entity.ToTable("SOLICITUD_ROL");

            entity.Property(e => e.IdSolicitud)
                .ValueGeneratedNever()
                .HasColumnName("id_solicitud");
            entity.Property(e => e.Comentario)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("comentario");
            entity.Property(e => e.EstadoSolicitud).HasColumnName("estado_solicitud");
            entity.Property(e => e.FechaAprovacion)
                .HasColumnType("date")
                .HasColumnName("fecha_aprovacion");
            entity.Property(e => e.FechaSolicitud)
                .HasColumnType("date")
                .HasColumnName("fecha_solicitud");
            entity.Property(e => e.RolSolicitado).HasColumnName("rol_solicitado");
            entity.Property(e => e.UsuarioAprovador).HasColumnName("usuario_aprovador");
            entity.Property(e => e.UsuarioSolicitud).HasColumnName("usuario_solicitud");

            entity.HasOne(d => d.EstadoSolicitudNavigation).WithMany(p => p.SolicitudRols)
                .HasForeignKey(d => d.EstadoSolicitud)
                .HasConstraintName("FK_solicitud_estado");

            entity.HasOne(d => d.RolSolicitadoNavigation).WithMany(p => p.SolicitudRols)
                .HasForeignKey(d => d.RolSolicitado)
                .HasConstraintName("FK_solicitud_rol");

            entity.HasOne(d => d.UsuarioAprovadorNavigation).WithMany(p => p.SolicitudRolUsuarioAprovadorNavigations)
                .HasForeignKey(d => d.UsuarioAprovador)
                .HasConstraintName("FK_solicitud_usuario_aprovador");

            entity.HasOne(d => d.UsuarioSolicitudNavigation).WithMany(p => p.SolicitudRolUsuarioSolicitudNavigations)
                .HasForeignKey(d => d.UsuarioSolicitud)
                .HasConstraintName("FK_solicitud_usuario");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario);

            entity.ToTable("USUARIO");

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Contrasena)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("contrasena");
            entity.Property(e => e.Correo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("correo");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.FechaInscrito)
                .HasColumnType("date")
                .HasColumnName("fecha_inscrito");
            entity.Property(e => e.Foto)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("foto");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Rol)
                .HasDefaultValueSql("((1))")
                .HasColumnName("rol");

            entity.HasOne(d => d.EstadoNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.Estado)
                .HasConstraintName("FK_usuario_estado");

            entity.HasOne(d => d.RolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.Rol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_usuario_rol");
        });

        modelBuilder.Entity<Valoracion>(entity =>
        {
            entity.HasKey(e => e.IdInteraccion);

            entity.ToTable("VALORACION");

            entity.Property(e => e.IdInteraccion)
                .ValueGeneratedNever()
                .HasColumnName("id_interaccion");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.FechaInteracion)
                .HasColumnType("date")
                .HasColumnName("fecha_interacion");
            entity.Property(e => e.Receta).HasColumnName("receta");
            entity.Property(e => e.Usuario).HasColumnName("usuario");
            entity.Property(e => e.Valoracion1).HasColumnName("valoracion");

            entity.HasOne(d => d.EstadoNavigation).WithMany(p => p.Valoracions)
                .HasForeignKey(d => d.Estado)
                .HasConstraintName("FK_valoracion_estado");

            entity.HasOne(d => d.RecetaNavigation).WithMany(p => p.Valoracions)
                .HasForeignKey(d => d.Receta)
                .HasConstraintName("FK_valoracion_receta");

            entity.HasOne(d => d.UsuarioNavigation).WithMany(p => p.Valoracions)
                .HasForeignKey(d => d.Usuario)
                .HasConstraintName("FK_valoracion_usuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
