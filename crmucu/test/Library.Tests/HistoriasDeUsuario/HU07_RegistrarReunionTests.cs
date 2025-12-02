using NUnit.Framework;
using System.Threading.Tasks;
using CrmUcu.Models.Personas;
using CrmUcu.Models.Interacciones;
using CrmUcu.Repositories;
using CrmUcu.Models.Enums;

namespace Library.Tests
{
    [TestFixture]
    public class HU07_RegistrarReunionTests
    {
        [Test]
        public async Task RegistrarReunion()
        {
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

            var cliente = repoClientes.CrearCliente("flor@gmail.com", "Florencia", "Ferreira", "098111222", vendedor.Id);

            var reunion = new Reunion(
                idCliente: cliente.Id,
                fecha: DateTime.Now.AddDays(1),
                descripcion: "Reunión de prueba",
                ubicacion: "Oficina Central",
                duracionMinutos: 60,
                estado: EstadoReunion.Agendada);

            var resultado = vendedor.RegistrarInteraccion(cliente.Id, reunion);

            Assert.That(resultado, Does.Contain("Reunion registrada exitosamente"));
            Assert.That(cliente.Interacciones[0], Is.InstanceOf<Reunion>());
            Assert.That(((Reunion)cliente.Interacciones[0]).Estado, Is.EqualTo(EstadoReunion.Agendada));

            await Task.CompletedTask;
        }
    }
}
