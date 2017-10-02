using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using SystemInterfaces;
using Actor.Interfaces;
using Akka.Actor;
using Akka.Util.Internal;
using BuildingSpecifications;
using Common;
using Common.DataEntites;
using Common.Dynamic;
using DataServices.Actors;
using DynamicExpresso;
using EventAggregator;
using GenSoft.DBContexts;
using GenSoft.Entities;
using GenSoft.Expressions;
using JB.Collections.Reactive;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using Process.WorkFlow;
using RevolutionData;
using RevolutionEntities.Process;
using Utilities;
using ViewModel.Interfaces;
using RevolutionEntities.ViewModels;
using ViewModel.WorkFlow;
using Type = System.Type;

namespace ActorBackBone
{
    [Export(typeof(IActorBackBone))]
    public class ActorBackBone : IActorBackBone
    {
        //ToDo:Get rid of private setter
        public static ActorBackBone Instance { get; private set; }
       
        public static ActorSystem System { get; private set; }


        public void Intialize(bool autoRun, List<IMachineInfo> machineInfo, List<ISystemProcessInfo> processInfos,
            List<IComplexEventAction> complexEventActions, List<IViewModelInfo> viewInfos)
        {
            try
            {
                System = ActorSystem.Create("System");

                System.ActorOf(Props.Create<ServiceManager>(autoRun, machineInfo, processInfos, complexEventActions, viewInfos),
                    "ServiceManager");
                Instance = this;
            }
            catch (Exception)
            {
                throw;
            }
        }

      
        public void Intialize(bool autoContinue, List<IViewModelInfo> viewInfos)
        {
            using (var ctx = new GenSoftDBContext())
            {
                var machineInfo = ctx.Machine.Select(x => ProcessExpressions.CreateMachineInfo(x)).ToList();

                var processInfos = ctx.SystemProcess.Select(x => ProcessExpressions.CreateProcessInfo(x)).ToList();
                List<IComplexEventAction> dbComplexAction = new List<IComplexEventAction>(); 

                Intialize(autoContinue, machineInfo, processInfos, dbComplexAction, viewInfos);
            }

            
        }

        public void Intialize(bool autoContinue, List<IComplexEventAction> processComplexEvents, List<IViewModelInfo> processViewModelInfos)
        {
            using (var ctx = new GenSoftDBContext())
            {
                var machineInfo = ctx.Machine.Select(x => ProcessExpressions.CreateMachineInfo(x)).ToList();

                var processInfos = ctx.SystemProcess.Select(x => ProcessExpressions.CreateProcessInfo(x)).ToList();
                

                Intialize(autoContinue, machineInfo, processInfos, processComplexEvents, processViewModelInfos);
            }
        }

        
    }
}
