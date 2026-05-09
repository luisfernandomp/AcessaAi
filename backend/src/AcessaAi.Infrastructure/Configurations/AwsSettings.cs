using System;

namespace AcessaAi.Infrastructure.Configurations;

public class AwsSettings
{
    public string Region { get; set; } 
    public string ServiceURL { get; set; } 
    public bool ForcePathStyle { get; set; } 
    public string AccessKey { get; set; } 
    public string SecretKey { get; set; } 
}
