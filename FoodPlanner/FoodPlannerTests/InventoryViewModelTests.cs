using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FoodPlanner.Models;
using FoodPlanner.ViewModels;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FoodPlannerTests {
    [TestClass]
    public class InventoryViewModelTests {
        [TestMethod]
        public void AddInventoryIngredient_CorrectIngrAdded_InventoryUpdated() {
            //arrange
            Ingredient expectedNewIngredient = new Ingredient();
            InventoryIngredient inventoryIngredient = new InventoryIngredient(expectedNewIngredient, 1);
            InventoryViewModel inventoryViewModel = new InventoryViewModel();
            bool ingredientAdded = false;

            //act
            inventoryViewModel.AddIngredientToInventory(expectedNewIngredient, 1);

            if (FoodPlanner.App.CurrentUser.InventoryIngredients.Contains(inventoryIngredient)) {
                ingredientAdded = true;
            }

            //assert
            Assert.IsTrue(ingredientAdded);
        }

        [TestMethod]
        public void RemoveInventoryIngredient_CorrectRemoved_InventoryUpdated() {
            //arrange
            InventoryViewModel inventoryViewModel = new InventoryViewModel();
            Ingredient ingredient = new Ingredient();
            InventoryIngredient inventoryIngredient = new InventoryIngredient();
            inventoryIngredient.Ingredient = ingredient;
            bool inventoryIngredientRemoved = false;

            //act
            inventoryViewModel.AddIngredientToInventory(ingredient);
            inventoryViewModel.RemoveIngredientFromInventory(inventoryIngredient);

            if (!FoodPlanner.App.CurrentUser.InventoryIngredients.Contains(inventoryIngredient)) {
                inventoryIngredientRemoved = true;
            }
            //assert
            Assert.IsTrue(inventoryIngredientRemoved);
        }
    }
}
