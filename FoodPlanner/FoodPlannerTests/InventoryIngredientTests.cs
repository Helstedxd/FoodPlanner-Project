using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FoodPlanner.Models;

namespace FoodPlannerTests {
    [TestClass]
    public class InventoryIngredientTests {

        [TestMethod]
        public void PurchaseDate_AutoSetInConstructor_SetToNow() {
            //arrange
            Ingredient testIngredient = new Ingredient();
            DateTime expectedPurchaseDate = DateTime.Now;

            //act - The property is set automaticly in the constructor
            InventoryIngredient testInventoryIngredient = new InventoryIngredient(testIngredient, 750);

            //assert
            Assert.AreEqual(expectedPurchaseDate, testInventoryIngredient.PurchaseDate);
        }

        [TestMethod]
        public void ExpirationDate_AutoSetInConstructor_SetTo7DaysAhead() {
            Ingredient testIngredient = new Ingredient();
            DateTime expectedExpirationDate = DateTime.Now.AddDays(7);
            
            //act - The property is set automaticly in the constructor
            InventoryIngredient testInventoryIngredient = new InventoryIngredient(testIngredient, 200);

            //assert
            Assert.AreEqual(expectedExpirationDate, testInventoryIngredient.ExpirationDate);
        }
    }
}
