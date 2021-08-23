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
                        totalCost += product.ProductPrice * quantityOrdered;                        
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
                    foreach(var product in keyValuePair.Value)
                    {
                        if(product.ProductId == cartProduct.ProductId)
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
        //    public static int CalculateTotalSales()
        //    {
        //        int totalSale = 0;
        //        foreach (KeyValuePair<string, List<ProductModel>> keyValuePair in CartItems)
        //        {
        //            if (keyValuePair.Key.Equals(UserSession.SingleInstance.GetLoggedInUser()))
        //            {
        //                for (int i = 0; i < keyValuePair.Value.Count; i++)
        //                {
        //                    totalSale += (keyValuePair.Value[i].ProductPrice * keyValuePair.Value[i].TotalNumberOfProduct);
        //                }
        //            }
        //        }
        //        return totalSale;
        //    }

        //    public static List<OnsiteProductsTransactionModel> ListOfOnsiteProducts(int transactionID)
        //    {
        //        List<OnsiteProductsTransactionModel> OnSiteProducts = new List<OnsiteProductsTransactionModel>();
        //        foreach (KeyValuePair<string, List<ProductModel>> keyValuePair in CartItems)
        //        {
        //            for (int i = 0; i < keyValuePair.Value.Count; i++)
        //            {
        //                OnSiteProducts.Add(
        //                   new OnsiteProductsTransactionModel
        //                   {
        //                       TransactionID = transactionID,
        //                       Product = keyValuePair.Value[i],
        //                       TotalProductsCount = keyValuePair.Value[i].TotalNumberOfProduct,
        //                       Administrator = UserSession.SingleInstance.GetLoggedInUser(),
        //                       SubTotalPrice = keyValuePair.Value[i].TotalNumberOfProduct * keyValuePair.Value[i].ProductPrice
        //                   }
        //               );
        //            }
        //        }
        //        return OnSiteProducts;
        //    }
        //}
    }
}
