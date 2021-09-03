using FlameAndWax.Data.Constants;
using FlameAndWax.Services.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlameAndWax.Services.Helpers
{
    public class ServiceHelper
    {
        public static Constants.ReviewScore BuildReviewScore(int reviewScore)
        {
            switch (reviewScore)
            {
                case 1: return Constants.ReviewScore.VeryPoor;
                case 2: return Constants.ReviewScore.Poor;
                case 3: return Constants.ReviewScore.Good;
                case 4: return Constants.ReviewScore.VeryGood;
                default: return Constants.ReviewScore.Excellent;
            }
        }

        public static Constants.ModeOfPayment BuildModeOfPayment(string modeOfPayment)
        {
            switch (modeOfPayment)
            {
                case nameof(Constants.ModeOfPayment.Cash):
                    return Constants.ModeOfPayment.Cash;
                case nameof(Constants.ModeOfPayment.Cheque):
                    return Constants.ModeOfPayment.Cheque;
                default:
                    return Constants.ModeOfPayment.Cash;
            }
        }

        public static Constants.Courier BuildCourier(string courier)
        {
            switch (courier)
            {
                case nameof(Constants.Courier.FoodPanda):
                    return Constants.Courier.FoodPanda;
                case nameof(Constants.Courier.JNT):
                    return Constants.Courier.JNT;
                case nameof(Constants.Courier.NinjaVan):
                    return Constants.Courier.NinjaVan;
                case nameof(Constants.Courier.GogoExpress):
                    return Constants.Courier.GogoExpress;
                default:
                    return Constants.Courier.FoodPanda;
            }
        }        

        public static Constants.CustomerAccountStatus ConvertStringToCustomerAccountStatus(string accountStatus)
        {
            switch (accountStatus)
            {
                case nameof(Constants.CustomerAccountStatus.Active):
                    return Constants.CustomerAccountStatus.Active;
                case nameof(Constants.CustomerAccountStatus.Banned):
                    return Constants.CustomerAccountStatus.Banned;
                default:
                    return Constants.CustomerAccountStatus.Active;
            }
        }

        public static Constants.OrderStatus ConvertStringtoOrderStatus(string orderStatus)
        {
            switch (orderStatus)
            {
                case nameof(Constants.OrderStatus.Pending):
                    return Constants.OrderStatus.Pending;
                case nameof(Constants.OrderStatus.Processing):
                    return Constants.OrderStatus.Processing;
                case nameof(Constants.OrderStatus.Shipping):
                    return Constants.OrderStatus.Shipping;
                case nameof(Constants.OrderStatus.Finished):
                    return Constants.OrderStatus.Finished;
                case nameof(Constants.OrderStatus.Cancelled):
                    return Constants.OrderStatus.Cancelled;
                default:
                    return Constants.OrderStatus.Pending;
            }
        }     

        public static Constants.Category ConvertStringToConstant(string value)
        {
            switch (value)
            {
                case nameof(Constants.Category.Soap):
                    return Constants.Category.Soap;
                case nameof(Constants.Category.Diffuser):
                    return Constants.Category.Diffuser;
                case nameof(Constants.Category.Candle):
                    return Constants.Category.Candle;
                default:
                    return Constants.Category.Soap;
            }
        }

        public static Constants.EmployeeAccountStatus ConvertStringToEmployeeAccountStatus(string accountStatus)
        {
            switch (accountStatus)
            {
                case nameof(Constants.EmployeeAccountStatus.Activated):
                    return Constants.EmployeeAccountStatus.Activated;
                case nameof(Constants.EmployeeAccountStatus.Deactivated):
                    return Constants.EmployeeAccountStatus.Deactivated;
                default:
                    return Constants.EmployeeAccountStatus.Activated;
            }
        }

        public static ServiceResult<T> BuildServiceResult<T>(T result, bool hasError, string errorContent)
        {
            return new ServiceResult<T>
            {
                Result = result,
                HasError = hasError,
                ErrorContent = errorContent
            };
        }
    }
}
