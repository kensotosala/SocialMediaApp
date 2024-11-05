using System;
using System.Collections.Generic;

namespace SocialMediaApp.Persistencia.Data;

public partial class AutenticacionSocial
{
    public int AutenticacionId { get; set; }

    public int? UsuarioId { get; set; }

    public int? ProveedorId { get; set; }

    public string Token { get; set; } = null!;

    public DateTime? FechaAutenticacion { get; set; }

    public virtual ProveedorAutenticacion? Proveedor { get; set; }

    public virtual Usuario? Usuario { get; set; }
}
