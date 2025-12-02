using CrmUcu.Core;
using CrmUcu.Models.Enums;
using System;
using System.Threading.Tasks;

namespace CrmUcu.Commands
{
    /// <summary>
    /// Comando para ver interacciones de un cliente con filtros opcionales.
    /// Aplica Command Pattern: encapsula la acción en una clase ejecutable.
    /// Aplica SRP: solo valida y ejecuta la consulta de interacciones.
    /// </summary>
    public class VerInteraccionesCommand : ICommand
    {
        private readonly Interfaz _interfaz;
        
        public VerInteraccionesCommand(Interfaz interfaz)
        {
            _interfaz = interfaz;
        }
        
        /// <summary>
        /// Ejecuta el comando para ver interacciones de un cliente.
        /// Soporta filtros por tipo de interacción y rango de fechas.
        /// </summary>
        public Task<string> ExecuteAsync(string[] args)
        {
            if (!_interfaz.EstaLogueado)
                return Task.FromResult("❌ Debes iniciar sesión primero.");
            
            if (!_interfaz.EsVendedor())
                return Task.FromResult("❌ Solo los vendedores pueden ver interacciones.");
            
            if (args.Length < 1)
                return Task.FromResult("❌ Faltan datos. Usa: Kbr4 verInteracciones <idCliente> [tipo] [fechaDesde:dd/MM/yyyy] [fechaHasta:dd/MM/yyyy]\n" +
                                     "Tipos: Llamada, Reunion, Mensaje, Mail, Cotizacion\n" +
                                     "Ejemplos:\n" +
                                     "  Kbr4 verInteracciones 1\n" +
                                     "  Kbr4 verInteracciones 1 Llamada\n" +
                                     "  Kbr4 verInteracciones 1 - 01/12/2024 31/12/2024");
            
            if (!int.TryParse(args[0], out int idCliente))
                return Task.FromResult("❌ El ID del cliente debe ser un número.");
            
            TipoInteraccion? tipoFiltro = null;
            DateTime? fechaDesde = null;
            DateTime? fechaHasta = null;
            
            // Parsear tipo de interacción
            if (args.Length > 1 && args[1] != "-")
            {
                if (Enum.TryParse<TipoInteraccion>(args[1], true, out var tipo))
                {
                    tipoFiltro = tipo;
                }
                else
                {
                    return Task.FromResult("❌ Tipo de interacción inválido. Usa: Llamada, Reunion, Mensaje, Mail, Cotizacion");
                }
            }
            
            // Parsear fechas
            if (args.Length > 2)
            {
                if (DateTime.TryParseExact(args[2], "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime desde))
                {
                    fechaDesde = desde;
                }
                else
                {
                    return Task.FromResult("❌ Formato de fecha desde inválido. Usa: dd/MM/yyyy");
                }
            }
            
            if (args.Length > 3)
            {
                if (DateTime.TryParseExact(args[3], "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime hasta))
                {
                    fechaHasta = hasta;
                }
                else
                {
                    return Task.FromResult("❌ Formato de fecha hasta inválido. Usa: dd/MM/yyyy");
                }
            }
            
            string resultado = _interfaz.VerInteraccionesCliente(idCliente, tipoFiltro, fechaDesde, fechaHasta);
            return Task.FromResult(resultado);
        }
    }
}
