using CrmUcu.Models.Enums;

namespace CrmUcu.Models.Personas
{
    /// <summary>
    /// Clase base para los usuarios del sistema (admins, vendedores, etc.).
    /// Extiende Persona y agrega credenciales y estado del usuario.
    /// </summary>
    public abstract class Usuario : Persona
    {
        public string NombreUsuario { get; set; } 
        public string Password { get; set; }
        public EstadoUsuario Estado { get; set; }

        /// <summary>
        /// Constructor por defecto. Deja al usuario en estado Activo.
        /// </summary>
        protected Usuario() : base()
        {
            Estado = EstadoUsuario.Activo;
        }

        /// <summary>
        /// Crea un usuario con sus datos personales y credenciales.
        /// </summary>
        protected Usuario(int id, string nombre, string apellido, string mail, string telefono, string nombreUsuario, string password) 
            : base(id, nombre, apellido, mail, telefono)
        {
            NombreUsuario = nombreUsuario;
            Password = password;
            Estado = EstadoUsuario.Activo;
        }

        /// <summary>
        /// Verifica si el usuario y la contraseña coinciden con los almacenados.
        /// </summary>
        public bool Autenticar(string nombreUsuario, string password)
        {
            return NombreUsuario == nombreUsuario && Password == password;
        }
    }
}
