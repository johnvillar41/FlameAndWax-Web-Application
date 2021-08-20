using FlameAndWax.Models;
using System.Collections.Generic;
using System.Linq;

namespace FlameAndWax
{
    public class Cart
    {
        private static readonly Dictionary<string, List<ProductViewModel>> CartItems = new Dictionary<string, List<ProductViewModel>>();

        public static void ClearCartItems()
        {
            foreach (KeyValuePair<string, List<ProductViewModel>> keyValuePair in CartItems)
            {
                //if (keyValuePair.Key.Equals(UserSession.SingleInstance.GetLoggedInUser()))
                //{
                //    keyValuePair.Value.Clear();
                //    return;
                //}
            }
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
                    for (int i = 0; i < keyValuePair.Value.Count; i++)
                    {
                        if (keyValuePair.Value[i].ProductId == cartProduct.ProductId)
                        {
                            return;
                        }
                        else
                        {
                            keyValuePair.Value.Add(cartProduct);
                            return;
                        }
                    }
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
        //    public static void RemoveCartItem(int productID)
        //    {
        //        foreach (KeyValuePair<string, List<ProductModel>> keyValuePair in CartItems)
        //        {
        //            if (keyValuePair.Key.Equals(UserSession.SingleInstance.GetLoggedInUser()))
        //            {
        //                for (int i = 0; i < keyValuePair.Value.Count; i++)
        //                {
        //                    if (keyValuePair.Value[i].Product_ID == productID)
        //                    {
        //                        keyValuePair.Value.Remove(keyValuePair.Value[i]);
        //                        break;
        //                    }
        //                }
        //            }
        //        }
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
