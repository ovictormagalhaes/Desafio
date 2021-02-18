using Desafio.Core.Entities;
using Desafio.Core.Interface.Crawlers;
using Desafio.Core.Interface.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Desafio.Application.Services
{
    public class MotherEffService : IMotherEffService
    {
        private readonly ILogger _logger;
        private readonly IMotherEffCrawler _motherEffCrawler;

        public MotherEffService(ILogger<MotherEffService> logger, IMotherEffCrawler motherEffCrawler)
        {
            _logger = logger;
            _motherEffCrawler = motherEffCrawler;
        }

        public async Task<MotherEffEntity> GetData(string text)
        {
            try
            {
                return await _motherEffCrawler.GetData(text);
            }
            catch(Exception ex)
            {
                _logger.LogWarning("Mother Eff Service Problem: {0}", ex.Message);
                return new MotherEffEntity() { Bytes = text.Length };
            }
        }
        
    }
}
