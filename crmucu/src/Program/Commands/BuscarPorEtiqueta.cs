using CrmUcu.Core;
using System.Threading.Tasks;

namespace CrmUcu.Commands
{
    /// <summary>
    /// Comando para buscar clientes por una etiqueta específica.
    /// </summary>
    public class BuscarPorEtiquetaCommand : ICommand
    {
        private readonly Interfaz _interfaz;

        /// <summary>
        /// Recibe la interfaz principal del sistema para ejecutar la búsqueda.
        /// </summary>
        public BuscarPorEtiquetaCommand(Interfaz interfaz)
        {
            _interfaz = interfaz;
        }

        /// <summary>
        /// Ejecuta el comando:
        /// Kbr4 buscarPorEtiqueta &lt;nombreEtiqueta&gt;
        /// </summary>
        public Task<string> ExecuteAsync(string[] args)
        {
            // Kbr4 buscarPorEtiqueta <nombreEtiqueta>
            // Ejemplo: Kbr4 buscarPorEtiqueta VIP
            
            if (!_interfaz.EstaLogueado)
                return Task.FromResult("❌ Debes iniciar sesión primero.");

            if (!_interfaz.EsVendedor())
                return Task.FromResult("❌ Solo los vendedores pueden buscar clientes por etiqueta.");

            if (args.Length < 1)
                return Task.FromResult("❌ Faltan datos. Usa: Kbr4 buscarPorEtiqueta <nombreEtiqueta>\n" +
                                     "Ejemplo: Kbr4 buscarPorEtiqueta VIP");

            string nombreEtiqueta = args[0];

            string resultado = _interfaz.BuscarClientesPorEtiqueta(nombreEtiqueta);

            return Task.FromResult(resultado);
        }
    }
}
