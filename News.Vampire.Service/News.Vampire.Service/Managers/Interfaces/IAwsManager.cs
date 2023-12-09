namespace News.Vampire.Service.Managers.Interfaces
{
    public interface IAwsManager
    {
        Task<AwsManager.ResponseOperation> UploadFileToS3Async(
            Stream sourceStream,
            string fileName,
            string databaseId,
            string folderPath);
    }
}
