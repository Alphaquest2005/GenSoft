using System;
using System.Collections.Generic;
using SystemInterfaces;
using Actor.Interfaces;
using Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using Common.DataEntites;
using Common.Dynamic;
using EventMessages;
using EventMessages.Commands;
using EventMessages.Events;
using RevolutionEntities;
using RevolutionEntities.Process;
using RevolutionEntities.ViewModels;
using Utilities;
using ViewModel.Interfaces;

namespace RevolutionData
{
    public static class ProcessActions
    {
        public const int NullProcess = -1;

        public static Dictionary<string, ProcessAction> Actions = new Dictionary<string, ProcessAction>()
        {
            {"ProcessStarted",  new ProcessAction(
                        action: async cp => await Task.Run(() => new SystemProcessStarted(
                            new StateEventInfo(cp.Actor.Process.Id, Context.Process.Events.ProcessStarted),
                            cp.Actor.Process, cp.Actor.Source)),
                        processInfo: cp => new StateCommandInfo(cp.Actor.Process.Id, Context.Process.Commands.StartProcess),
                        expectedSourceType: new SourceType(typeof(IComplexEventService)))},
            {"StartProcess", new ProcessAction(
                action: async cp => await Task.Run(() => new StartSystemProcess(NullProcess,//HACK: to keep this generic, the process that was used to create action will be used
                    new StateCommandInfo(cp.Actor.Process.Id, Context.Process.Commands.StartProcess),
                    cp.Actor.Process, cp.Actor.Source)),
                processInfo: cp => new StateCommandInfo(cp.Actor.Process.Id, Context.Process.Commands.StartProcess),
                expectedSourceType: new SourceType(typeof(IComplexEventService))) },
            {"StartProcessWithValidatedUser",  new ProcessAction(
                       action: async cp =>  await Task.Run(() => new StartSystemProcess(NullProcess,//HACK: to keep this generic, the process that was used to create action will be used
                           new StateCommandInfo(cp.Actor.Process.Id, Context.Process.Commands.StartProcess), cp.Actor.Process, cp.Actor.Source)),
                       processInfo: cp => new StateCommandInfo(cp.Actor.Process.Id, Context.Process.Commands.StartProcess),
                       expectedSourceType: new SourceType(typeof(IComplexEventService)))},
            {"CompleteProcess",  new ProcessAction(
                action: async cp => await Task.Run(() => new SystemProcessCompleted(new StateEventInfo(cp.Actor.Process.Id, Context.Process.Events.ProcessCompleted),cp.Actor.Process, cp.Actor.Source)),
                processInfo: cp => new StateCommandInfo(cp.Actor.Process.Id, Context.Process.Commands.CompleteProcess),
                expectedSourceType: new SourceType(typeof(IComplexEventService)))},

            {"CleanUpProcess",   new ProcessAction(
                action: async cp =>  await Task.Run(() => new CleanUpSystemProcess(cp.Actor.Process.Id,new StateCommandInfo(cp.Actor.Process.Id, Context.Process.Commands.CleanUpProcess), cp.Actor.Process, cp.Actor.Source)),
                processInfo: cp => new StateCommandInfo(cp.Actor.Process.Id, Context.Process.Commands.CleanUpProcess),
                expectedSourceType: new SourceType(typeof(IComplexEventService)))},

            {"CleanUpParentProcess",   new ProcessAction(
                action: async cp =>  await Task.Run(() => new CleanUpSystemProcess(cp.Actor.Process.ParentProcessId,new StateCommandInfo(cp.Actor.Process.ParentProcessId, Context.Process.Commands.CleanUpProcess), cp.Actor.Process, cp.Actor.Source)),
                processInfo: cp => new StateCommandInfo(cp.Actor.Process.Id -1, Context.Process.Commands.CleanUpProcess),
                expectedSourceType: new SourceType(typeof(IComplexEventService)))},

        };
        
        


