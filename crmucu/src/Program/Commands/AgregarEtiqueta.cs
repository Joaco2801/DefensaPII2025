using CrmUcu.Core;
using System.Threading.Tasks;

namespace CrmUcu.Commands
{
    /// <summary>
    /// Comando para agregar una etiqueta a un cliente desde la consola.
    /// </summary>
    public class AgregarEtiquetaCommand : ICommand
    {
        private readonly Interfaz _interfaz;

        /// <summary>
        /// Recibe la interfaz principal del sistema para ejecutar la lógica.
        /// </summary>
        public AgregarEtiquetaCommand(Interfaz interfaz)
        {
            _interfaz = interfaz;
        }

        /// <summary>
        /// Ejecuta el comando:
        /// Kbr4 agregarEtiqueta &lt;idCliente&gt; &lt;nombreEtiqueta&gt; [color] [descripcion]
        /// </summary>
        public Task<string> ExecuteAsync(string[] args)
        {
            // Kbr4 agregarEtiqueta <idCliente> <nombreEtiqueta> [color] [descripcion]
            // Ejemplo: Kbr4 agregarEtiqueta 1 VIP "#FFD700" "Cliente prioritario"
            
            if (!_interfaz.EstaLogueado)
                return Task.FromResult("❌ Debes iniciar sesión primero.");

            if (!_interfaz.EsVendedor())
                return Task.FromResult("❌ Solo los vendedores pueden agregar etiquetas a clientes.");

            if (args.Length < 2)
                return Task.FromResult("❌ Faltan datos. Usa: Kbr4 agregarEtiqueta <idCliente> <nombreEtiqueta> [color] [descripcion]\n" +
                                     "Ejemplo: Kbr4 agregarEtiqueta 1 VIP \"#FFD700\" \"Cliente prioritario\"");

            if (!int.TryParse(args[0], out int idCliente))
                return Task.FromResult("❌ El ID del cliente debe ser un número.");

            string nombreEtiqueta = args[1];
            string? color = args.Length > 2 ? args[2] : null;
            string? descripcion = args.Length > 3 ? args[3] : null;

            bool agregado = _interfaz.AgregarEtiquetaACliente(idCliente, nombreEtiqueta, color, descripcion);

            return Task.FromResult(agregado
                ? $"✅ Etiqueta '{nombreEtiqueta}' agregada al cliente exitosamente."
                : $"❌ No se pudo agregar la etiqueta. Verifica que el cliente exista.");
        }
    }
}