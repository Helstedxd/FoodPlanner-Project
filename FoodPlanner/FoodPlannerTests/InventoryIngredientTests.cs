using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FoodPlanner.Models;

namespace FoodPlannerTests {
    [TestClass]
    public class InventoryIngredientTests {

        [TestMethod]
        public void PurchaseDate_AutoSetInConstructor_SetToNow() {
            //Arrange
            InventoryIngredient inventoryIngredientTest = new InventoryIngredient(new Ingredient(), 750);
            DateTime expectedPurchaseDate = DateTime.Now;

            //act - blank because the property is set automaticly in the constructor

            //assert
            Assert.AreEqual(expectedPurchaseDate, inventoryIngredientTest.PurchaseDate);
        }

        [TestMethod]
        public void ExpirationDate_AutoSetInConstructor_SetTo7DaysAhead() {
            DateTime expectedExpirationDate = DateTime.Now.AddDays(7);
            InventoryIngredient inventoryIngredientTest = new InventoryIngredient(new Ingredient(), 750);

            //act - blank because the property is set automaticly in the constructor

            //assert
            Assert.AreEqual(expectedExpirationDate, inventoryIngredientTest.ExpirationDate);
        }
    }
}
