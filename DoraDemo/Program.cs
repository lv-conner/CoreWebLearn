using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using Dora.DynamicProxy;
using Dora.Interception;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;

namespace DoraDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var demo = new ServiceCollection()
                    .AddSingleton<Demo, Demo>()
                    .BuildInterceptableServiceProvider()
                    .GetRequiredService<Demo>();
            Debug.Assert(demo.Invoke("foobar") == "FOOBAR");

            Console.ReadKey();
        }
    }
    public class Demo
    {
        [ConvertArguments]
        public virtual string Invoke([UpperCase]string input)
        {
            return input;
        }
    }


    public interface IArgumentConvertor
    {
        object Convert(ArgumentConveresionContext context);
    }

    public class ArgumentConveresionContext
    {
        public ParameterInfo ParameterInfo { get; }
        public object Value { get; }

        public ArgumentConveresionContext(ParameterInfo parameterInfo, object valule)
        {
            this.ParameterInfo = parameterInfo;
            this.Value = valule;
        }
    }

    public interface IArgumentConvertorProvider
    {
        IArgumentConvertor GetArgumentConvertor();
    }


    [AttributeUsage(AttributeTargets.Parameter)]
    public class UpperCaseAttribute : Attribute, IArgumentConvertor, IArgumentConvertorProvider
    {
        public object Convert(ArgumentConveresionContext context)
        {
            if (context.ParameterInfo.ParameterType == typeof(string))
            {
                return context.Value?.ToString()?.ToUpper();
            }
            return context.Value;
        }

        public IArgumentConvertor GetArgumentConvertor()
        {
            return this;
        }
    }


    public class ArgumentConversionInterceptor
    {
        private InterceptDelegate _next;

        public ArgumentConversionInterceptor(InterceptDelegate next)
        {
            _next = next;
        }

        public Task InvokeAsync(InvocationContext invocationContext)
        {
            var parameters = invocationContext.TargetMethod.GetParameters();
            for (int index = 0; index < invocationContext.Arguments.Length; index++)
            {
                var parameter = parameters[index];
                var converterProviders = parameter.GetCustomAttributes(false).OfType<IArgumentConvertorProvider>().ToArray();
                if (converterProviders.Length > 0)
                {
                    var convertors = converterProviders.Select(it => it.GetArgumentConvertor()).ToArray();
                    var value = invocationContext.Arguments[0];
                    foreach (var convertor in convertors)
                    {
                        var context = new ArgumentConveresionContext(parameter, value);
                        value = convertor.Convert(context);
                    }
                    invocationContext.Arguments[index] = value;
                }
            }
            return _next(invocationContext);
        }
    }

    public class ConvertArgumentsAttribute : InterceptorAttribute
    {
        public override void Use(IInterceptorChainBuilder builder)
        {
            builder.Use<ArgumentConversionInterceptor>(this.Order);
        }
    }
}
