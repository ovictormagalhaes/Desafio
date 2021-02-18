using Desafio.Core.Entities;
using System.Threading.Tasks;

namespace Desafio.Core.Interface.Services
{
    public interface IMotherEffService
    {
        Task<MotherEffEntity> GetData(string text);
    }
}
