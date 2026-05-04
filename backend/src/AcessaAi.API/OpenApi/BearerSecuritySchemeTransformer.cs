using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace AcessaAi.API.OpenApi;

internal sealed class BearerSecuritySchemeTransformer(
    IAuthenticationSchemeProvider authenticationSchemeProvider) : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var authSchems = await authenticationSchemeProvider.GetAllSchemesAsync();

        if(!authSchems.Any(s => s.Name == "Bearer"))
            return;

        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Cole apenas o token JWT (sem o prefixo 'Bearer ')."
        };

        var bearerRef = new OpenApiSecuritySchemeReference("Bearer", document);

        foreach(var pathItem in document.Paths.Values)
        {
            if (pathItem.Operations is null)
                continue;

            foreach(var operation in pathItem.Operations.Values)
            {
                operation.Security ??= new List<OpenApiSecurityRequirement>();
                operation.Security.Add(new OpenApiSecurityRequirement
                {
                    [bearerRef] = []
                });
            }
        }
    }
}
