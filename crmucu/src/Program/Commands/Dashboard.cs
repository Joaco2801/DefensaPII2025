using CrmUcu.Core;
using System.Threading.Tasks;

namespace CrmUcu.Commands
{
    /// <summary>
    /// Comando para mostrar el dashboard del vendedor
    /// con resumen de clientes, interacciones y reuniones.
    /// </summary>
    public class DashboardCommand : ICommand
    {
        private readonly Interfaz _interfaz;

        /// <summary>
        /// Inicializa el comando con la interfaz principal del sistema.
        /// </summary>
        public DashboardCommand(Interfaz interfaz)
        {
            _interfaz = interfaz;
        }

        /// <summary>
        /// Ejecuta: Kbr4 dashboard [diasInteracciones] [cantidadInteracciones].
        /// Si no se indican parámetros, usa 7 días y 5 interacciones por defecto.
        /// </summary>
        public Task<string> ExecuteAsync(string[] args)
        {
            // Kbr4 dashboard [diasInteracciones] [cantidadInteracciones]
            // Ejemplo: Kbr4 dashboard
            //          Kbr4 dashboard 7 10
            
            if (!_interfaz.EstaLogueado)
                return Task.FromResult("❌ Debes iniciar sesión primero.");

            if (!_interfaz.EsVendedor())
                return Task.FromResult("❌ Solo los vendedores pueden ver el dashboard.");

            int diasInteracciones = 7;          // Por defecto últimos 7 días
            int cantidadInteracciones = 5;      // Por defecto 5 interacciones

            // Parsear parámetros opcionales
            if (args.Length > 0)
            {
                if (!int.TryParse(args[0], out diasInteracciones))
                    return Task.FromResult("❌ Los días deben ser un número.");
                
                if (diasInteracciones < 1)
                    return Task.FromResult("❌ Los días deben ser al menos 1.");
            }

            if (args.Length > 1)
            {
                if (!int.TryParse(args[1], out cantidadInteracciones))
                    return Task.FromResult("❌ La cantidad de interacciones debe ser un número.");
                
                if (cantidadInteracciones < 1)
                    return Task.FromResult("❌ La cantidad de interacciones debe ser al menos 1.");
            }

            string resultado = _interfaz.VerDashboard(diasInteracciones, cantidadInteracciones);

            return Task.FromResult(resultado);
        }
    }
}
