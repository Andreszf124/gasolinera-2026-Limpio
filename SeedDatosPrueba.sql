-- ============================================================
-- Seed de datos de prueba - Gasolinera 2026
-- ------------------------------------------------------------
-- Correr DESPUES de levantar la app al menos una vez,
-- porque los roles se crean solos en Application_Start.
-- Se puede correr varias veces: no duplica datos.
-- ============================================================

SET NOCOUNT ON;

-- ==================== CATEGORIAS ====================
INSERT INTO dbo.Categorias (Nombre, Descripcion, Activa)
SELECT 'Lubricantes', 'Aceites y lubricantes para motor', 1
WHERE NOT EXISTS (SELECT 1 FROM dbo.Categorias WHERE Nombre = 'Lubricantes');

INSERT INTO dbo.Categorias (Nombre, Descripcion, Activa)
SELECT 'Filtros', 'Filtros de aire, aceite y combustible', 1
WHERE NOT EXISTS (SELECT 1 FROM dbo.Categorias WHERE Nombre = 'Filtros');

DECLARE @IdLubricantes int = (SELECT CategoriaId FROM dbo.Categorias WHERE Nombre = 'Lubricantes');
DECLARE @IdFiltros     int = (SELECT CategoriaId FROM dbo.Categorias WHERE Nombre = 'Filtros');

-- ==================== PRODUCTOS ====================
INSERT INTO dbo.Productoes (Nombre, Precio, Stock, CategoriaId)
SELECT 'Aceite 20W-50', 8500, 25, @IdLubricantes
WHERE NOT EXISTS (SELECT 1 FROM dbo.Productoes WHERE Nombre = 'Aceite 20W-50');

INSERT INTO dbo.Productoes (Nombre, Precio, Stock, CategoriaId)
SELECT 'Filtro de aceite', 4200, 30, @IdFiltros
WHERE NOT EXISTS (SELECT 1 FROM dbo.Productoes WHERE Nombre = 'Filtro de aceite');

INSERT INTO dbo.Productoes (Nombre, Precio, Stock, CategoriaId)
SELECT 'Filtro de aire', 5600, 18, @IdFiltros
WHERE NOT EXISTS (SELECT 1 FROM dbo.Productoes WHERE Nombre = 'Filtro de aire');

-- ==================== EMPLEADOS ====================
INSERT INTO dbo.Empleadoes (NombreCompleto, Correo, Telefono, Cargo)
SELECT 'Carlos Rojas', 'carlos@gasolinera.com', '8888-3333', 'Mecánico'
WHERE NOT EXISTS (SELECT 1 FROM dbo.Empleadoes WHERE Correo = 'carlos@gasolinera.com');

INSERT INTO dbo.Empleadoes (NombreCompleto, Correo, Telefono, Cargo)
SELECT 'María López', 'maria@gasolinera.com', '8888-2222', 'Cajera'
WHERE NOT EXISTS (SELECT 1 FROM dbo.Empleadoes WHERE Correo = 'maria@gasolinera.com');

DECLARE @IdMecanico int = (SELECT IdEmpleado FROM dbo.Empleadoes WHERE Correo = 'carlos@gasolinera.com');
DECLARE @IdCajera   int = (SELECT IdEmpleado FROM dbo.Empleadoes WHERE Correo = 'maria@gasolinera.com');

-- ==================== CLIENTES ====================
-- El cliente del usuario admin de prueba (para ver datos al iniciar sesion)
INSERT INTO dbo.Clientes (NombreCompleto, Correo, Telefono, Direccion)
SELECT 'Administrador de Prueba', 'admin@gasolinera.com', '2222-0000', 'San José'
WHERE NOT EXISTS (SELECT 1 FROM dbo.Clientes WHERE Correo = 'admin@gasolinera.com');

INSERT INTO dbo.Clientes (NombreCompleto, Correo, Telefono, Direccion)
SELECT 'Ana Jiménez', 'ana.jimenez@correo.com', '8777-1111', 'Alajuela'
WHERE NOT EXISTS (SELECT 1 FROM dbo.Clientes WHERE Correo = 'ana.jimenez@correo.com');

DECLARE @IdClienteAdmin int = (SELECT IdCliente FROM dbo.Clientes WHERE Correo = 'admin@gasolinera.com');
DECLARE @IdClienteAna   int = (SELECT IdCliente FROM dbo.Clientes WHERE Correo = 'ana.jimenez@correo.com');

-- ==================== VEHICULOS ====================
INSERT INTO dbo.Vehiculoes (Placa, Marca, Modelo, Anio, TipoVehiculo, Color, IdCliente, FechaRegistro)
SELECT 'ABC123', 'Toyota', 'Corolla', 2020, 'Carro', 'Gris', @IdClienteAdmin, GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM dbo.Vehiculoes WHERE Placa = 'ABC123');

INSERT INTO dbo.Vehiculoes (Placa, Marca, Modelo, Anio, TipoVehiculo, Color, IdCliente, FechaRegistro)
SELECT 'XYZ789', 'Honda', 'CR-V', 2018, 'SUV', 'Rojo', @IdClienteAna, GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM dbo.Vehiculoes WHERE Placa = 'XYZ789');

DECLARE @IdVehiculoAdmin int = (SELECT IdVehiculo FROM dbo.Vehiculoes WHERE Placa = 'ABC123');
DECLARE @IdVehiculoAna   int = (SELECT IdVehiculo FROM dbo.Vehiculoes WHERE Placa = 'XYZ789');

