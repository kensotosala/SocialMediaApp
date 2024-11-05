using System;
using System.Collections.Generic;

namespace SocialMediaApp.Persistencia.Data;

public partial class RecuperacionContrasena
{
    public int RecuperacionId { get; set; }

    public int? UsuarioId { get; set; }

    public string Token { get; set; } = null!;

    public DateTime? FechaSolicitud { get; set; }

    public DateTime? FechaExpiracion { get; set; }

    public virtual Usuario? Usuario { get; set; }
}
