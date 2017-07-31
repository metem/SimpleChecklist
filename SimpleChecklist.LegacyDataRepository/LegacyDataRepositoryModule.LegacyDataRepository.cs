using System;
using Autofac;
using AutoMapper;
using SimpleChecklist.Common.Interfaces;
using SimpleChecklist.Common.Interfaces.Utils;
using SimpleChecklist.LegacyDataRepository.Models.Collections;

namespace SimpleChecklist.LegacyDataRepository
{
    public class LegacyDataRepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Mapper.Initialize(expression =>
            {
                expression.CreateMap<ToDoItem, Common.Entities.ToDoItem>().ReverseMap();
                expression.CreateMap<DoneItem, Common.Entities.DoneItem>().ReverseMap();
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
