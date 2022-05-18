using Atrasti.Data.Core;
using Atrasti.Data.Repository;
using Atrasti.Data.Store;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using Atrasti.Data.Repository.Chat;

namespace Atrasti.Data
{
    public static class DataInitializer
    {
        public static void InitializeRepositories(this IServiceCollection collection)
        {
            collection.AddScoped<ICompanyRepository, CompanyRepository>();
            collection.AddScoped<IProductRepository, ProductRepository>();
            collection.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            collection.AddScoped<IForumRepository, ForumRepository>();
            collection.AddScoped<IUserRepository, UserRepository>();
            collection.AddScoped<IChatRepository, ChatRepository>();
            collection.AddScoped<IUserDataRepository, UserDataRepository>();
            collection.AddScoped<IConstructionRepository, ConstructionRepository>();
            collection.AddScoped<IShorteningRepository, ShorteningRepository>();
            collection.AddScoped<IAgentRepository, AgentRepository>();
        }

        public static IdentityBuilder InitializeDapperStores(this IdentityBuilder builder)
        {
            builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            builder.Services.AddScoped<IForumRepository, ForumRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IChatRepository, ChatRepository>();
            builder.Services.AddScoped<IUserDataRepository, UserDataRepository>();
            builder.Services.AddScoped<IConstructionRepository, ConstructionRepository>();
            builder.Services.AddScoped<IShorteningRepository, ShorteningRepository>();
            builder.Services.AddScoped<IAgentRepository, AgentRepository>();
            builder.Services.AddScoped<IUserCacheRepository, UserCacheRepository>();
            AddStores(builder.Services, builder.UserType, builder.RoleType);
            return builder;
        }

        public static void AddStores(IServiceCollection services, Type userType, Type roleType)
        {
            Type keyType = userType.BaseType.GenericTypeArguments[0];
            var userClaimType = typeof(IdentityUserClaim<>).MakeGenericType(keyType);
            var userLoginType = typeof(IdentityUserLogin<>).MakeGenericType(keyType);
            var userTokenType = typeof(IdentityUserToken<>).MakeGenericType(keyType);

            var userRoleType = typeof(IdentityUserRole<>).MakeGenericType(keyType);
            var roleClaimType = typeof(IdentityRoleClaim<>).MakeGenericType(keyType);

            var userStoreType =
                typeof(UserStore<,,,,>).MakeGenericType(keyType, userType, userClaimType, userLoginType, userTokenType);
            var roleStoreType = typeof(RoleStore<,,,>).MakeGenericType(keyType, roleType, userRoleType, roleClaimType);

            services.AddScoped<ConnectionProvider>();
            services.AddScoped(typeof(IUserStore<>).MakeGenericType(userType), userStoreType);
            services.AddScoped(typeof(IRoleStore<>).MakeGenericType(roleType), roleStoreType);
            services.AddScoped(typeof(UserManager<>).MakeGenericType(userType));
            services.AddScoped(typeof(SignInManager<>).MakeGenericType(userType));
        }
    }
}