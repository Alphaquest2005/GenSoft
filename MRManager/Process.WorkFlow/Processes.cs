using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using SystemInterfaces;
using Actor.Interfaces;
using Domain.Interfaces;
using EventMessages.Commands;
using EventMessages.Events;
using Interfaces;
using RevolutionData;
using RevolutionData.Context;
using RevolutionEntities.Process;
using RevolutionEntities.ViewModels;
using Utilities;
using ViewModel.Interfaces;

namespace Process.WorkFlow
{

   

    public static class Processes
    {
        public static readonly List<IProcessInfo> ProcessInfos = new List<IProcessInfo>
        {
            //new Process(0,0, "Uknown Process", "Unknown Process", "Unknown"),
            new ProcessInfo(1, 0, "Starting System", "Prepare system for Intial Use", "Start","System"),
            new ProcessInfo(2, 1, "User SignOn", "User Login", "User","System"),
            new ProcessInfo(3, 2, "Load User Screen", "User Screen", "UserScreen", "joe")
        };




        public static List<IComplexEventAction> ProcessComplexEvents = new List<IComplexEventAction>
        {

             new ComplexEventAction(
                "100",
                1, new List<IProcessExpectedEvent>
                {
                    new ProcessExpectedEvent ("ServiceManagerStarted", 1, typeof (IServiceStarted<IServiceManager>), e => e != null, new StateEventInfo(1, RevolutionData.Context.Actor.Events.ActorStarted), new SourceType(typeof(IServiceManager))),
                    
                },
                typeof(ISystemProcessStarted),
                processInfo:new StateCommandInfo(1,RevolutionData.Context.Process.Commands.StartProcess ),
                action: ProcessActions.Actions["ProcessStarted"]),

             

            new ComplexEventAction(
                "102",
                1, new List<IProcessExpectedEvent>
                {
                    new ProcessExpectedEvent ("ProcessStarted", 1, typeof (ISystemProcessStarted), e => e != null, new StateEventInfo(1, RevolutionData.Context.Process.Events.ProcessStarted), new SourceType(typeof(IProcessService))),
                    new ProcessExpectedEvent ("ViewCreated", 1, typeof (IViewModelCreated<IScreenModel>), e => e != null, new StateEventInfo(1,"ScreenViewCreated", "ScreenView Created","This view contains all views", RevolutionData.Context.ViewModel.Commands.CreateViewModel), new SourceType(typeof(IViewModelService) )),
                    new ProcessExpectedEvent ("ViewLoaded", 1, typeof (IViewModelLoaded<IMainWindowViewModel,IScreenModel>), e => e != null, new StateEventInfo(1,"ScreenViewLoaded","ScreenView Model loaded in MainWindowViewModel","Only ViewModel in Body", RevolutionData.Context.ViewModel.Commands.LoadViewModel), new SourceType(typeof(IViewModelService) ))
                },
                typeof(ISystemProcessCompleted),
                processInfo:new StateCommandInfo(1,RevolutionData.Context.Process.Commands.CompleteProcess ),
                action: ProcessActions.Actions["CompleteProcess"]),

           new ComplexEventAction(
                "103",
                1, new List<IProcessExpectedEvent>
                {
                    new ProcessExpectedEvent ("ProcessCompleted", 1, typeof (ISystemProcessCompleted), e => e != null, new StateEventInfo(1, RevolutionData.Context.Process.Events.ProcessCompleted), new SourceType(typeof(IComplexEventService))),

                },
                typeof(ISystemProcessStarted),
                processInfo:new StateCommandInfo(1,RevolutionData.Context.Process.Commands.StartProcess),
                action: ProcessActions.Actions["StartProcess"]),

            new ComplexEventAction(
                "104",
                1, new List<IProcessExpectedEvent>
                {
                    new ProcessExpectedEvent ("ProcessCompleted", 1, typeof (ISystemProcessCompleted), e => e != null, new StateEventInfo(1, RevolutionData.Context.Process.Events.ProcessCompleted), new SourceType(typeof(IComplexEventService))),

                },
                typeof(ISystemProcessCleanedUp),
                processInfo:new StateCommandInfo(1,RevolutionData.Context.Process.Commands.CleanUpProcess ),
                action: ProcessActions.Actions["CleanUpProcess"]),

            
            //new ComplexEventAction(
            //    "106",
            //    1, new List<IProcessExpectedEvent>
            //    {
            //        new ProcessExpectedEvent ("ProcessEventError", 1, typeof (IProcessEventFailure), e => e != null, new StateEventInfo(1, Context.Process.Events.Error), new SourceType(typeof(IComplexEventService))),

            //    },
            //    typeof(IProcessEventFailure),
            //    processInfo:new StateCommandInfo(1,Context.Process.Commands.Error ),
            //    action: ProcessActions.ShutDownApplication),

            ComplexActions.StartProcess(2),
            new ComplexEventAction(
                
                key:"201",
                processId:2,
                events:new List<IProcessExpectedEvent>
                {
                    new ProcessExpectedEvent (key: "ProcessStarted",
                                              processId: 2,
                                              eventPredicate: e => e != null,
                                              eventType: typeof (ISystemProcessStarted),
                                              processInfo: new StateEventInfo(2,RevolutionData.Context.Process.Events.ProcessStarted),
                                              expectedSourceType:new SourceType(typeof(IComplexEventService)))
                    
                },
                expectedMessageType:typeof(IProcessStateMessage<ISignInInfo>),
                action:ProcessActions.SignIn.IntializeSigninProcessState,
                processInfo:new StateCommandInfo(2, RevolutionData.Context.Process.Commands.CreateState)),
            new ComplexEventAction(
                key:"202",
                processId:2,
                events:new List<IProcessExpectedEvent>
                {
                    new ProcessExpectedEvent<IEntityViewWithChangesFound<ISignInInfo>> (
                        "UserNameFound", 2, e => e.Entity != null && e.Changes.Count == 1 && e.Changes.ContainsKey(nameof(ISignInInfo.Usersignin)), expectedSourceType: new SourceType(typeof(IEntityViewRepository)), processInfo: new StateEventInfo(2, RevolutionData.Context.User.Events.UserNameFound))
                },
                expectedMessageType:typeof(IProcessStateMessage<ISignInInfo>),
                action: ProcessActions.SignIn.UserNameFound,
                processInfo: new StateCommandInfo(2, RevolutionData.Context.Process.Commands.UpdateState)),
            new ComplexEventAction(
                key:"203",
                processId: 2,
                events: new List<IProcessExpectedEvent>
                {
                    new ProcessExpectedEvent<IEntityViewWithChangesFound<ISignInInfo>> (processId: 2,
                                                        eventPredicate: e => e.Entity != null && e.Changes.Count == 2 && e.Changes.ContainsKey(nameof(ISignInInfo.Password)),
                                                        processInfo: new StateEventInfo(2, RevolutionData.Context.User.Events.UserFound),
                                                        expectedSourceType: new SourceType(typeof(IEntityViewRepository)),
                                                        key: "ValidatedUser")
                },
                expectedMessageType: typeof(IProcessStateMessage<ISignInInfo>),
                action: ProcessActions.SignIn.SetProcessStatetoValidatedUser,
                processInfo: new StateCommandInfo(2, RevolutionData.Context.Process.Commands.UpdateState)),
            new ComplexEventAction(
                key:"204",
                processId: 2,
                events: new List<IProcessExpectedEvent>
                {
                    new ProcessExpectedEvent<IEntityViewWithChangesFound<ISignInInfo>> (processId: 2,
                                                        eventPredicate: e => e.Entity != null && e.Changes.Count == 2 && e.Changes.ContainsKey(nameof(ISignInInfo.Password)),
                                                        processInfo: new StateEventInfo(2, RevolutionData.Context.User.Events.UserFound),
                                                        expectedSourceType: new SourceType(typeof(IEntityViewRepository)),
                                                        key: "ValidatedUser")
                },
                expectedMessageType:typeof(IUserValidated),
                processInfo:new StateCommandInfo(2, RevolutionData.Context.Domain.Commands.PublishDomainEvent),
                action: ProcessActions.SignIn.UserValidated),

            new ComplexEventAction(
                "205",
                2, new List<IProcessExpectedEvent>
                {
                    new ProcessExpectedEvent ("ValidatedUser", 2, typeof (IUserValidated), e => e != null, new StateEventInfo(2, RevolutionData.Context.User.Events.UserFound), new SourceType(typeof(IComplexEventService))),
                    
                },
                typeof(ISystemProcessCompleted),
                processInfo:new StateCommandInfo(2,RevolutionData.Context.Process.Commands.CompleteProcess ),
                action: ProcessActions.Actions["CompleteProcess"]),

             new ComplexEventAction(
                "206",
                2, new List<IProcessExpectedEvent>
                {
                    new ProcessExpectedEvent ("ValidatedUser", 2, typeof (IUserValidated), e => e != null, new StateEventInfo(2, RevolutionData.Context.User.Events.UserFound), new SourceType(typeof(IComplexEventService))),

                },
                typeof(ISystemProcessStarted),
                processInfo:new StateCommandInfo(2,RevolutionData.Context.Process.Commands.StartProcess ),
                action: ProcessActions.Actions["StartProcessWithValidatedUser"]),

            new ComplexEventAction(
                "207",
                2, new List<IProcessExpectedEvent>
                {
                    new ProcessExpectedEvent ("ProcessCompleted", 2, typeof (ISystemProcessCompleted), e => e != null, new StateEventInfo(2, RevolutionData.Context.Process.Events.ProcessCompleted), new SourceType(typeof(IComplexEventService))),

                },
                typeof(ISystemProcessCleanedUp),
                processInfo:new StateCommandInfo(2,RevolutionData.Context.Process.Commands.CleanUpProcess ),
                action: ProcessActions.Actions["CleanUpProcess"]),

            
            ComplexActions.GetComplexAction("StartProcess",new Type[0], new object[]{3}),



            ComplexActions.GetComplexAction("IntializePulledProcessState",new[]{typeof(IPatientInfo)},  new object[]{3, "Patient",}),


            ComplexActions.RequestCompositeStateList<IResponseOptionInfo>(3, new Dictionary<string, dynamic>(), new List<ViewModelEntity>()
            {
                new ViewModelEntity(typeof(IQuestionInfo), "Question","QuestionId"),
                new ViewModelEntity(typeof(IPatientSyntomInfo), "PatientSyntom","PatientSyntomId"),
            })

            //ComplexActions.GetComplexAction("UpdateStateList",new[]{typeof(IPatientInfo)},  new object[]{3}),
            
            //ComplexActions.GetComplexAction("RequestPulledState",new[]{typeof(IPatientInfo), typeof(IPatientDetailsInfo) },  new object[]{3,"Patient",}),

            
            //ComplexActions.GetComplexAction("UpdateState",new[]{typeof(IPatientDetailsInfo) },  new object[]{3}),
            //ComplexActions.GetComplexAction("UpdateStateWhenDataChanges",new[]{typeof(IPatientInfo), typeof(IPatientDetailsInfo) },  new object[]{3,  (Expression<Func<IPatientInfo, object>>)(c => c.Id) , (Expression<Func<IPatientDetailsInfo, object>>)(v => v.Id)}),


           // ComplexActions.GetComplexAction("RequestState",new[]{typeof(IPatientInfo), typeof(IPatientAddressesInfo) },  new object[]{3, (Expression<Func<IPatientAddressesInfo, object>>)(x => x.Id)}),
           // ComplexActions.GetComplexAction("UpdateState",new[]{typeof(IPatientAddressesInfo) },  new object[]{3}),


          //  ComplexActions.GetComplexAction("RequestState",new[]{typeof(IPatientInfo), typeof(IPatientPhoneNumbersInfo) },  new object[]{3, (Expression<Func<IPatientPhoneNumbersInfo, object>>)(x => x.Id)}),
          //  ComplexActions.GetComplexAction("UpdateState",new[]{typeof(IPatientPhoneNumbersInfo) },  new object[]{3}),

         //   ComplexActions.GetComplexAction("RequestState",new[]{typeof(IPatientInfo), typeof(IPatientNextOfKinsInfo) },  new object[]{3, (Expression<Func<IPatientNextOfKinsInfo, object>>)(x => x.Id)}),
          //  ComplexActions.GetComplexAction("UpdateState",new[]{typeof(IPatientNextOfKinsInfo) },  new object[]{3}),

            
         //   ComplexActions.GetComplexAction("RequestPulledState",new[]{typeof(IPatientInfo), typeof(INonResidentInfo) },  new object[]{3, "NonResident", }),
         //   ComplexActions.GetComplexAction("UpdateState",new[]{typeof(INonResidentInfo) },  new object[]{3}),

            
         //   ComplexActions.GetComplexAction("RequestPulledState",new[]{typeof(IPatientInfo), typeof(IPatientVitalsInfo) },  new object[]{3, "Vitals", }),
         //   ComplexActions.GetComplexAction("UpdateState",new[]{typeof(IPatientVitalsInfo) },  new object[]{3}),
         //   ComplexActions.GetComplexAction("UpdateStateWhenDataChanges",new[]{typeof(IPatientInfo), typeof(IPatientVitalsInfo) },  new object[]{3,  (Expression<Func<IPatientInfo, object>>)(c => c.Id) , (Expression<Func<IPatientVitalsInfo, object>>)(v => v.Id)}),



            //ComplexActions.GetComplexAction("RequestStateList",new[]{typeof(IPatientInfo), typeof(IPatientVisitInfo) },  new object[]{3,  (Expression<Func<IPatientInfo, object>>)(c => c.Id) , (Expression<Func<IPatientVisitInfo, object>>)(v => v.PatientId) }),
          // ComplexActions.GetComplexAction("UpdateStateList",new[]{typeof(IPatientVisitInfo) },  new object[]{3}),
         //   ComplexActions.GetComplexAction("UpdateStateWhenDataChanges",new[]{typeof(IPatientInfo), typeof(IPatientVisitInfo) },  new object[]{3,  (Expression<Func<IPatientInfo, object>>)(c => c.Id) , (Expression<Func<IPatientVisitInfo, object>>)(v => v.Id)}),


          //  ComplexActions.GetComplexAction("RequestStateList",new[]{typeof(IPatientVisitInfo), typeof(IPatientSyntomInfo) },  new object[]{3,  (Expression<Func<IPatientVisitInfo, object>>)(c => c.Id) , (Expression<Func<IPatientSyntomInfo, object>>)(v => v.PatientVisitId) }),
         //   ComplexActions.GetComplexAction("RequestStateList",new[]{typeof(ISyntoms), typeof(IPatientSyntomInfo) },  new object[]{3,  (Expression<Func<ISyntoms, object>>)(c => c.Id) , (Expression<Func<IPatientSyntomInfo, object>>)(v => v.SyntomId) }),
          //  ComplexActions.GetComplexAction("UpdateStateList",new[]{typeof(IPatientSyntomInfo) },  new object[]{3}),
          //  ComplexActions.GetComplexAction("UpdateStateWhenDataChanges",new[]{typeof(IPatientSyntoms), typeof(IPatientSyntomInfo) },  new object[]{3,  (Expression<Func<IPatientSyntoms, object>>)(c => c.Id) , (Expression<Func<IPatientSyntomInfo, object>>)(v => v.Id)}),
         //   ComplexActions.GetComplexAction("RequestStateList",new[]{typeof(IPatientSyntomInfo), typeof(ISyntomMedicalSystemInfo) },  new object[]{3,  (Expression<Func<IPatientSyntomInfo, object>>)(c => c.SyntomId) , (Expression<Func<ISyntomMedicalSystemInfo, object>>)(v => v.SyntomId) }),


         //   ComplexActions.GetComplexAction("UpdateStateList",new[]{typeof(ISyntomMedicalSystemInfo) },  new object[]{3}),
         //   ComplexActions.GetComplexAction("UpdateStateWhenDataChanges",new[]{typeof(ISyntomMedicalSystems), typeof(ISyntomMedicalSystemInfo) },  new object[]{3,  (Expression<Func<ISyntomMedicalSystems, object>>)(c => c.Id) , (Expression<Func<ISyntomMedicalSystemInfo, object>>)(v => v.Id)}),


         //   ComplexActions.GetComplexAction("UpdateStateWhenDataChanges",new[]{typeof(IInterviews), typeof(IInterviewInfo) },  new object[]{3,  (Expression<Func<IInterviews, object>>)(c => c.Id) , (Expression<Func<IInterviewInfo, object>>)(v => v.Id)}),


         //   ComplexActions.GetComplexAction("RequestStateList",new[]{typeof(IInterviewInfo), typeof(IQuestionResponseOptionInfo) },  new object[]{3,  (Expression<Func<IInterviewInfo, object>>)(c => c.Id) , (Expression<Func<IQuestionResponseOptionInfo, object>>)(v => v.InterviewId) }),
          //  ComplexActions.GetComplexAction("UpdateStateList",new[]{typeof(IQuestionResponseOptionInfo) },  new object[]{3}),

         //   ComplexActions.GetComplexAction("UpdateStateWhenDataChanges",new[]{typeof(IQuestionInfo), typeof(IQuestionResponseOptionInfo) },  new object[]{3,  (Expression<Func<IQuestionInfo, object>>)(c => c.Id) , (Expression<Func<IQuestionResponseOptionInfo, object>>)(v => v.Id)}),
         //   ComplexActions.GetComplexAction("UpdateStateWhenDataChanges",new[]{typeof(IResponseInfo), typeof(IQuestionResponseOptionInfo) },  new object[]{3,  (Expression<Func<IResponseInfo, object>>)(c => c.Id) , (Expression<Func<IQuestionResponseOptionInfo, object>>)(v => v.Id)}),
         //   ComplexActions.GetComplexAction("UpdateStateWhenDataChanges",new[]{typeof(IResponseOptions), typeof(IQuestionResponseOptionInfo) },  new object[]{3,  (Expression<Func<IResponseOptions, object>>)(c => c.Id) , (Expression<Func<IQuestionResponseOptionInfo, object>>)(v => v.Id)}),
          //  ComplexActions.GetComplexAction("UpdateStateList",new[]{typeof(IQuestionInfo) },  new object[]{3}),
         //   ComplexActions.GetComplexAction("UpdateStateWhenDataChanges",new[]{typeof(IQuestions), typeof(IQuestionInfo) },  new object[]{3,  (Expression<Func<IQuestions, object>>)(c => c.Id) , (Expression<Func<IQuestionInfo, object>>)(v => v.Id)}),

         //   ComplexActions.GetComplexAction("RequestStateList",new[]{typeof(IInterviewInfo), typeof(IQuestionInfo) },  new object[]{3,  (Expression<Func<IInterviewInfo, object>>)(c => c.Id) , (Expression<Func<IQuestionInfo, object>>)(v => v.InterviewId) }),

            


            //EntityComplexActions.GetComplexAction("IntializeCache",new[]{typeof(ISyntomPriority) },  new object[]{3} ),
            //EntityComplexActions.GetComplexAction("IntializeCache",new[]{typeof(ISyntomStatus) },  new object[]{3} ),
            //EntityComplexActions.GetComplexAction("IntializeCache",new[]{typeof(IVisitType) },  new object[]{3} ),
            //EntityComplexActions.GetComplexAction("IntializeCache",new[]{typeof(IPhase) },  new object[]{3} ),
            //EntityComplexActions.GetComplexAction("IntializeCache",new[]{typeof(IMedicalCategory) },  new object[]{3} ),
            //EntityComplexActions.GetComplexAction("IntializeCache",new[]{typeof(IMedicalSystems) },  new object[]{3} ),
            //EntityComplexActions.GetComplexAction("IntializeCache",new[]{typeof(IQuestionResponseTypes) },  new object[]{3} ),
            //EntityComplexActions.GetComplexAction("IntializeCache",new[]{typeof(ISex) },  new object[]{3} ),
            

            //EntityViewComplexActions.GetComplexAction("IntializeCache",new[]{typeof(IDoctorInfo) },  new object[]{3} ),
        };


