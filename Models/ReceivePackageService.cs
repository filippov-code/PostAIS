using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace PostAIS.Models
{
    public class ReceivePackageService : Service
    {
        public override OperationType OperationType => OperationType.Receive;
        public Guid PackageToReceiveId { get; set; }
        public PackageToReceive? PackageToReceive { get; set; }

        public override string ToString()
        {
            return PackageToReceive != null ?
                $"Отправитель: {PackageToReceive.SenderFio} \n" +
                $"Откуда: {PackageToReceive.SenderAddress} \n" +
                $"Тип посылки: {PackageToReceive.PackageType} \n" +
                $"Телефон получателя: {PackageToReceive.ReceiverTelephoneNumber} \n" : "null";
        }
    }
}
