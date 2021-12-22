namespace Commons.Extensions
{
    public class ServiceFactory<T>
    {


        private const string DEFAULT = "default";
        private readonly Dictionary<string, T> _services;

        public ServiceFactory()
        {
            _services = new Dictionary<string, T>();
        }

        public void Register(string name, T service)
        {
            _services[name] = service;
        }

        public T Resolve(string name)
        {
            return _services[name];
        }

        public void RegisterDefault(T service)
        {
            _services[DEFAULT] = service;
        }

        public T ResolveDefault()
        {
            return _services[DEFAULT];
        }

    }
}
