using System;
using System.Collections.Generic;

namespace SocialMediaApp.Persistencia.Data;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public string? FotoPerfil { get; set; }

    public string? Biografia { get; set; }

    public string? Ubicacion { get; set; }

    public string? Intereses { get; set; }

    public bool? EsPremium { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual ICollection<Usuario> BuscarUsuarios { get; set; } = new List<Usuario>();

    public virtual ICollection<Amistade> AmistadeAmigos { get; set; } = new List<Amistade>();

    public virtual ICollection<Amistade> AmistadeUsuarios { get; set; } = new List<Amistade>();

    public virtual ICollection<AutenticacionSocial> AutenticacionSocials { get; set; } = new List<AutenticacionSocial>();

    public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

    public virtual ICollection<Evento> Eventos { get; set; } = new List<Evento>();

    public virtual ICollection<InvitadosEvento> InvitadosEventos { get; set; } = new List<InvitadosEvento>();

    public virtual ICollection<Mensaje> MensajeEmisors { get; set; } = new List<Mensaje>();

    public virtual ICollection<Mensaje> MensajeReceptors { get; set; } = new List<Mensaje>();

    public virtual ICollection<Notificacione> Notificacione { get; set; } = new List<Notificacione>();

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    public virtual ICollection<Publicacione> Publicaciones { get; set; } = new List<Publicacione>();

    public virtual ICollection<Reaccione> Reacciones { get; set; } = new List<Reaccione>();

    public virtual ICollection<RecuperacionContrasena> RecuperacionContrasenas { get; set; } = new List<RecuperacionContrasena>();
}
