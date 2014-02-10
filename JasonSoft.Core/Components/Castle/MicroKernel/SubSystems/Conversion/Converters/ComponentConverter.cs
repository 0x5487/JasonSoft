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
using JasonSoft.Components.Castle.Core;
using JasonSoft.Components.Castle.Core.Configuration;
using JasonSoft.Components.Castle.MicroKernel.Util;

namespace JasonSoft.Components.Castle.MicroKernel.SubSystems.Conversion
{
    [Serializable]
    public class ComponentConverter : AbstractTypeConverter, IKernelDependentConverter
    {
        public override bool CanHandleType(Type type, IConfiguration configuration)
        {
            if (configuration.Value != null)
            {
                return ReferenceExpressionUtil.IsReference(configuration.Value.Trim());
            }
            else
            {
                return CanHandleType(type);
            }
        }

        public override bool CanHandleType(Type type)
        {
            if (Context.Kernel == null) return false;

            return Context.Kernel.HasComponent(type);
        }

        public override object PerformConversion(String value, Type targetType)
        {
            if (!ReferenceExpressionUtil.IsReference(value))
            {
                String message =
                    String.Format(
                        "Could not convert expression {0} to type {1}. Expecting a reference override like ${some key}",
                        value, targetType.FullName);
                throw new ConverterException(message);
            }

            String key = ReferenceExpressionUtil.ExtractComponentKey(value);

            DependencyModel dependency = new DependencyModel(DependencyType.ServiceOverride, key, targetType, false);

            return Context.Kernel.Resolver.Resolve(CreationContext.Empty, null, Context.CurrentModel, dependency);
        }

        public override object PerformConversion(IConfiguration configuration, Type targetType)
        {
            return PerformConversion(configuration.Value, targetType);
        }
    }
}