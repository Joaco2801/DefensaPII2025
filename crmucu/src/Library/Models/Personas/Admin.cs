using CrmUcu.Models.Enums;
using CrmUcu.Repositories;

namespace CrmUcu.Models.Personas
{
    /// <summary>
    /// Representa un usuario administrador del sistema CRM.
    /// Puede crear y gestionar otros usuarios.
    /// </summary>
    public class Admin : Usuario
    {
        /// <summary>
        /// IDs de usuarios creados por este administrador.
        /// </summary>
        public List<int>? usuariosCreados { get; set; } = new();

        /// <summary>
        /// Constructor por defecto.
        /// </summary>
        public Admin() : base()
        {
        }
        /// <summary>
        /// Crea un administrador con sus datos básicos.
        /// </summary>
        public Admin(int id, string nombre, string apellido, string mail, string telefono, string nombreUsuario,
            string password) : base(id, nombre, apellido, mail, telefono, nombreUsuario, password)
        {

        }
    }
}
