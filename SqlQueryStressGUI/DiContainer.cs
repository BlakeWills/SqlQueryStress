using System;

namespace SqlQueryStressGUI
{
    public sealed class DiContainer
    {
        private static bool _isInit = false;

        private DiContainer(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            Instance = this;
        }

        public static DiContainer Instance { get; private set; }

        public IServiceProvider ServiceProvider { get; }

        public static void Initialize(IServiceProvider serviceProvider)
        {
            if(!_isInit)
            {
                _isInit = true;
                Instance = new DiContainer(serviceProvider);
            }
        }
    }
}
