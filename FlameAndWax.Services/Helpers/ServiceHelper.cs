using FlameAndWax.Data.Constants;
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
    }
}
