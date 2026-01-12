using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ZHSystem.Application.Common;
using ZHSystem.Application.Mapping;
using System.Reflection;


namespace ZHSystem.Application
{
    public static  class ServiceExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<MappingProfile>();

            var loggerFactory = LoggerFactory.Create(builder => { });

            var mapperConfig = new MapperConfiguration(
                configExpression,
                loggerFactory
            );

            var mapper = mapperConfig.CreateMapper();

            services.AddSingleton<IMapper>(mapper);
            services.AddSingleton<IConfigurationProvider>(mapperConfig);

             
            //services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>)); 
            });
            //services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
