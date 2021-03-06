﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Couchbase.Authentication.SASL;
using Couchbase.Configuration.Client;
using Couchbase.Configuration.Server.Providers.CarrierPublication;
using Couchbase.Configuration.Server.Serialization;
using Couchbase.Core.Transcoders;
using Couchbase.IO;
using Couchbase.IO.Converters;
using Couchbase.IO.Operations;
using Couchbase.IO.Operations.Authentication;
using Moq;
using NUnit.Framework;

namespace Couchbase.UnitTests.Configuration.Server.Providers.CarrierPublication
{
    [TestFixture]
    public class CarrierPublicationProviderTests
    {
        [TestCase(true)]
        [TestCase(false)]
        public void Only_Execute_SelectBuket_When_EnhancedAuthentication_Is_Enabled(bool enabled)
        {
            var mockConnectionPool = new Mock<IConnectionPool>();
            mockConnectionPool.Setup(x => x.EndPoint)
                .Returns(new IPEndPoint(IPAddress.Any, 0));
            var mockSaslMech = new Mock<ISaslMechanism>();
            var mockIOService = new Mock<IIOService>();
            mockIOService.Setup(x => x.ConnectionPool)
                .Returns(mockConnectionPool.Object);
            mockIOService.Setup(x => x.SupportsEnhancedAuthentication)
                .Returns(enabled);
            mockIOService.Setup(x => x.Execute(It.IsAny<IOperation>()))
                .Returns(new OperationResult {Success = true});
            mockIOService.Setup(x => x.Execute(It.IsAny<IOperation<BucketConfig>>()))
                .Returns(new OperationResult<BucketConfig>
                {
                    Success = true,
                    Value = new BucketConfig {Name = "default", Nodes = new [] { new Node {Hostname = "localhost"} }}
                });

            var config = new ClientConfiguration();
            var provider = new CarrierPublicationProvider(
                config,
                pool => mockIOService.Object,
                (c, e) => mockConnectionPool.Object,
                (ac, b, c, d) => mockSaslMech.Object,
                new DefaultConverter(),
                new DefaultTranscoder()
            );

            provider.GetConfig("bucket", "username", "password");
            mockIOService.Verify(x => x.Execute(It.IsAny<SelectBucket>()), Times.Exactly(enabled ? 1 : 0));
        }
    }
}
