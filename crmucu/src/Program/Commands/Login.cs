using CrmUcu.Core;
using System.Threading.Tasks;

namespace CrmUcu.Commands
{
    /// <summary>
    /// Comando para iniciar sesión en el sistema.
    /// </summary>
    public class LoginCommand : ICommand
    {
        private readonly Interfaz _interfaz;

        /// <summary>
        /// Inicializa el comando con la interfaz principal.
        /// </summary>
        public LoginCommand(Interfaz interfaz)
        {
            _interfaz = interfaz;
        }

        /// <summary>
        /// Ejecuta: Kbr4 login &lt;usuario&gt; &lt;contraseña&gt;.
        /// Si el login es correcto, indica el rol (Admin o Vendedor).
        /// </summary>
        /// <param name="args">Usuario y contraseña.</param>
        /// <returns>Mensaje de éxito o error de autenticación.</returns>
        public Task<string> ExecuteAsync(string[] args)
        {
            // Comando: Kbr4 login <usuario> <contraseña>
            if (args.Length < 2)
                return Task.FromResult("Faltan datos. Usa: Kbr4 login <usuario> <contraseña>");

            string username = args[0];
            string password = args[1];

            var usuario = _interfaz.IniciarSesion(username, password);

            if (usuario == null)
                return Task.FromResult("Credenciales inválidas.");

            string rol = _interfaz.EsAdmin() ? "Admin" : "Vendedor";
            return Task.FromResult($"Login exitoso como {rol}: {usuario.Nombre} {usuario.Apellido}");
        }
    }
}
