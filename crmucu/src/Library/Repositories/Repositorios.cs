namespace CrmUcu.Repositories
{
    /// <summary>
    /// Clase abstracta que define la estructura común para todos los repositorios.
    /// Aplica Template Method: define el esqueleto de operaciones que las subclases implementan.
    /// Aplica DIP (Dependency Inversion): depende de abstracciones, no de implementaciones concretas.
    /// Aplica OCP (Open/Closed): abierto a extensión (nuevos repos), cerrado a modificación.
    /// </summary>
    public abstract class Repositorio<T>
    {
        protected List<T> elementos = new();
        
     
        public abstract void Agregar(T elemento);
        
      
        public abstract List<T> Buscar(string filtro);
        
      
        public abstract List<T> ObtenerTodos();
     
        public abstract void Eliminar(int id);
        
    
        public abstract void Editar(T elemento);
    }
}
