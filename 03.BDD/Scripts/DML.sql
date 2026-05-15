-- =============================================
-- Seleccionar la base de datos
-- =============================================


USE FederacionFutbolDB;
GO
-- ===============================================
-- INSERCIONES DE PRUEBA 
-- ===============================================

-- Inserción de 5 registros para PARTIDO_FUTBOL
INSERT INTO PARTIDO_FUTBOL (EQUIPO_LOCAL, EQUIPO_VISITA, FECHA, LUGAR) VALUES
('Liga de Quito', 'Barcelona SC', '2026-06-15 16:00:00', 'Estadio Rodrigo Paz Delgado'),
('Independiente del Valle', 'Emelec', '2026-06-16 19:00:00', 'Estadio Banco Guayaquil'),
('El Nacional', 'Aucas', '2026-06-20 12:00:00', 'Estadio Olímpico Atahualpa'),
('Deportivo Cuenca', 'Mushuc Runa', '2026-06-21 15:30:00', 'Estadio Alejandro Serrano Aguilar'),
('Universidad Católica', 'Delfín', '2026-06-25 18:00:00', 'Estadio Olímpico Atahualpa');
GO

-- Inserción de 20 registros para LOCALIDAD_PARTIDO (4 localidades por cada uno de los 5 partidos)
INSERT INTO LOCALIDAD_PARTIDO (CODIGO_PARTIDO, CODIGO_LOCALIDAD, DISPONIBILIDAD, PRECIO) VALUES
-- Partido 1
(1, 'GENERAL', 1500, 10.00), (1, 'TRIBUNA', 800, 15.00), (1, 'PALCO', 300, 25.00), (1, 'GENERAL VISITA', 500, 10.00),
-- Partido 2
(2, 'GENERAL', 1200, 12.00), (2, 'TRIBUNA', 600, 18.00), (2, 'PALCO', 200, 30.00), (2, 'GENERAL VISITA', 400, 12.00),
-- Partido 3
(3, 'GENERAL', 2000, 8.00),  (3, 'TRIBUNA', 1000, 12.00),(3, 'PALCO', 400, 20.00), (3, 'GENERAL VISITA', 800, 8.00),
-- Partido 4
(4, 'GENERAL', 1000, 10.00), (4, 'TRIBUNA', 500, 15.00), (4, 'PALCO', 150, 25.00), (4, 'GENERAL VISITA', 300, 10.00),
-- Partido 5
(5, 'GENERAL', 800, 8.00),   (5, 'TRIBUNA', 400, 12.00), (5, 'PALCO', 100, 20.00), (5, 'GENERAL VISITA', 200, 8.00);
GO


USE TicketPremiumDB;
GO
-- La contraseña de este hash es: admin123
INSERT INTO USUARIO (NOMBRES, CORREO, PASSWORD_HASH, ESTADO) 
VALUES ('Pedro', 'pedro@gmail.com', '$2a$11$HNW9bzq.U0Mgi01KjPEaU..OPJd/4Fyziq6ZQzDNUCeEVWiTh.kk.', 1);
GO
