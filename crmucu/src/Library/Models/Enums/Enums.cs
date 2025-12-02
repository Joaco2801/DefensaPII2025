namespace CrmUcu.Models.Enums
{
    public enum EstadoUsuario
    {
        Activo,
        Suspendido,
        Eliminado
    }

    public enum Genero
    {
        Masculino,
        Femenino,
        Otro,
        NoEspecifica
    }

    public enum TipoInteraccion
    {
        Llamada,
        Reunion,
        Mensaje,
        Mail,
        Cotizacion,
        Venta
    }

    public enum EstadoCotizacion
    {
        Pendiente,
        Aceptada,
        Rechazada,
        Vencida
    }

    public enum EstadoReunion
    {
        Agendada,
        Realizada,
        Cancelada
    }
}
