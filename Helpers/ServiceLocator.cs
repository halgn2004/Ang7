namespace Ang7.Helpers
{
    public static class ServiceLocator
    {
        private static IServiceProvider? _serviceProvider;

        public static void SetServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static T GetService<T>() where T : class
        {
            return _serviceProvider?.GetService<T>() ?? throw new InvalidOperationException("Service provider is not set.");
        }
    }
}
