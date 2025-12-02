using NUnit.Framework;
using System.Threading.Tasks;
using CrmUcu.Core;
using CrmUcu.Models.Personas;
using CrmUcu.Repositories;

namespace Library.Tests
{
    [TestFixture]
    public class HU03_EliminarClienteTests
    {
        [Test]
        public async Task EliminarClienteExistente()
        {
            // Arrange
            var interfaz = new Interfaz();

            var vendedor = new Vendedor(
                id: 1,
                mail: "nacho@gmail.com",
                nombre: "Nacho",
                apellido: "Silva",
                telefono: "098xxxyyy",
                nombreUsuario: "Tachoviendo",
                password: "123nacho");

            // Login del vendedor
            interfaz.IniciarSesion(vendedor.NombreUsuario, vendedor.Password);

            // Crear cliente
            vendedor.CrearCliente("Florencia", "Ferreira", "flor@gmail.com", "098111222");

            var repoClientes = RepositorioCliente.ObtenerInstancia();
            var cliente = repoClientes.ObtenerClientePorId(1);

            Assert.That(cliente, Is.Not.Null);

            // Act: eliminar cliente
            var resultado = vendedor.EliminarCliente(cliente.Id);

            // Assert
            Assert.That(resultado, Does.Contain("eliminado exitosamente"));
            Assert.That(repoClientes.ObtenerClientePorId(cliente.Id), Is.Null);

            await Task.CompletedTask;
        }
        [Test]
        public async Task eliminarClienteInexistente()
        {
            // Arrange
            var vendedor = new Vendedor(
                id: 1,
                mail: "nacho@gmail.com",
                nombre: "Nacho",
                apellido: "Silva",
                telefono: "098xxxyyy",
                nombreUsuario: "Tachoviendo",
                password: "123nacho");

            // Act
            var resultado = vendedor.EliminarCliente(999); // ID inexistente

            // Assert
            Assert.That(resultado, Is.EqualTo("Cliente no encontrado"));

            await Task.CompletedTask;
        }

    }
}