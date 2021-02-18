using Desafio.Infraestruture.IOC;
using Desafio.Infraestruture.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace Desafio.Startup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DesejaModificarBuffer();
            CreateHostBuilder(args).Build().Run();
        }

        private static void DesejaModificarBuffer()
        {
            Console.WriteLine("Deseja modificar o buffer? (S/N)");
            var deseja = Console.ReadLine();
            if (deseja.ToUpper().Equals("S")){
                ModificarBuffer();
            }
            else if (deseja.ToUpper().Equals("N"))
            {
                PathArquivo();
            }
        }

        private static void ModificarBuffer()
        {
            Console.WriteLine("Digite o buffer em bytes (máximo 1048576 = 1MB)?");
            var bytesTxt = Console.ReadLine();
            try
            {
                var bytes = long.Parse(bytesTxt);
                if(bytes <= 1048576)
                {
                    WorkerSettings.MaxBufferBytes = bytes;
                    PathArquivo();
                }
                else
                {
                    ModificarBuffer();
                }
            }
            catch
            {
                ModificarBuffer();
            }
        }

        private static void PathArquivo()
        {
            Console.WriteLine("Preencha o path para o arquivo gerado? (Exemplo: D:\\Desafio)");
            var path = Console.ReadLine();

            if (Directory.Exists(path))
            {
                WorkerSettings.FilePath = path;
            }
            else
            {
                Console.WriteLine("Diretório não existe");
                PathArquivo();
            }
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) => {
                    services.AddHostedService<WorkerDesafio>();
                    Ioc.Register(services);
                });
    }
}
