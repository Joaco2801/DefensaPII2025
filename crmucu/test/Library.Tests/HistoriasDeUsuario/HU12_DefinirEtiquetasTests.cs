using NUnit.Framework;
using CrmUcu.Core;
using CrmUcu.Models.Personas;
using CrmUcu.Repositories;
using System.Threading.Tasks;

namespace Library.Tests
{
    [TestFixture]
    public class HU12_AgregarEtiquetaInterfazTests
    {
        [Test]
        public async Task AgregarEtiquetaACliente()
        {
            var repoClientes = RepositorioCliente.ObtenerInstancia();
            repoClientes.ObtenerTodos().Clear();

            var interfaz = new Interfaz();

            var vendedor = new Vendedor(
                1,
                "nacho@gmail.com",
                "Nacho",
                "Silva",
                "098xxxyyy",
                "Tachoviendo",
                "123nacho");

            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Clear();
            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Add(vendedor);

            interfaz.IniciarSesion("Tachoviendo", "123nacho");

            var cliente = repoClientes.CrearCliente("flor@gmail.com", "Florencia", "Ferreira", "098111222", vendedor.Id);

            var ok = interfaz.AgregarEtiquetaACliente(cliente.Id, "VIP", "#FFD700", "Cliente prioritario");

            Assert.That(ok, Is.True);
            Assert.That(cliente.Etiquetas.Count, Is.EqualTo(1));
            Assert.That(cliente.Etiquetas[0].Nombre, Is.EqualTo("VIP"));
            Assert.That(cliente.Etiquetas[0].Color, Is.EqualTo("#FFD700"));
            Assert.That(cliente.Etiquetas[0].Descripcion, Is.EqualTo("Cliente prioritario"));

            await Task.CompletedTask;
        }

        [Test]
        public async Task AgregarEtiquetaAClienteClienteInexistente()
        {
            var interfaz = new Interfaz();

            var vendedor = new Vendedor(1, "vend@test.com", "Test", "Vendedor", "099000000", "user", "pass");
            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Clear();
            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Add(vendedor);

            interfaz.IniciarSesion("user", "pass");

            var ok = interfaz.AgregarEtiquetaACliente(999, "VIP", "#FFD700", "Cliente prioritario");

            Assert.That(ok, Is.False);

            await Task.CompletedTask;
        }

        [Test]
        public async Task AgregarEtiquetaAClienteSinDescripcion()
        {
            var repoClientes = RepositorioCliente.ObtenerInstancia();
            repoClientes.ObtenerTodos().Clear();

            var interfaz = new Interfaz();

            var vendedor = new Vendedor(1, "vend@test.com", "Test", "Vendedor", "099000000", "user", "pass");
            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Clear();
            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Add(vendedor);

            interfaz.IniciarSesion("user", "pass");

            var cliente = repoClientes.CrearCliente("cli@test.com", "Cli", "ente", "099111111", vendedor.Id);

            var ok = interfaz.AgregarEtiquetaACliente(cliente.Id, "Frecuente", null, null);

            Assert.That(ok, Is.True);
            Assert.That(cliente.Etiquetas.Count, Is.EqualTo(1));
            Assert.That(cliente.Etiquetas[0].Nombre, Is.EqualTo("Frecuente"));
            Assert.That(cliente.Etiquetas[0].Color, Is.Null);
            Assert.That(cliente.Etiquetas[0].Descripcion, Is.Null);

            await Task.CompletedTask;
        }
    }
}
