using System;

namespace AcessaAi.Infrastructure.Configurations;

public class S3Settings
{
    public string BucketName { get; set; } = default!;
    public string PublicBaseUrl { get; set; } = default!;
}
