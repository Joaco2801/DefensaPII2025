using CrmUcu.Core;
using System.Threading.Tasks;

namespace CrmUcu.Commands
{
    /// <summary>
    /// Comando para listar todos los vendedores registrados en el sistema.
    /// </summary>
    public class ListarVendedoresCommand : ICommand
    {
        private readonly Interfaz _interfaz;

        /// <summary>
        /// Inicializa el comando con la interfaz principal.
        /// </summary>
        public ListarVendedoresCommand(Interfaz interfaz)
        {
            _interfaz = interfaz;
        }

        /// <summary>
        /// Ejecuta: Kbr4 listarVendedores.
        /// Requiere tener sesión iniciada.
        /// </summary>
        /// <param name="args">No se usan argumentos para este comando.</param>
        /// <returns>Un texto con el listado de vendedores o un mensaje informativo.</returns>
        public Task<string> ExecuteAsync(string[] args)
        {
            // Kbr4 listarVendedores
            
            if (!_interfaz.EstaLogueado)
                return Task.FromResult("❌ Debes iniciar sesión primero.");

            var vendedores = _interfaz.ObtenerTodosLosVendedores();

            if (vendedores.Count == 0)
                return Task.FromResult("📋 No hay vendedores registrados.");

            string respuesta = $"📋 **Vendedores ({vendedores.Count}):**\n\n";
            
            foreach (var v in vendedores)
            {
                respuesta += $"**ID: {v.Id}** - {v.Nombre} {v.Apellido}\n";
                respuesta += $"   Usuario: {v.NombreUsuario} | Mail: {v.Mail}\n";
                respuesta += $"   Clientes: {v.Clientes.Count}\n\n";
            }

            return Task.FromResult(respuesta);
        }
    }
}