-- ==================== ORDENES DE SERVICIO ====================
-- Estado: 1=Pendiente, 2=EnProgreso, 3=Completado, 4=Entregado, 5=Cancelado
INSERT INTO dbo.OrdenServicios (IdVehiculo, NombreCliente, TipoServicio, DescripcionServicio,
                                FechaEntrada, FechaFinalizacion, Estado, IdEmpleado,
                                CostoEstimado, ObservacionesMecanico)
SELECT @IdVehiculoAdmin, 'Administrador de Prueba', 'Cambio de Aceite',
       'Cambio de aceite y filtro de motor.', GETDATE(), NULL, 1, @IdMecanico, 25000, NULL
WHERE NOT EXISTS (SELECT 1 FROM dbo.OrdenServicios WHERE IdVehiculo = @IdVehiculoAdmin);

INSERT INTO dbo.OrdenServicios (IdVehiculo, NombreCliente, TipoServicio, DescripcionServicio,
                                FechaEntrada, FechaFinalizacion, Estado, IdEmpleado,
                                CostoEstimado, ObservacionesMecanico)
SELECT @IdVehiculoAna, 'Ana Jiménez', 'Revisión de Frenos',
       'Ruido al frenar en las llantas delanteras.', GETDATE(), GETDATE(), 3, @IdMecanico,
       40000, 'Se cambiaron las pastillas delanteras.'
WHERE NOT EXISTS (SELECT 1 FROM dbo.OrdenServicios WHERE IdVehiculo = @IdVehiculoAna);

-- ==================== VENTAS ====================
INSERT INTO dbo.Ventas (Fecha, IdCliente, IdEmpleado, TipoVenta, TipoPago,
                        Subtotal, Descuento, Impuesto, Total, MetodoPago, Estado,
                        IdOrdenServicio, PuntosUsados)
SELECT GETDATE(), @IdClienteAdmin, @IdCajera, 'Combustible', 'Dinero',
       20000, 0, 2600, 22600, 'Efectivo', 'Activa', NULL, 0
WHERE NOT EXISTS (SELECT 1 FROM dbo.Ventas WHERE IdCliente = @IdClienteAdmin AND TipoVenta = 'Combustible');

INSERT INTO dbo.Ventas (Fecha, IdCliente, IdEmpleado, TipoVenta, TipoPago,
                        Subtotal, Descuento, Impuesto, Total, MetodoPago, Estado,
                        IdOrdenServicio, PuntosUsados)
SELECT GETDATE(), @IdClienteAna, @IdCajera, 'Productos', 'Dinero',
       12700, 0, 1651, 14351, 'Tarjeta', 'Activa', NULL, 0
WHERE NOT EXISTS (SELECT 1 FROM dbo.Ventas WHERE IdCliente = @IdClienteAna AND TipoVenta = 'Productos');

DECLARE @IdVentaAdmin int = (SELECT TOP 1 IdVenta FROM dbo.Ventas
                             WHERE IdCliente = @IdClienteAdmin AND TipoVenta = 'Combustible');

-- ==================== FACTURA ====================
INSERT INTO dbo.Facturas (IdVenta, FechaEmision, NumeroFactura, Total, Estado,
                          Observaciones, FechaAprobacion, AprobadoPorId)
SELECT @IdVentaAdmin, GETDATE(), 'FAC-0001', 22600, 'Pendiente', NULL, NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM dbo.Facturas WHERE NumeroFactura = 'FAC-0001');

-- ==================== CASHBACK ====================
-- 1 punto por cada 200 colones: 22600 / 200 = 113 puntos
INSERT INTO dbo.Cashbacks (IdCliente, PuntosAcumulados, PuntosCanjeados, PuntosDisponibles, FechaActualizacion)
SELECT @IdClienteAdmin, 113, 0, 113, GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM dbo.Cashbacks WHERE IdCliente = @IdClienteAdmin);

-- TipoMovimiento: 1=Acumulacion, 2=Canje
INSERT INTO dbo.MovimientoCashbacks (IdCliente, IdVenta, Monto, PuntosGenerados,
                                     TipoMovimiento, FechaMovimiento, UsuarioResponsableId, Observaciones)
SELECT @IdClienteAdmin, @IdVentaAdmin, 22600, 113, 1, GETDATE(), NULL, 'Puntos por compra de combustible'
WHERE NOT EXISTS (SELECT 1 FROM dbo.MovimientoCashbacks WHERE IdVenta = @IdVentaAdmin);

-- ==================== RESUMEN ====================
SELECT 'Categorias' AS Tabla, COUNT(*) AS Filas FROM dbo.Categorias
UNION ALL SELECT 'Productos', COUNT(*) FROM dbo.Productoes
UNION ALL SELECT 'Empleados', COUNT(*) FROM dbo.Empleadoes
UNION ALL SELECT 'Clientes', COUNT(*) FROM dbo.Clientes
UNION ALL SELECT 'Vehiculos', COUNT(*) FROM dbo.Vehiculoes
UNION ALL SELECT 'OrdenesServicio', COUNT(*) FROM dbo.OrdenServicios
UNION ALL SELECT 'Ventas', COUNT(*) FROM dbo.Ventas
UNION ALL SELECT 'Facturas', COUNT(*) FROM dbo.Facturas
UNION ALL SELECT 'Cashbacks', COUNT(*) FROM dbo.Cashbacks;
