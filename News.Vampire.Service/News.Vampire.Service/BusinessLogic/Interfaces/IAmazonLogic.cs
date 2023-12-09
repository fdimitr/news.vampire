using News.Vampire.Service.Managers;

namespace News.Vampire.Service.BusinessLogic.Interfaces
{
    public interface IAmazonLogic
    {
        Task<AwsManager.ResponseOperation> DownloadImageAndSaveToS3Async(string databaseId, string folderPath, Uri imageUrl);
    }
}
