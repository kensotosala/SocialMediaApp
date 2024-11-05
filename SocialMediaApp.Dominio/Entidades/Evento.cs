using System;
using System.Collections.Generic;

namespace SocialMediaApp.Persistencia.Data;

public partial class Evento
{
    public int EventoId { get; set; }

    public int? UsuarioId { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public DateTime? FechaEvento { get; set; }

    public string? Ubicacion { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual ICollection<InvitadosEvento> InvitadosEventos { get; set; } = new List<InvitadosEvento>();

    public virtual Usuario? Usuario { get; set; }
}
