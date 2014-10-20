namespace FoodPlanner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Foods",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RecipeID = c.Int(nullable: false),
                        IngredientID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Ingredients", t => t.IngredientID, cascadeDelete: true)
                .ForeignKey("dbo.Recipes", t => t.RecipeID, cascadeDelete: true)
                .Index(t => t.RecipeID)
                .Index(t => t.IngredientID);
            
            CreateTable(
                "dbo.Ingredients",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 200),
                        Unit = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Recipes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 200),
                        ImagePath = c.String(),
                        Description = c.String(),
                        CookingTime = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Title, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Foods", "RecipeID", "dbo.Recipes");
            DropForeignKey("dbo.Foods", "IngredientID", "dbo.Ingredients");
            DropIndex("dbo.Recipes", new[] { "Title" });
            DropIndex("dbo.Ingredients", new[] { "Name" });
            DropIndex("dbo.Foods", new[] { "IngredientID" });
            DropIndex("dbo.Foods", new[] { "RecipeID" });
            DropTable("dbo.Recipes");
            DropTable("dbo.Ingredients");
            DropTable("dbo.Foods");
        }
    }
}
