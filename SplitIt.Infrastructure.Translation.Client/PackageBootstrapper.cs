using Microsoft.Extensions.DependencyInjection;
using Splitit.Infrastructure.IoC;

namespace SplitIt.Infrastructure.Translation.Client
{
    public class Bootstrap:IBootstrap
    {
        public void Register(IServiceCollection serviceCollection)
        {
            //TODO: Register dependencies here
        }

        public int Bootorder { get; } = int.MaxValue;
    }
}