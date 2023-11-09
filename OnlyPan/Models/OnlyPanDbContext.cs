using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OnlyPan.Models;

public partial class OnlyPanDbContext : DbContext
{
    public OnlyPanDbContext()
    {
    }

    public OnlyPanDbContext(DbContextOptions<OnlyPanDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Auditorium> Auditoria { get; set; }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<Comentario> Comentarios { get; set; }

    public virtual DbSet<Donacion> Donacions { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<Etiquetum> Etiqueta { get; set; }

    public virtual DbSet<ImagenRecetum> ImagenReceta { get; set; }

    public virtual DbSet<Ingrediente> Ingredientes { get; set; }

    public virtual DbSet<RecetaChef> RecetaChefs { get; set; }

    public virtual DbSet<RecetaIngrediente> RecetaIngredientes { get; set; }

    public virtual DbSet<Recetum> Receta { get; set; }

    public virtual DbSet<ReplicaUsuario> ReplicaUsuarios { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<SeguirUsuario> SeguirUsuarios { get; set; }

    public virtual DbSet<SolicitudRol> SolicitudRols { get; set; }

    public virtual DbSet<Unidad> Unidads { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Valoracion> Valoracions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:OnlyPanDbContext");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auditorium>(entity =>
        {
            entity.HasKey(e => e.IdAuditoria);

            entity.ToTable("AUDITORIA");

            entity.Property(e => e.IdAuditoria).HasColumnName("id_auditoria");
            entity.Property(e => e.Accion)
                .IsUnicode(false)
                .HasColumnName("accion");
            entity.Property(e => e.Comando)
                .IsUnicode(false)
                .HasColumnName("comando");
            entity.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.Tabla)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tabla");
            entity.Property(e => e.Usuario)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.HasKey(e => e.IdCategoria);

            entity.ToTable("CATEGORIA");

            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.DescripcionCategoria)
                .IsUnicode(false)
                .HasColumnName("descripcion_categoria");
            entity.Property(e => e.NombreCategoria)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_categoria");
        });

        modelBuilder.Entity<Comentario>(entity =>
        {
            entity.HasKey(e => e.IdComentario);

            entity.ToTable("COMENTARIO");

            entity.Property(e => e.IdComentario).HasColumnName("id_comentario");
            entity.Property(e => e.Comentario1)
                .IsUnicode(false)
                .HasColumnName("comentario");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.IdReceta).HasColumnName("id_receta");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

            entity.HasOne(d => d.IdRecetaNavigation).WithMany(p => p.Comentarios)
                .HasForeignKey(d => d.IdReceta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_COMENTARIO_RECETA");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Comentarios)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_COMENTARIO_USUARIO");
        });

        modelBuilder.Entity<Donacion>(entity =>
        {
            entity.HasKey(e => e.IdDonacion);

            entity.ToTable("DONACION");

            entity.Property(e => e.IdDonacion).HasColumnName("id_donacion");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.IdChef).HasColumnName("id_chef");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Imagen)
                .HasColumnType("image")
                .HasColumnName("imagen");
            entity.Property(e => e.Monto)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("monto");

