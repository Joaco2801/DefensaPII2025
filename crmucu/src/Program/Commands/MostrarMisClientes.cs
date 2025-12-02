using CrmUcu.Core;
using System.Linq;
using System.Threading.Tasks;

namespace CrmUcu.Commands
{
    /// <summary>
    /// Comando para mostrar la cartera de clientes del vendedor logueado.
    /// </summary>
    public class MostrarMisClientesCommand : ICommand
    {
        private readonly Interfaz _interfaz;

        /// <summary>
        /// Inicializa el comando con la interfaz principal del sistema.
        /// </summary>
        /// <param name="interfaz">Instancia de <see cref="Interfaz"/> usada para acceder a la lógica.</param>
        public MostrarMisClientesCommand(Interfaz interfaz)
        {
            _interfaz = interfaz;
        }

        /// <summary>
        /// Ejecuta el comando que lista los clientes del vendedor actual.
        /// Requiere sesión activa y rol de vendedor.
        /// </summary>
        /// <param name="args">No se utilizan argumentos para este comando.</param>
        /// <returns>Texto con el listado de clientes o un mensaje informativo.</returns>
        public Task<string> ExecuteAsync(string[] args)
        {
            if (!_interfaz.EstaLogueado)
                return Task.FromResult("❌ Debes iniciar sesión primero.");

            if (!_interfaz.EsVendedor())
                return Task.FromResult("❌ Solo los vendedores pueden ver su cartera.");

            var clientes = _interfaz.ObtenerClientesDeVendedorActual();

            if (clientes.Count == 0)
                return Task.FromResult("📋 No tienes clientes.");

            string respuesta = $"📋 **Tus clientes ({clientes.Count}):**\n\n";
            
            foreach (var c in clientes)
            {
                respuesta += $"**ID: {c.Id}** - {c.Nombre} {c.Apellido}\n";
                respuesta += $"   Mail: {c.Mail} | Tel: {c.Telefono}\n";
                
                if (c.Etiquetas != null && c.Etiquetas.Count > 0)
                {
                    string etiquetas = string.Join(", ", c.Etiquetas.Select(e => e.Nombre));
                    respuesta += $"   🏷️ Etiquetas: {etiquetas}\n";
                }
                
                respuesta += "\n";
            }

            return Task.FromResult(respuesta);
        }
    }
}
