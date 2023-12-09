using News.Vampire.Service.BusinessLogic.Interfaces;
using News.Vampire.Service.Managers;
using News.Vampire.Service.Managers.Interfaces;

namespace News.Vampire.Service.BusinessLogic
{
    public class AmazonLogic : IAmazonLogic
    {
        private readonly IAwsManager _awsManager;

        public AmazonLogic(IAwsManager awsManager)
        {
            _awsManager = awsManager;
        }

        public async Task<AwsManager.ResponseOperation> DownloadImageAndSaveToS3Async(string databaseId, string folderPath, Uri imageUrl)
        {
            using var httpClient = new HttpClient();
            var content = await httpClient.GetByteArrayAsync(imageUrl);
            using var imageStream = new MemoryStream(content);
            return await _awsManager.UploadFileToS3Async(imageStream, Path.GetFileName(imageUrl.LocalPath), databaseId, folderPath);
        }
    }
}
