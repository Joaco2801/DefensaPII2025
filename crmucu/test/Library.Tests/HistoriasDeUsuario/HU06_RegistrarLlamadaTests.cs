using NUnit.Framework;
using System.Threading.Tasks;
using CrmUcu.Models.Personas;
using CrmUcu.Models.Interacciones;
using CrmUcu.Repositories;

namespace Library.Tests
{
    [TestFixture]
    public class HU06_RegistrarLlamadaTests
    {
        [Test]
        public async Task RegistrarLlamada()
        {
            // Arrange
            var repoClientes = RepositorioCliente.ObtenerInstancia();
            repoClientes.ObtenerTodos().Clear();

            var vendedor = new Vendedor(
                id: 1,
                mail: "nacho@gmail.com",
                nombre: "Nacho",
                apellido: "Silva",
                telefono: "098xxxyyy",
                nombreUsuario: "Tachoviendo",
                password: "123nacho");

            // Crear cliente
            var cliente = repoClientes.CrearCliente("flor@gmail.com", "Florencia", "Ferreira", "098111222", vendedor.Id);

            // Crear interacción de llamada
            var llamada = new Llamada(
                idCliente: cliente.Id,
                fecha: DateTime.Now,
                descripcion: "Llamada de prueba",
                esEntrante: true,
                duracionSegundos: 120,
                contestada: true);

            // Act
            var resultado = vendedor.RegistrarInteraccion(cliente.Id, llamada);

            // Assert
            Assert.That(resultado, Does.Contain("Llamada registrada exitosamente"));
            Assert.That(cliente.Interacciones, Is.Not.Empty);
            Assert.That(cliente.Interacciones[0], Is.InstanceOf<Llamada>());

            await Task.CompletedTask;
        }

        [Test]
        public async Task RegistrarLlamadaClienteInexistente()
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

            var llamada = new Llamada(
                idCliente: 999, // cliente inexistente
                fecha: DateTime.Now,
                descripcion: "Llamada a cliente inexistente",
                esEntrante: true,
                duracionSegundos: 60,
                contestada: false);

            // Act
            var resultado = vendedor.RegistrarInteraccion(999, llamada);

            // Assert
            Assert.That(resultado, Is.EqualTo("Cliente no encontrado"));

            await Task.CompletedTask;
        }
    }
}