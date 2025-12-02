using NUnit.Framework;
using CrmUcu.Models.Personas;
using CrmUcu.Repositories;


namespace Library.Tests
{
    [TestFixture]  
    public class HU01_CrearClienteTests
    {
        [Test]  
        public void CrearCliente()
        {
            // Arrange
            var repoCliente = RepositorioCliente.ObtenerInstancia();  
            var vendedor = new Vendedor(1, "nacho@gmail.com", "Nacho", "Silva", "098xxxyyy", "Tachoviendo", "123nacho");
            
            string nombre = "Florencia";
            string apellido = "Ferreira";
            string mail = "flor@gmail.com";
            string celular = "098111222";
            
            // Act
            vendedor.CrearCliente(nombre, apellido, mail, celular);
            var cliente = repoCliente._clientes[0];
            
            // Assert 
            Assert.That(cliente, Is.Not.Null);
            Assert.That(cliente.Nombre, Is.EqualTo(nombre));  
            Assert.That(cliente.Apellido, Is.EqualTo(apellido));  
            Assert.That(cliente.Mail, Is.EqualTo(mail));  
            Assert.That(cliente.Telefono, Is.EqualTo(celular));  
        }
    }
}
