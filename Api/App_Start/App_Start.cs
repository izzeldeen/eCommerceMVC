//// -----------------------------------------------------------------------
//// <copyright file="App_Start.cs" company="Fluent.Infrastructure">
////     Copyright © Fluent.Infrastructure. All rights reserved.
//// </copyright>
////-----------------------------------------------------------------------
/// See more at: https://github.com/dn32/Fluent.Infrastructure/wiki
////-----------------------------------------------------------------------

//using Api.DataBase;
using System.Web.Http;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Api.App_Start), "PreStart")]

namespace Api {
    public static class App_Start {
        public static void PreStart() {
          
            //     FluentStartup.Initialize(typeof(DbContextLocal));
            HttpConfiguration config = GlobalConfiguration.Configuration;

            config.Formatters.JsonFormatter
                        .SerializerSettings
                        .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }
    }
}