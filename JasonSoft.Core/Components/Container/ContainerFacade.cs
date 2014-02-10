using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JasonSoft.Components.Container
{
    public static class ContainerFacade 
    {
        private static IObjectContainer _container;

        public static IObjectContainer Container 
        {
            get 
            { 
                return _container; 
            }
            set 
            {
                _container = value;
            }
        }
    }
}
