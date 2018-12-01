using System;
using Autofac;
using AutoMapper;
using SimpleChecklist.Common.Interfaces;
using SimpleChecklist.Common.Interfaces.Utils;
using SimpleChecklist.Models.Collections;

namespace SimpleChecklist.LegacyDataRepository
{
    public class LegacyDataRepositoryModule : Module
    {
        private static string HexConverter(Xamarin.Forms.Color c)
        {
            return "#" + ((int) (c.R * 255)).ToString("X2")
                       + ((int) (c.G * 255)).ToString("X2")
                       + ((int) (c.B * 255)).ToString("X2");
        }

        protected override void Load(ContainerBuilder builder)
        {
            Mapper.Initialize(expression =>
            {
                expression
                    .CreateMap<ToDoItem, Common.Entities.ToDoItem>()
                    .ForMember(item => item.Color,
                        opt => opt.MapFrom(item => HexConverter(item.TaskListColor.CurrentColor)))
                    .ForMember(item => item.Data, opt => opt.MapFrom(item => item.Description))
                    .ForMember(item => item.CreationDateTime,
                        opt => opt.MapFrom(item => new DateTime(
                            item.CreationDateTime.ToUniversalTime().Year,
                            item.CreationDateTime.ToUniversalTime().Month,
                            item.CreationDateTime.ToUniversalTime().Day,
                            item.CreationDateTime.ToUniversalTime().Hour,
                            item.CreationDateTime.ToUniversalTime().Minute,
                            item.CreationDateTime.ToUniversalTime().Second,
                            DateTimeKind.Utc)));

                expression.CreateMap<DoneItem, Common.Entities.DoneItem>()
                    .ForMember(item => item.Color,
                        opt => opt.MapFrom(item => HexConverter(item.TaskListColor.CurrentColor)))
                    .ForMember(item => item.Data, opt => opt.MapFrom(item => item.Description))
                    .ForMember(item => item.CreationDateTime,
                        opt => opt.MapFrom(item => new DateTime(
                            item.CreationDateTime.ToUniversalTime().Year,
                            item.CreationDateTime.ToUniversalTime().Month,
                            item.CreationDateTime.ToUniversalTime().Day,
                            item.CreationDateTime.ToUniversalTime().Hour,
                            item.CreationDateTime.ToUniversalTime().Minute,
                            item.CreationDateTime.ToUniversalTime().Second,
                            DateTimeKind.Utc)))
                    .ForMember(item => item.FinishDateTime,
                        opt => opt.MapFrom(item => new DateTime(
                            item.FinishDateTime.ToUniversalTime().Year,
                            item.FinishDateTime.ToUniversalTime().Month,
                            item.FinishDateTime.ToUniversalTime().Day,
                            item.FinishDateTime.ToUniversalTime().Hour,
                            item.FinishDateTime.ToUniversalTime().Minute,
                            item.FinishDateTime.ToUniversalTime().Second,
                            DateTimeKind.Utc)));
            });

            builder.RegisterDecorator<IRepository>(
                (c, repository) => new LegacyFileRepository(repository, c.Resolve<Func<string, IFile>>()),
                "fileRepository",
                "legacyFileRepository");

            builder.RegisterType<TaskListObservableCollection>().SingleInstance();
            builder.RegisterType<DoneListObservableCollection>().SingleInstance();
        }
    }
}
