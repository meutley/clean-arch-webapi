using System;
using Microsoft.Extensions.DependencyInjection;

using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Raven.Client.Documents.Indexes;
using Raven.DependencyInjection;

using SourceName.Domain;
using SourceName.Domain.Users;
using SourceName.Infrastructure.Raven.Repositories;

namespace SourceName.Infrastructure.Raven
{
    internal static class RavenModule
    {
        internal static void AddRavenModule(this IServiceCollection services)
        {
#if (UseRaven)
            services.AddRavenDbDocStore(opts =>
            {
                opts.BeforeInitializeDocStore = docStore =>
                {
                    docStore.Conventions.FindCollectionName = GetCollectionNameForType;
                };
            });
            services.AddRavenDbAsyncSession();

            services.AddScoped<IUserRepository, UserRepository>();

            CreateIndexes(services);
#endif
        }

        private static void CreateIndexes(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var store = serviceProvider.GetService<IDocumentStore>();
            IndexCreation.CreateIndexes(typeof(RavenModule).Assembly, store);
        }

        private static string GetCollectionNameForType(Type type)
        {
            var collectionNameAttribute = Attribute.GetCustomAttribute(
                type,
                typeof(CollectionNameAttribute)
            ) as CollectionNameAttribute;
            if (collectionNameAttribute is object)
            {
                return collectionNameAttribute.Name;
            }
            return DocumentConventions.DefaultGetCollectionName(type);
        }
    }
}