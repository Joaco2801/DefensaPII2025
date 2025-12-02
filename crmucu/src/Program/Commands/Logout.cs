using CrmUcu.Core;
using System.Threading.Tasks;

namespace CrmUcu.Commands
{
    /// <summary>
    /// Comando para cerrar la sesión del usuario actual.
    /// </summary>
    public class LogoutCommand : ICommand
    {
        private readonly Interfaz _interfaz;

        /// <summary>
        /// Inicializa el comando con la interfaz principal.
        /// </summary>
        /// <param name="interfaz">Interfaz del sistema CRM.</param>
        public LogoutCommand(Interfaz interfaz)
        {
            _interfaz = interfaz;
        }

        /// <summary>
        /// Ejecuta el cierre de sesión si hay un usuario logueado.
        /// </summary>
        /// <param name="args">No se utilizan argumentos para este comando.</param>
        /// <returns>Mensaje indicando el resultado del logout.</returns>
        public Task<string> ExecuteAsync(string[] args)
        {
            if (!_interfaz.EstaLogueado)
                return Task.FromResult("No hay ninguna sesión activa.");

            string nombreUsuario = _interfaz.UsuarioActual.Nombre;
            _interfaz.CerrarSesion();
            
            return Task.FromResult($"Sesión cerrada para {nombreUsuario}.");
        }
    }
}
