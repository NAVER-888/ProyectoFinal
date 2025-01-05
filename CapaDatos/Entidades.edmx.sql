
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/02/2025 17:25:42
-- Generated from EDMX file: C:\Users\USUARIO\OneDrive\Documentos\ERICK\SISE\CICLO III\BLOQUE II\DESARROLLO DE APLICACIONES\Proyecto Final\ProyectoFinal\CapaDatos\Entidades.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [BDProyectoFinal];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Usuarios'
CREATE TABLE [dbo].[Usuarios] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [nombre] nvarchar(100)  NOT NULL,
    [clave] nvarchar(100)  NOT NULL,
    [rol] nvarchar(50)  NOT NULL,
    [estado] bit  NOT NULL
);
GO

-- Creating table 'Productos'
CREATE TABLE [dbo].[Productos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [nombre] nvarchar(100)  NOT NULL,
    [descripcion] nvarchar(255)  NULL,
    [precio] decimal(18,2)  NOT NULL,
    [stock] int  NOT NULL
);
GO

-- Creating table 'Clientes'
CREATE TABLE [dbo].[Clientes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [nombre] nvarchar(100)  NOT NULL,
    [direccion] nvarchar(255)  NULL,
    [telefono] nvarchar(20)  NULL,
    [correo] nvarchar(100)  NULL
);
GO

-- Creating table 'Ventas'
CREATE TABLE [dbo].[Ventas] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [fecha] datetime  NOT NULL,
    [total] decimal(18,2)  NOT NULL,
    [ClienteId] int  NOT NULL
);
GO

-- Creating table 'DetalleVentas'
CREATE TABLE [dbo].[DetalleVentas] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [cantidad] int  NOT NULL,
    [subtotal] decimal(18,2)  NOT NULL,
    [VentaId] int  NOT NULL,
    [ProductoId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Usuarios'
ALTER TABLE [dbo].[Usuarios]
ADD CONSTRAINT [PK_Usuarios]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Productos'
ALTER TABLE [dbo].[Productos]
ADD CONSTRAINT [PK_Productos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Clientes'
ALTER TABLE [dbo].[Clientes]
ADD CONSTRAINT [PK_Clientes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Ventas'
ALTER TABLE [dbo].[Ventas]
ADD CONSTRAINT [PK_Ventas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DetalleVentas'
ALTER TABLE [dbo].[DetalleVentas]
ADD CONSTRAINT [PK_DetalleVentas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ClienteId] in table 'Ventas'
ALTER TABLE [dbo].[Ventas]
ADD CONSTRAINT [FK_VentaCliente]
    FOREIGN KEY ([ClienteId])
    REFERENCES [dbo].[Clientes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VentaCliente'
CREATE INDEX [IX_FK_VentaCliente]
ON [dbo].[Ventas]
    ([ClienteId]);
GO

-- Creating foreign key on [VentaId] in table 'DetalleVentas'
ALTER TABLE [dbo].[DetalleVentas]
ADD CONSTRAINT [FK_DetalleVentaVenta]
    FOREIGN KEY ([VentaId])
    REFERENCES [dbo].[Ventas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DetalleVentaVenta'
CREATE INDEX [IX_FK_DetalleVentaVenta]
ON [dbo].[DetalleVentas]
    ([VentaId]);
GO

-- Creating foreign key on [ProductoId] in table 'DetalleVentas'
ALTER TABLE [dbo].[DetalleVentas]
ADD CONSTRAINT [FK_DetalleVentaProducto]
    FOREIGN KEY ([ProductoId])
    REFERENCES [dbo].[Productos]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DetalleVentaProducto'
CREATE INDEX [IX_FK_DetalleVentaProducto]
ON [dbo].[DetalleVentas]
    ([ProductoId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------