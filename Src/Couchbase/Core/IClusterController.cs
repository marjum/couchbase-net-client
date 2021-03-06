﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Couchbase.Authentication;
using Couchbase.Configuration.Client;
using Couchbase.Configuration.Server.Providers;
using Couchbase.Core.Transcoders;
using Couchbase.IO.Converters;

namespace Couchbase.Core
{
    internal interface IClusterController : IConfigPublisher, IDisposable
    {
        ICluster Cluster { get; }

        DateTime LastConfigCheckedTime { get; set; }

        List<IConfigProvider> ConfigProviders { get; }

        ClientConfiguration Configuration { get; }

        IByteConverter Converter { get; }

        ITypeTranscoder Transcoder { get; }

        IConfigProvider GetProvider(string name);

        IBucket CreateBucket(string bucketName, IAuthenticator authenticator = null);

        IBucket CreateBucket(string bucketName, string username, string password, IAuthenticator authenticator = null);

        void DestroyBucket(IBucket bucket);

        bool IsObserving(string bucketName);

        /// <summary>
        /// Gets the first <see cref="CouchbaseBucket"/> instance found./>
        /// </summary>
        /// <returns></returns>
        IBucket GetBucket(IAuthenticator authenticator);

        [Obsolete("Use IClusterManager.ClusterInfo() instead")]
        IClusterInfo Info();

        void CheckConfigUpdate(string bucketName, IPEndPoint excludeEndPoint);
    }
}

#region [ License information          ]

/* ************************************************************
 *
 *    @author Couchbase <info@couchbase.com>
 *    @copyright 2014 Couchbase, Inc.
 *
 *    Licensed under the Apache License, Version 2.0 (the "License");
 *    you may not use this file except in compliance with the License.
 *    You may obtain a copy of the License at
 *
 *        http://www.apache.org/licenses/LICENSE-2.0
 *
 *    Unless required by applicable law or agreed to in writing, software
 *    distributed under the License is distributed on an "AS IS" BASIS,
 *    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *    See the License for the specific language governing permissions and
 *    limitations under the License.
 *
 * ************************************************************/

#endregion