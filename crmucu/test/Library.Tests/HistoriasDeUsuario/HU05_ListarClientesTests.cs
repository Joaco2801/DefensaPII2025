using NUnit.Framework;
using System.Threading.Tasks;
using CrmUcu.Repositories;
using CrmUcu.Models.Personas;

namespace Library.Tests
{
    [TestFixture]
    public class HU05_ListarClientesTests
    {
        [Test]
        public async Task ListarClientes()
        {
            // Arrange
            var repoClientes = RepositorioCliente.ObtenerInstancia();

            // Limpiar repositorio para asegurar estado controlado
            repoClientes.ObtenerTodos().Clear();

            var vendedor = new Vendedor(
                id: 1,
                mail: "nacho@gmail.com",
                nombre: "Nacho",
                apellido: "Silva",
                telefono: "098xxxyyy",
                nombreUsuario: "Tachoviendo",
                password: "123nacho");

            // Crear clientes
            var cliente1 = repoClientes.CrearCliente("flor@gmail.com", "Florencia", "Ferreira", "098111222", vendedor.Id);
            var cliente2 = repoClientes.CrearCliente("juan@gmail.com", "Juan", "Pérez", "098333444", vendedor.Id);

            // Act
            var clientes = repoClientes.ObtenerTodos();

            // Assert
            Assert.That(clientes.Count, Is.EqualTo(2));
            Assert.That(clientes[0].Nombre, Is.EqualTo("Florencia"));
            Assert.That(clientes[1].Nombre, Is.EqualTo("Juan"));

            await Task.CompletedTask;
        }
        [Test]
        public async Task listarClientesSinClientes()
        {
            // Arrange
            var repoClientes = RepositorioCliente.ObtenerInstancia();
            repoClientes.ObtenerTodos().Clear();

            // Act
            var clientes = repoClientes.ObtenerTodos();

            // Assert
            Assert.That(clientes, Is.Empty);

            await Task.CompletedTask;
        }

    }
}