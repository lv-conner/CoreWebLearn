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

namespace Basic
{
    class Program
    {
        static void Main(string[] args)
        {
            LoadAssembly();
            Console.WriteLine("MainThread" + Thread.CurrentThread.ManagedThreadId);
            TrackFileChange();

            //TestLazy();
            //GetOptions();
            //GetServices();
            Console.ReadKey();
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

        static void Config()
        {
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
            //使用Configure进行配置Options时，DI容器实际注册的是IConfigureOptions<Options>
            //由于AddOptions()中添加了IOptions<>,OptionsManager<>，因此使用DI容器进行获取IOption<>时，均会获取OptionsManager<>
            //IOptions的Value至获取由OptionsManager的Create获取。
            //最终Options的创建由IOptionsFactory获取。IOptionFactory的实现OptionsFactory中的构造函数包含了使用Configure<>方法调用注册的IConfigureOptions<>
            //最终使用这些配置对新建的Options进行配置
            //因此Options模式，要求Options需要具备无参构造函数。
            IServiceProvider serviceProvider = new ServiceCollection().AddOptions()
                .Configure<StringOptions>("1",so =>
                {
                    so.name = "tem";
                })
                .Configure<StringOptions>("2",op =>
                {
                    op.name = "tim1";
                })
                .Configure<StringOptions>("3",op1 =>
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
}
