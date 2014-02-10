// Copyright 2004-2008 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using JasonSoft.Components.Castle.Core.Resource;

namespace JasonSoft.Components.Castle.MicroKernel.SubSystems.Resource
{
    /// <summary>
    /// An implementation of <c>a</c> should 
    /// be able to return instances of <see cref="IResource"/>
    /// for a given resource identifier.
    /// </summary>
    public interface IResourceSubSystem : ISubSystem
    {
        IResource CreateResource(CustomUri uri);

        IResource CreateResource(CustomUri uri, String basePath);

        IResource CreateResource(String resource);

        IResource CreateResource(String resource, String basePath);

        void RegisterResourceFactory(IResourceFactory resourceFactory);
    }
}