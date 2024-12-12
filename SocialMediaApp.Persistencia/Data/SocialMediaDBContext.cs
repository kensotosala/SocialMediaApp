using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SocialMediaApp.Persistencia.Data;

public partial class SocialMediaDBContext : DbContext
{
    public SocialMediaDBContext()
    {
    }

    public SocialMediaDBContext(DbContextOptions<SocialMediaDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Amistade> Amistades { get; set; }

    public virtual DbSet<AutenticacionSocial> AutenticacionSocials { get; set; }

    public virtual DbSet<Comentario> Comentarios { get; set; }

    public virtual DbSet<Evento> Eventos { get; set; }

    public virtual DbSet<InvitadosEvento> InvitadosEventos { get; set; }

    public virtual DbSet<Mensaje> Mensajes { get; set; }

    public virtual DbSet<Notificacione> Notificaciones { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<ProveedorAutenticacion> ProveedorAutenticacions { get; set; }

    public virtual DbSet<Publicacione> Publicaciones { get; set; }

    public virtual DbSet<Reaccione> Reacciones { get; set; }

    public virtual DbSet<RecuperacionContrasena> RecuperacionContrasenas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-K4U6LQP\\SQLEXPRESS;Database=SocialMediaDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Amistade>(entity =>
        {
            entity.HasKey(e => e.AmistadId).HasName("PK__Amistade__8668448BD35FD1BB");

            entity.Property(e => e.AmistadId).HasColumnName("AmistadID");
            entity.Property(e => e.AmigoId).HasColumnName("AmigoID");
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.FechaAceptacion).HasColumnType("datetime");
            entity.Property(e => e.FechaSolicitud)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Amigo).WithMany(p => p.AmistadeAmigos)
                .HasForeignKey(d => d.AmigoId)
                .HasConstraintName("FK__Amistades__Amigo__48CFD27E");

            entity.HasOne(d => d.Usuario).WithMany(p => p.AmistadeUsuarios)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK__Amistades__Usuar__47DBAE45");
        });

