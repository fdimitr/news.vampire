namespace News.Vampire.Service.Managers.Interfaces
{
    public interface IDownloadManager
    {
        Task StartSessionAsync(CancellationToken stoppingToken);
    }
}