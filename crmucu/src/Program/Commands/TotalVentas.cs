using CrmUcu.Core;
using System;
using System.Threading.Tasks;

namespace CrmUcu.Commands
{
    /// <summary>
    /// Comando para calcular el total de ventas en un periodo.
    /// Aplica Command Pattern y SRP.
    /// </summary>
    public class TotalVentasCommand : ICommand
    {
        private readonly Interfaz _interfaz;
        
        public TotalVentasCommand(Interfaz interfaz)
        {
            _interfaz = interfaz;
        }
        
        /// <summary>
        /// Ejecuta el comando para obtener el total de ventas en un rango de fechas.
        /// </summary>
        public Task<string> ExecuteAsync(string[] args)
        {
            if (!_interfaz.EstaLogueado)
                return Task.FromResult("❌ Debes iniciar sesión primero.");
            
            if (!_interfaz.EsVendedor())
                return Task.FromResult("❌ Solo los vendedores pueden ver el total de ventas.");
            
            if (args.Length < 2)
                return Task.FromResult("❌ Faltan datos. Usa: Kbr4 totalVentas <fechaDesde:dd/MM/yyyy> <fechaHasta:dd/MM/yyyy>\n" +
                                     "Ejemplo: Kbr4 totalVentas 01/12/2024 31/12/2024");
            
            if (!DateTime.TryParseExact(args[0], "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime fechaDesde))
                return Task.FromResult("❌ Formato de fecha desde inválido. Usa: dd/MM/yyyy");
            
            if (!DateTime.TryParseExact(args[1], "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime fechaHasta))
                return Task.FromResult("❌ Formato de fecha hasta inválido. Usa: dd/MM/yyyy");
            
            string resultado = _interfaz.ObtenerTotalVentas(fechaDesde, fechaHasta);
            return Task.FromResult(resultado);
        }
    }
}