        public static class ComplexActions
        {
            public static ComplexEventAction StartProcess(int processId)
            {
                return new ComplexEventAction(
                    $"StartProcess-{processId}",
                    processId, new List<IProcessExpectedEvent>
                    {
                        new ProcessExpectedEvent("ProcessServiceStarted", processId, typeof(IServiceStarted<IProcessService>),
                            e => e != null, new StateEventInfo(processId, RevolutionData.Context.Actor.Events.ActorStarted),
                            new SourceType(typeof(IProcessService))),

                    },
                    typeof(ISystemProcessStarted),
                    processInfo: new StateCommandInfo(processId, RevolutionData.Context.Process.Commands.StartProcess),
                    action: ProcessActions.Actions["ProcessStarted"]);
            }

            //public static ComplexEventAction IntializePulledProcessState(int processId, string entityName,Type type)
            //{
            //    return (ComplexEventAction)typeof(ComplexActions).GetMethod("IntializePulledProcessState").MakeGenericMethod(type).Invoke(null, new object[] {processId, entityName});
            //}
            public static ComplexEventAction IntializePulledProcessState<TEntityView>(int processId, string entityName) where TEntityView : IEntityView
            {
                return new ComplexEventAction(

                    key: $"InitalizeProcessState-{typeof(TEntityView).GetFriendlyName()}",
                    processId: processId,
                    events: new List<IProcessExpectedEvent>
                    {
                        new ProcessExpectedEvent(key: "ProcessStarted",
                            processId: processId,
                            eventPredicate: e => e != null,
                            eventType: typeof (ISystemProcessStarted),
                            processInfo: new StateEventInfo(processId, RevolutionData.Context.Process.Events.ProcessStarted),
                            expectedSourceType: new SourceType(typeof (IComplexEventService))),
                        
                    },
                    expectedMessageType: typeof (IProcessStateMessage<TEntityView>),
                    action: ProcessActions.IntializePulledProcessState<TEntityView>(entityName),
                    processInfo: new StateCommandInfo(processId, RevolutionData.Context.Process.Commands.CreateState));
            }


