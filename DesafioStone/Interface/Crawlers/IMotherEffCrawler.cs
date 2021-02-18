using Desafio.Core.Entities;
using System.Threading.Tasks;

namespace Desafio.Core.Interface.Crawlers
{
    public interface IMotherEffCrawler
    {
        Task<MotherEffEntity> GetData(string text);
    }
}
