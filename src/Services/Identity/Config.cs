// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
// ReSharper disable StringLiteralTypo

namespace Identity;

public static class Config
{
    public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
    {
        new ApiResource("resource_ocelot",new [] { JwtClaimTypes.Role }) { Scopes = { "ocelotfull_scope" } },
        new ApiResource("resource_assessment",new [] { JwtClaimTypes.Role }) { Scopes = { "assessmentfull_scope" } },
        new ApiResource("resource_account",new [] { JwtClaimTypes.Role }) { Scopes = { "accountfull_scope" } },
        new ApiResource("resource_order",new [] { JwtClaimTypes.Role }) { Scopes = { "orderfull_scope" } },
        new ApiResource("resource_notification",new [] { JwtClaimTypes.Role }) { Scopes = { "notification_scope" } },
        new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
    };

    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.Email(),
            new IdentityResources.OpenId(), // sub
            new IdentityResources.Profile(),
            new IdentityResource() { Name = "roles", DisplayName = "Roles", Description = "User roles", UserClaims = new[] { "role" } }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("ocelotfull_scope", "Full permission for ocelot gateway"),
            new ApiScope("scope1"),
            new ApiScope("scope2"),
            new ApiScope("assessmentfull_scope", "Full permission for assessment"),
            new ApiScope("accountfull_scope", "Full permission for account"),
            new ApiScope("orderfull_scope", "Full permission for order"),
            new ApiScope("notification_scope", "Full permission for notification"),
            new ApiScope(IdentityServerConstants.LocalApi.ScopeName),
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        { 
            new Client
            {
                ClientName = "Asp.Net Core MVC",
                ClientId = "WebMvcClient",
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = { "ocelotfull_scope", "assessmentfull_scope", "accountfull_scope", "orderfull_scope", "notification_scope", IdentityServerConstants.LocalApi.ScopeName },
            },
            new Client
            {
                ClientName = "Asp.Net Core MVC",
                ClientId = "WebMvcClientForUserPass",
                AllowOfflineAccess = true, // refresh token
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.LocalApi.ScopeName, 
                    "roles",
                    "ocelotfull_scope",
                    "assessmentfull_scope",
                    "accountfull_scope",
                    "orderfull_scope",
                    "notification_scope",
                    IdentityServerConstants.StandardScopes.OfflineAccess, // refresh token
                },
                AccessTokenLifetime = 1 * 60 * 60,
                RefreshTokenExpiration = TokenExpiration.Sliding,
                // AbsoluteRefreshTokenLifetime = (int)(DateTime.Now.AddDays(60) - DateTime.Now).TotalSeconds,
                RefreshTokenUsage = TokenUsage.ReUse
            },
        };
}