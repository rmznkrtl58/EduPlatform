// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace EduPlatform.IdentityServer
{
    public static class Config
    {

        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                     new IdentityResources.OpenId(),//olmazsa olmaz
                     new IdentityResources.Email(),
                     new IdentityResources.Profile(),
                     new IdentityResource()
                     {
                         Name="roles",DisplayName="roles",Description="Kullanıcı Rolleri",
                         UserClaims=new[]{ClaimTypes.Role}
                     }
                   };




        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
            new ApiScope("catalog_fullpermission","Catalog Api için full erişim"),
            new ApiScope("photo_stock_fullpermission","Photo Stock Api için full erişim"),
            new ApiScope("basket_fullpermission","Basket Api için full erişim"),
            new ApiScope("discount_fullpermission","Discount Api için full erişim"),
            new ApiScope("order_fullpermission","Order Api için full erişim"),
            new ApiScope("fake_Payment_fullpermission","payment Api için full erişim"),
            new ApiScope("gateway_fullpermission","gateway Api için full erişim"),
            new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };



        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("resource_catalog"){Scopes={"catalog_fullpermission" } },
                new ApiResource("resource_photo_Stock"){Scopes={"photo_stock_fullpermission" } },
                new ApiResource("resource_basket"){Scopes={"basket_fullpermission" } },
				new ApiResource("resource_discount"){Scopes={ "discount_fullpermission" } },
				new ApiResource("resource_order"){Scopes={ "order_fullpermission" } },
				new ApiResource("resource_fake_payment"){Scopes={ "fake_Payment_fullpermission" } },
				new ApiResource("resource_gateway"){Scopes={ "gateway_fullpermission" } },
				new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
            };


        //AspNetCoreMvc yani tokenı almak isteyen clientim.
        public static IEnumerable<Client> Clients =>
            new Client[]
            {     
               new Client()
               {   //yani benim clientim hangi mikroservicelerime istek atabilir burda bunun yapılandırmasını yapıyoruz.
                   ClientName="Asp.Net Core Mvc",
                   ClientId="WebMvcClient",
                   ClientSecrets={new Secret("Secret123".Sha256())},
                   AllowedGrantTypes=GrantTypes.ClientCredentials,
                   AllowedScopes={
                       "catalog_fullpermission", 
                       "photo_stock_fullpermission",
					   "gateway_fullpermission",
					   IdentityServerConstants.LocalApi.ScopeName
                   }
               },
               new Client()
               {   //Kullanıcı adı ve şifreyle giriş yapıldığındaki durumda
                   ClientName="Asp.Net Core Mvc",
                   ClientId="WebMvcClientForUser",
                   AllowOfflineAccess=true,
                   ClientSecrets={new Secret("secretforuser123".Sha256())},
                   AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,
				   AllowedScopes={
					   "basket_fullpermission",
					   "discount_fullpermission",
					   "order_fullpermission",
					   "fake_Payment_fullpermission",
					   "gateway_fullpermission",
					   IdentityServerConstants.StandardScopes.OpenId,
                       IdentityServerConstants.StandardScopes.Email,
                       IdentityServerConstants.StandardScopes.Profile,
                       IdentityServerConstants.StandardScopes.OfflineAccess,//olmak zorunda refresh token için
                       IdentityServerConstants.LocalApi.ScopeName,
					   "roles"
				       },
                   AccessTokenLifetime=1*60*60,//1 saat
                   RefreshTokenExpiration=TokenExpiration.Absolute,//kesin tarih değişme yok.
                   AbsoluteRefreshTokenLifetime=(int) (DateTime.Now.AddDays(60)- DateTime.Now).TotalSeconds,//60 gün
                   RefreshTokenUsage=TokenUsage.ReUse,//aynı refreh token kullanılabilsinmi evet
			   }
			};
    }
}