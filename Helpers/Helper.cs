using PostAIS.Database;
using PostAIS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace PostAIS.Helpers
{
    public static class Helper
    {
        public static double GetDeliveryCost(double length, double width, double height, double weight, DeliveryType deliveryType)
        {
            double deliveryK = deliveryType switch
            {
                DeliveryType.Express => 2,
                DeliveryType.Normal => 1.5,
                DeliveryType.Simple => 1,
                _ => 1
            };
            return (length + width + height) * weight * deliveryK * 50;
        }

        public static string[] GetDeliveryTypeRuNames()
        {
            var deliveryTypes = Enum.GetValues<DeliveryType>();
            string[] ruNames = new string[deliveryTypes.Length];
            for (int i = 0; i < ruNames.Length; i++)
            {
                ruNames[i] = deliveryTypes[i] switch
                {
                    DeliveryType.Express => "Экспресс",
                    DeliveryType.Normal => "Быстрая",
                    DeliveryType.Simple => "Обычная",
                    _ => throw new NotImplementedException(deliveryTypes[i].ToString())
                };
            }
            return ruNames;
        }

        public static string[] GetPackageTypeRuNames()
        {
            var packageTypes = Enum.GetValues<PackageType>();
            string[] ruNames = new string[packageTypes.Length];
            for (int i = 0; i < ruNames.Length; i++)
            {
                ruNames[i] = packageTypes[i] switch
                {
                    PackageType.Letter => "Письмо",
                    PackageType.Parcel => "Бандероль",
                    PackageType.Package => "Посылка",
                    _ => throw new NotImplementedException(packageTypes[i].ToString())
                };
            }
            return ruNames;
        }

        public static int GetServiceCode()
        {
            using (var dbContext = new PostAisDbContext())
            {
                return dbContext.Services.Count() == 0 ? 1 : dbContext.Services.Max(x => x.Code) + 1; 
            }
        }

        public static int GetWaitingTimeForClient()
        {
            using (var dbContext = new PostAisDbContext())
            {
                var servicesCount = dbContext.Services.Count();
                return servicesCount * 2;
            }
        }

        public static string GetDeliveryTime(DeliveryType deliveryType)
        {
            return deliveryType switch
            {
                DeliveryType.Express => "1-2 дня",
                DeliveryType.Normal => "2-3 дня",
                DeliveryType.Simple => "3-4 дня",
                _ => throw new NotImplementedException(deliveryType.ToString())
            };
        }

    }
}
