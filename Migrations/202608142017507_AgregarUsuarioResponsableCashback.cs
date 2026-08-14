namespace Gasolinera.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgregarUsuarioResponsableCashback : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MovimientoCashbacks", "UsuarioResponsableId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MovimientoCashbacks", "UsuarioResponsableId");
        }
    }
}
