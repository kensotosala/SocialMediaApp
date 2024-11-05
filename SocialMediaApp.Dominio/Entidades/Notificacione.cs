using System;
using System.Collections.Generic;

namespace SocialMediaApp.Persistencia.Data;

public partial class Notificacione
{
    public int NotificacionId { get; set; }

    public int? UsuarioId { get; set; }

    public string? Tipo { get; set; }

    public string? Descripcion { get; set; }

    public DateTime? Fecha { get; set; }

    public bool? EsLeida { get; set; }

    public virtual Usuario? Usuario { get; set; }
}
