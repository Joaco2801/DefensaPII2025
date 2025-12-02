using CrmUcu.Core;
using System.Linq;
using System.Threading.Tasks;

namespace CrmUcu.Commands
{
    /// <summary>
    /// Comando para buscar clientes por un criterio (nombre, teléfono, mail, etc.).
    /// </summary>
    public class BuscarClientesCommand : ICommand
    {
        private readonly Interfaz _interfaz;

        /// <summary>
        /// Recibe la interfaz principal del sistema para realizar las búsquedas.
        /// </summary>
        public BuscarClientesCommand(Interfaz interfaz)
        {
            _interfaz = interfaz;
        }

        /// <summary>
        /// Ejecuta el comando:
        /// Kbr4 buscarClientes &lt;criterio&gt;
        /// </summary>
        public Task<string> ExecuteAsync(string[] args)
        {
            // Kbr4 buscarClientes <criterio>
            // Ejemplo: Kbr4 buscarClientes Juan
            //          Kbr4 buscarClientes 099
            //          Kbr4 buscarClientes @mail.com
            
            if (!_interfaz.EstaLogueado)
                return Task.FromResult("❌ Debes iniciar sesión primero.");

            if (args.Length < 1)
                return Task.FromResult("❌ Faltan datos. Usa: Kbr4 buscarClientes <criterio>\n" +
                                     "Puedes buscar por: nombre, apellido, teléfono o email");

            string criterio = string.Join(" ", args); // Por si el criterio tiene espacios

            var clientes = _interfaz.BuscarClientesPor(criterio);

            if (clientes.Count == 0)
                return Task.FromResult($"🔍 No se encontraron clientes con '{criterio}'");

            string respuesta = $"🔍 **Clientes encontrados ({clientes.Count}):**\n\n";
            
            foreach (var c in clientes)
            {
                respuesta += $"**ID: {c.Id}** - {c.Nombre} {c.Apellido}\n";
                respuesta += $"   Mail: {c.Mail} | Tel: {c.Telefono}\n\n";
            }

            return Task.FromResult(respuesta);
        }
    }
}
