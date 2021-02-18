using Desafio.Application.Services;
using Desafio.Core.Interface.Crawlers;
using Desafio.Core.Interface.Repositories;
using Desafio.Core.Interface.Services;
using Desafio.Infraestruture.Data;
using Desafio.Infraestruture.External.Crawlers;
using Microsoft.Extensions.DependencyInjection;

namespace Desafio.Infraestruture.IOC
{
    public class Ioc
    {
        public static void Register(IServiceCollection services)
        {
            services.AddHttpClient();

            services.AddSingleton<IFileRepository, FileRepository>();

            services.AddSingleton<ILoremCrawler, LoremCrawler>();
            services.AddSingleton<IMotherEffCrawler, MotherEffCrawler>();

            services.AddSingleton<ILoremService, LoremService>();
            services.AddSingleton<IMotherEffService, MotherEffService>();

        }
    }
}
