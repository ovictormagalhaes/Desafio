using Desafio.Core.Entities;
using System.Threading.Tasks;

namespace Desafio.Core.Interface.Crawlers
{
    public interface ILoremCrawler
    {
        Task<LoremEntity> GetData();
    }
}
