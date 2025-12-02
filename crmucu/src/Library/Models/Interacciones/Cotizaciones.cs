using System;
using CrmUcu.Models.Enums;
using CrmUcu.Models.Interacciones;

namespace CrmUcu.Models.Interacciones
{
    /// <summary>
    /// Representa una cotización realizada a un cliente.
    /// Es un tipo de interacción que registra un monto ofrecido y su estado.
    /// </summary>
    public class Cotizacion : Interaccion
    {
        /// <summary>
        /// Monto total cotizado al cliente.
        /// </summary>
        public decimal Monto { get; set; }

        /// <summary>
        /// Estado actual de la cotización (por ejemplo: Pendiente, Aprobada, Rechazada).
        /// </summary>
        public EstadoCotizacion Estado { get; set; }  // ← AGREGAR
        
        /// <summary>
        /// Constructor por defecto de la cotización.
        /// Requerido para ciertas operaciones del framework/serialización.
        /// </summary>
        public Cotizacion() : base() { }
        
        /// <summary>
        /// Crea una nueva cotización asociada a un cliente.
        /// </summary>
        /// <param name="idCliente">Id del cliente al que se le realiza la cotización.</param>
        /// <param name="fecha">Fecha en la que se registra la cotización.</param>
        /// <param name="descripcion">Descripción breve de la cotización.</param>
        /// <param name="monto">Monto ofertado al cliente.</param>
        public Cotizacion(int idCliente, DateTime fecha, string descripcion, decimal monto) 
            : base(idCliente, fecha, descripcion)
        {
            Tipo = TipoInteraccion.Cotizacion;
            Monto = monto;
            Estado = EstadoCotizacion.Pendiente;  // ← AGREGAR (por defecto pendiente)
        }
    }
}
