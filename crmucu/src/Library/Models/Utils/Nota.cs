namespace CrmUcu.Models.Utils
{
    /// <summary>
    /// Representa una nota asociada a una interacción o registro del sistema.
    /// Guarda un texto corto con la fecha en que se creó.
    /// </summary>
    public class Nota
    {
        private static int _contadorId = 1;
        
        public int Id { get; set; }
        public string Texto { get; set; } = string.Empty;
        public DateTime Fecha { get; set; } = DateTime.Now;
    
        /// <summary>
        /// Crea una nota vacía y le asigna un Id y fecha actual.
        /// </summary>
        public Nota()
        {
            Id = _contadorId++;
            Fecha = DateTime.Now;
        }
        
        /// <summary>
        /// Crea una nota con texto y fecha actual.
        /// </summary>
        public Nota(string texto)
        {
            Id = _contadorId++;
            Texto = texto;
            Fecha = DateTime.Now;
        }
    }
}
