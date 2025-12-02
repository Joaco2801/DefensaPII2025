namespace CrmUcu.Models.Personas
{
    /// <summary>
    /// Clase base para las personas del sistema (clientes, usuarios, etc.).
    /// Contiene los datos comunes como Id, nombre, apellido, mail y teléfono.
    /// </summary>
    public abstract class Persona
    {
        public int Id { get; set; }
        public string Mail { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }

        /// <summary>
        /// Constructor protegido por defecto.
        /// </summary>
        protected Persona() { }

        /// <summary>
        /// Crea una persona con sus datos básicos.
        /// </summary>
        /// <param name="id">Id de la persona.</param>
        /// <param name="nombre">Nombre.</param>
        /// <param name="apellido">Apellido.</param>
        /// <param name="mail">Correo electrónico.</param>
        /// <param name="telefono">Teléfono de contacto.</param>
        protected Persona(int id, string nombre, string apellido, string mail, string telefono)
        {
            Id = id;
            Nombre = nombre;
            Apellido = apellido;
            Mail = mail;
            Telefono = telefono;
        }
    }
}
