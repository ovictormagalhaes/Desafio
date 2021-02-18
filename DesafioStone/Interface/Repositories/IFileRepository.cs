using System.Threading.Tasks;

namespace Desafio.Core.Interface.Repositories
{
    public interface IFileRepository
    {
        Task Create(string filename, string text);
        Task Append(string filename, string text);
    }
}
