using System;
using System.Collections.Generic;
using System.Linq;
using Monster.Edu.Ec.Federacion.Datos; // Referencia a la capa EF
using Monster.Edu.Ec.Federacion.WCF.Models;

namespace Monster.Edu.Ec.Federacion.WCF
{
    public class FederacionService : IFederacionService
    {
        // Método 1: Partidos cuya fecha es mayor o igual a la actual
        public List<PartidoDTO> ObtenerPartidosDisponibles()
        {
            using (var context = new FederacionFutbolDBEntities())
            {
                var fechaActual = DateTime.Now;
                return context.PARTIDO_FUTBOL
                              .Where(p => p.FECHA >= fechaActual)
                              .Select(p => new PartidoDTO
                              {
                                  Codigo = p.CODIGO,
                                  EquipoLocal = p.EQUIPO_LOCAL,
                                  EquipoVisita = p.EQUIPO_VISITA,
                                  Fecha = p.FECHA,
                                  Lugar = p.LUGAR
                              }).ToList();
            }
        }

        // Método 2: Localidades del partido seleccionado con disponibilidad > 0
        public List<LocalidadDTO> ObtenerLocalidadesDisponibles(int codigoPartido)
        {
            using (var context = new FederacionFutbolDBEntities())
            {
                return context.LOCALIDAD_PARTIDO
                              .Where(l => l.CODIGO_PARTIDO == codigoPartido && l.DISPONIBILIDAD > 0)
                              .Select(l => new LocalidadDTO
                              {
                                  CodigoLocalidad = l.CODIGO_LOCALIDAD,
                                  Disponibilidad = l.DISPONIBILIDAD,
                                  Precio = l.PRECIO
                              }).ToList();
            }
        }

        // Método 3: Decrementar disponibilidad al comprar
        public bool DisminuirDisponibilidad(int codigoPartido, string codigoLocalidad, int boletosComprados)
        {
            using (var context = new FederacionFutbolDBEntities())
            {
                var localidad = context.LOCALIDAD_PARTIDO
                                       .FirstOrDefault(l => l.CODIGO_PARTIDO == codigoPartido &&
                                                            l.CODIGO_LOCALIDAD == codigoLocalidad);

                if (localidad != null && localidad.DISPONIBILIDAD >= boletosComprados)
                {
                    localidad.DISPONIBILIDAD -= boletosComprados;
                    context.SaveChanges();
                    return true; // Compra exitosa
                }
                return false; // No hay suficientes boletos o no existe la localidad
            }
        }
    }
}