        public static IProcessAction DisplayError => new ProcessAction(
                        action: async cp =>
                        {
                            MessageBox.Show(cp.Messages["ProcessEventError"].Exception.Message + "-----" +
                                            cp.Messages["ProcessEventError"].Exception.StackTrace);
                            
                            return await Task.Run(() => new FailedMessageData(cp, new StateEventInfo(cp.Actor.Process.Id,Context.Process.Events.Error), cp.Actor.Process,cp.Actor.Source));
                        }, 
                        processInfo: cp => new StateCommandInfo(cp.Actor.Process.Id, Context.Process.Commands.Error),
                        expectedSourceType: new SourceType(typeof(IComplexEventService)));
        public static IProcessAction ShutDownApplication => new ProcessAction(
                        action: cp =>
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                    MessageBox.Show(cp.Messages["ProcessEventError"].Exception.Message + "-----" +
                                            cp.Messages["ProcessEventError"].Exception.StackTrace);
                                        Application.Current?.Shutdown();
                            
                            });
                            return null;
                        },
                        processInfo: cp => new StateCommandInfo(cp.Actor.Process.Id, Context.Process.Commands.Error),
                        expectedSourceType: new SourceType(typeof(IComplexEventService)));

        public static IProcessAction IntializeProcessState(IDynamicEntityType entityType)
        {
            return new ProcessAction(
                action: async cp =>
                         await Task.Run(() => new LoadEntitySet(entityType,
                            new StateCommandInfo(cp.Actor.Process.Id,
                                Context.Entity.Commands.LoadEntitySetWithChanges),
                            cp.Actor.Process, cp.Actor.Source)),
                processInfo:
                    cp =>
                        new StateCommandInfo(cp.Actor.Process.Id,
                            Context.Entity.Commands.LoadEntitySetWithChanges),
                // take shortcut cud be IntialState
                expectedSourceType: new SourceType(typeof (IComplexEventService)));
        }

        public static IProcessAction UpdateEntityViewState(IDynamicEntityType entityType) 
        {
            return new ProcessAction(
                action: async cp =>
                    {
                        var ps = new ProcessStateEntity(
                             process: cp.Actor.Process,
                             entity: cp.Messages["Entity"].Entity,
                             info: new StateInfo(cp.Actor.Process.Id, new State($"Loaded {cp.Messages["Entity"].Entity.EntityType} Data", $"Loaded{cp.Messages["Entity"].Entity.EntityType}", "")));
                        return await Task.Run(() => new UpdateProcessStateEntity(
                                    state: ps,
                                    process: cp.Actor.Process,
                                    processInfo: new StateCommandInfo(cp.Actor.Process.Id, Context.Process.Commands.UpdateState),
                                    source: cp.Actor.Source));
                    },
                processInfo:
                    cp =>
                        new StateCommandInfo(cp.Actor.Process.Id,
                            Context.Process.Commands.UpdateState),
                // take shortcut cud be IntialState
                expectedSourceType: new SourceType(typeof(IComplexEventService)));
        }

        public static IProcessAction UpdateEntityViewStateList(IDynamicEntityType entityType)
        {
            return new ProcessAction(
                action: async cp =>
                    {
                        //v.EntitySet.Value.Add(entityType.DefaultEntity());
                        cp.Messages["EntityViewSet"].EntitySet.Add(entityType.DefaultEntity());
                        var ps = new ProcessStateList(
                             process: cp.Actor.Process,
                             entity: ((List<IDynamicEntity>)cp.Messages["EntityViewSet"].EntitySet).FirstOrDefault(),
                             entitySet: cp.Messages["EntityViewSet"].EntitySet,
                             selectedEntities: new List<IDynamicEntity>(),
                             stateInfo: new StateInfo(cp.Actor.Process.Id, new State($"Loaded {entityType} Data", $"Loaded{entityType}", "")));
                        return await Task.Run(() => new UpdateProcessStateList(
                                    entityType: entityType,
                                    state: ps,
                                    process: cp.Actor.Process,
                                    processInfo: new StateCommandInfo(cp.Actor.Process.Id, Context.Process.Commands.UpdateState),
                                    source: cp.Actor.Source));
                    },
                processInfo:
                    cp =>
                        new StateCommandInfo(cp.Actor.Process.Id,
                            Context.Process.Commands.UpdateState),
                // take shortcut cud be IntialState
                expectedSourceType: new SourceType(typeof(IComplexEventService)));
        }

        public static IProcessAction RequestState(IDynamicEntityType entityType,string property)
        {
            return new ProcessAction(
                action: async cp =>
                {

                    var key = property;
                    var value = cp.Messages["CurrentEntity"].Entity.Properties["Id"];
                    var changes = new Dictionary<string, dynamic>() { { key, value } };
                    return await Task.Run(() => new GetEntityWithChanges(entityType.DefaultEntity(),changes,
                         new StateCommandInfo(cp.Actor.Process.Id, Context.Entity.Commands.GetEntity),
                         cp.Actor.Process, cp.Actor.Source));
                },
                processInfo: cp =>
                    new StateCommandInfo(cp.Actor.Process.Id,
                        Context.Entity.Commands.GetEntity),
                // take shortcut cud be IntialState
                expectedSourceType: new SourceType(typeof(IComplexEventService))

                );
        }

        
        public static IProcessAction RequestStateList(IDynamicEntityType entityType, string currentProperty, string viewProperty) 
        {
            return new ProcessAction(
                action: async cp =>
                {

                    var key = viewProperty;
                    
                    var value = cp.Messages["CurrentEntity"].Entity[currentProperty];
                    var changes = new Dictionary<string, dynamic>() { {key,value} };
                    return await Task.Run(() => new GetEntitySetWithChanges("ExactMatch", entityType,changes,
                        new StateCommandInfo(cp.Actor.Process.Id, Context.Entity.Commands.GetEntity),
                        cp.Actor.Process, cp.Actor.Source));
                },
                processInfo: cp =>
                    new StateCommandInfo(cp.Actor.Process.Id,
                        Context.Entity.Commands.GetEntity),
                // take shortcut cud be IntialState
                expectedSourceType: new SourceType(typeof (IComplexEventService))

                );
        }

        public static IProcessAction RequestCompositStateList(IDynamicEntityType entityType, Dictionary<string, dynamic> changes, List<ViewModelEntity> entities)
        {
            return new ProcessAction(
                action: async cp =>
                {
                    
                        var centity = cp.Messages.FirstOrDefault(x => x.Key.Contains("CurrentEntity")).Value?.Entity;
                        var entitytype = centity.EntityType;
                        var ve = entities.FirstOrDefault(x => x.EntityType == entitytype);
                        var key = ve.Property;
                        var value = centity.GetType().GetProperty("Id").GetValue(centity);
                        if (changes.ContainsKey(key))
                        {
                            changes[key] = value;
                        }
                        else
                        {
                            changes.Add(key, value);
                        }
                    
                    
                    return await Task.Run(() => new GetEntitySetWithChanges("ExactMatch",entityType,changes,
                        new StateCommandInfo(cp.Actor.Process.Id, Context.Entity.Commands.GetEntity),
                        cp.Actor.Process, cp.Actor.Source));
                },
                processInfo: cp =>
                    new StateCommandInfo(cp.Actor.Process.Id,
                        Context.Entity.Commands.GetEntity),
                // take shortcut cud be IntialState
                expectedSourceType: new SourceType(typeof(IComplexEventService))

            );
        }

    //    public class SignIn
    //    {
    //        public static IProcessAction IntializeSigninProcessState => new ProcessAction(
    //            action: async cp =>
    //            {
    //                var ps = new ProcessStateEntity(
    //                    process: cp.Actor.Process,
    //                    //Todo: get rid of this type name
    //                    entity: new DynamicEntity(DynamicEntityType.DynamicEntityTypes["SignInInfo"],-1, new Dictionary<string, object>(){}),
    //                    info: new StateInfo(cp.Actor.Process.Id,
    //                        new State(name: "AwaitUserName", status: "Waiting for User Name",
    //                            notes:
    //                                "Please Enter your User Name. If this is your First Time Login In please Contact the Receptionist for your user info.")));
    //                return await Task.Run(() => new UpdateProcessStateEntity(ps,
    //                    new StateCommandInfo(cp.Actor.Process.Id, Context.Process.Commands.UpdateState),
    //                    cp.Actor.Process, cp.Actor.Source));

    //            },
    //            processInfo: cp => new StateCommandInfo(cp.Actor.Process.Id, Context.Process.Commands.CreateState),
    //            // take shortcut cud be IntialState
    //            expectedSourceType: new SourceType(typeof (IComplexEventService)));

    //        public static IProcessAction UserNameFound => new ProcessAction(
    //            action: async cp =>
    //            {
    //                var ps = new ProcessStateEntity(cp.Actor.Process, cp.Messages["UserNameFound"].Entity,
    //                    new StateInfo(cp.Actor.Process.Id, "WelcomeUser",
    //                        $"Welcome {cp.Messages["UserNameFound"].Entity.Usersignin}", "Please Enter your Password"));
    //                return await Task.Run(() => new UpdateProcessStateEntity(ps,
    //                    new StateCommandInfo(cp.Actor.Process.Id, Context.Process.Commands.UpdateState),
    //                    cp.Actor.Process, cp.Actor.Source));
    //            },
    //            processInfo: cp => new StateCommandInfo(cp.Actor.Process.Id, Context.Process.Commands.UpdateState),
    //            expectedSourceType: new SourceType(typeof (IComplexEventService))
    //            );

    //        public static IProcessAction SetProcessStatetoValidatedUser => new ProcessAction(
    //            action: async cp =>
    //            {
    //                var ps = new ProcessStateEntity(cp.Actor.Process, cp.Messages["ValidatedUser"].Entity,
    //                    new StateInfo(cp.Actor.Process.Id, "UserValidated",
    //                        $"User: {cp.Messages["ValidatedUser"].Entity.Usersignin} Validated", "User Validated"));
    //                return await Task.Run(() => new UpdateProcessStateEntity(ps,
    //                    new StateCommandInfo(cp.Actor.Process.Id, Context.Process.Commands.UpdateState),
    //                    cp.Actor.Process, cp.Actor.Source));
    //            },
    //            processInfo: cp => new StateCommandInfo(cp.Actor.Process.Id, Context.Process.Commands.UpdateState),
    //            expectedSourceType: new SourceType(typeof (IComplexEventService)));


    //        public static IProcessAction UserValidated => new ProcessAction(
    //            action: async cp =>
    //                     await Task.Run(() => new UserValidated(cp.Messages["ValidatedUser"].Entity,
    //                        new StateEventInfo(cp.Actor.Process.Id, Context.Domain.Events.DomainEventPublished),
    //                        cp.Actor.Process, cp.Actor.Source)),
    //            processInfo: cp => new StateCommandInfo(cp.Actor.Process.Id, Context.Domain.Commands.PublishDomainEvent),
    //            expectedSourceType: new SourceType(typeof (IComplexEventService))
    //            );


    //    }

    //}

    //public partial class EntityComplexActions
    //{
    //    public static ComplexEventAction GetComplexAction(string method,  object[] args)
    //    {
    //        return (ComplexEventAction)typeof(EntityComplexActions).GetMethod(method).Invoke(null, args);
    //    }
    //    public static ComplexEventAction IntializeCache(int processId, IDynamicEntityType entityType) 
    //    {
    //        return new ComplexEventAction(
    //            key: $"{entityType}EntityCache-1",
    //            processId: processId,
    //            events: new List<IProcessExpectedEvent>
    //            {
    //                new ProcessExpectedEvent(key: "ProcessStarted",
    //                    processId: processId,
    //                    eventPredicate: e => e != null,
    //                    eventType: typeof (ISystemProcessStarted),
    //                    processInfo: new StateEventInfo(processId, Context.Process.Events.ProcessStarted),
    //                    expectedSourceType: new SourceType(typeof (IComplexEventService)))

    //            },
    //            expectedMessageType: typeof(IProcessStateMessage),
    //            action: new ProcessAction(
    //                action: async cp =>
    //                    await Task.Run(() => new LoadEntitySet(entityType,
    //                        new StateCommandInfo(3, Context.Entity.Commands.LoadEntitySetWithChanges),
    //                        cp.Actor.Process, cp.Actor.Source)),
    //                processInfo:
    //                cp =>
    //                    new StateCommandInfo(cp.Actor.Process.Id,
    //                        Context.Entity.Commands.LoadEntitySetWithChanges),
    //                // take shortcut cud be IntialState
    //                expectedSourceType: new SourceType(typeof(IComplexEventService))),
    //            processInfo: new StateCommandInfo(processId, Context.Process.Commands.CreateState));
    //    }

        
        

    }

   
}
