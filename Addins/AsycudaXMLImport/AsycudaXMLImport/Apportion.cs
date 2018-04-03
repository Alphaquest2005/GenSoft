using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using SystemInterfaces;
using Common;
using Common.DataEntites;
using EventAggregator;
using EventMessages.Commands;
using Process.WorkFlow;
using RevolutionEntities.Process;
using DomainUtilities;
using Reactive.Bindings;
using RevolutionData.Context;
using Utilities;

namespace AsycudaXMLImport
{
    public class Apportion
    {
        public static ISystemSource Source { get; } = new Source(Guid.NewGuid(), $"Addin:<AsycudaXMLImport>",
            new SourceType(typeof(Import)), Processes.IntialSystemProcess, Processes.IntialSystemProcess.MachineInfo);

        static Apportion()
        {
            EventMessageBus.Current.GetEvent<IStartAddin>(
                    new StateCommandInfo(Processes.IntialSystemProcess,
                        RevolutionData.Context.CommandFunctions.UpdateCommandData("AsycudaXMLImport",
                            RevolutionData.Context.Addin.Commands.StartAddin), Guid.NewGuid()), Source)
                .Where(x => x.Action.Action == "Apportion" && x.Action.Addin == "AsycudaXMLImport").Subscribe(OnApportion);
        }

        private static void OnApportion(IStartAddin msg)
        {
            DocSetId.Value = msg.Entity.Id;

            EventMessageBus.Current.GetEvent<IEntitySetWithChangesLoaded>(
                    new StateEventInfo(msg.Process, Entity.Events.EntitySetLoaded, Source.SourceId), Source).Where(x => x.EntityType.Name == "AsycudaDocumentSetExpenses" && x.Process.Id == msg.Process.Id)
                .Where(x => x.ProcessInfo.EventKey == Source.SourceId)
                .Subscribe(x => OnSetExpensesReceived(x));

            EventMessageBus.Current.Publish(new GetEntitySetWithChanges("ExactMatch",
                DynamicEntityTypeExtensions.GetOrAddDynamicEntityType("AsycudaDocumentSetExpenses"), new Dictionary<string, dynamic>() { { "AsycudaDocumentSetId", msg.Entity.Id } },
                process: msg.Process,
                processInfo: new StateCommandInfo(msg.Process, RevolutionData.Context.CommandFunctions.UpdateCommandData("AsycudaDocumentSetExpenses", Entity.Commands.LoadEntitySetWithChanges), Source.SourceId),
                source: Source));


          EventMessageBus.Current.GetEvent<IEntitySetWithChangesLoaded>(
                    new StateEventInfo(msg.Process, Entity.Events.EntitySetLoaded, Source.SourceId), Source).Where(x => x.EntityType.Name == "xcuda_Item" && x.Process.Id == msg.Process.Id)
                .Where(x => x.ProcessInfo.EventKey == Source.SourceId)
                .Subscribe(x => OnDocumentItemsReceived(x));



            // get Documents with value for documentSet

            // get document expenses for document set
            // Sum documents to get Total document value
            // Apportion expenes to document Expenses based on factor
            // publish document expenses
            // for each document in document set
            // get document expenses
            // get document items statistical value
            // sum document item statistical value to get document total
            // apportion document expenses to document items
            // publish document item expenses

        }


        private static readonly ReactiveProperty<List<(int expenseId, double amount)>> DocSetExpenses = new ReactiveProperty<List<(int expenseId, double amount)>>();
        private static void OnSetExpensesReceived(IEntitySetWithChangesLoaded msg)
        {
            DocSetExpenses.Value = msg.EntitySet.Select(x =>  (expenseId: Convert.ToInt32(x.Properties["ExpenseId"]), amounts:Convert.ToDouble(x.Properties["Amount"]))).ToList();
            EventMessageBus.Current.GetEvent<IEntitySetWithChangesLoaded>(
                    new StateEventInfo(msg.Process, Entity.Events.EntitySetLoaded, Source.SourceId), Source).Where(x => x.EntityType.Name == "xcuda_ASYCUDA_ExtendedProperties" && x.Process.Id == msg.Process.Id)
                .Where(x => x.ProcessInfo.EventKey == Source.SourceId)
                .Subscribe(x => OnDocumentsReceived(x));

            EventMessageBus.Current.Publish(new GetEntitySetWithChanges("ExactMatch",
                DynamicEntityTypeExtensions.GetOrAddDynamicEntityType("xcuda_ASYCUDA_ExtendedProperties"), new Dictionary<string, dynamic>() { { "AsycudaDocumentSetId", DocSetId.Value } },
                process: msg.Process,
                processInfo: new StateCommandInfo(msg.Process, RevolutionData.Context.CommandFunctions.UpdateCommandData("xcuda_ASYCUDA_ExtendedProperties", Entity.Commands.LoadEntitySetWithChanges), Source.SourceId),
                source: Source));
        }

