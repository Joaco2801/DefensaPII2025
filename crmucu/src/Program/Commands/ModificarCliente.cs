using CrmUcu.Core;
using System.Threading.Tasks;

namespace CrmUcu.Commands
{
    /// <summary>
    /// Comando para modificar datos básicos de un cliente.
    /// </summary>
    public class ModificarClienteCommand : ICommand
    {
        private readonly Interfaz _interfaz;

        /// <summary>
        /// Usa la interfaz principal para aplicar los cambios.
        /// </summary>
        public ModificarClienteCommand(Interfaz interfaz)
        {
            _interfaz = interfaz;
        }

        /// <summary>
        /// Ejecuta: Kbr4 modificarCliente &lt;id&gt; &lt;campo&gt; &lt;nuevoValor&gt;.
        /// Campos: nombre, apellido, mail, telefono.
        /// </summary>
        public Task<string> ExecuteAsync(string[] args)
        {
            // Kbr4 modificarCliente <id> <campo> <nuevoValor>
            // Ejemplo: Kbr4 modificarCliente 1 nombre Juan
            
            if (!_interfaz.EstaLogueado)
                return Task.FromResult("❌ Debes iniciar sesión primero.");

            if (!_interfaz.EsVendedor())
                return Task.FromResult("❌ Solo los vendedores pueden modificar clientes.");

            if (args.Length < 3)
                return Task.FromResult("❌ Faltan datos. Usa: Kbr4 modificarCliente <id> <campo> <nuevoValor>\n" +
                                     "Campos disponibles: nombre, apellido, mail, telefono");

            if (!int.TryParse(args[0], out int idCliente))
                return Task.FromResult("❌ El ID debe ser un número.");

            string campo = args[1].ToLower();
            string nuevoValor = args[2];

            string resultado;

            switch (campo)
            {
                case "nombre":
                    resultado = _interfaz.ModificarClienteComoVendedor(idCliente, nuevoNombre: nuevoValor);
                    break;
                    
                case "apellido":
                    resultado = _interfaz.ModificarClienteComoVendedor(idCliente, nuevoApellido: nuevoValor);
                    break;
                    
                case "mail":
                    resultado = _interfaz.ModificarClienteComoVendedor(idCliente, nuevoMail: nuevoValor);
                    break;
                    
                case "telefono":
                    resultado = _interfaz.ModificarClienteComoVendedor(idCliente, nuevoTelefono: nuevoValor);
                    break;
                    
                default:
                    return Task.FromResult("❌ Campo inválido. Usa: nombre, apellido, mail o telefono");
            }

            return Task.FromResult(resultado.Contains("exitosamente") 
                ? $"✅ {resultado}" 
                : $"❌ {resultado}");
        }
    }
}
