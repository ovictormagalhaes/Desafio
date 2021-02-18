using Desafio.Core.Interface.Repositories;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Desafio.Infraestruture.Data
{
    public class FileRepository : IFileRepository
    {
        public async Task Create(string filename, string text)
        {
            using (StreamWriter writer = new StreamWriter(filename, true))
            {
                await writer.WriteAsync(text);
            }
        }
        public async Task Append(string filename, string text)
        {
            using (StreamWriter writer = new StreamWriter(filename, true))
            {
                await writer.WriteAsync(text);
            }
        }

    }
}
