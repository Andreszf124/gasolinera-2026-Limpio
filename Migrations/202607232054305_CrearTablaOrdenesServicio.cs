namespace gasolinera_2026.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CrearTablaOrdenesServicio : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clientes",
                c => new
                    {
                        IdCliente = c.Int(nullable: false, identity: true),
                        NombreCompleto = c.String(nullable: false, maxLength: 100),
                        Correo = c.String(nullable: false, maxLength: 100),
                        Telefono = c.String(maxLength: 20),
                        Direccion = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.IdCliente);
            
            CreateTable(
                "dbo.Empleadoes",
                c => new
                    {
                        IdEmpleado = c.Int(nullable: false, identity: true),
                        NombreCompleto = c.String(nullable: false, maxLength: 100),
                        Correo = c.String(nullable: false, maxLength: 100),
                        Telefono = c.String(maxLength: 20),
                        Cargo = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.IdEmpleado);
            
            CreateTable(
                "dbo.OrdenServicios",
                c => new
                    {
                        IdOrdenServicio = c.Int(nullable: false, identity: true),
                        PlacaVehiculo = c.String(nullable: false, maxLength: 20),
                        MarcaModelo = c.String(nullable: false, maxLength: 50),
                        Anio = c.Int(nullable: false),
                        Diagnostico = c.String(nullable: false, maxLength: 500),
                        TrabajosRealizados = c.String(maxLength: 1000),
                        CostoManoObra = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RepuestosUtilizadosResumen = c.String(maxLength: 500),
                        FechaEntrada = c.DateTime(nullable: false),
                        FechaFinalizacion = c.DateTime(),
                        Estado = c.Int(nullable: false),
                        IdCliente = c.Int(nullable: false),
                        IdEmpleado = c.Int(),
                    })
                .PrimaryKey(t => t.IdOrdenServicio)
                .ForeignKey("dbo.Clientes", t => t.IdCliente, cascadeDelete: true)
                .ForeignKey("dbo.Empleadoes", t => t.IdEmpleado)
                .Index(t => t.IdCliente)
                .Index(t => t.IdEmpleado);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrdenServicios", "IdEmpleado", "dbo.Empleadoes");
            DropForeignKey("dbo.OrdenServicios", "IdCliente", "dbo.Clientes");
            DropIndex("dbo.OrdenServicios", new[] { "IdEmpleado" });
            DropIndex("dbo.OrdenServicios", new[] { "IdCliente" });
            DropTable("dbo.OrdenServicios");
            DropTable("dbo.Empleadoes");
            DropTable("dbo.Clientes");
        }
    }
}
