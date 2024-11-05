using System;
using System.Collections.Generic;

namespace SocialMediaApp.Persistencia.Data;

public partial class ProveedorAutenticacion
{
    public int ProveedorId { get; set; }

    public string NombreProveedor { get; set; } = null!;

    public virtual ICollection<AutenticacionSocial> AutenticacionSocials { get; set; } = new List<AutenticacionSocial>();
}
