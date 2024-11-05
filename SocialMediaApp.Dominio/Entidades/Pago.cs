using System;
using System.Collections.Generic;

namespace SocialMediaApp.Persistencia.Data;

public partial class Pago
{
    public int PagoId { get; set; }

    public int? UsuarioId { get; set; }

    public decimal? Monto { get; set; }

    public DateTime? FechaPago { get; set; }

    public string? Estado { get; set; }

    public virtual Usuario? Usuario { get; set; }
}
