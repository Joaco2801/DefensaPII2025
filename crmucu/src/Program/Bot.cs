using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace CrmUcu
{
    /// <summary>
    /// Clase principal del bot de Discord.
    /// Gestiona la conexión con Discord y delega el manejo de comandos a CommandHandler.
    /// Aplica SRP: solo se encarga de la conexión y lifecycle del bot.
    /// Aplica Controller: actúa como punto de entrada para eventos de Discord.
    /// </summary>
    public class Bot
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandHandler _commandHandler;
        
        /// <summary>
        /// Constructor del bot. Inicializa el cliente de Discord y el manejador de comandos.
        /// </summary>
        public Bot()
        {
            _client = new DiscordSocketClient();
            _client.Log += Log;
            
            _commandHandler = new CommandHandler();
            _client.MessageReceived += _commandHandler.HandleMessageAsync;
        }
        
        /// <summary>
        /// Inicia el bot y se conecta a Discord.
        /// </summary>
        public async Task StartAsync()
        {
            string token = "";
            
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
        }
        
        /// <summary>
        /// Registra mensajes de log del cliente de Discord.
        /// </summary>
        private Task Log(LogMessage log)
        {
            Console.WriteLine(log);
            return Task.CompletedTask;
        }
    }
}
