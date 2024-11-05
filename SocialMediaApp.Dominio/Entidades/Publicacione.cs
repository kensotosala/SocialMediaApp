using System;
using System.Collections.Generic;

namespace SocialMediaApp.Persistencia.Data;

public partial class Publicacione
{
    public int PublicacionId { get; set; }

    public int? UsuarioId { get; set; }

    public string? Texto { get; set; }

    public string? Imagen { get; set; }

    public string? Enlace { get; set; }

    public DateTime? FechaPublicacion { get; set; }

    public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

    public virtual ICollection<Reaccione> Reacciones { get; set; } = new List<Reaccione>();

    public virtual Usuario? Usuario { get; set; }
}
