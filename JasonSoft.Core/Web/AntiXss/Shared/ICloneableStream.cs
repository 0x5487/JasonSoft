// ***************************************************************
// <copyright file="ICloneableStream.cs" company="Microsoft">
//      Copyright (C) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
//      ...
// </summary>
// ***************************************************************

namespace JasonSoft.Web.Security.AntiXss.Internal
{
    using System;
    using System.IO;

    internal interface ICloneableStream
    {
        Stream Clone();
    }
}