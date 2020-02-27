using Microsoft.Extensions.DependencyInjection;

namespace TheInternet.SystemTests.Raw.Infrastructure
{
    public static class Container
    {
        /// <summary>
        /// Populate the container with any underlying infrastructure. Interfaces and implementations can be overridden by additional consumers. 
        /// </summary>
        /// <param name="services"></param>
        public static void Populate(IServiceCollection services)
        {
            if (services == null) throw new System.ArgumentNullException(nameof(services));


        }
    }
}
