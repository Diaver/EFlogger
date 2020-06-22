using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using EFlogger.Profiling.Helpers;
using EFlogger.Profiling.SqlFormatters;

//using StackExchange.Profiling.Storage;

namespace EFlogger.Profiling
{
     /// <summary>
        /// Various configuration properties.
        /// </summary>
        public static class Settings
        {
            private static readonly HashSet<string> assembliesToExclude;
            private static readonly HashSet<string> typesToExclude;
            private static readonly HashSet<string> methodsToExclude;

            static Settings()
            {
                var props = from p in typeof(Settings).GetProperties(BindingFlags.Public | BindingFlags.Static)
                            let t = typeof(DefaultValueAttribute)
                            where p.IsDefined(t, inherit: false)
                            let a = p.GetCustomAttributes(t, inherit: false).Single() as DefaultValueAttribute
                            select new { PropertyInfo = p, DefaultValue = a };

                foreach (var pair in props)
                {
                    pair.PropertyInfo.SetValue(null, Convert.ChangeType(pair.DefaultValue.Value, pair.PropertyInfo.PropertyType), null);
                }

                // this assists in debug and is also good for prd, the version is a hash of the main assembly 

                string location = typeof (Settings).Assembly.Location;
              

                try
                {
                    var files = new List<string>();
                    files.Add(location);

                    string customUITemplatesPath = "";
                 
                    if (System.IO.Directory.Exists(customUITemplatesPath))
                    {
                        files.AddRange(System.IO.Directory.EnumerateFiles(customUITemplatesPath));
                    }

                    using (var sha256 = new System.Security.Cryptography.SHA256CryptoServiceProvider())
                    {
                        byte[] hash = new byte[sha256.HashSize / 8];
                        foreach (string file in files)
                        {
                            // sha256 can throw a FIPS exception, but SHA256CryptoServiceProvider is FIPS BABY - FIPS 
                            byte[] contents = System.IO.File.ReadAllBytes(file);
                            byte[] hashfile = sha256.ComputeHash(contents);
                            for (int i = 0; i < (sha256.HashSize / 8); i++)
                            {
                                hash[i] = (byte)(hashfile[i] ^ hash[i]);
                            }
                        }
                        Version = System.Convert.ToBase64String(hash);
                    }
                }
                catch
                {
                    Version = Guid.NewGuid().ToString();
                }

                typesToExclude = new HashSet<string>
                {
                    // while we like our Dapper friend, we don't want to see him all the time
                    "SqlMapper"
                };

                methodsToExclude = new HashSet<string>
                {
                    "lambda_method",
                    ".ctor"
                };

                assembliesToExclude = new HashSet<string>
                {
                    // our assembly
                    typeof(Settings).Assembly.GetName().Name,

                    // reflection emit
                    "Anonymously Hosted DynamicMethods Assembly",

                    // the man
                    "System.Core",
                    "System.Data",
                    "System.Data.Entity",
                    "System.Data.Linq",
                    "System.Web",
                    "System.Web.Mvc",
                    "System.Windows.Forms",
                    "mscorlib",
                    "EntityFramework",
                };

                InitSettings();

            }

             static void InitSettings()
             {
                 //SqlFormatter = new InlineFormatter();
                 SqlFormatter = new SqlServerFormatter();
                 // for normal usage, this will return a System.Diagnostics.Stopwatch to collect times - unit tests can explicitly set how much time elapses
                 StopwatchProvider = StopwatchWrapper.StartNew;
             }

            /// <summary>
            /// Assemblies to exclude from the stack trace report.
            /// Add to this using the <see cref="ExcludeAssembly"/> method.
            /// </summary>
            public static IEnumerable<string> AssembliesToExclude
            {
                get { return assembliesToExclude; }
            }

            /// <summary>
            /// Types to exclude from the stack trace report.
            /// Add to this using the <see cref="ExcludeType"/> method.
            /// </summary>
            public static IEnumerable<string> TypesToExclude
            {
                get { return typesToExclude; }
            }

            /// <summary>
            /// Methods to exclude from the stack trace report.
            /// Add to this using the <see cref="ExcludeMethod"/> method.
            /// </summary>
            public static IEnumerable<string> MethodsToExclude
            {
                get { return methodsToExclude; }
            }

            /// <summary>
            /// Excludes the specified assembly from the stack trace output.
            /// </summary>
            /// <param name="assemblyName">The short name of the assembly. AssemblyName.Name</param>
            public static void ExcludeAssembly(string assemblyName)
            {
                assembliesToExclude.Add(assemblyName);
            }

            /// <summary>
            /// Excludes the specified type from the stack trace output.
            /// </summary>
            /// <param name="typeToExclude">The System.Type name to exclude</param>
            public static void ExcludeType(string typeToExclude)
            {
                typesToExclude.Add(typeToExclude);
            }

            /// <summary>
            /// Excludes the specified method name from the stack trace output.
            /// </summary>
            /// <param name="methodName">The name of the method</param>
            public static void ExcludeMethod(string methodName)
            {
                methodsToExclude.Add(methodName);
            }

            /// <summary>
            /// The maximum number of unviewed profiler sessions (set this low cause we don't want to blow up headers)
            /// </summary>
            [DefaultValue(20)]
            public static int MaxUnviewedProfiles { get; set; }

            /// <summary>
            /// The max length of the stack string to report back; defaults to 120 chars.
            /// </summary>
            [DefaultValue(120)]
            public static int StackMaxLength { get; set; }

            /// <summary>
            /// Any Timing step with a duration less than or equal to this will be hidden by default in the UI; defaults to 2.0 ms.
            /// </summary>
            [DefaultValue(2.0)]
            public static decimal TrivialDurationThresholdMilliseconds { get; set; }



            /// <summary>
            /// By default, SqlTimings will grab a stack trace to help locate where queries are being executed.
            /// When this setting is true, no stack trace will be collected, possibly improving profiler performance.
            /// </summary>
            [DefaultValue(false), Obsolete("Use ExcludeStackTraceSnippetFromCustomTimings")]
            public static bool ExcludeStackTraceSnippetFromSqlTimings { get; set; }

            /// <summary>
            /// By default, <see cref="CustomTiming"/>s created by this assmebly will grab a stack trace to help 
            /// locate where Remote Procedure Calls are being executed.  When this setting is true, no stack trace 
            /// will be collected, possibly improving profiler performance.
            /// </summary>
            [DefaultValue(false)]
            public static bool ExcludeStackTraceSnippetFromCustomTimings { get; set; }




            /// <summary>
            /// Maximum payload size for json responses in bytes defaults to 2097152 characters, which is equivalent to 4 MB of Unicode string data.
            /// </summary>
            [DefaultValue(2097152)]
            public static int MaxJsonResponseSize { get; set; }

          
            /// <summary>
            /// The formatter applied to any SQL before being set in a <see cref="CustomTiming.CommandString"/>.
            /// </summary>
            public static ISqlFormatter SqlFormatter { get; set; }

            /// <summary>
            /// Assembly version of this dank MiniProfiler.
            /// </summary>
            public static string Version { get; private set; }

            /// <summary>
            /// Allows switching out stopwatches for unit testing.
            /// </summary>
            internal static Func<IStopwatch> StopwatchProvider { get; set; }
           
        }
    }
