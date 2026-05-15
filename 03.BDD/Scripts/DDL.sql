-- ======================================================================
-- SCRIPT DDL: SISTEMA FEDERACIÓN DE FÚTBOL
-- ======================================================================

USE master;
go

IF( EXISTS ( SELECT name FROM master.sys.databases WHERE name = 'FederacionFutbolDB' ) )
BEGIN
	DROP DATABASE FederacionFutbolDB;
END;
go

CREATE DATABASE FederacionFutbolDB;
GO

USE FederacionFutbolDB;
GO

-- Tabla: PARTIDO_FUTBOL
CREATE TABLE PARTIDO_FUTBOL (
    CODIGO INT IDENTITY(1,1) PRIMARY KEY, 
    EQUIPO_LOCAL VARCHAR(100) NOT NULL,
    EQUIPO_VISITA VARCHAR(100) NOT NULL,
    FECHA DATETIME NOT NULL,
    LUGAR VARCHAR(200) NOT NULL
);
GO

-- Tabla: LOCALIDAD_PARTIDO
-- Usamos una llave primaria compuesta (CODIGO_PARTIDO, CODIGO_LOCALIDAD) 
-- para garantizar que no se repita una misma localidad en un mismo partido.
CREATE TABLE LOCALIDAD_PARTIDO (
    CODIGO_PARTIDO INT NOT NULL,
    CODIGO_LOCALIDAD VARCHAR(50) NOT NULL, -- Ej: 'PALCO', 'TRIBUNA', 'GENERAL'
    DISPONIBILIDAD INT NOT NULL,
    PRECIO DECIMAL(10,2) NOT NULL,
    CONSTRAINT PK_LocalidadPartido PRIMARY KEY (CODIGO_PARTIDO, CODIGO_LOCALIDAD),
    CONSTRAINT FK_Localidad_Partido FOREIGN KEY (CODIGO_PARTIDO) 
        REFERENCES PARTIDO_FUTBOL(CODIGO) ON DELETE CASCADE
);
GO

--**********************************************************************************************************************
-- ======================================================================
-- SCRIPT DDL: SISTEMA TICKET PREMIUM
-- ======================================================================
USE master;
GO

IF( EXISTS ( SELECT name FROM master.sys.databases WHERE name = 'TicketPremiumDB' ) )
BEGIN
    -- Forzar el cierre de conexiones activas antes de eliminar
    ALTER DATABASE TicketPremiumDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE TicketPremiumDB;
END;
GO

CREATE DATABASE TicketPremiumDB;
GO

-- =============================================
-- Seleccionar la Base de Datos
-- =============================================
USE TicketPremiumDB;
GO

-- Tabla: USUARIO
CREATE TABLE USUARIO (
    ID_USUARIO INT IDENTITY(1,1) PRIMARY KEY,
    NOMBRES VARCHAR(100) NOT NULL,
    CORREO VARCHAR(100) NOT NULL UNIQUE,
    PASSWORD_HASH VARCHAR(255) NOT NULL, -- Almacenará el hash encriptado con BCrypt
    ESTADO BIT DEFAULT 1 -- 1: Activo, 0: Inactivo
);
GO

-- Tabla: FACTURA
CREATE TABLE FACTURA (
    ID_FACTURA INT IDENTITY(1,1) PRIMARY KEY,
    ID_USUARIO INT NOT NULL, -- Vinculación con el usuario comprador
    FECHA_EMISION DATETIME NOT NULL DEFAULT GETDATE(),
    SUBTOTAL DECIMAL(10,2) NOT NULL,
    IVA DECIMAL(10,2) NOT NULL, -- Valor calculado del impuesto
    TOTAL_FINAL DECIMAL(10,2) NOT NULL,
    
    CONSTRAINT FK_Factura_Usuario FOREIGN KEY (ID_USUARIO) 
        REFERENCES USUARIO(ID_USUARIO)
);
GO

-- Tabla: DETALLE_FACTURA
CREATE TABLE DETALLE_FACTURA (
    ID_DETALLE INT IDENTITY(1,1) PRIMARY KEY,
    ID_FACTURA INT NOT NULL,

    CODIGO_PARTIDO INT NOT NULL, 
    CODIGO_LOCALIDAD VARCHAR(50) NOT NULL, 
    
    BOLETOS_VENDIDOS INT NOT NULL,
    TOTAL_RECAUDADO DECIMAL(10,2) NOT NULL, -- Boletos * Precio Unitario
    
    CONSTRAINT FK_Detalle_Factura FOREIGN KEY (ID_FACTURA) 
        REFERENCES FACTURA(ID_FACTURA) ON DELETE CASCADE
);
GO

