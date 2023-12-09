using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using News.Vampire.Service.Configuration;
using News.Vampire.Service.Managers.Interfaces;

namespace News.Vampire.Service.Managers
{
    public class AwsManager : IAwsManager
    {
        public class ResponseOperation
        {
            public bool IsSucceed { get; set; }
            public string? Message { get; set; }
            public string? Response { get; set; }
        }

        private readonly AppSettings _appSettings;

        public AwsManager(IConfiguration configuration)
        {
            _appSettings = new AppSettings();
            configuration.GetSection(AppSettings.Section).Bind(_appSettings);
        }

        public async Task<ResponseOperation> UploadFileToS3Async(
            Stream sourceStream,
            string fileName,
            string databaseId,
            string folderPath)
        {
            var bucketName = _appSettings.AmazonPublicS3BucketName;
            var keyName = $@"database.{databaseId}/{folderPath}/{fileName}";
            var responseOperation = new ResponseOperation();

            try
            {
                string regionEndpointName = _appSettings.AmazonS3RegionEndpoint;

                RegionEndpoint regionEndpoint = RegionEndpoint.GetBySystemName(regionEndpointName);
                var credentials = new BasicAWSCredentials(_appSettings.AmazonAccessKey, _appSettings.AmazonSecretKey);

                var client = new AmazonS3Client(credentials, new AmazonS3Config { RegionEndpoint = regionEndpoint });

                var request = new PutObjectRequest
                {
                    InputStream = sourceStream,
                    BucketName = bucketName,
                    Key = keyName,
                    CannedACL = S3CannedACL.PublicRead
                };

                var response = await client.PutObjectAsync(request);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    responseOperation.IsSucceed = true;
                    responseOperation.Response =
                        $@"https://s3.{regionEndpointName}.{_appSettings.AmazonS3AwsUrl}/{bucketName}/{keyName}";
                }
                else
                {
                    responseOperation.IsSucceed = false;
                    responseOperation.Message = $"Could not upload {keyName} to {bucketName}.";
                }
            }
            catch (Exception ex)
            {
                responseOperation.IsSucceed = false;
                responseOperation.Message = $"Error: '{ex.Message}' when writing an object";
            }

            return responseOperation;
        }

    }
}