            public static ComplexEventAction UpdateState<TEntityView>(int processId) where TEntityView : IEntityView
            {
                return new ComplexEventAction(
                    key: $"UpdateState-{typeof(TEntityView).GetFriendlyName()}",
                    processId: processId,
                    actionTrigger: ActionTrigger.Any, 
                    events: new List<IProcessExpectedEvent>
                    {
                                new ProcessExpectedEvent<IEntityViewWithChangesUpdated<TEntityView>>(processId: processId,
                            eventPredicate: e => e.Entity != null,
                            processInfo: new StateEventInfo(processId, Entity.Events.EntityUpdated),
                            expectedSourceType: new SourceType(typeof(IEntityRepository)),
                            key: "EntityView"),
                                   new ProcessExpectedEvent<IEntityViewWithChangesFound<TEntityView>>(processId: processId,
                            eventPredicate: e => e.Entity != null,
                            processInfo: new StateEventInfo(processId, Entity.Events.EntityFound),
                            expectedSourceType: new SourceType(typeof(IEntityRepository)),
                            key: "EntityView"),
                                   new ProcessExpectedEvent<IEntityFound<TEntityView>>(processId: processId,
                            eventPredicate: e => e.Entity != null,
                            processInfo: new StateEventInfo(processId, Entity.Events.EntityFound),
                            expectedSourceType: new SourceType(typeof(IEntityRepository)),
                            key: "EntityView")
                    },
                    expectedMessageType: typeof(IProcessStateMessage<TEntityView>),
                    action: ProcessActions.UpdateEntityViewState<TEntityView>(),
                    processInfo: new StateCommandInfo(processId, RevolutionData.Context.Process.Commands.UpdateState));
            }

            
            public static ComplexEventAction RequestState<TCurrentEntity, TEntityView>(int processId, Expression<Func<TEntityView, dynamic>> property) where TEntityView : IEntityView where TCurrentEntity : IEntityId
            {
                return new ComplexEventAction(
                    key: $"RequestState-{typeof(TCurrentEntity).GetFriendlyName()}-{typeof(TEntityView).GetFriendlyName()}-{property.GetMemberName()}",
                    processId: processId,
                    actionTrigger: ActionTrigger.Any, 
                    events: new List<IProcessExpectedEvent>
                    {
                        new ProcessExpectedEvent<ICurrentEntityChanged<TCurrentEntity>>(
                            "CurrentEntity", processId, e => e.Entity != null,
                            expectedSourceType: new SourceType(typeof (IViewModel)),
                            //todo: check this cuz it comes from viewmodel
                            processInfo: new StateEventInfo(processId, RevolutionData.Context.Process.Events.CurrentEntityChanged)),

                        new ProcessExpectedEvent<IEntityFound<TCurrentEntity>>(
                            "CurrentEntity", processId, e => e.Entity != null,
                            expectedSourceType: new SourceType(typeof (IViewModel)),
                            //todo: check this cuz it comes from viewmodel
                            processInfo: new StateEventInfo(processId, Entity.Events.EntityFound)),
                        new ProcessExpectedEvent<IEntityUpdated<TCurrentEntity>>(
                            "CurrentEntity", processId, e => e.Entity != null,
                            expectedSourceType: new SourceType(typeof (IViewModel)),
                            //todo: check this cuz it comes from viewmodel
                            processInfo: new StateEventInfo(processId, Entity.Events.EntityUpdated)),
                        new ProcessExpectedEvent<IEntityViewWithChangesFound<TCurrentEntity>>(
                            "CurrentEntity", processId, e => e.Entity != null,
                            expectedSourceType: new SourceType(typeof (IViewModel)),
                            //todo: check this cuz it comes from viewmodel
                            processInfo: new StateEventInfo(processId, EntityView.Events.EntityViewFound))
                    },
                    expectedMessageType: typeof(IProcessStateMessage<TEntityView>),
                    action: ProcessActions.RequestState(property),
                    processInfo: new StateCommandInfo(processId, RevolutionData.Context.Process.Commands.UpdateState));
            }

