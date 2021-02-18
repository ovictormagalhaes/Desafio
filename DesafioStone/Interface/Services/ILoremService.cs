
using Desafio.Core.Entities;
using System.Threading.Tasks;

namespace Desafio.Core.Interface.Services
{
    public interface ILoremService
    {
        Task<LoremEntity> GetData();
    }
}
