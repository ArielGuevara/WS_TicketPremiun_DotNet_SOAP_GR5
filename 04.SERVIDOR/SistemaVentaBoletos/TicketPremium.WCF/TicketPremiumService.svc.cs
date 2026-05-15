//using Monster.Edu.Ec.TicketPremium.Datos; // Referencia a tu capa de datos EF
using Monster.Edu.Ec.TicketPremium.WCF.Modelos;
using System;
using System.Linq;
using TicketPremium.Datos;

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
    }
}