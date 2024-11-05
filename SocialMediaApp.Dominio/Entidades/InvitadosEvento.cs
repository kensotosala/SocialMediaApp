using System;
using System.Collections.Generic;

namespace SocialMediaApp.Persistencia.Data;

public partial class InvitadosEvento
{
    public int InvitadoId { get; set; }

    public int? EventoId { get; set; }

    public int? UsuarioId { get; set; }

    public string? Confirmacion { get; set; }

    public virtual Evento? Evento { get; set; }

    public virtual Usuario? Usuario { get; set; }
}
