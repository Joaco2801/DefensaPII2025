using CrmUcu.Models.Enums;

namespace CrmUcu.Models.Interacciones
{
    /// <summary>
    /// Representa un mail como interacción con un cliente.
    /// Maneja asunto, destinatarios, si fue leído y si fue respondido.
    /// </summary>
    public class Mail : Interaccion, IRespondible
    {
        public bool EsEntrante { get; set; }
        public string Asunto { get; set; } 
        public List<string> Destinatarios { get; set; } = new();
        public bool Leido { get; set; }
        public bool Respondida { get; set; }

        /// <summary>
        /// Crea un mail asociado a un cliente con sus datos principales.
        /// </summary>
        public Mail(int idCliente, DateTime fecha, string descripcion, bool esEntrante, string asunto, List<string> destinatarios) 
            : base(idCliente, fecha, descripcion)
        {
            Tipo = TipoInteraccion.Mail;
            EsEntrante = esEntrante;
            Asunto = asunto;
            Destinatarios = destinatarios ?? new List<string>();
            Leido = false;
            Respondida = false;
        }

        /// <summary>
        /// Marca el mail como leído y respondido.
        /// </summary>
        public void MarcarComoLeido()
        {
            Leido = true;
            Respondida = true; // Si lo lee, asumimos que responde
        }
    }
}
