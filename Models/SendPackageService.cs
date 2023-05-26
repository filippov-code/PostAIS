using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostAIS.Models
{
    public class SendPackageService : Service
    {
        public override OperationType OperationType => OperationType.Send;
        public string RecipientFIO { get; set; }
        public string Address { get; set; }
        public DeliveryType DeliveryType { get; set; }
        public PackageType PackageType { get; set; }
        public double Weight { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }
        public double Height { get; set; }
        public double Cost { get; set; }

        public override string ToString()
        {
            return
                $"Тип посылки: {PackageType} \n" +
                $"Адрес: {Address} \n" +
                $"Вес: {Weight} \n" +
                $"Ширина: {Width} \n" +
                $"Высота: {Height} \n" +
                $"Длина: {Length} \n" +
                $"Тип доставки: {DeliveryType} \n" +
                $"Стоимость: {Cost} \n";
        }
    }
    public enum PackageType
    {
        Letter, //письмо
        Parcel, //бандероль
        Package //посылка
    }

    public enum DeliveryType
    {
        Express,
        Normal,
        Simple
    }
}
