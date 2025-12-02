using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrmUcu.Models.Personas;
using CrmUcu.Models.Interacciones;
using CrmUcu.Models.Utils;
using CrmUcu.Repositories;

namespace Library.Tests
{
    [TestFixture]
    public class HU08_RegistrarCorreoTests
    {
        [Test]
        public async Task RegistrarCorreo()
        {
            var repoClientes = RepositorioCliente.ObtenerInstancia();
            repoClientes.ObtenerTodos().Clear();

            var vendedor = new Vendedor(
                1,
                "nacho@gmail.com",
                "Nacho",
                "Silva",
                "098xxxyyy",
                "Tachoviendo",
                "123nacho");

            var cliente = repoClientes.CrearCliente("flor@gmail.com", "Florencia", "Ferreira", "098111222", vendedor.Id);

            var correo = new Mail(
                cliente.Id,
                DateTime.Now,
                "Correo de prueba",
                true,
                "Asunto de prueba",
                new List<string> { "destinatario@gmail.com" });

            var resultado = vendedor.RegistrarInteraccion(cliente.Id, correo);

            Assert.That(resultado, Does.Contain("Mail registrada exitosamente"));
            Assert.That(cliente.Interacciones, Is.Not.Empty);
            Assert.That(cliente.Interacciones[0], Is.InstanceOf<Mail>());
            Assert.That(((Mail)cliente.Interacciones[0]).Respondida, Is.False);

            await Task.CompletedTask;
        }

        [Test]
        public async Task RegistrarCorreoClienteInexistent()
        {
            var vendedor = new Vendedor(
                1,
                "nacho@gmail.com",
                "Nacho",
                "Silva",
                "098xxxyyy",
                "Tachoviendo",
                "123nacho");

            var correo = new Mail(
                999,
                DateTime.Now,
                "Correo a cliente inexistente",
                true,
                "Asunto inexistente",
                new List<string> { "otro@gmail.com" });

            var resultado = vendedor.RegistrarInteraccion(999, correo);

            Assert.That(resultado, Is.EqualTo("Cliente no encontrado"));

            await Task.CompletedTask;
        }
    }
}