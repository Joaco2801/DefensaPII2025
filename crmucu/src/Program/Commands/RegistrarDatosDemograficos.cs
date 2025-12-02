using CrmUcu.Core;
using System;
using System.Threading.Tasks;

namespace CrmUcu.Commands
{
    /// <summary>
    /// Comando para registrar datos demográficos extra de un cliente
    /// (género y fecha de nacimiento).
    /// </summary>
    public class RegistrarDatosDemograficosCommand : ICommand
    {
        private readonly Interfaz _interfaz;

        /// <summary>
        /// Usa la interfaz principal del sistema para ejecutar la acción.
        /// </summary>
        /// <param name="interfaz">Instancia de <see cref="Interfaz"/>.</param>
        public RegistrarDatosDemograficosCommand(Interfaz interfaz)
        {
            _interfaz = interfaz;
        }

        /// <summary>
        /// Ejecuta:
        /// <c>Kbr4 registrarDatosExtra &lt;idCliente&gt; &lt;genero&gt; &lt;fechaNacimiento:dd/MM/yyyy&gt;</c>.
        /// Solo puede usarlo un vendedor logueado.
        /// </summary>
        /// <param name="args">
        /// Argumentos: id del cliente, género y fecha de nacimiento en formato dd/MM/yyyy.
        /// </param>
        /// <returns>Mensaje de éxito o error para mostrar en consola.</returns>
        public Task<string> ExecuteAsync(string[] args)
        {
            // Kbr4 registrarDatosExtra <idCliente> <genero> <fechaNacimiento:dd/MM/yyyy>
            // Ejemplo: Kbr4 registrarDatosExtra 1 Masculino 15/06/1990
            
            if (!_interfaz.EstaLogueado)
                return Task.FromResult("❌ Debes iniciar sesión primero.");

            if (!_interfaz.EsVendedor())
                return Task.FromResult("❌ Solo los vendedores pueden registrar datos extra.");

            if (args.Length < 3)
                return Task.FromResult("❌ Faltan datos. Usa: Kbr4 registrarDatosExtra <idCliente> <genero> <fechaNacimiento:dd/MM/yyyy>\n" +
                                     "Géneros: Masculino, Femenino, Otro, NoEspecifica\n" +
                                     "Ejemplo: Kbr4 registrarDatosExtra 1 Masculino 15/06/1990");

            if (!int.TryParse(args[0], out int idCliente))
                return Task.FromResult("❌ El ID del cliente debe ser un número.");

            string genero = args[1];

            // Parsear fecha de nacimiento
            if (!DateTime.TryParseExact(args[2], "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime fechaNacimiento))
                return Task.FromResult("❌ Formato de fecha inválido. Usa: dd/MM/yyyy (ejemplo: 15/06/1990)");

            string resultado = _interfaz.RegistrarDatosDemograficos(idCliente, genero, fechaNacimiento);

            return Task.FromResult(resultado.Contains("exitosamente") 
                ? $"✅ {resultado}" 
                : $"❌ {resultado}");
        }
    }
}
