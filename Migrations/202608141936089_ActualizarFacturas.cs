namespace Gasolinera.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActualizarFacturas : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Facturas", "Estado", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.Facturas", "Observaciones", c => c.String(maxLength: 500));
            AddColumn("dbo.Facturas", "FechaAprobacion", c => c.DateTime());
            AddColumn("dbo.Facturas", "AprobadoPorId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Facturas", "AprobadoPorId");
            DropColumn("dbo.Facturas", "FechaAprobacion");
            DropColumn("dbo.Facturas", "Observaciones");
            DropColumn("dbo.Facturas", "Estado");
        }
    }
}
