namespace CrmUcu.Models.Interacciones
{
    /// <summary>
    /// Define una interacción que puede requerir o tener una respuesta
    /// (por ejemplo, mails, mensajes o llamadas entrantes).
    /// </summary>
    public interface IRespondible
    {       
        bool Respondida { get; set; }
        bool EsEntrante { get; set; }
    }
}