        modelBuilder.Entity<AutenticacionSocial>(entity =>
        {
            entity.HasKey(e => e.AutenticacionId).HasName("PK__Autentic__3E8C6FA33F996853");

            entity.ToTable("AutenticacionSocial");

            entity.Property(e => e.AutenticacionId).HasColumnName("AutenticacionID");
            entity.Property(e => e.ProveedorId).HasColumnName("ProveedorID");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Proveedor).WithMany(p => p.AutenticacionSocials)
                .HasForeignKey(d => d.ProveedorId)
                .HasConstraintName("FK__Autentica__Prove__412EB0B6");

            entity.HasOne(d => d.Usuario).WithMany(p => p.AutenticacionSocials)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK__Autentica__Usuar__403A8C7D");
        });

        modelBuilder.Entity<Comentario>(entity =>
        {
            entity.HasKey(e => e.ComentarioId).HasName("PK__Comentar__F1844958F051413B");

            entity.Property(e => e.ComentarioId).HasColumnName("ComentarioID");
            entity.Property(e => e.FechaComentario)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PublicacionId).HasColumnName("PublicacionID");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Publicacion).WithMany(p => p.Comentarios)
                .HasForeignKey(d => d.PublicacionId)
                .HasConstraintName("FK__Comentari__Publi__5165187F");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Comentarios)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK__Comentari__Usuar__52593CB8");
        });

        modelBuilder.Entity<Evento>(entity =>
        {
            entity.HasKey(e => e.EventoId).HasName("PK__Eventos__1EEB590194AF442E");

            entity.Property(e => e.EventoId).HasColumnName("EventoID");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaEvento).HasColumnType("datetime");
            entity.Property(e => e.Titulo).HasMaxLength(255);
            entity.Property(e => e.Ubicacion).HasMaxLength(255);
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Eventos)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK__Eventos__Usuario__6477ECF3");
        });

        modelBuilder.Entity<InvitadosEvento>(entity =>
        {
            entity.HasKey(e => e.InvitadoId).HasName("PK__Invitado__5144C194AC3FFF19");

            entity.ToTable("InvitadosEvento");

            entity.Property(e => e.InvitadoId).HasColumnName("InvitadoID");
            entity.Property(e => e.Confirmacion).HasMaxLength(50);
            entity.Property(e => e.EventoId).HasColumnName("EventoID");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Evento).WithMany(p => p.InvitadosEventos)
                .HasForeignKey(d => d.EventoId)
                .HasConstraintName("FK__Invitados__Event__68487DD7");

            entity.HasOne(d => d.Usuario).WithMany(p => p.InvitadosEventos)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK__Invitados__Usuar__693CA210");
        });

        modelBuilder.Entity<Mensaje>(entity =>
        {
            entity.HasKey(e => e.MensajeId).HasName("PK__Mensajes__FEA0557F42DF0FA5");

            entity.Property(e => e.MensajeId).HasColumnName("MensajeID");
            entity.Property(e => e.EmisorId).HasColumnName("EmisorID");
            entity.Property(e => e.EsLeido).HasDefaultValue(false);
            entity.Property(e => e.FechaEnvio)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ReceptorId).HasColumnName("ReceptorID");

            entity.HasOne(d => d.Emisor).WithMany(p => p.MensajeEmisors)
                .HasForeignKey(d => d.EmisorId)
                .HasConstraintName("FK__Mensajes__Emisor__59FA5E80");

            entity.HasOne(d => d.Receptor).WithMany(p => p.MensajeReceptors)
                .HasForeignKey(d => d.ReceptorId)
                .HasConstraintName("FK__Mensajes__Recept__5AEE82B9");
        });

        modelBuilder.Entity<Notificacione>(entity =>
        {
            entity.HasKey(e => e.NotificacionId).HasName("PK__Notifica__BCC120C45CCB3289");

            entity.Property(e => e.NotificacionId).HasColumnName("NotificacionID");
            entity.Property(e => e.EsLeida).HasDefaultValue(false);
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Tipo).HasMaxLength(50);
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Notificaciones)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK__Notificac__Usuar__5FB337D6");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.PagoId).HasName("PK__Pagos__F00B6158D3D080C9");

            entity.Property(e => e.PagoId).HasColumnName("PagoID");
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.FechaPago)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Monto).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK__Pagos__UsuarioID__6D0D32F4");
        });

        modelBuilder.Entity<ProveedorAutenticacion>(entity =>
        {
            entity.HasKey(e => e.ProveedorId).HasName("PK__Proveedo__61266BB90D2692F0");

            entity.ToTable("ProveedorAutenticacion");

            entity.HasIndex(e => e.NombreProveedor, "UQ__Proveedo__C20DF553C5E11F14").IsUnique();

            entity.Property(e => e.ProveedorId).HasColumnName("ProveedorID");
            entity.Property(e => e.NombreProveedor).HasMaxLength(50);
        });

        modelBuilder.Entity<Publicacione>(entity =>
        {
            entity.HasKey(e => e.PublicacionId).HasName("PK__Publicac__10DF15AA493E4395");

            entity.Property(e => e.PublicacionId).HasColumnName("PublicacionID");
            entity.Property(e => e.Enlace).HasMaxLength(255);
            entity.Property(e => e.FechaPublicacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Imagen).HasMaxLength(255);
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Publicaciones)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK__Publicaci__Usuar__4D94879B");
        });

        modelBuilder.Entity<Reaccione>(entity =>
        {
            entity.HasKey(e => e.ReaccionId).HasName("PK__Reaccion__EAF7E4CA3ABA3DBD");

            entity.Property(e => e.ReaccionId).HasColumnName("ReaccionID");
            entity.Property(e => e.PublicacionId).HasColumnName("PublicacionID");
            entity.Property(e => e.TipoReaccion).HasMaxLength(50);
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Publicacion).WithMany(p => p.Reacciones)
                .HasForeignKey(d => d.PublicacionId)
                .HasConstraintName("FK__Reaccione__Publi__5629CD9C");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Reacciones)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK__Reaccione__Usuar__571DF1D5");
        });

        modelBuilder.Entity<RecuperacionContrasena>(entity =>
        {
            entity.HasKey(e => e.RecuperacionId).HasName("PK__Recupera__32BA9F43384A32D2");

            entity.ToTable("RecuperacionContrasena");

            entity.Property(e => e.RecuperacionId).HasColumnName("RecuperacionID");
            entity.Property(e => e.FechaExpiracion).HasColumnType("datetime");
            entity.Property(e => e.FechaSolicitud)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Token).HasMaxLength(255);
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Usuario).WithMany(p => p.RecuperacionContrasenas)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK__Recuperac__Usuar__440B1D61");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuarios__2B3DE7988E65E65D");

            entity.HasIndex(e => e.NombreUsuario, "UQ__Usuarios__6B0F5AE0201BB4EF").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Usuarios__A9D105346C388A5A").IsUnique();

            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");
            entity.Property(e => e.Apellido).HasMaxLength(64);
            entity.Property(e => e.AutenticacionExternal).HasDefaultValue(false);
            entity.Property(e => e.Contraseña).HasMaxLength(128);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.EsPremium).HasDefaultValue(false);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FotoPerfil).HasMaxLength(255);
            entity.Property(e => e.Intereses).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(64);
            entity.Property(e => e.NombreUsuario).HasMaxLength(50);
            entity.Property(e => e.SalContraseña).HasMaxLength(128);
            entity.Property(e => e.Ubicacion).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
