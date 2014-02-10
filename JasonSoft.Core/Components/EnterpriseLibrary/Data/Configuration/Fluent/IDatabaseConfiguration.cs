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

using JasonSoft.Components.EnterpriseLibrary.Common;

namespace JasonSoft.Components.EnterpriseLibrary.Data.Configuration.Fluent
{
    ///<summary>
    /// Supports configuring the data connections via fluent-style interface.
    ///</summary>
    public interface IDatabaseConfiguration : IFluentInterface
    {
        ///<summary>
        /// Configure a named database.
        ///</summary>
        ///<param name="databaseName">Name of database to configure</param>
        ///<returns></returns>
        IDatabaseConfigurationProperties ForDatabaseNamed(string databaseName);
    }
}
