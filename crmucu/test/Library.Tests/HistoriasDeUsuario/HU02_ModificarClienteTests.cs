using NUnit.Framework;
using System.Threading.Tasks;
using CrmUcu.Core;
using CrmUcu.Models.Personas;
using CrmUcu.Repositories;

namespace Library.Tests
{
    [TestFixture]
    public class HU02_ModificarClienteTests
    {
        [Test]
        public async Task modificarClienteCambiaNombreDevuelveExito()
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

            // Simular login del vendedor
            interfaz.IniciarSesion(vendedor.NombreUsuario, vendedor.Password);

            // Crear cliente desde el vendedor
            vendedor.CrearCliente("Florencia", "Ferreira", "flor@gmail.com", "098111222");

            // Recuperar el cliente recién creado desde el repositorio
            var repoClientes = RepositorioCliente.ObtenerInstancia();
            var cliente = repoClientes.ObtenerClientePorId(1); // primer cliente creado

            Assert.That(cliente, Is.Not.Null);

            // Act: modificar nombre a través de la Interfaz
            var resultado = interfaz.ModificarClienteComoVendedor(
                idCliente: cliente.Id,
                nuevoNombre: "Juana");

            // Assert
            Assert.That(resultado, Does.Contain("exitosamente"));
            Assert.That(cliente.Nombre, Is.EqualTo("Juana"));

            await Task.CompletedTask;
        }
    }
}