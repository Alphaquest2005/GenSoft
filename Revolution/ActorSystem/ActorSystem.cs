using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using SystemInterfaces;
using Actor.Interfaces;
using Akka.Actor;
using DataServices.Actors;
using GenSoft.DBContexts;
using GenSoft.Expressions;
using Microsoft.EntityFrameworkCore;
using ViewModel.Interfaces;

namespace ActorBackBone
{
    [Export(typeof(IActorBackBone))]
    public class ActorBackBone : IActorBackBone
    {
        //ToDo:Get rid of private setter
        public static ActorBackBone Instance { get; private set; }
       
        public static ActorSystem System { get; private set; }


        public void Intialize(bool autoRun, List<IMachineInfo> machineInfo, 
            List<IComplexEventAction> complexEventActions, List<IViewModelInfo> viewInfos)
        {
            try
            {
                System = ActorSystem.Create("System");

                System.ActorOf(Props.Create<ServiceManager>(autoRun, machineInfo, complexEventActions, viewInfos),
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

                
                List<IComplexEventAction> dbComplexAction = new List<IComplexEventAction>(); 

                Intialize(autoContinue, machineInfo, dbComplexAction, viewInfos);
            }

            
        }

        public void Intialize(bool autoContinue, List<IComplexEventAction> processComplexEvents, List<IViewModelInfo> processViewModelInfos)
        {
            using (var ctx = new GenSoftDBContext())
            {
                var machineInfo = ctx.Machine.Select(x => ProcessExpressions.CreateMachineInfo(x)).ToList();

                Intialize(autoContinue, machineInfo, processComplexEvents, processViewModelInfos);
            }
        }

        
    }
}
