using CrmUcu.Core;
using System.Threading.Tasks;

namespace CrmUcu.Commands
{
    /// <summary>
    /// Comando para detectar contactos pendientes de respuesta
    /// después de ciertos días.
    /// </summary>
    public class DetectarContactosPendientesCommand : ICommand
    {
        private readonly Interfaz _interfaz;

        /// <summary>
        /// Inicializa el comando con la interfaz principal del sistema.
        /// </summary>
        public DetectarContactosPendientesCommand(Interfaz interfaz)
        {
            _interfaz = interfaz;
        }

        /// <summary>
        /// Ejecuta: Kbr4 contactosPendientes [dias].
        /// Si no se indica, usa 3 días por defecto.
        /// Solo puede usarlo un vendedor logueado.
        /// </summary>
        public Task<string> ExecuteAsync(string[] args)
        {
            // Kbr4 contactosPendientes [dias]
            // Ejemplo: Kbr4 contactosPendientes
            //          Kbr4 contactosPendientes 7
            
            if (!_interfaz.EstaLogueado)
                return Task.FromResult("❌ Debes iniciar sesión primero.");

            if (!_interfaz.EsVendedor())
                return Task.FromResult("❌ Solo los vendedores pueden ver contactos pendientes.");

            int diasSinRespuesta = 3; // Por defecto 3 días

            // Si se especifica días, parsear
            if (args.Length > 0)
            {
                if (!int.TryParse(args[0], out diasSinRespuesta))
                    return Task.FromResult("❌ Los días deben ser un número.");
                
                if (diasSinRespuesta < 1)
                    return Task.FromResult("❌ Los días deben ser al menos 1.");
            }

            string resultado = _interfaz.DetectarContactosPendientes(diasSinRespuesta);

            return Task.FromResult(resultado);
        }
    }
}