            public static ComplexEventAction RequestPulledState<TCurrentEntity, TEntityView>(int processId, string entityName) where TEntityView : IEntityView where TCurrentEntity : IEntityId
            {
                return new ComplexEventAction(
                    key: $"RequestPullState-{typeof(TCurrentEntity).GetFriendlyName()}-{typeof(TEntityView).GetFriendlyName()}",
                    processId: processId,
                    actionTrigger: ActionTrigger.Any,
                    events: new List<IProcessExpectedEvent>
                    {
                        new ProcessExpectedEvent<ICurrentEntityChanged<TCurrentEntity>>(
                            "CurrentEntity", processId, e => e.Entity != null,
                            expectedSourceType: new SourceType(typeof (IViewModel)),
                            //todo: check this cuz it comes from viewmodel
                            processInfo: new StateEventInfo(processId, RevolutionData.Context.Process.Events.CurrentEntityChanged)),

                        new ProcessExpectedEvent<IEntityFound<TCurrentEntity>>(
                            "CurrentEntity", processId, e => e.Entity != null,
                            expectedSourceType: new SourceType(typeof (IViewModel)),
                            //todo: check this cuz it comes from viewmodel
                            processInfo: new StateEventInfo(processId, Entity.Events.EntityFound)),
                        new ProcessExpectedEvent<IEntityUpdated<TCurrentEntity>>(
                            "CurrentEntity", processId, e => e.Entity != null,
                            expectedSourceType: new SourceType(typeof (IViewModel)),
                            //todo: check this cuz it comes from viewmodel
                            processInfo: new StateEventInfo(processId, Entity.Events.EntityUpdated)),
                        new ProcessExpectedEvent<IEntityViewWithChangesFound<TCurrentEntity>>(
                            "CurrentEntity", processId, e => e.Entity != null,
                            expectedSourceType: new SourceType(typeof (IViewModel)),
                            //todo: check this cuz it comes from viewmodel
                            processInfo: new StateEventInfo(processId, EntityView.Events.EntityViewFound))
                    },
                    expectedMessageType: typeof(IProcessStateMessage<TEntityView>),
                    action: ProcessActions.RequestPulledState<TEntityView>(entityName),
                    processInfo: new StateCommandInfo(processId, RevolutionData.Context.Process.Commands.UpdateState));
            }



