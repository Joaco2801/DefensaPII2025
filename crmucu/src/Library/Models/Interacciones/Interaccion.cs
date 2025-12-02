using System;
using CrmUcu.Models.Utils;
using CrmUcu.Models.Enums;

namespace CrmUcu.Models.Interacciones
{
    /// <summary>
    /// Clase base para todas las interacciones del CRM. 
    /// Contiene los datos comunes como Id, cliente asociado, fecha,
    /// descripción, tipo y notas adicionales.
    /// </summary>
    public abstract class Interaccion
    {
        private static int _contadorId = 1;
        
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
        public TipoInteraccion Tipo { get; set; }
        public List<Nota> Notas { get; set; }
        
        /// <summary>
        /// Constructor por defecto utilizado para herencia o serialización.
        /// </summary>
        protected Interaccion() { }
        
        /// <summary>
        /// Crea una interacción básica asociada a un cliente y asigna un Id automático.
        /// </summary>
        protected Interaccion(int idCliente, DateTime fecha, string descripcion)
        {
            Id = _contadorId++;
            IdCliente = idCliente;
            Fecha = fecha;
            Descripcion = descripcion;
            Notas = new List<Nota>();
        }
    }
}
