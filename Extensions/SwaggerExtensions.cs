using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace VotingSystem.Extensions
{
    //  This is the filter that injects custom Swagger tags
    public class SwaggerTagsDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Tags = new List<OpenApiTag>
    {
                new OpenApiTag { Name = "Roles", Description = "Manage system roles" },


             // here to add the documention of your swagger  u find the reigster of this method in the appServiceExtensions
             //  (opt.AddSwaggerTags();)  (opt.EnableAnnotations();)


    };
        }

    }

    // 🔹 This extension method makes it easy to add the tags filter
    public static class SwaggerExtensions
    {
        public static void AddSwaggerTags(this SwaggerGenOptions options)
        {
            options.DocumentFilter<SwaggerTagsDocumentFilter>();
        }
    }
}
