using CrmUcu.Core;
using System.Threading.Tasks;

namespace CrmUcu.Commands
{
    /// <summary>
    /// Comando para suspender usuarios del sistema.
    /// Solo accesible para administradores.
    /// Aplica Command Pattern y SRP.
    /// </summary>
    public class SuspenderUsuarioCommand : ICommand
    {
        private readonly Interfaz _interfaz;
        
        public SuspenderUsuarioCommand(Interfaz interfaz)
        {
            _interfaz = interfaz;
        }
        
        /// <summary>
        /// Ejecuta el comando para suspender un usuario por ID.
        /// </summary>
        public Task<string> ExecuteAsync(string[] args)
        {
            if (!_interfaz.EstaLogueado)
                return Task.FromResult("❌ Debes iniciar sesión primero.");
            
            if (!_interfaz.EsAdmin())
                return Task.FromResult("❌ Solo los administradores pueden suspender usuarios.");
            
            if (args.Length < 1)
                return Task.FromResult("❌ Faltan datos. Usa: Kbr4 suspenderUsuario <id>");
            
            if (!int.TryParse(args[0], out int idUsuario))
                return Task.FromResult("❌ El ID debe ser un número.");
            
            string resultado = _interfaz.SuspenderUsuario(idUsuario);
            return Task.FromResult(resultado.Contains("exitosamente") 
                ? $"✅ {resultado}" 
                : $"❌ {resultado}");
        }
    }
}
