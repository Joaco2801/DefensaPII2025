namespace CrmUcu.Models.Personas
{
    /// <summary>
    /// Representa una etiqueta que se puede asignar a clientes
    /// </summary>
    public class Etiqueta
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Color { get; set; }
        public string? Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        /// <summary>
        /// Constructor por defecto.
        /// </summary>
        public Etiqueta() { }

        /// <summary>
        /// Crea una etiqueta con nombre y datos opcionales.
        /// </summary>
        public Etiqueta(string nombre, string? color = null, string? descripcion = null)
        {
            Nombre = nombre;
            Color = color;
            Descripcion = descripcion;
        }
    }
}
