namespace gasolinera_2026.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgregarTablaProducto : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Productoes",
                c => new
                    {
                        IdProducto = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Stock = c.Int(nullable: false),
                        Categoria = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdProducto);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Productoes");
        }
    }
}
