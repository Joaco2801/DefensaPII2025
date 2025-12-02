using NUnit.Framework;
using System.Threading.Tasks;
using CrmUcu.Core;
using CrmUcu.Models.Personas;
using CrmUcu.Repositories;

namespace Library.Tests
{
    [TestFixture]
    public class HU20_ReasignarClienteTests
    {
        [Test]
        public async Task ClientePerteneceAlVendedor()
        {
            var repoClientes = RepositorioCliente.ObtenerInstancia();
            repoClientes.ObtenerTodos().Clear();

            var repoVendedores = RepositorioVendedor.ObtenerInstancia();
            repoVendedores.ObtenerTodos().Clear();

            var interfaz = new Interfaz();

            var vendedor1 = new Vendedor(1, "vend1@test.com", "Vend", "Uno", "099000001", "user1", "pass1");
            var vendedor2 = new Vendedor(2, "vend2@test.com", "Vend", "Dos", "099000002", "user2", "pass2");

            repoVendedores.ObtenerTodos().Add(vendedor1);
            repoVendedores.ObtenerTodos().Add(vendedor2);

            interfaz.IniciarSesion("user1", "pass1");

            var cliente = repoClientes.CrearCliente("cli@test.com", "Cli", "ente", "099111111", vendedor1.Id);
            
            Assert.That(cliente.IdVendedor, Is.EqualTo(vendedor1.Id));

            await Task.CompletedTask;
        }
    }
}