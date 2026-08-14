namespace Gasolinera.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgregarCategoria : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categorias",
                c => new
                    {
                        CategoriaId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Descripcion = c.String(maxLength: 500),
                        Activa = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CategoriaId);
            
            AddColumn("dbo.Ventas", "TipoPago", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Ventas", "PuntosUsados", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Productoes", "CategoriaId", c => c.Int(nullable: false));
            CreateIndex("dbo.Productoes", "CategoriaId");
            AddForeignKey("dbo.Productoes", "CategoriaId", "dbo.Categorias", "CategoriaId", cascadeDelete: true);
            DropColumn("dbo.Productoes", "Categoria");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Productoes", "Categoria", c => c.Int(nullable: false));
            DropForeignKey("dbo.Productoes", "CategoriaId", "dbo.Categorias");
            DropIndex("dbo.Productoes", new[] { "CategoriaId" });
            DropColumn("dbo.Productoes", "CategoriaId");
            DropColumn("dbo.Ventas", "PuntosUsados");
            DropColumn("dbo.Ventas", "TipoPago");
            DropTable("dbo.Categorias");
        }
    }
}
