using CrmUcu.Core;
using System.Threading.Tasks;

namespace CrmUcu.Commands;
//datos del vendedor y la diferencia del total de ventas (mes B – mes A)
//Como usuario quiero ver un listado de los vendedores que hayan
//disminuido sus ventas en los últimos 2 meses.
public class VentasDisminuidasCommand : ICommand //Comando Destinado para que los ADMINS
{
    private readonly Interfaz _interfaz;

    public VentasDisminuidasCommand(Interfaz interfaz)
    {
        
    }

    public Task<string> ExecuteAsync(string[] args)
    {
        if (!_interfaz.EstaLogueado)
            return Task.FromResult("❌ Debes iniciar sesión primero.");

        if (!_interfaz.EsAdmin())
            return Task.FromResult("❌ Solo los administradores pueden ver la Disminución de ventas");

        int mesA = 1; // ejemplo de meses para
        int mesB = 3;
        
        return Task.FromResult($"desde el mes {mesA} al mes {mesB}, los Vendedores que disminuyeron ventas fueron: ");
    }
}