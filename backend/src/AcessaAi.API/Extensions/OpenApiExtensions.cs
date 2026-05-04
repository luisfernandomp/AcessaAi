using AcessaAi.API.OpenApi;

namespace AcessaAi.API.Extensions;

public static class OpenApiExtensions
{
    public static IServiceCollection AddOpenApiWithJwt(this IServiceCollection services)
    {
        services.AddOpenApi(opt =>
        {
            opt.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
        });

        return services;
    }
}
