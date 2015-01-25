using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FoodPlanner.Models;
using FoodPlanner.ViewModels;

namespace FoodPlannerTests {
    [TestClass]
    public class InventoryViewModelTests {
        [TestMethod]
        public void AddInventoryIngredient_CorrectIngrAdded_IngrAdded() {
            //arrange
            Ingredient expectedNewInvenIngredient = new Ingredient();
            expectedNewInvenIngredient.Name = "Milk";

            //act
            

            //assert
        }

        [TestMethod]
        public void AddInventoryIngredient_With1Quantity_CorrectQuan() {

        }
    }
}
