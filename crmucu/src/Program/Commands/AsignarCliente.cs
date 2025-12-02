using CrmUcu.Core;
using System.Threading.Tasks;

namespace CrmUcu.Commands
{
    /// <summary>
    /// Comando para asignar un cliente a otro vendedor desde la consola.
    /// </summary>
    public class AsignarClienteCommand : ICommand
    {
        private readonly Interfaz _interfaz;

        /// <summary>
        /// Recibe la interfaz principal del sistema para ejecutar la acción.
        /// </summary>
        public AsignarClienteCommand(Interfaz interfaz)
        {
            _interfaz = interfaz;
        }

        /// <summary>
        /// Ejecuta el comando:
        /// Kbr4 asignarCliente &lt;idCliente&gt; &lt;idVendedorDestino&gt;
        /// </summary>
        public Task<string> ExecuteAsync(string[] args)
        {
            // Kbr4 asignarCliente <idCliente> <idVendedorDestino>
            // Ejemplo: Kbr4 asignarCliente 1 2
            
            if (!_interfaz.EstaLogueado)
                return Task.FromResult("❌ Debes iniciar sesión primero.");

            if (!_interfaz.EsVendedor())
                return Task.FromResult("❌ Solo los vendedores pueden asignar clientes.");

            if (args.Length < 2)
                return Task.FromResult("❌ Faltan datos. Usa: Kbr4 asignarCliente <idCliente> <idVendedorDestino>\n" +
                                     "Ejemplo: Kbr4 asignarCliente 1 2");

            if (!int.TryParse(args[0], out int idCliente))
                return Task.FromResult("❌ El ID del cliente debe ser un número.");

            if (!int.TryParse(args[1], out int idVendedorDestino))
                return Task.FromResult("❌ El ID del vendedor destino debe ser un número.");

            string resultado = _interfaz.AsignarClienteAOtroVendedor(idCliente, idVendedorDestino);

            return Task.FromResult(resultado.Contains("exitosamente") 
                ? $"✅ {resultado}" 
                : $"❌ {resultado}");
        }
    }
}
