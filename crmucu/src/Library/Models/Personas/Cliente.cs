using CrmUcu.Models.Enums;
using System;
using System.Collections.Generic;
using CrmUcu.Models.Interacciones;

namespace CrmUcu.Models.Personas
{
    /// <summary>
    /// Representa un cliente del CRM, con datos personales,
    /// vendedor asignado y su actividad (etiquetas, interacciones, ventas, cotizaciones).
    /// </summary>
    public class Cliente : Persona
    {
        public int IdVendedor { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public Genero? Genero { get; set; }

        public List<Etiqueta> Etiquetas { get; set; } = new List<Etiqueta>();
        public List<Interaccion> Interacciones { get; set; } = new List<Interaccion>();
        public List<Venta> Ventas { get; set; } = new List<Venta>();
        public List<Cotizacion> Cotizaciones { get; set; } = new List<Cotizacion>();

        /// <summary>
        /// Constructor por defecto.
        /// </summary>
        public Cliente() : base() { }

        /// <summary>
        /// Crea un cliente con sus datos básicos y el vendedor responsable.
        /// </summary>
        public Cliente(int id, string mail, string nombre, string apellido, string telefono, int idVendedor)
            : base(id, mail, nombre, apellido, telefono)
        {
            IdVendedor = idVendedor;
        }

        /// <summary>
        /// Devuelve un resumen de la información del cliente en formato de texto.
        /// </summary>
        public string MostrarInfo()
        {
            return $"1. Nombre: {Nombre}\n" +
                   $"2. Apellido: {Apellido}\n" +
                   $"3. Mail: {Mail}\n" +
                   $"4. Teléfono: {Telefono}\n" +
                   $"5. Fecha de Nacimiento: {FechaNacimiento?.ToString("dd/MM/yyyy") ?? "No especificada"}\n" +
                   $"6. Género: {Genero?.ToString() ?? "No especificado"}\n" +
                   $"7. Etiquetas: {(Etiquetas.Any() ? string.Join(", ", Etiquetas.Select(e => e.Nombre)) : "Sin etiquetas")}\n" +
                   $"8. Ventas:\n{(Ventas.Any() ? string.Join("\n", Ventas.Select(v => $"- {v.Producto} ({v.Fecha:dd/MM/yyyy}) - ${v.Monto}")) : "Sin ventas")}";
        }
    }
}