            public static ComplexEventAction GetComplexAction(string method, Type[] genericTypes, object[] args)
            {
                return genericTypes == null || !genericTypes.Any() 
                        ? (ComplexEventAction)typeof(ComplexActions).GetMethod(method).Invoke(null, args)
                        : (ComplexEventAction)typeof(ComplexActions).GetMethod(method).MakeGenericMethod(genericTypes).Invoke(null, args);
            }
            public static ComplexEventAction UpdateStateList<TEntityView>(int processId) where TEntityView : IEntityView
            {
                return new ComplexEventAction(
                    key: $"UpdateStateList-{typeof(TEntityView).GetFriendlyName()}",
                    processId: processId,
                    events: new List<IProcessExpectedEvent>
                    {
                            new ProcessExpectedEvent<IEntityViewSetWithChangesLoaded<TEntityView>>(
                        "EntityViewSet",processId, e => e.EntitySet != null, expectedSourceType: new SourceType(typeof(IEntityViewRepository)),
                        processInfo: new StateEventInfo(processId, EntityView.Events.EntityViewSetLoaded))
                    },
                    expectedMessageType: typeof(IProcessStateList<TEntityView>),
                    action: ProcessActions.UpdateEntityViewStateList<TEntityView>(),
                    processInfo: new StateCommandInfo(processId, RevolutionData.Context.Process.Commands.UpdateState));
            }
            public static ComplexEventAction RequestStateList<TCurrentEntity,TEntityView>(int processId, Expression<Func<TCurrentEntity, object>> currentProperty, Expression<Func<TEntityView, object>> viewProperty) where TEntityView : IEntityView where TCurrentEntity:IEntityId
            {
                return new ComplexEventAction(
                    key: $"RequestStateList-{typeof(TCurrentEntity).GetFriendlyName()}-{typeof(TEntityView).GetFriendlyName()}-{viewProperty.GetMemberName()}",
                    processId: processId,
                    actionTrigger: ActionTrigger.Any, 
                    events: new List<IProcessExpectedEvent>
                    {
                        new ProcessExpectedEvent<ICurrentEntityChanged<TCurrentEntity>>(
                            "CurrentEntity", processId, e => e.Entity != null,
                            expectedSourceType: new SourceType(typeof (IViewModel)),
                            //todo: check this cuz it comes from viewmodel
                            processInfo: new StateEventInfo(processId, RevolutionData.Context.Process.Events.CurrentEntityChanged)),
                        
                    },
                    expectedMessageType: typeof(IProcessStateMessage<TEntityView>),
                    action: ProcessActions.RequestStateList(currentProperty,viewProperty),
                    processInfo: new StateCommandInfo(processId, RevolutionData.Context.Process.Commands.UpdateState));
            }

