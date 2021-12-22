using Commons.Extensions;
using Commons.HttpClientService;
using System.Security.Authentication;

namespace Microsoft.Extensions.DependencyInjection.Extensions
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            IConfigurationSection configurationSection = configuration.GetSection("HttpClient");

            int connectionTimeoutInMillis = configurationSection.GetValue<int>("ConnectionTimeout", 1000); // default 1 secs.
            int socketTimeoutInMillis = configurationSection.GetValue<int>("SocketTimeout", 5000); // default 5 secs.
            int maximumConnectionsPerRoute = configurationSection.GetValue<int>("MaximumConnectionsPerRoute", 200);
            int pooledConnectionLifetime = configurationSection.GetValue<int>("PooledConnectionLifetime", 10); // default 10 mins.
            int pooledConnectionIdleTimeout = configurationSection.GetValue<int>("PooledConnectionIdleTimeout", 5); // default 5 mins.
            bool byPassSSLChecks = configurationSection.GetValue<bool>("ByPassSSLChecks", false);

            var socketsHandler = new SocketsHttpHandler
            {
                ConnectTimeout = TimeSpan.FromMilliseconds(connectionTimeoutInMillis),
                PooledConnectionLifetime = TimeSpan.FromMinutes(pooledConnectionLifetime),
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(pooledConnectionIdleTimeout),
                MaxConnectionsPerServer = maximumConnectionsPerRoute,
                AllowAutoRedirect = true
            };

            if (byPassSSLChecks) // not tested this feature
            {
                socketsHandler.SslOptions = new System.Net.Security.SslClientAuthenticationOptions
                {
                    EnabledSslProtocols = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12,
                    EncryptionPolicy = System.Net.Security.EncryptionPolicy.NoEncryption,
                    CertificateRevocationCheckMode = System.Security.Cryptography.X509Certificates.X509RevocationMode.NoCheck,
                    ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection(),
                    RemoteCertificateValidationCallback = (message, cert, chain, errors) => { return true; }
                };


            }
            services.AddHttpClient("default", client =>
            {
                client.Timeout = TimeSpan.FromMilliseconds(socketTimeoutInMillis);
            }).ConfigurePrimaryHttpMessageHandler(() => socketsHandler);

            services.AddHttpClient("blocking", client =>
            {
                client.Timeout = TimeSpan.FromMilliseconds(300_000);
            }).ConfigurePrimaryHttpMessageHandler(() => socketsHandler);

            services.TryAddSingleton(sp => CreateHttpClientServiceFactory(sp, configuration));

            return services;
        }

        private static ServiceFactory<IHttpClientService> CreateHttpClientServiceFactory(IServiceProvider sp, IConfiguration configuration)
        {
            IConfigurationSection configurationSection = configuration.GetSection("HttpClient");

            IHttpClientService blockingHttpClient = ActivatorUtilities.CreateInstance<HttpClientService>(sp, true);
            IHttpClientService defaultHttpClient = ActivatorUtilities.CreateInstance<HttpClientService>(sp, false);

            var factory = new ServiceFactory<IHttpClientService>();
            factory.RegisterDefault(defaultHttpClient);
            factory.Register(HttpClientConfigurationNames.COMMONS_HTTP_CLIENT_SERVICE, defaultHttpClient);
            factory.Register(HttpClientConfigurationNames.COMMONS_BLOCKING_HTTP_CLIENT_SERVICE, blockingHttpClient);
            return factory;
        }

    }
}
