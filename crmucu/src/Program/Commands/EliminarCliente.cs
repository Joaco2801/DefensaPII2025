using CrmUcu.Core;
using System.Threading.Tasks;

namespace CrmUcu.Commands
{
    /// <summary>
    /// Comando para eliminar un cliente desde la consola.
    /// Solo puede ser usado por un vendedor logueado.
    /// </summary>
    public class EliminarClienteCommand : ICommand
    {
        private readonly Interfaz _interfaz;

        /// <summary>
        /// Inicializa el comando con la interfaz principal del sistema.
        /// </summary>
        public EliminarClienteCommand(Interfaz interfaz)
        {
            _interfaz = interfaz;
        }

        /// <summary>
        /// Ejecuta: Kbr4 eliminarCliente &lt;id&gt;.
        /// Valida sesión, rol de vendedor y el formato del ID.
        /// </summary>
        /// <param name="args">Argumentos del comando, donde args[0] es el ID del cliente.</param>
        /// <returns>Mensaje de resultado para mostrar en consola.</returns>
        public Task<string> ExecuteAsync(string[] args)
        {
            // Kbr4 eliminarCliente <id>
            
            if (!_interfaz.EstaLogueado)
                return Task.FromResult("❌ Debes iniciar sesión primero.");

            if (!_interfaz.EsVendedor())
                return Task.FromResult("❌ Solo los vendedores pueden eliminar clientes.");

            if (args.Length < 1)
                return Task.FromResult("❌ Faltan datos. Usa: Kbr4 eliminarCliente <id>");

            if (!int.TryParse(args[0], out int idCliente))
                return Task.FromResult("❌ El ID debe ser un número.");

            string resultado = _interfaz.EliminarClienteComoVendedor(idCliente);

            return Task.FromResult(resultado.Contains("exitosamente") 
                ? $"✅ {resultado}" 
                : $"❌ {resultado}");
        }
    }
}
