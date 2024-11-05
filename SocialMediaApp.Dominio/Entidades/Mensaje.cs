using System;
using System.Collections.Generic;

namespace SocialMediaApp.Persistencia.Data;

public partial class Mensaje
{
    public int MensajeId { get; set; }

    public int? EmisorId { get; set; }

    public int? ReceptorId { get; set; }

    public string? MensajeTexto { get; set; }

    public DateTime? FechaEnvio { get; set; }

    public bool? EsLeido { get; set; }

    public virtual Usuario? Emisor { get; set; }

    public virtual Usuario? Receptor { get; set; }
}