            public static IComplexEventAction UpdateStateWhenDataChanges<TEntity, TView>(int processId, Expression<Func<TEntity, object>> currentProperty, Expression<Func<TView, object>> viewProperty) where TEntity : IEntityId where TView : IEntityView
            {
                return new ComplexEventAction(
                    key: $"UpdateStateWhenDataChanges{typeof(TEntity).Name}-{typeof(TView).Name}-{viewProperty.GetMemberName()}",
                    processId: 3,
                    actionTrigger:ActionTrigger.Any, 
                    events: new List<IProcessExpectedEvent>
                    {
                        new ProcessExpectedEvent<IEntityUpdated<TEntity>>(processId: processId,
                            eventPredicate: e => e.Entity != null,
                            processInfo: new StateEventInfo(processId, Entity.Events.EntityUpdated),
                            expectedSourceType: new SourceType(typeof (IEntityRepository)),
                            key: "UpdatedEntity"),
                        new ProcessExpectedEvent<IEntityViewWithChangesUpdated<TEntity>>(processId: processId,
                            eventPredicate: e => e.Entity != null,
                            processInfo: new StateEventInfo(processId, EntityView.Events.EntityViewUpdated),
                            expectedSourceType: new SourceType(typeof (IEntityRepository)),
                            key: "UpdatedEntity"),


                    },
                    expectedMessageType: typeof(IProcessStateMessage<TView>),
                    action: GetView(currentProperty, viewProperty),
                    processInfo: new StateCommandInfo(processId, RevolutionData.Context.Process.Commands.UpdateState));
            }