        private static readonly ReactiveProperty<double> DocSetTotalValue = new ReactiveProperty<double>(0);
        private static readonly ReactiveProperty<int> DocSetId = new ReactiveProperty<int>();
        private static void OnDocumentsReceived(IEntitySetWithChangesLoaded msg)
        {
            if(Math.Abs(DocSetTotalValue.Value) > 0) return;
            DocSetTotalValue.Value = msg.EntitySet.Select(x => Convert.ToDouble(x.Properties["Total_CIF"])).Sum();
            if (DocSetTotalValue.Value == 0) return;
            EventMessageBus.Current.GetEvent<IEntitySetWithChangesLoaded>(
                    new StateEventInfo(msg.Process, Entity.Events.EntitySetLoaded, Source.SourceId), Source).Where(x => x.EntityType.Name == "xcuda_Valuation" && x.Process.Id == msg.Process.Id)
                .Where(x => x.ProcessInfo.EventKey == Source.SourceId)
                .Subscribe(x => OnDocumentValuationReceived(x));
            foreach (var doc in msg.EntitySet)
            {
                EventMessageBus.Current.Publish(new GetEntitySetWithChanges(match: "ExactMatch",
                entityType: DynamicEntityTypeExtensions.GetOrAddDynamicEntityType("xcuda_Valuation"),
                changes: new Dictionary<string, dynamic>() { { "ASYCUDA_Id", doc.Properties["ASYCUDA_Id"] } },
                process: msg.Process,
                processInfo: new StateCommandInfo(msg.Process, RevolutionData.Context.CommandFunctions.UpdateCommandData("xcuda_Valuation", Entity.Commands.LoadEntitySetWithChanges), Source.SourceId),
                source: Source));
            }
            
        }

        private static void OnDocumentValuationReceived(IEntitySetWithChangesLoaded msg)
        {
            var docType = DynamicEntityTypeExtensions.GetOrAddDynamicEntityType("DocumentExpenses");
            EventMessageBus.Current.GetEvent<IEntitySetWithChangesLoaded>(
                    new StateEventInfo(msg.Process, Entity.Events.EntitySetLoaded, Source.SourceId), Source).Where(x => x.EntityType.Name == "DocumentExpenses" && x.Process.Id == msg.Process.Id)
                .Where(x => x.ProcessInfo.EventKey == Source.SourceId)
                .Subscribe(x => OnDocumentExpensesReceived(x));
            
            foreach (var val in msg.EntitySet)
            {
                var dval = Convert.ToDouble(val.Properties["Total_CIF"]);
               
                foreach (var d in DocSetExpenses.Value)
                {
                    var expEntity = new DynamicEntity(docType,0, new Dictionary<string, object>());
                    expEntity.Properties["ASYCUDA_Id"] =  val.Properties["ASYCUDA_Id"];
                    expEntity.Properties["ExpenseId"] = d.expenseId;
                    expEntity.Properties["Amount"] =  d.amount *(dval/DocSetTotalValue.Value);
                    EventMessageBus.Current.Publish(new UpdateEntityWithChanges(expEntity,
                                                                                expEntity.Properties.ToDictionary(x => x.Key, x => x.Value as dynamic),
                                                                                new StateCommandInfo(msg.Process, CommandFunctions.UpdateCommandData("DocumentExpenses", Entity.Commands.UpdateEntity)),
                                                                                msg.Process,
                                                                                Source));
                }
                var t = 0;
                EventMessageBus.Current.GetEvent<IEntityWithChangesUpdated>(
                        new StateEventInfo(msg.Process, Entity.Events.EntitySetLoaded, Source.SourceId), Source)
                    .Where(x => x.EntityType.Name == "DocumentExpenses" && x.Process.Id == msg.Process.Id)
                    .Subscribe(x =>
                    {
                        t += 1;
                        if(t == DocSetExpenses.Value.Count)
                        EventMessageBus.Current.Publish(new GetEntitySetWithChanges(match: "ExactMatch",
                            entityType: DynamicEntityTypeExtensions.GetOrAddDynamicEntityType("DocumentExpenses"),
                            changes: new Dictionary<string, dynamic>() {{"ASYCUDA_Id", val.Properties["ASYCUDA_Id"]}},
                            process: msg.Process,
                            processInfo: new StateCommandInfo(msg.Process,
                                RevolutionData.Context.CommandFunctions.UpdateCommandData("DocumentExpenses",
                                    Entity.Commands.LoadEntitySetWithChanges), Source.SourceId),
                            source: Source));
                    });

                
            }

            
        }

