using CrmUcu.Core;
using System.Threading.Tasks;

namespace CrmUcu.Commands
{
    /// <summary>
    /// Comando para eliminar un usuario (admin o vendedor) desde la consola.
    /// Solo puede ser usado por un administrador.
    /// </summary>
    public class EliminarUsuarioCommand : ICommand
    {
        private readonly Interfaz _interfaz;

        /// <summary>
        /// Recibe la interfaz principal del sistema para ejecutar la acción.
        /// </summary>
        public EliminarUsuarioCommand(Interfaz interfaz)
        {
            _interfaz = interfaz;
        }

        /// <summary>
        /// Ejecuta: Kbr4 eliminarUsuario &lt;id&gt;.
        /// Valida sesión, rol de admin y el ID.
        /// </summary>
        /// <param name="args">Argumentos del comando, donde args[0] es el ID del usuario.</param>
        /// <returns>Mensaje de resultado listo para mostrar en consola.</returns>
        public Task<string> ExecuteAsync(string[] args)
        {
            // Kbr4 eliminarUsuario <id>
            
            if (!_interfaz.EstaLogueado)
                return Task.FromResult("❌ Debes iniciar sesión primero.");

            if (!_interfaz.EsAdmin())
                return Task.FromResult("❌ Solo los administradores pueden eliminar usuarios.");

            if (args.Length < 1)
                return Task.FromResult("❌ Faltan datos. Usa: Kbr4 eliminarUsuario <id>");

            if (!int.TryParse(args[0], out int idUsuario))
                return Task.FromResult("❌ El ID debe ser un número.");

            string resultado = _interfaz.EliminarUsuario(idUsuario);

            return Task.FromResult(resultado.Contains("exitosamente") 
                ? $"✅ {resultado}" 
                : $"❌ {resultado}");
        }
    }
}
