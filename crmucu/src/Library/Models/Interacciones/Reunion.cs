using CrmUcu.Models.Enums;
using CrmUcu.Models.Personas;

namespace CrmUcu.Models.Interacciones
{
    /// <summary>
    /// Representa una reunión con un cliente.
    /// Permite registrar ubicación, duración, estado y participantes.
    /// </summary>
    public class Reunion : Interaccion
    {
        public string Ubicacion { get; set; }
        public int DuracionMinutos { get; set; }
        public EstadoReunion Estado { get; set; }
        public List<Persona> Participantes { get; set; }

        /// <summary>
        /// Crea una reunión básica para un cliente, marcada como agendada.
        /// </summary>
        public Reunion(int idCliente, DateTime fecha, string descripcion) 
            : base(idCliente, fecha, descripcion)
        {
            Tipo = TipoInteraccion.Reunion;
            Estado = EstadoReunion.Agendada;
            Participantes = new List<Persona>();
        }

        /// <summary>
        /// Crea una reunión con datos completos (ubicación, duración y estado).
        /// </summary>
        public Reunion(int idCliente, DateTime fecha, string descripcion, string ubicacion, int duracionMinutos, EstadoReunion estado) 
            : base(idCliente, fecha, descripcion)
        {
            Tipo = TipoInteraccion.Reunion;
            Ubicacion = ubicacion;
            DuracionMinutos = duracionMinutos;
            Estado = estado;
            Participantes = new List<Persona>();
        }

        /// <summary>
        /// Agrega un participante a la reunión si no está repetido.
        /// </summary>
        public void AgregarParticipante(Persona persona)
        {
            if (persona != null && !Participantes.Contains(persona))
            {
                Participantes.Add(persona);
            }
        }

        /// <summary>
        /// Marca la reunión como realizada si no fue cancelada.
        /// </summary>
        public void MarcarComoCompletada()
        {
            if (Estado != EstadoReunion.Cancelada)
            {
                Estado = EstadoReunion.Realizada;
            }
        }

        /// <summary>
        /// Marca la reunión como cancelada si no fue realizada.
        /// </summary>
        public void Cancelar()
        {
            if (Estado != EstadoReunion.Realizada)
            {
                Estado = EstadoReunion.Cancelada;
            }
        }
    }
}
