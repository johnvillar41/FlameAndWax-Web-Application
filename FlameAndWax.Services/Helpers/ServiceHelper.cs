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
                case 1: return Constants.ReviewScore.Good;
                case 2: return Constants.ReviewScore.Bad;
                default: return Constants.ReviewScore.Meh;
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
                default:
                    return Constants.Courier.FoodPanda;
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
