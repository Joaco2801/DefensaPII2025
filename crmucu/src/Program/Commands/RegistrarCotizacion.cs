using CrmUcu.Core;
using System;
using System.Threading.Tasks;

namespace CrmUcu.Commands
{
    /// <summary>
    /// Comando para registrar una cotización para un cliente.
    /// Solo puede ser usado por un vendedor logueado.
    /// </summary>
    public class RegistrarCotizacionCommand : ICommand
    {
        private readonly Interfaz _interfaz;

        /// <summary>
        /// Inicializa el comando con la interfaz principal del sistema.
        /// </summary>
        /// <param name="interfaz">Instancia de <see cref="Interfaz"/> usada para la lógica de negocio.</param>
        public RegistrarCotizacionCommand(Interfaz interfaz)
        {
            _interfaz = interfaz;
        }

        /// <summary>
        /// Ejecuta: Kbr4 registrarCotizacion &lt;idCliente&gt; &lt;monto&gt; &lt;descripcion...&gt;.
        /// El monto se pasa como número decimal y el resto forma la descripción.
        /// </summary>
        /// <param name="args">
        /// Argumentos del comando: id del cliente, monto y descripción de la cotización.
        /// </param>
        /// <returns>Mensaje de éxito o error listo para mostrar en consola.</returns>
        public Task<string> ExecuteAsync(string[] args)
        {
            // Kbr4 registrarCotizacion <idCliente> <monto> <descripcion...>
            // Ejemplo: Kbr4 registrarCotizacion 1 15000.50 Cotización para sistema de gestión completo
            
            if (!_interfaz.EstaLogueado)
                return Task.FromResult("❌ Debes iniciar sesión primero.");

            if (!_interfaz.EsVendedor())
                return Task.FromResult("❌ Solo los vendedores pueden registrar cotizaciones.");

            if (args.Length < 3)
                return Task.FromResult("❌ Faltan datos. Usa: Kbr4 registrarCotizacion <idCliente> <monto> <descripcion>\n" +
                                     "Ejemplo: Kbr4 registrarCotizacion 1 15000.50 Cotización para sistema completo");

            if (!int.TryParse(args[0], out int idCliente))
                return Task.FromResult("❌ El ID del cliente debe ser un número.");

            if (!decimal.TryParse(args[1], out decimal monto))
                return Task.FromResult("❌ El monto debe ser un número decimal (ejemplo: 15000.50).");

            // El resto de los argumentos es la descripción
            string descripcion = string.Join(" ", args, 2, args.Length - 2);

            string resultado = _interfaz.RegistrarCotizacion(idCliente, descripcion, monto);

            return Task.FromResult(resultado.Contains("exitosamente") 
                ? $"✅ {resultado}" 
                : $"❌ {resultado}");
        }
    }
}
