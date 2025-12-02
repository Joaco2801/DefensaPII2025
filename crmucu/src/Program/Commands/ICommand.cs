namespace CrmUcu.Commands
{
    /// <summary>
    /// Interfaz que define el contrato para todos los comandos del sistema.
    /// Aplica Command Pattern: encapsula acciones como objetos ejecutables.
    /// Aplica ISP (Interface Segregation): interfaz pequeña y específica.
    /// Aplica OCP: nuevos comandos se agregan sin modificar código existente.
    /// Permite polimorfismo: todos los comandos se ejecutan uniformemente.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Ejecuta el comando con los argumentos proporcionados.
        /// </summary>
        /// <param name="args">Argumentos del comando desde Discord</param>
        /// <returns>Mensaje de respuesta para el usuario</returns>
        Task<string> ExecuteAsync(string[] args);
    }
}