        private static readonly ReactiveProperty<Dictionary<int,List<(int expenseId, double amount)>>> DocExpenses = new ReactiveProperty<Dictionary<int, List<(int expenseId, double amount)>>>(new Dictionary<int, List<(int expenseId, double amount)>>());
        private static void OnDocumentExpensesReceived(IEntitySetWithChangesLoaded msg)
        {
            var f = msg.EntitySet.FirstOrDefault();
            if (f == null) return;
            var docExpenses = msg.EntitySet.Select(x => (expenseId: Convert.ToInt32(x.Properties["ExpenseId"]), amounts: Convert.ToDouble(x.Properties["Amount"]))).ToList();
            if (DocExpenses.Value.ContainsKey(Convert.ToInt32(f.Properties["ASYCUDA_Id"]))) return; ///////////////////////// - ---- because of double event because of event store
            DocExpenses.Value.Add(Convert.ToInt32(f.Properties["ASYCUDA_Id"]),  docExpenses);

            EventMessageBus.Current.Publish(new GetEntitySetWithChanges(match: "ExactMatch",
                entityType: DynamicEntityTypeExtensions.GetOrAddDynamicEntityType("xcuda_Item"),
                changes: new Dictionary<string, dynamic>() { { "ASYCUDA_Id", f.Properties["ASYCUDA_Id"] } },
                process: msg.Process,
                processInfo: new StateCommandInfo(msg.Process, RevolutionData.Context.CommandFunctions.UpdateCommandData("xcuda_Item", Entity.Commands.LoadEntitySetWithChanges), Source.SourceId),
                source: Source));
        }

        private static readonly ReactiveProperty<List<IDynamicEntity>> valueItemMessages = new ReactiveProperty<List<IDynamicEntity>>(new List<IDynamicEntity>());
        private static void OnDocumentItemsReceived(IEntitySetWithChangesLoaded msg)
        {
            if (!msg.EntitySet.Any()) return;
            foreach (var itm in msg.EntitySet)
            {
                EventMessageBus.Current.Publish(new GetEntitySetWithChanges(match: "ExactMatch",
                    entityType: DynamicEntityTypeExtensions.GetOrAddDynamicEntityType("xcuda_Valuation_item"),
                    changes: new Dictionary<string, dynamic>() { { "Item_Id", itm.Properties["Item_Id"] } },
                    process: msg.Process,
                    processInfo: new StateCommandInfo(msg.Process, RevolutionData.Context.CommandFunctions.UpdateCommandData("xcuda_Valuation_item", Entity.Commands.LoadEntitySetWithChanges), Source.SourceId),
                    source: Source));
            }
            var cnt = msg.EntitySet.Count;
            var t = 0;
            EventMessageBus.Current.GetEvent<IEntitySetWithChangesLoaded>(
                    new StateEventInfo(msg.Process, Entity.Events.EntitySetLoaded, Source.SourceId), Source)
                .Where(x => x.EntityType.Name == "xcuda_Valuation_item" && x.Process.Id == msg.Process.Id)
                .Subscribe(x =>
                {
                    t += 1;
                    var val = x.EntitySet.FirstOrDefault();
                    valueItemMessages.Value.Add(val);
                    if (t == cnt)
                        OnDocumentValuaionItemsReceived(msg.Process);
                });
        }

        private static void OnDocumentValuaionItemsReceived(ISystemProcess msgProcess)
        {

            var type = DynamicEntityTypeExtensions.GetOrAddDynamicEntityType("DocumentItemExpenses");
            var dval = valueItemMessages.Value.Select(x => Convert.ToDouble(x.Properties["Total_CIF_itm"])).Sum();
            foreach (var itm in valueItemMessages.Value)
            {
                var ival = Convert.ToDouble(itm.Properties["Total_CIF_itm"]);
                foreach (var d in DocSetExpenses.Value)
                {
                    var expEntity = new DynamicEntity(type, 0, new Dictionary<string, object>());
                    expEntity.Properties["Item_Id"] = itm.Properties["Item_Id"];
                    expEntity.Properties["ExpenseId"] = d.expenseId;
                    expEntity.Properties["Amount"] = d.amount * (ival / dval);
                    EventMessageBus.Current.Publish(new UpdateEntityWithChanges(expEntity,
                        expEntity.Properties.ToDictionary(x => x.Key, x => x.Value as dynamic),
                        new StateCommandInfo(msgProcess, CommandFunctions.UpdateCommandData("DocumentItemExpenses", Entity.Commands.UpdateEntity)),
                        msgProcess,
                        Source));
                }
            }

            var cnt = valueItemMessages.Value.Count * DocSetExpenses.Value.Count;
            var t = 0;
            EventMessageBus.Current.GetEvent<IEntityWithChangesUpdated>(
                    new StateEventInfo(msgProcess, Entity.Events.EntityUpdated, Source.SourceId), Source)
                .Where(x => x.EntityType.Name == "DocumentItemExpenses" && x.Process.Id == msgProcess.Id)
                .Subscribe(x =>
                {
                    t += 1;
                    if (t == cnt)
                        MessageBox.Show("Complete");
                });
        }
    }
}