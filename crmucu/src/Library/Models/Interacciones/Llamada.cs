using CrmUcu.Models.Enums;

namespace CrmUcu.Models.Interacciones
{
    /// <summary>
    /// Representa una llamada registrada como interacción con un cliente.
    /// Incluye información básica como si fue entrante, duración y si fue contestada.
    /// </summary>
    public class Llamada : Interaccion, IRespondible
    {
        public bool EsEntrante { get; set; }
        public int DuracionSegundos { get; set; }
        public bool Contestada { get; set; }
        public bool Respondida { get; set; }
        
        /// <summary>
        /// Constructor que crea una llamada y asigna sus datos principales.
        /// </summary>
        public Llamada(int idCliente, DateTime fecha, string descripcion, bool esEntrante, int duracionSegundos, bool contestada) 
            : base(idCliente, fecha, descripcion)
        {
            Tipo = TipoInteraccion.Llamada;
            EsEntrante = esEntrante;
            DuracionSegundos = duracionSegundos;
            Contestada = contestada;
            Respondida = contestada;
        }

        /// <summary>
        /// Indica si la llamada fue contestada.
        /// </summary>
        public bool FueContestada()
        {
            return Contestada;
        }
    }
}

