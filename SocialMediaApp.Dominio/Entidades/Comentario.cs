using System;
using System.Collections.Generic;

namespace SocialMediaApp.Persistencia.Data;

public partial class Comentario
{
    public int ComentarioId { get; set; }

    public int? PublicacionId { get; set; }

    public int? UsuarioId { get; set; }

    public string? Texto { get; set; }

    public DateTime? FechaComentario { get; set; }

    public virtual Publicacione? Publicacion { get; set; }

    public virtual Usuario? Usuario { get; set; }
}
