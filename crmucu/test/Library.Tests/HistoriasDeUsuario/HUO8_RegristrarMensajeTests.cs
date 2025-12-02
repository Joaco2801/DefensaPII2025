
using NUnit.Framework;
using CrmUcu.Models.Personas;
using CrmUcu.Repositories;
using CrmUcu.Models.Enums;
using CrmUcu.Models.Interacciones;
using System.Collections.Generic;

namespace Library.Tests
{
    [TestFixture]
    public class HU_RegistrarMensajeTests
    {
        private Vendedor _vendedor;
        private Cliente _cliente;

        [SetUp]
        public void Setup()
        {
            var repoVendedores = RepositorioVendedor.ObtenerInstancia();
            _vendedor = repoVendedores.BuscarPorId(1);
            
            var repoClientes = RepositorioCliente.ObtenerInstancia();
            _cliente = repoClientes.CrearCliente(
                "test@mail.com",
                "Test",
                "Cliente",
                "099111222",
                _vendedor.Id
            );
        }

        [Test]
        public void RegistrarMensajeSaliente()
        {
            List<string> destinatarios = new List<string> { "cliente@mail.com" };

            string resultado = _vendedor.RegistrarInteraccion(
                _cliente.Id,
                new Mensaje( 
                    _cliente.Id,
                    System.DateTime.Now,
                    "Recordatorio de reunión",
                    false,
                    "Seguimiento",
                    destinatarios
                )
            );

            Assert.That(resultado, Does.Contain("exitosamente"));
            Assert.That(_cliente.Interacciones.Count, Is.EqualTo(1));
            Assert.That(_cliente.Interacciones[0].Tipo, Is.EqualTo(TipoInteraccion.Mensaje));
        }
    }
}

