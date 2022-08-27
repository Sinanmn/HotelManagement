using Autofac;
using HotelManagement.Common.Utilities;
using System;

namespace HotelManagement.Common
{
    public class CommonModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterType<DateTimeUtility>().As<IDateTimeUtility>()
                .InstancePerLifetimeScope();


            base.Load(builder);
        }
    }
}
