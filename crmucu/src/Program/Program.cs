namespace CrmUcu
{
    /// <summary>
    /// Punto de entrada principal de la aplicación.
    /// Inicializa y ejecuta el bot de Discord.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Método principal que inicia el bot y lo mantiene en ejecución.
        /// </summary>
        public static async Task Main(string[] args)
        {
            var bot = new Bot();
            await bot.StartAsync();
            await Task.Delay(-1); // Mantiene el bot corriendo indefinidamente
        }
    }
}
