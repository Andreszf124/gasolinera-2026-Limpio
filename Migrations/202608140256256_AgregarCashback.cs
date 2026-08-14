namespace Gasolinera.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgregarCashback : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cashbacks",
                c => new
                    {
                        IdCashback = c.Int(nullable: false, identity: true),
                        IdCliente = c.Int(nullable: false),
                        PuntosAcumulados = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PuntosCanjeados = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PuntosDisponibles = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FechaActualizacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IdCashback)
                .ForeignKey("dbo.Clientes", t => t.IdCliente, cascadeDelete: true)
                .Index(t => t.IdCliente);
            
            CreateTable(
                "dbo.MovimientoCashbacks",
                c => new
                    {
                        IdMovimientoCashback = c.Int(nullable: false, identity: true),
                        IdCliente = c.Int(nullable: false),
                        IdVenta = c.Int(),
                        Monto = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PuntosGenerados = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TipoMovimiento = c.Int(nullable: false),
                        FechaMovimiento = c.DateTime(nullable: false),
                        Observaciones = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.IdMovimientoCashback)
                .ForeignKey("dbo.Clientes", t => t.IdCliente, cascadeDelete: true)
                .ForeignKey("dbo.Ventas", t => t.IdVenta)
                .Index(t => t.IdCliente)
                .Index(t => t.IdVenta);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MovimientoCashbacks", "IdVenta", "dbo.Ventas");
            DropForeignKey("dbo.MovimientoCashbacks", "IdCliente", "dbo.Clientes");
            DropForeignKey("dbo.Cashbacks", "IdCliente", "dbo.Clientes");
            DropIndex("dbo.MovimientoCashbacks", new[] { "IdVenta" });
            DropIndex("dbo.MovimientoCashbacks", new[] { "IdCliente" });
            DropIndex("dbo.Cashbacks", new[] { "IdCliente" });
            DropTable("dbo.MovimientoCashbacks");
            DropTable("dbo.Cashbacks");
        }
    }
}
