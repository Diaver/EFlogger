using System;
using System.Collections.Concurrent;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Reflection;
using EFlogger.Network.Network;
using EFlogger.Profiling;

namespace EFlogger.EntityFramework6
{
   /// <summary>
    /// Provides helper methods to help with initializing the MiniProfiler for Entity Framework 6.
    /// </summary>
    public class EFloggerFor6
    {
        public static void StopSendToClient()
        {
            Logger.IsSendToNetwork = false;
        }

        public static void StartSendToClient()
        {
            Logger.IsSendToNetwork = true;
        }

        public static void WriteMessage(string message)
        {
            Logger.WriteMessage(message);
        }

        public static void ClearLog()
        {
            Logger.ClearLog();
        }

        public static void SetProfilerClientIP(string profilerClientIP)
        {
            CommandSender.IP = profilerClientIP;
        }

        public static void EnableDecompiling()
        {
            Logger.IsEnableDecompiling = true;
        }

        public static void ExcludeAssembly(string assemblyName)
        {
            Settings.ExcludeAssembly(assemblyName);
        }

        public static void SetLogDelegate(Action<string> action)
        {
            Logger.SetLogDelegate(action);
        }

        /// <summary>
        /// A cache so we don't have to do reflection every time someone asks for the MiniProfiler implementation for a DB Provider.
        /// </summary>
        private static readonly ConcurrentDictionary<DbProviderServices,DbProviderServices> ProviderCache = new ConcurrentDictionary<DbProviderServices, DbProviderServices>();
        private static readonly ConcurrentDictionary<IDbConnectionFactory, IDbConnectionFactory> ProviderCache2 = new ConcurrentDictionary<IDbConnectionFactory, IDbConnectionFactory>(); 


        /// <summary>
        /// Registers the WrapProviderService method with the Entity Framework 6 DbConfiguration as a replacement service for DbProviderServices.
        /// </summary>
        public static void Initialize()
        {
            try
            {
                //Database.DefaultConnectionFactory = new ProfiledDbConnectionFactory(Database.DefaultConnectionFactory);
                DbConfiguration.Loaded += (_, a) =>
                {
                    a.ReplaceService<DbProviderServices>((s, k) => WrapProviderService(s));
                 //   a.ReplaceService<IDbConnectionFactory>((s, k) => new ProfiledDbConnectionFactory(s)); 
                };

                ExcludeEntityFrameworkAssemblies();
            }
            catch (SqlException ex)
            {
                // Try to prevent tripping this harmless Exception when initializing the DB
                // Issue in EF6 upgraded from EF5 on first db call in debug mode: http://entityframework.codeplex.com/workitem/594
                if (!ex.Message.Contains("Invalid column name 'ContextKey'"))
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Wraps the provided DbProviderServices class in a MiniProfiler profiled DbService and returns the wrapped service.
        /// </summary>
        /// <param name="services">The DbProviderServices service to wrap.</param>
        /// <returns>A wrapped version of the DbProviderService service.</returns>
        private static DbProviderServices WrapProviderService(DbProviderServices services)
        {
         
           // First let's check our cache.
            if (ProviderCache.ContainsKey(services))
            {
                return ProviderCache[services];
            }

            // Then let's see if our type is already wrapped.
            var serviceType = services.GetType();
            while (serviceType != null)
            {
                if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(EFProfiledDbProviderServices<>))
                {
                    ProviderCache[services] = services;
                    return services;
                }

                serviceType = serviceType.BaseType;
            }
     
            var genericType = typeof(EFProfiledDbProviderServices<>);
            Type[] typeArgs = { services.GetType() };
            var createdType = genericType.MakeGenericType(typeArgs);
            var instanceProperty = createdType.GetField("Instance", BindingFlags.Public | BindingFlags.Static);
            var instance = instanceProperty.GetValue(null) as DbProviderServices;
            ProviderCache[services] = instance;
            return instance;

        }


        private static IDbConnectionFactory WrapProviderService2(IDbConnectionFactory services)
        {

            return new ProfiledDbConnectionFactory(services);

        }


       

        private static void ExcludeEntityFrameworkAssemblies()
        {
            Settings.ExcludeAssembly("EntityFramework");
            Settings.ExcludeAssembly("EntityFramework.SqlServer");
            Settings.ExcludeAssembly("EntityFramework.SqlServerCompact");
            Settings.ExcludeAssembly(typeof(EFloggerFor6).Assembly.GetName().Name);
        }
    }
}
