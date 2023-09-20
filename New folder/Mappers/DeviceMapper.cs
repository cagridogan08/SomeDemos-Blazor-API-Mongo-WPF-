using System;
using System.Text.Json;
using Iotech.Link.Libs.Core.Base.Data.Conf;
using Iotech.Link.Libs.Driver.IDC850Driver;
using Iotech.Link.Libs.Driver.IMU100Driver;
using Iotech.Link.Libs.Driver.MBDriver;
using Iotech.Link.Libs.Driver.OPCUADriver;
using Iotech.Link.Libs.Driver.SNMPDriver;
using Iotech.Link.Libs.Modules.LinkRestAPI.Enums;
using Iotech.Link.Libs.Modules.LinkRestAPI.Models;
using WpfAppWithRedisCache.Mappers.Abstract;

namespace WpfAppWithRedisCache.Mappers
{
    public class DeviceMapper : MapperBase<Device, DeviceConf>
    {
        public override Device Map(DeviceConf element)
        {
            var dev = new Device()
            {
                ConfJson = JsonSerializer.Serialize(element),
                DeviceId = element.DeviceId,
                Name = element.DeviceName,
            };
            dev.DeviceType = element switch
            {
                MBDevice => DeviceType.MB,
                SNMPDevice => DeviceType.SNMP,
                OPCUADevice => DeviceType.OPCUA,
                IMU100Device => DeviceType.IMU100,
                IDC850Device => DeviceType.IDC850,
                _ => dev.DeviceType
            };
            dev.EntityOwnerModule = dev.DeviceType.ToString();
            return dev;
        }

        public override DeviceConf Map(Device element)
        {
            switch (element.DeviceType)
            {
                case DeviceType.MB:
                    return JsonSerializer.Deserialize<MBDevice>(element.ConfJson);
                case DeviceType.OPCUA:
                    return JsonSerializer.Deserialize<OPCUADevice>(element.ConfJson);
                case DeviceType.SNMP:
                    return JsonSerializer.Deserialize<SNMPDevice>(element.ConfJson);
                case DeviceType.IMU100:
                    return JsonSerializer.Deserialize<IMU100Device>(element.ConfJson);
                case DeviceType.IDC850:
                    return JsonSerializer.Deserialize<IDC850Device>(element.ConfJson);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
