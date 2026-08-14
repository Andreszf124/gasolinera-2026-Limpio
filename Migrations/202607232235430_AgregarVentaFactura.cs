namespace Gasolinera.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgregarVentaFactura : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Facturas",
                c => new
                    {
                        IdFactura = c.Int(nullable: false, identity: true),
                        IdVenta = c.Int(nullable: false),
                        FechaEmision = c.DateTime(nullable: false),
                        NumeroFactura = c.String(nullable: false, maxLength: 50),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.IdFactura)
                .ForeignKey("dbo.Ventas", t => t.IdVenta, cascadeDelete: true)
                .Index(t => t.IdVenta);
            
            CreateTable(
                "dbo.Ventas",
                c => new
                    {
                        IdVenta = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        IdCliente = c.Int(nullable: false),
                        IdEmpleado = c.Int(nullable: false),
                        TipoVenta = c.String(nullable: false, maxLength: 50),
                        Subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Descuento = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Impuesto = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MetodoPago = c.String(nullable: false, maxLength: 50),
                        Estado = c.String(nullable: false, maxLength: 50),
                        IdOrdenServicio = c.Int(),
                    })
                .PrimaryKey(t => t.IdVenta)
                .ForeignKey("dbo.Clientes", t => t.IdCliente, cascadeDelete: true)
                .ForeignKey("dbo.Empleadoes", t => t.IdEmpleado, cascadeDelete: true)
                .ForeignKey("dbo.OrdenServicios", t => t.IdOrdenServicio)
                .Index(t => t.IdCliente)
                .Index(t => t.IdEmpleado)
                .Index(t => t.IdOrdenServicio);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Facturas", "IdVenta", "dbo.Ventas");
            DropForeignKey("dbo.Ventas", "IdOrdenServicio", "dbo.OrdenServicios");
            DropForeignKey("dbo.Ventas", "IdEmpleado", "dbo.Empleadoes");
            DropForeignKey("dbo.Ventas", "IdCliente", "dbo.Clientes");
            DropIndex("dbo.Ventas", new[] { "IdOrdenServicio" });
            DropIndex("dbo.Ventas", new[] { "IdEmpleado" });
            DropIndex("dbo.Ventas", new[] { "IdCliente" });
            DropIndex("dbo.Facturas", new[] { "IdVenta" });
            DropTable("dbo.Ventas");
            DropTable("dbo.Facturas");
        }
    }
}