            entity.HasOne(d => d.IdChefNavigation).WithMany(p => p.DonacionIdChefNavigations)
                .HasForeignKey(d => d.IdChef)
                .HasConstraintName("FK_DONACION_CHEF");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.DonacionIdUsuarioNavigations)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK_DONACION_USUARIO");
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.HasKey(e => e.IdEstado);

            entity.ToTable("ESTADO");

            entity.Property(e => e.IdEstado).HasColumnName("id_estado");
            entity.Property(e => e.DescripcionEstado)
                .IsUnicode(false)
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

            entity.Property(e => e.IdEtiqueta).HasColumnName("id_etiqueta");
            entity.Property(e => e.NombreEtiqueta)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_etiqueta");
        });

        modelBuilder.Entity<ImagenRecetum>(entity =>
        {
            entity.HasKey(e => new { e.IdImagen, e.IdReceta });

            entity.ToTable("IMAGEN_RECETA");

            entity.Property(e => e.IdImagen)
                .ValueGeneratedOnAdd()
                .HasColumnName("id_imagen");
            entity.Property(e => e.IdReceta).HasColumnName("id_receta");
            entity.Property(e => e.Imagen)
                .HasColumnType("image")
                .HasColumnName("imagen");
        });

        modelBuilder.Entity<Ingrediente>(entity =>
        {
            entity.HasKey(e => e.IdIngrediente);

            entity.ToTable("INGREDIENTE");

            entity.Property(e => e.IdIngrediente).HasColumnName("id_ingrediente");
            entity.Property(e => e.DescripcionIngrediente)
                .IsUnicode(false)
                .HasColumnName("descripcion_ingrediente");
            entity.Property(e => e.NombreIngrediente)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_ingrediente");
        });

        modelBuilder.Entity<RecetaChef>(entity =>
        {
            entity.HasKey(e => new { e.IdChef, e.IdReceta });

            entity.ToTable("RECETA_CHEF");

            entity.Property(e => e.IdChef).HasColumnName("id_chef");
            entity.Property(e => e.IdReceta).HasColumnName("id_receta");
            entity.Property(e => e.FechaActualizacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_actualizacion");

            entity.HasOne(d => d.IdChefNavigation).WithMany(p => p.RecetaChefs)
                .HasForeignKey(d => d.IdChef)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RECETA_CHEF_USUARIO");

            entity.HasOne(d => d.IdRecetaNavigation).WithMany(p => p.RecetaChefs)
                .HasForeignKey(d => d.IdReceta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RECETA_CHEF_RECETA");
        });

        modelBuilder.Entity<RecetaIngrediente>(entity =>
        {
            entity.HasKey(e => new { e.IdReceta, e.IdIngrediente });

            entity.ToTable("RECETA_INGREDIENTE");

            entity.Property(e => e.IdReceta).HasColumnName("id_receta");
            entity.Property(e => e.IdIngrediente).HasColumnName("id_ingrediente");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.IdUnidad).HasColumnName("id_unidad");

            entity.HasOne(d => d.IdIngredienteNavigation).WithMany(p => p.RecetaIngredientes)
                .HasForeignKey(d => d.IdIngrediente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_INGREDIENTE");

            entity.HasOne(d => d.IdRecetaNavigation).WithMany(p => p.RecetaIngredientes)
                .HasForeignKey(d => d.IdReceta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RECETA");

            entity.HasOne(d => d.IdUnidadNavigation).WithMany(p => p.RecetaIngredientes)
                .HasForeignKey(d => d.IdUnidad)
                .HasConstraintName("FK_RECETA_INGREDIENTE_UNIDAD");
        });

        modelBuilder.Entity<Recetum>(entity =>
        {
            entity.HasKey(e => e.IdReceta);

            entity.ToTable("RECETA");

            entity.Property(e => e.IdReceta).HasColumnName("id_receta");
            entity.Property(e => e.DescripcionReceta)
                .IsUnicode(false)
                .HasColumnName("descripcion_receta");
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.IdEstado).HasColumnName("id_estado");
            entity.Property(e => e.IdEtiqueta).HasColumnName("id_etiqueta");
            entity.Property(e => e.Instrucciones)
                .IsUnicode(false)
                .HasColumnName("instrucciones");
            entity.Property(e => e.NValoraciones)
                .HasDefaultValueSql("((0))")
                .HasColumnName("n_valoraciones");
            entity.Property(e => e.NombreReceta)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_receta");
            entity.Property(e => e.Valoracion)
                .HasDefaultValueSql("((0))")
                .HasColumnName("valoracion");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Receta)
                .HasForeignKey(d => d.IdCategoria)
                .HasConstraintName("FK_RECETA_CATEGORIA");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Receta)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("FK_RECETA_ESTADO");

            entity.HasOne(d => d.IdEtiquetaNavigation).WithMany(p => p.Receta)
                .HasForeignKey(d => d.IdEtiqueta)
                .HasConstraintName("FK_RECETA_ETIQUETA");
        });

        modelBuilder.Entity<ReplicaUsuario>(entity =>
        {
            entity.HasKey(e => e.IdReplica);

            entity.ToTable("REPLICA_USUARIO");

            entity.Property(e => e.IdReplica).HasColumnName("id_replica");
            entity.Property(e => e.Comentario)
                .HasColumnType("text")
                .HasColumnName("comentario");
            entity.Property(e => e.FechaReplica)
                .HasColumnType("datetime")
                .HasColumnName("fecha_replica");
            entity.Property(e => e.IdReceta).HasColumnName("id_receta");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

            entity.HasOne(d => d.IdRecetaNavigation).WithMany(p => p.ReplicaUsuarios)
                .HasForeignKey(d => d.IdReceta)
                .HasConstraintName("FK_REPLICA_USUARIO_RECETA");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.ReplicaUsuarios)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK_REPLICA_USUARIO_USUARIO");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol);

            entity.ToTable("ROL");

            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.NombreRol)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_rol");
        });

        modelBuilder.Entity<SeguirUsuario>(entity =>
        {
            entity.HasKey(e => new { e.IdSeguidor, e.IdSeguido });

            entity.ToTable("SEGUIR_USUARIO");

            entity.Property(e => e.IdSeguidor).HasColumnName("id_seguidor");
            entity.Property(e => e.IdSeguido).HasColumnName("id_seguido");
            entity.Property(e => e.FechaSeguimiento)
                .HasColumnType("datetime")
                .HasColumnName("fecha_seguimiento");

            entity.HasOne(d => d.IdSeguidoNavigation).WithMany(p => p.SeguirUsuarioIdSeguidoNavigations)
                .HasForeignKey(d => d.IdSeguido)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SEGUIR_USUARIO_SEGUIDO");

            entity.HasOne(d => d.IdSeguidorNavigation).WithMany(p => p.SeguirUsuarioIdSeguidorNavigations)
                .HasForeignKey(d => d.IdSeguidor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SEGUIR_USUARIO_SEGUIDOR");
        });

        modelBuilder.Entity<SolicitudRol>(entity =>
        {
            entity.HasKey(e => new { e.IdUsuarioSolicitud, e.IdRolSolicitud });

            entity.ToTable("SOLICITUD_ROL");

            entity.Property(e => e.IdUsuarioSolicitud).HasColumnName("id_usuario_solicitud");
            entity.Property(e => e.IdRolSolicitud).HasColumnName("id_rol_solicitud");
            entity.Property(e => e.Comentario)
                .HasColumnType("text")
                .HasColumnName("comentario");
            entity.Property(e => e.FechaAprovacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_aprovacion");
            entity.Property(e => e.FechaSolicitud)
                .HasColumnType("datetime")
                .HasColumnName("fecha_solicitud");
            entity.Property(e => e.IdEstado).HasColumnName("id_estado");
            entity.Property(e => e.IdUsuarioAprovador).HasColumnName("id_usuario_aprovador");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.SolicitudRols)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("FK_SOLICITUD_ROL_ESTADO");

            entity.HasOne(d => d.IdRolSolicitudNavigation).WithMany(p => p.SolicitudRols)
                .HasForeignKey(d => d.IdRolSolicitud)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SOLICITUD_ROL_ROL");

            entity.HasOne(d => d.IdUsuarioAprovadorNavigation).WithMany(p => p.SolicitudRolIdUsuarioAprovadorNavigations)
                .HasForeignKey(d => d.IdUsuarioAprovador)
                .HasConstraintName("FK_SOLICITUD_ROL_USUARIO_APROVADOR");

            entity.HasOne(d => d.IdUsuarioSolicitudNavigation).WithMany(p => p.SolicitudRolIdUsuarioSolicitudNavigations)
                .HasForeignKey(d => d.IdUsuarioSolicitud)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SOLICITUD_ROL_USUARIO_SOLICITADOR");
        });

        modelBuilder.Entity<Unidad>(entity =>
        {
            entity.HasKey(e => e.IdUnidad);

            entity.ToTable("UNIDAD");

            entity.Property(e => e.IdUnidad).HasColumnName("id_unidad");
            entity.Property(e => e.NombreCorto)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("nombre_corto");
            entity.Property(e => e.NombreLargo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_largo");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario);

            entity.ToTable("USUARIO");

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Activo)
                .HasDefaultValueSql("((1))")
                .HasColumnName("activo");
            entity.Property(e => e.Biografia)
                .HasColumnType("text")
                .HasColumnName("biografia");
            entity.Property(e => e.CodigoActivacion)
                .IsUnicode(false)
                .HasColumnName("codigo_activacion");
            entity.Property(e => e.Contrasena)
                .IsUnicode(false)
                .HasColumnName("contrasena");
            entity.Property(e => e.ContrasenaToken)
                .IsUnicode(false)
                .HasColumnName("contrasena_token");
            entity.Property(e => e.Correo)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("correo");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.FechaInscrito)
                .HasColumnType("datetime")
                .HasColumnName("fecha_inscrito");
            entity.Property(e => e.Foto)
                .HasColumnType("image")
                .HasColumnName("foto");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Rol)
                .HasDefaultValueSql("((1))")
                .HasColumnName("rol");

            entity.HasOne(d => d.EstadoNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.Estado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ESTADO");

            entity.HasOne(d => d.RolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.Rol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ROL");
        });

        modelBuilder.Entity<Valoracion>(entity =>
        {
            entity.HasKey(e => new { e.IdUsuario, e.IdReceta });

            entity.ToTable("VALORACION");

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.IdReceta).HasColumnName("id_receta");
            entity.Property(e => e.FechaInteracion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_interacion");
            entity.Property(e => e.IdEstado).HasColumnName("id_estado");
            entity.Property(e => e.Valoration).HasColumnName("valoration");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Valoracions)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("FK_VALORACION_ESTADO");

            entity.HasOne(d => d.IdRecetaNavigation).WithMany(p => p.Valoracions)
                .HasForeignKey(d => d.IdReceta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VALORACION_RECETA");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Valoracions)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VALORACION_USUARIO");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
