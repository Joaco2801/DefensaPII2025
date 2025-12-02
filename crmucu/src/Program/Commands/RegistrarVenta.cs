using CrmUcu.Core;
using System.Threading.Tasks;

namespace CrmUcu.Commands
{
    /// <summary>
    /// Comando para registrar una venta a partir de una cotización existente.
    /// Implementa la regla de negocio: toda venta debe provenir de una cotización.
    /// Aplica Command Pattern y SRP.
    /// </summary>
    public class RegistrarVentaCommand : ICommand
    {
        private readonly Interfaz _interfaz;
        
        public RegistrarVentaCommand(Interfaz interfaz)
        {
            _interfaz = interfaz;
        }
        
        /// <summary>
        /// Ejecuta el comando para convertir una cotización en venta.
        /// </summary>
        public Task<string> ExecuteAsync(string[] args)
        {
            if (!_interfaz.EstaLogueado)
                return Task.FromResult("❌ Debes iniciar sesión primero.");
            
            if (!_interfaz.EsVendedor())
                return Task.FromResult("❌ Solo los vendedores pueden registrar ventas.");
            
            if (args.Length < 3)
                return Task.FromResult("❌ Faltan datos. Usa: Kbr4 registrarVenta <idCliente> <idCotizacion> <producto>\n" +
                                     "Ejemplo: Kbr4 registrarVenta 1 5 \"Sistema de gestión completo\"");
            
            if (!int.TryParse(args[0], out int idCliente))
                return Task.FromResult("❌ El ID del cliente debe ser un número.");
            
            if (!int.TryParse(args[1], out int idCotizacion))
                return Task.FromResult("❌ El ID de la cotización debe ser un número.");
            
            string producto = string.Join(" ", args, 2, args.Length - 2);
            
            string resultado = _interfaz.ConvertirCotizacionAVenta(idCliente, idCotizacion, producto);
            return Task.FromResult(resultado.Contains("exitosamente") 
                ? $"✅ {resultado}" 
                : $"❌ {resultado}");
        }
    }
}
