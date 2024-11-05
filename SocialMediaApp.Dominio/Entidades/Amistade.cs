using System;
using System.Collections.Generic;

namespace SocialMediaApp.Persistencia.Data;

public partial class Amistade
{
    public int AmistadId { get; set; }

    public int? UsuarioId { get; set; }

    public int? AmigoId { get; set; }

    public string? Estado { get; set; }

    public DateTime? FechaSolicitud { get; set; }

    public DateTime? FechaAceptacion { get; set; }

    public virtual Usuario? Amigo { get; set; }

    public virtual Usuario? Usuario { get; set; }
}
