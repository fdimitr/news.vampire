using News.Vampire.Service.Models;
using System.Threading;
using System.Threading.Tasks;

namespace News.Vampire.Service.Managers.Interfaces
{
    public interface IDownloadManager
    {
        Task StartSessionAsync(CancellationToken stoppingToken);
    }
}