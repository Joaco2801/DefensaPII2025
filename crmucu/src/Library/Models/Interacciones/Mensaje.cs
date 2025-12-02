using CrmUcu.Models.Interacciones;
using CrmUcu.Models.Enums;

/// <summary>
/// Representa un mensaje 
/// como interacción con un cliente.
/// </summary>
public class Mensaje : Interaccion, IRespondible
{
    public bool EsEntrante { get; set; }
    public string Asunto { get; set; } 
    public List<string> Destinatarios { get; set; } = new();
    public bool Leido { get; set; }
    public bool Respondida { get; set; }
    
    /// <summary>
    /// Crea un mensaje asociado a un cliente con sus datos principales.
    /// </summary>
    public Mensaje(int idCliente, DateTime fecha, string descripcion, bool esEntrante, string asunto, List<string> destinatarios) 
        : base(idCliente, fecha, descripcion)
    {
        Tipo = TipoInteraccion.Mensaje;
        EsEntrante = esEntrante;
        Asunto = asunto;
        Destinatarios = destinatarios ?? new List<string>();
        Leido = false;
        Respondida = false;
    }
}
