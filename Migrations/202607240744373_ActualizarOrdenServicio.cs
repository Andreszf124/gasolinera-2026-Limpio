namespace gasolinera_2026.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActualizarOrdenServicio : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrdenServicios", "IdCliente", "dbo.Clientes");
            DropIndex("dbo.OrdenServicios", new[] { "IdCliente" });
            AddColumn("dbo.OrdenServicios", "TipoVehiculo", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.OrdenServicios", "ListaRepuestosUtilizados", c => c.String(maxLength: 500));
            AddColumn("dbo.OrdenServicios", "NombreCliente", c => c.String(nullable: false));
            DropColumn("dbo.OrdenServicios", "RepuestosUtilizadosResumen");
            DropColumn("dbo.OrdenServicios", "IdCliente");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrdenServicios", "IdCliente", c => c.Int(nullable: false));
            AddColumn("dbo.OrdenServicios", "RepuestosUtilizadosResumen", c => c.String(maxLength: 500));
            DropColumn("dbo.OrdenServicios", "NombreCliente");
            DropColumn("dbo.OrdenServicios", "ListaRepuestosUtilizados");
            DropColumn("dbo.OrdenServicios", "TipoVehiculo");
            CreateIndex("dbo.OrdenServicios", "IdCliente");
            AddForeignKey("dbo.OrdenServicios", "IdCliente", "dbo.Clientes", "IdCliente", cascadeDelete: true);
        }
    }
}
