using FlameAndWax.Models;
using System.Collections.Generic;
using System.Linq;

namespace FlameAndWax
{
    public class Cart
    {
        private static readonly Dictionary<string, List<ProductViewModel>> CartItems = new Dictionary<string, List<ProductViewModel>>();
        public static void ClearCartItems(string user)
        {
            foreach (KeyValuePair<string, List<ProductViewModel>> keyValuePair in CartItems)
            {
                if (keyValuePair.Key.Equals(user))
                {
                    keyValuePair.Value.Clear();
                    return;
                }
            }
        }

        public static double CalculateTotalCartCost(string user, int quantityOrdered)
        {
            double totalCost = 0.0f;
            foreach (KeyValuePair<string, List<ProductViewModel>> keyValuePair in CartItems)
            {
                if (keyValuePair.Key.Equals(user))
                {
                    foreach (var product in keyValuePair.Value)
                    {
                        product.ProductSubTotalPrice = product.ProductPrice * quantityOrdered;
                        totalCost += product.ProductSubTotalPrice;
                    }
                }
            }
            return totalCost;
        }

        public static double GetTotalCartCost(string user)
        {
            double totalCost = 0.0f;
            foreach (KeyValuePair<string, List<ProductViewModel>> keyValuePair in CartItems)
            {
                if (keyValuePair.Key.Equals(user))
                {
                    foreach (var product in keyValuePair.Value)
                    {                        
                        totalCost += product.ProductSubTotalPrice;                        
                    }
                }
            }
            return totalCost;
        }

        public static void AddCartItem(ProductViewModel cartProduct, string loggedInUser)
        {
            if (!CartItems.Keys.Contains(loggedInUser))
            {
                CartItems.Add(loggedInUser, new List<ProductViewModel>() { cartProduct });
                return;
            }

            foreach (KeyValuePair<string, List<ProductViewModel>> keyValuePair in CartItems)
            {
                if (keyValuePair.Key.Equals(loggedInUser))
                {
                    foreach (var product in keyValuePair.Value)
                    {
                        if (product.ProductId == cartProduct.ProductId)
                        {
                            return;
                        }
                    }
                    keyValuePair.Value.Add(cartProduct);
                }
            }
        }
        public static List<ProductViewModel> GetCartItems(string user)
        {
            foreach (KeyValuePair<string, List<ProductViewModel>> keyValuePair in CartItems)
            {
                if (keyValuePair.Key.Equals(user))
                {
                    return keyValuePair.Value;
                }
            }
            return new List<ProductViewModel>();
        }

        public static int GetCartItemsCount(string user)
        {
            foreach (KeyValuePair<string, List<ProductViewModel>> keyValuePair in CartItems)
            {
                if (keyValuePair.Key.Equals(user))
                {
                    return keyValuePair.Value.Count;
                }
            }
            return 0;
        }

        public static void RemoveCartItem(int productID, string user)
        {
            foreach (KeyValuePair<string, List<ProductViewModel>> keyValuePair in CartItems)
            {
                if (keyValuePair.Key.Equals(user))
                {
                    foreach (var product in keyValuePair.Value)
                    {
                        if (productID == product.ProductId)
                        {
                            keyValuePair.Value.Remove(product);
                            return;
                        }
                    }
                }
            }
        }

        public static bool IncrementProductCount(int productID, string user)
        {
            foreach (KeyValuePair<string, List<ProductViewModel>> keyValuePair in CartItems)
            {
                if (keyValuePair.Key.Equals(user))
                {
                    foreach (var product in keyValuePair.Value)
                    {
                        if (productID == product.ProductId)
                        {
                            if (product.QuantityOrdered >= product.StockQuantity)
                                return false;
                            product.QuantityOrdered++;
                            product.ProductSubTotalPrice = product.ProductPrice * product.QuantityOrdered;

                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static void DecrementProductCount(int productID, string user)
        {
            foreach (KeyValuePair<string, List<ProductViewModel>> keyValuePair in CartItems)
            {
                if (keyValuePair.Key.Equals(user))
                {
                    foreach (var product in keyValuePair.Value)
                    {
                        if (productID == product.ProductId)
                        {
                            product.QuantityOrdered--;
                            product.ProductSubTotalPrice = product.ProductPrice * product.QuantityOrdered;
                            if (product.QuantityOrdered <= 0)
                            {
                                product.QuantityOrdered = 1;
                                product.ProductSubTotalPrice = product.ProductPrice;
                            }
                            return;
                        }
                    }
                }
            }
        }
    }
}
