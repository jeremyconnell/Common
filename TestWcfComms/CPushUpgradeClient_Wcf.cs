using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace TestWcfComms
{
    internal class CPushUpgradeClient_Wcf : CPushUpgradeClient
    {
        //Constants
        private const string RELATIVE_URL = "/webservices/wcf/WcfPushUpgrade.svc";
        private const int TIMEOUT_SECS = 45;

        //Private
        private WRPushUpgrade.WcfPushUpgradeClient _client;

        //Constructors
        public CPushUpgradeClient_Wcf(string hostName) : this(hostName, false) { }
        public CPushUpgradeClient_Wcf(string hostName, bool useSsl) : base(hostName, useSsl)
        {
            var timeout = TimeSpan.FromSeconds(TIMEOUT_SECS);
            var binding = new BasicHttpBinding //WSHttpBinding
            {
                Name = "basicHttpBinding", //"WSHttpBinding"
                MaxBufferSize = 2147483647,
                MaxReceivedMessageSize = 2147483647,
                SendTimeout = timeout,
                OpenTimeout = timeout,
                ReceiveTimeout = timeout
            };
            _client = new WRPushUpgrade.WcfPushUpgradeClient(binding, new EndpointAddress(this.Url));
        }

        //Constants
        protected override string RelativePath { get { return RELATIVE_URL; } }

        //Magic Method
        protected override byte[] Transport(int enum_, byte[] input, EGzip gzip, EEncryption algorithm, ESerialisation formatIn, ESerialisation formatOut)
        {
            return _client.TransportInterface(
                enum_,
                input,
                (IWcfPushUpgrade.EGZip_)gzip,
                (IWcfPushUpgrade.EEncryption_)algorithm,
                (IWcfPushUpgrade.ESerialisation_)formatIn,
                (IWcfPushUpgrade.ESerialisation_)formatOut);
        }

        protected override Task<byte[]> TransportAsync(int enum_, byte[] input, EGzip gzip, EEncryption algorithm, ESerialisation formatIn, ESerialisation formatOut)
        {
            return _client.TransportInterfaceAsync(
                enum_,
                input,
                (IWcfPushUpgrade.EGZip_)gzip,
                (IWcfPushUpgrade.EEncryption_)algorithm,
                (IWcfPushUpgrade.ESerialisation_)formatIn,
                (IWcfPushUpgrade.ESerialisation_)formatOut);
        }
    }
}
