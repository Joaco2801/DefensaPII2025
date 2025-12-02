using CrmUcu.Core;
using System.Threading.Tasks;

namespace CrmUcu.Commands
{
    /// <summary>
    /// Comando para mostrar clientes sin interacción en los últimos N días.
    /// </summary>
    public class ClientesSinInteraccionCommand : ICommand
    {
        private readonly Interfaz _interfaz;

        /// <summary>
        /// Usa la interfaz principal del sistema para ejecutar la lógica.
        /// </summary>
        public ClientesSinInteraccionCommand(Interfaz interfaz)
        {
            _interfaz = interfaz;
        }

        /// <summary>
        /// Ejecuta: Kbr4 clientesSinInteraccion [dias].
        /// Si no se indica, usa 30 días por defecto.
        /// </summary>
        public Task<string> ExecuteAsync(string[] args)
        {
            // Kbr4 clientesSinInteraccion [dias]
            // Ejemplo: Kbr4 clientesSinInteraccion
            //          Kbr4 clientesSinInteraccion 30
            
            if (!_interfaz.EstaLogueado)
                return Task.FromResult("❌ Debes iniciar sesión primero.");

            if (!_interfaz.EsVendedor())
                return Task.FromResult("❌ Solo los vendedores pueden ver clientes sin interacción.");

            int diasSinInteraccion = 30;

            if (args.Length > 0)
            {
                if (!int.TryParse(args[0], out diasSinInteraccion))
                    return Task.FromResult("❌ Los días deben ser un número.");
                
                if (diasSinInteraccion < 1)
                    return Task.FromResult("❌ Los días deben ser al menos 1.");
            }

            string resultado = _interfaz.DetectarClientesSinInteraccion(diasSinInteraccion);

            return Task.FromResult(resultado);
        }
    }
}
