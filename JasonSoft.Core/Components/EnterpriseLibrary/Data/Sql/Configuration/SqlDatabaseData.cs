//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;
using System.Configuration;
using JasonSoft.Components.EnterpriseLibrary.Common.Configuration;
using JasonSoft.Components.EnterpriseLibrary.Common.Configuration.ContainerModel;
using JasonSoft.Components.EnterpriseLibrary.Data.Configuration;
using JasonSoft.Components.EnterpriseLibrary.Data.Instrumentation;

namespace JasonSoft.Components.EnterpriseLibrary.Data.Sql.Configuration
{
    /// <summary>
    /// Describes a <see cref="SqlDatabase"/> instance, aggregating information from a 
    /// <see cref="ConnectionStringSettings"/>.
    /// </summary>
    public class SqlDatabaseData : DatabaseData
    {
        ///<summary>
        /// Initializes a new instance of the <see cref="SqlDatabase"/> class with a connection string and a configuration
        /// source.
        ///</summary>
        ///<param name="connectionStringSettings">The <see cref="ConnectionStringSettings"/> for the represented database.</param>
        ///<param name="configurationSource">The <see cref="IConfigurationSource"/> from which additional information can 
        /// be retrieved if necessary.</param>
        public SqlDatabaseData(ConnectionStringSettings connectionStringSettings, IConfigurationSource configurationSource)
            : base(connectionStringSettings, configurationSource)
        {
        }

        /// <summary>
        /// Creates a <see cref="TypeRegistration"/> instance describing the <see cref="SqlDatabase"/> represented by 
        /// this configuration object.
        /// </summary>
        /// <returns>A <see cref="TypeRegistration"/> instance describing a database.</returns>
        public override IEnumerable<TypeRegistration> GetRegistrations()
        {
            yield return new TypeRegistration<Database>(
                () => new SqlDatabase(
                    ConnectionString,
                    Common.Configuration.ContainerModel.Container.Resolved<IDataInstrumentationProvider>(Name)))
                {
                    Name = Name,
                    Lifetime = TypeRegistrationLifetime.Transient
                };
        }
    }
}
