using Desafio.Core.Entities;
using Desafio.Core.Interface.Crawlers;
using Desafio.Core.Interface.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Desafio.Application.Services
{
    public class LoremService : ILoremService
    {
        private readonly ILoremCrawler _loremCrawler;

        public LoremService(ILoremCrawler loremCrawler)
        {
            _loremCrawler = loremCrawler;
        }

        public async Task<LoremEntity> GetData()
        {
            try
            {
                return await _loremCrawler.GetData();
            }
            catch
            {
                Random random = new Random();
                return new LoremEntity() { Text = RandomString(random.Next(0, 10000)) };
            }
        }

        private string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
