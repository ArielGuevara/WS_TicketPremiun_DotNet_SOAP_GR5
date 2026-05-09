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
