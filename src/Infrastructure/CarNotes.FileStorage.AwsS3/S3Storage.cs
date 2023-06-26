using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using CarNotes.Domain.Interfaces.Services;
using CarNotes.Domain.Models;

namespace CarNotes.FileStorage.AwsS3;

public class S3Storage : IFileStorageService<S3FileObject>
{
    private readonly S3Options _s3Options;

    public S3Storage(S3Options s3Options)
    {
        _s3Options = s3Options;
    }

    public async Task<FileStorageResponse> UploadFileAsync(S3FileObject fileObject)
    {
        var credentials = new BasicAWSCredentials(
            _s3Options.AccessKey, _s3Options.SecretKey);
        var config = new AmazonS3Config();
        var response = new FileStorageResponse();

        try
        {
            var uploadRequest = new TransferUtilityUploadRequest()
            {
                InputStream = fileObject.Stream,
                Key = fileObject.FullName,
                BucketName = fileObject.BucketName,
                CannedACL = S3CannedACL.NoACL
            };

            using var client = new AmazonS3Client(credentials, config);
            var transferUtility = new TransferUtility(client);

            await transferUtility.UploadAsync(uploadRequest);

            response.StatusCode = 201;
            response.Message = $"{fileObject.FullName} has been uploaded sucessfully.";
        }
        catch (AmazonS3Exception ex)
        {
            response.StatusCode = (int)ex.StatusCode;
            response.Message = ex.Message;
        }
        catch (Exception ex)
        {
            response.StatusCode = 500;
            response.Message = ex.Message;
        }

        return response;
    }
}