            public static IProcessAction GetView<TEntity,TView>(Expression<Func<TEntity, object>> currentProperty, Expression<Func<TView, object>> viewProperty) where TView : IEntityView
            {
                return new ProcessAction(
                    action:
                        async cp =>
                        {
                            var key = default(TView).GetMemberName(viewProperty);
                            var value = currentProperty.Compile().Invoke(cp.Messages["UpdatedEntity"].Entity);
                            var changes = new Dictionary<string, dynamic>() { { key, value } };

                            return await Task.Run(() => new GetEntityViewWithChanges<TView>(changes,
                                new StateCommandInfo(cp.Actor.Process.Id, EntityView.Commands.GetEntityView),
                                cp.Actor.Process, cp.Actor.Source));
                        },
                    processInfo: cp =>
                        new StateCommandInfo(cp.Actor.Process.Id,
                            EntityView.Commands.GetEntityView),
                    // take shortcut cud be IntialState
                    expectedSourceType: new SourceType(typeof(IComplexEventService))

                    );
            }


            public static ComplexEventAction RequestCompositeStateList<TEntityView>(int processId, Dictionary<string, dynamic>changes, List<ViewModelEntity> entities) where TEntityView : IEntityView
            {
                
                return new ComplexEventAction(
                    key: $"RequestCompositState-{string.Join(",", entities.Select(x => x.EntityType.GetFriendlyName()))}",
                    processId: processId,
                    actionTrigger: ActionTrigger.Any,
                    events: new List<IProcessExpectedEvent>(entities.Select(x => (IProcessExpectedEvent)typeof(Processes.ComplexActions).GetMethod("CreateProcessExpectedEvent").MakeGenericMethod(x.EntityType).Invoke(null,new object[] { processId })).ToList()),
                                       
                    expectedMessageType: typeof(IProcessStateMessage<TEntityView>),
                    action: ProcessActions.RequestCompositStateList<TEntityView>(changes,entities),
                    processInfo: new StateCommandInfo(processId, RevolutionData.Context.Process.Commands.UpdateState));
            }

            public static IProcessExpectedEvent CreateProcessExpectedEvent<T>(int processId)
            {
               return new ProcessExpectedEvent<ICurrentEntityChanged<T>>(processId: processId,
                            eventPredicate: e => e.Entity != null,
                            processInfo: new StateEventInfo(processId, RevolutionData.Context.Process.Events.CurrentEntityChanged),
                            expectedSourceType: new SourceType(typeof(IComplexEventService)),
                            key: $"CurrentEntity-{typeof(T).GetFriendlyName()}");
            }
        }
    }

    
}

