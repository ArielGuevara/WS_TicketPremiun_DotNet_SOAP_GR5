//using Monster.Edu.Ec.TicketPremium.Datos; // Referencia a tu capa de datos EF
using Monster.Edu.Ec.TicketPremium.WCF.Modelos;
using System;
using System.Linq;
using TicketPremium.Datos;
using TicketPremium.WCF.FederacionWS;

namespace Monster.Edu.Ec.TicketPremium.WCF
{
    public class TicketPremiumService : ITicketPremiumService
    {
        
        public UsuarioDTO IniciarSesion(string correo, string password)
        {
            using (var context = new TicketPremiumDBEntities())
            {
                // 1. Buscar al usuario por su correo
                var usuario = context.USUARIO.FirstOrDefault(u => u.CORREO == correo);

                if (usuario != null)
                {
                    // 2. Verificar la contraseña ingresada contra el Hash de la BD
                    bool passwordValido = BCrypt.Net.BCrypt.Verify(password, usuario.PASSWORD_HASH);

                    if (passwordValido)
                    {
                        // 3. Login exitoso: Retornamos los datos y un Token simulado
                        return new UsuarioDTO
                        {
                            IdUsuario = usuario.ID_USUARIO,
                            Nombres = usuario.NOMBRES,
                            Correo = usuario.CORREO,
                            TokenSession = Guid.NewGuid().ToString() // Token único temporal
                        };
                    }
                }

                // Retorna null si las credenciales son incorrectas
                return null;
            }
        }

        public bool CerrarSesion(string token)
        {
            // Para el backend stateless, solo validamos que llegue un token.
            if (!string.IsNullOrEmpty(token))
            {
                return true;
            }
            return false;
        }

        public bool RegistrarUsuario(string nombres, string correo, string password)
        {
            using (var context = new TicketPremiumDBEntities())
            {
                // 1. Validar si el correo ya está registrado en la base de datos
                bool usuarioExiste = context.USUARIO.Any(u => u.CORREO == correo);

                if (usuarioExiste)
                {
                    return false;
                }

                // 2. Encriptar la contraseña de forma segura con BCrypt
                string hashGenerado = BCrypt.Net.BCrypt.HashPassword(password);

                // 3. Crear el nuevo objeto de Entity Framework
                var nuevoUsuario = new USUARIO
                {
                    NOMBRES = nombres,
                    CORREO = correo,
                    PASSWORD_HASH = hashGenerado,
                    ESTADO = true // Por defecto lo creamos activo
                };

                // 4. Agregar al contexto y guardar cambios en SQL Server
                context.USUARIO.Add(nuevoUsuario);
                context.SaveChanges();

                return true; // Registro exitoso
            }
        }

        public FacturaDTO ComprarBoletos(int idUsuario, int codigoPartido, string codigoLocalidad, int cantidadBoletos, decimal precioUnitario)
        {
            // 1. Nos comunicamos con el Web Service de la Federación
            // Usamos el namespace que le dimos a la referencia de servicio en el Paso 1
            FederacionServiceClient federacionClient = new FederacionServiceClient();

            try
            {
                // Invocamos el método del otro sistema para restar la disponibilidad
                bool boletosDescontados = federacionClient.DisminuirDisponibilidad(codigoPartido, codigoLocalidad, cantidadBoletos);

                if (!boletosDescontados)
                {
                    return new FacturaDTO { Mensaje = "Error: No hay suficientes boletos disponibles o la localidad no existe." };
                }

                // 2. Si la Federación confirmó el descuento, procedemos a facturar
                decimal subtotal = cantidadBoletos * precioUnitario;
                decimal iva = subtotal * 0.15m; // 15% de IVA
                decimal totalFinal = subtotal + iva;

                using (var context = new TicketPremiumDBEntities())
                {
                    // 3. Crear la cabecera de la factura
                    var nuevaFactura = new FACTURA
                    {
                        ID_USUARIO = idUsuario,
                        FECHA_EMISION = DateTime.Now,
                        SUBTOTAL = subtotal,
                        IVA = iva,
                        TOTAL_FINAL = totalFinal
                    };

                    // 4. Crear el detalle
                    var nuevoDetalle = new DETALLE_FACTURA
                    {
                        CODIGO_PARTIDO = codigoPartido,
                        CODIGO_LOCALIDAD = codigoLocalidad,
                        BOLETOS_VENDIDOS = cantidadBoletos,
                        TOTAL_RECAUDADO = subtotal // Subtotal sin IVA por los boletos
                    };

                    // Entity Framework permite agregar el detalle directamente a la cabecera
                    nuevaFactura.DETALLE_FACTURA.Add(nuevoDetalle);

                    // 5. Guardar en SQL Server
                    context.FACTURA.Add(nuevaFactura);
                    context.SaveChanges();

                    return new FacturaDTO
                    {
                        IdFactura = nuevaFactura.ID_FACTURA,
                        FechaEmision = nuevaFactura.FECHA_EMISION,
                        Subtotal = nuevaFactura.SUBTOTAL,
                        Iva = nuevaFactura.IVA,
                        TotalFinal = nuevaFactura.TOTAL_FINAL,
                        Mensaje = "Compra exitosa y factura generada."
                    };
                }
            }
            catch (Exception ex)
            {
                return new FacturaDTO { Mensaje = "Error interno al procesar la compra: " + ex.Message };
            }
            finally
            {
                // Es buena práctica cerrar el cliente WCF
                if (federacionClient.State == System.ServiceModel.CommunicationState.Opened)
                {
                    federacionClient.Close();
                }
            }
        }
    }
}