using System;
using CrmUcu.Models.Enums;
using CrmUcu.Models.Interacciones;

namespace CrmUcu.Models.Interacciones
{
    /// <summary>
    /// Representa una venta realizada a un cliente,
    /// normalmente generada a partir de una cotización.
    /// </summary>
    public class Venta : Interaccion
    {
        public string Producto { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public int IdCotizacion { get; set; }
        
        /// <summary>
        /// Constructor por defecto para usos internos o serialización.
        /// </summary>
        public Venta() : base() { }
        
        /// <summary>
        /// Crea una venta asociada a un cliente y a una cotización previa.
        /// </summary>
        public Venta(int idCliente, DateTime fecha, string descripcion, string producto, decimal monto, int idCotizacion)
            : base(idCliente, fecha, descripcion)
        {
            Tipo = TipoInteraccion.Venta;
            Producto = producto;
            Monto = monto;
            IdCotizacion = idCotizacion;
        }
    }
}
