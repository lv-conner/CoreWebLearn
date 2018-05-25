using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Basic.CustomerLoggerExtension;
using System.Reflection;
using System.IO;
using Basic.Options;
using RabbitMQ.Client;
using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.FileProviders;
using System.Threading;
using System.Collections.Generic;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Basic
{
    class Program
    {
        static void Main(string[] args)
        {
            //OptionsMonitorManager();
            //Watch();
            //ManyOptions();
            //Scheme();
            //AssemblyLoad();
            //LoadAssembly();
            //Console.WriteLine("MainThread" + Thread.CurrentThread.ManagedThreadId);
            //TrackFileChange();
            //Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            //DapperCode();
            //FileTest.TestLog();
            //DefaulfEnviromentValue();
            //Logging();
            //ThreadBase();
            //TestLazy();
            //GetOptions();
            //GetServices();
            //CommandLine(args);
            PocketGo.Authentication();
            //DapperLearn.Learn();
            //inte();
            Console.ReadKey();
        }

        static void inte()
        {
            InterLock locker = new InterLock()
            {
                Id = "001"
            };


            var locker1 = Interlocked.Exchange(ref locker, new InterLock() { Id = "002" });

            Console.WriteLine(locker1.Id);


        }





        static void DefaulfEnviromentValue()
        {
            var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables(prefix: "ASPNETCORE_")
            .Build();
            foreach (var item in configuration.AsEnumerable())
            {
                Console.WriteLine($"{ "key=" + item.Key + "\tvalue=" + item.Value }");
            }
        }

        static void CommandLine(string[] args)
        {
            while (true)
            {
                try
                {
                    Console.Write("Enter command line switches:");
                    string arguments = Console.ReadLine();
                    Dictionary<string, string> mapping = new Dictionary<string, string>
                    {
                        ["--a"] = "architecture ",
                        ["-a"] = "architecture ",
                        ["--r"] = "runtime",
                        ["-r"] = "runtime",
                    };
                    IConfiguration config = new ConfigurationBuilder()
                        .AddCommandLine(arguments.Split(' '), mapping)
                        .Build();

                    foreach (var section in config.GetChildren())
                    {
                        Console.WriteLine($"{section.Key}: {section.Value}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }


        static async void ThreadBase()
        {
            Task t = Task.Run(() =>
            {
                Console.WriteLine("first");
            }).ContinueWith(t1 =>
            {
                Console.WriteLine("second");
            }).ContinueWith(t2 =>
            {
                Console.WriteLine("thrid");
            });
            await (t);
        }


        static void ReferenceNotNull(string s)
        {

        }
        static async Task<string> GetName()
        {
            return await Task.Run(() =>
            {
                return "tim";
            });
        }
        static void DapperCode()
        {
            using (SqlConnection sqlcon = new SqlConnection("Server=PRCNMG1L0311;database=CacheDb;user id=sa;password=Root@admin"))
            {
                var cacheList = sqlcon.Query<Cache>("select Id,Value from SqlCacheTable");
            }
        }

        internal class Person
        {
            public string name { get; set; }
            public string id { get; set; }
        }

        /// <summary>
        /// 多次使用Actoin<Options>配置Options。则最后所有的Action都会被依次调用。
        /// </summary>
        static void ManyOptions()
        {
            IServiceProvider serviceProvider = new ServiceCollection()
                .AddOptions()
                .Configure<Person>(p =>
                {
                    p.name = "tim";
                })
                .Configure<Person>(p =>
                {
                    p.id = "001";
                }).BuildServiceProvider();
            Person person = serviceProvider.GetService<IOptions<Person>>().Value;

        }


        /// <summary>
        /// Authtication中间件解析
        /// 处理流程如下：
        /// 先获取IAuthenticationHandlerProvider对象用于获取所有注册的认证处理器
        /// 然后根据IAuthenticationSchemeProvider获取所有注册的认证处理器对应的名称。处理器，以及显示名称
        /// AuthenticationScheme ：HandlerType ,DisplayName，Name(Authenticationscheme)
        /// 然后根据IAuthenticationSchemeProvider中获取的认证处理器。通过名称逐个获取HandlerType进行检查，如果该处理器是实现了IAuthenticationRequestHandler的处理器。
        /// 则让该处理器尝试接管该请求。一旦实现了IAuthenticationRequestHandler的处理器的HandleRequestAsync()方法返回true。则会导致认证过程结束。
        /// 如果没有，则获取默认认证模式的处理器。让其接管认证请求。并设置HttpContext.User属性值。至此。认证结束。
        /// </summary>
        static async void Scheme()
        {
            var services = new ServiceCollection();
            //AddAuthentication
            //services.TryAddScoped<IAuthenticationHandlerProvider, AuthenticationHandlerProvider>();
            //services.TryAddSingleton<IAuthenticationSchemeProvider, AuthenticationSchemeProvider>();
            services.AddAuthentication("cookie")
                .AddCookie("cookie")
                .AddCookie("auth1")
                ;
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            IAuthenticationSchemeProvider schemeProvider = serviceProvider.GetService<IAuthenticationSchemeProvider>();
            IAuthenticationHandlerProvider handlerProvider = serviceProvider.GetService<IAuthenticationHandlerProvider>();
            var sechemes = await schemeProvider.GetAllSchemesAsync();
            foreach (var item in sechemes)
            {
                Console.WriteLine(item.DisplayName + "\t" + item.Name + "\t" + item.HandlerType.FullName);
            }
        }
        /// <summary>
        /// 注册认证管理器的方式
        /// AddAuthentication主要是添加相关模式提供器和处理提供器也可以在其中直接对AuthenticationOptions进行操作，
        /// 添加处理器和处理模式的映射。也可以通过其他扩展方法来注册Action.
        /// </summary>
        static void AddSchemes()
        {
            new ServiceCollection().AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "cookie";
                options.AddScheme("auth1", builder =>
                 {
                     builder.HandlerType = typeof(CookieAuthenticationHandler);
                     builder.DisplayName = "cookie1";
                 });
            });
            //等价于
            new ServiceCollection().AddAuthentication("cookie").AddCookie("auth1", "cookie1", op => { });
        }



        internal class Cache
        {
            public string Id { get; set; }
            public DateTimeOffset ExpiresAtTime { get; set; }
            public long SlidingExpirationInSeconds { get; set; }
        }

        static void WatchFile()
        {
            FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(@"D:\hello.txt");
            fileSystemWatcher.Changed += (sender, args) =>
            {

            };
        }


        static void ReadFile()
        {
            IFileProvider fileProvider = new PhysicalFileProvider(@"D:\");
            fileProvider.GetDirectoryContents("").Where(p => p.IsDirectory).ToList();
        }

        static void Config()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder().AddJsonFile(@"D:\hello.txt");
        }
        static void paraTest(CancellationToken token)
        {
            Console.WriteLine("task start");
            Thread.Sleep(300);
            if (token.IsCancellationRequested)
            {
                return;
            }
            Console.WriteLine("do some job");
        }


        static void FileWatch(CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                return;
            }
            IFileProvider fileProvider = new PhysicalFileProvider(@"D:\");
            var changeToken = fileProvider.Watch("hello.txt");
            changeToken.RegisterChangeCallback(o =>
            {

            }, null);
        }

        static void Watch()
        {
            var path = @"D:\";
            var fileName = "hello.txt";
            IFileProvider fileProvider = new PhysicalFileProvider(path);
            ChangeToken.OnChange(() => fileProvider.Watch(fileName), () =>
               {
                   Console.WriteLine(File.ReadAllText(Path.Combine(path + fileName)));
               });
        }
        static void LogOptions()
        {
            var envConfig = new ConfigurationBuilder().AddEnvironmentVariables().Build();
            var name = envConfig.GetValue<string>("name");

            var config = new ConfigurationBuilder().AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json")).Build();
            var logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Log.txt");
            var services = new ServiceCollection()
                .AddOptions()
                .AddLogging()
                //.Configure<FileLoggerOptions>(options =>
                //{
                //    options.Path = logFilePath;
                //})
                .Configure<FileLoggerOptions>(config.GetSection("fileLoggerOptions"))
                .Configure<DBLoggerOptions>(config.GetSection("DBOptions"))
                .BuildServiceProvider();
            var logger = services.GetService<ILoggerFactory>()
                .AddConsole()
                .AddDebug()
                .AddDBLogger(services.GetService<IOptions<DBLoggerOptions>>().Value)
                .AddFileLogger(services.GetService<IOptions<FileLoggerOptions>>().Value)
                .CreateLogger<Program>();
            logger.LogInformation("Hello");
            try
            {
                throw new IndexOutOfRangeException("Out");
            }
            catch (Exception e)
            {
                logger.LogError(10002, e, e.Message);
            }
        }
        /// <summary>
        /// OptionsMonitor的实现原理，以及Configuration的CancellationTokenSource的实现目的。
        /// </summary>
        static void OptionsMonitorManager()
        {
            var config = new ConfigurationBuilder().AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), false, true).Build();
            IServiceProvider serviceProvider = new ServiceCollection().AddOptions().Configure<StringOptions>(config.GetSection("StringOptions")).BuildServiceProvider();
            //要实现IOptionsMonitors的O你Change事件。则该Options内容提供商（ConfigurationProvider）必须可以实现对原值的监视。
            //OptionsMonitor内部使用ChangToken来注册，其中IChange Token的提供者为ConfigurationChangeTokenSource的IConfiguation
            //也就是 OptionsMonitor.OnChange() => _onchange += Action;
            //OptionsMonitor创建时会对当前数据源进行监视；
            //ChangToken.OnChange(ConfigurationChangeTokenSource.GetReloadChangeToken => IConfiguration.GetReloadChangeToken => CancellationTokenSource)
            //事件的触发流程。
            //在FileConfiguraionProvider的创建过程中。使用FileConfigurationSource的FileProvider.Watch(使用FileSystemWatcher来监听指定文件)来获取IChangeToken。然后当文件内容发生变化的时候
            //触发Reload方法。然后在Reload方法执行完之后，触发ConfigurationProvider.OnReload方法，执行OnReload方法。OnReload方法将执行CancellationTokenSource.OnReload方法。该方法将导致CancellationTokenSource
            //调用Cancel方法。使用OptionsMonitor.OnChange() => _onchange += Action;的方法被调用。
            //我们也可以通过一下方法注册Reload时的回调方法，来获取文件的改变信息。
            config.GetReloadToken().RegisterChangeCallback(o =>
                {
                    Console.WriteLine("reload");
                }, null);
            IOptionsMonitor<StringOptions> options = serviceProvider.GetService<IOptionsMonitor<StringOptions>>();
            options.OnChange((op, value) =>
            {
                Console.WriteLine(op.name);
                Console.WriteLine(value);
            });

            Debug.Assert(options.CurrentValue.name == "tim");
            options.CurrentValue.name = "lv";
            Console.ReadKey();
        }


        static void LoadAssembly()
        {
            var asm = Assembly.Load("System.IO");
            var exportType = asm.ExportedTypes.ToList();
            var definedType = asm.DefinedTypes.ToList();
            var types = asm.GetTypes();
            var type1 = asm.GetExportedTypes();
            var list = new List<Type>();
            asm.GetModules().Aggregate<Module, List<Type>>(list, (l, m) =>
            {
                l.AddRange(m.GetTypes());
                return l;
            });
        }
        static void GetOptions()
        {
            //option模式
            //使用Configure进行配置Options时，DI容器实际注册的是IConfigureOptions<Options>,实现则是ConfigureNamedOptions
            //由于AddOptions()中添加了IOptions<>,OptionsManager<>，因此使用DI容器进行获取IOption<>时，均会获取OptionsManager<>
            //IOptions的Value至获取由OptionsManager的Create获取。
            //最终Options的创建由IOptionsFactory获取。IOptionFactory的实现OptionsFactory中的构造函数包含了使用Configure<>方法调用注册的IConfigureOptions<>
            //最终使用这些配置对新建的Options进行配置
            //因此Options模式，要求Options需要具备无参构造函数。
            IServiceProvider serviceProvider = new ServiceCollection().AddOptions()
                .Configure<StringOptions>("1", so =>
                 {
                     so.name = "tem";
                 })
                .Configure<StringOptions>("2", op =>
                 {
                     op.name = "tim1";
                 })
                .Configure<StringOptions>("3", op1 =>
                 {
                     op1.name = "tim2";
                 })
                .ConfigureAll<StringOptions>(str =>
                {
                    str.name = "tim";
                })

                .BuildServiceProvider();

            var name = serviceProvider.GetService<IConfigureOptions<StringOptions>>();
            var stringOptions = serviceProvider.GetService<IOptions<StringOptions>>();
            var options = new StringOptions();
            var optionsList = serviceProvider.GetServices<IConfigureOptions<StringOptions>>();
            Debug.Assert(optionsList.Count() == 2);
            name.Configure(options);

        }


        static void OptionMore()
        {
            //IServiceProvider serviceProvider = new ServiceCollection().AddOptions().ConfigureAll
        }

        static void SessionCheck()
        {

        }

        static void TrackFileChange()
        {
            ChangeToken.OnChange(() =>
            {
                return new PhysicalFileProvider(Directory.GetCurrentDirectory()).Watch("/*.txt");
            }, () =>
            {
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                Console.WriteLine("change");
            });
        }

        static void GetServices()
        {
            IServiceProvider serviceProvider = new ServiceCollection().AddTransient<IService, Servier1>().AddTransient<IService, Service2>().BuildServiceProvider();

            var services = serviceProvider.GetServices<IService>();
            Debug.Assert(services.Count() == 2);
        }

        static void TestLazy()
        {
            var lazyOptions = new Lazy<StringOptions>();
            var options = lazyOptions.Value;
        }


        static void TestGenicClass()
        {
            IServiceProvider serviceProvider = new ServiceCollection()
                .AddTransient(typeof(ISay<>), typeof(Say<>))
                .BuildServiceProvider();
            ISay<string> say = serviceProvider.GetService<ISay<string>>();
        }
        //程序集加载需要将相关信息添加到依赖文件中
        //Assembly asm1 = Assembly.LoadFrom(Path.Combine(Directory.GetCurrentDirectory(), "DBLogger.dll"));
        //var asm1 = Assembly.Load(new AssemblyName("SimpleLib"));
        //
        //.net core通过反射加载没有添加相应引用的程序集。需要在program.deps.json文件中target：programname dependencies中添加相应的依赖。
        //target中添加dll映射。
        //然后在libraries中注册影响的类库
        //
        static void AssemblyLoad()
        {

            var assembly = Assembly.Load(new AssemblyName("DBLogger"));
            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddAssembly(assembly);
            //var types = assembly.GetTypes();
            //foreach (var item in types)
            //{
            //    var ItemInterface = item.GetInterfaces();
            //    foreach (var inter in ItemInterface)
            //    {
            //        serviceCollection.AddTransient(inter, item);
            //    }
            //    //serviceProvider.AddTransient(typeof(ILogger), item);
            //}
            var service = serviceCollection.BuildServiceProvider().GetService<ILogger>();
            //AssemblyName assembly = new AssemblyName("DBLogger");
        }

        static void Logging()
        {
            IServiceProvider serviceProvider = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.AddConsole();
                }).BuildServiceProvider();
            ILogger logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<int>();
            logger.LogInformation("Hello");
        }







        /// <summary>
        /// 服务范围的实现方式。
        /// DependencyInjection 
        /// </summary>
        static void DependencyInjection()
        {
            IServiceProvider serviceProvider = new ServiceCollection()
                .AddTransient(typeof(IRepository<>), typeof(Repository<>))
                .BuildServiceProvider();
            using (IServiceScope serviceScope = serviceProvider.GetService<IServiceScopeFactory>().CreateScope())
            {
                var repsitory = serviceScope.ServiceProvider.GetService<IRepository<string>>();
            }
        }


    }

    /// <summary>
    /// 添加程序集
    /// </summary>
    public static class DependencyInjectionExtension
    {
        public static ServiceCollection AddAssembly(this ServiceCollection services, Assembly assembly)
        {
            var types = assembly.GetTypes().Where(p => p.IsAbstract == false);
            foreach (var type in types)
            {
                foreach (var inter in type.GetInterfaces())
                {
                    services.AddTransient(inter, type);
                }
            }
            return services;
        }
    }
    public interface ISay<T>
    {

    }
    public class Say<T> : ISay<T>
    {

    }
    public class StringOptions
    {
        public string name { get; set; }
    }

    public interface IService
    {

    }

    public class Servier1 : IService
    {

    }
    public class Service2 : IService
    {

    }


    public interface IRepository<T> where T : class
    {
        void Save();
        T Get();
    }

    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly List<T> List = new List<T>();
        public T Get()
        {
            return List.FirstOrDefault();
        }

        public void Save()
        {
        }
    }

    public class Company<T> where T : class
    {
        private IRepository<T> repository;
        //属性注入。
        public IRepository<T> Repository { get => repository; set => repository = value; }
        //方法注入
        //属性注入和方法注入最终实现都是一样的。因为属性本质上就是方法
        public void SetRepository(IRepository<T> repository)
        {
            Repository = repository;
        }


        //构造函数注入
        public Company(IRepository<T> repository)
        {
            this.Repository = repository;
        }

        public void Save()
        {
            //somecheck
            Repository.Save();
        }

        public T Get()
        {
            //somecheck
            return Repository.Get();
        }
    }

    public class InterLock
    {
        public string Id { get; set; }
    }
}
