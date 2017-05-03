﻿// <autogenerated>
//   This file was generated by T4 code generator Amoeba-Master.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>


using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Core.Common.UI;
using CommonMessages;
using EventAggregator;
using EventMessages;
using Interfaces;
using Utilities;

namespace ViewModels
{
	public partial class ViewEntityViewModel_AutoGen 
	{
		MessageSource msgSource => new MessageSource(this.ToString());
		protected virtual void WireEvents()
		{
			EventMessageBus.Current.GetEvent<CurrentEntityChanged<IEntities>>(msgSource).Subscribe(x => handleEntityIdChanged(x.EntityId));      
			EventMessageBus.Current.GetEvent<CurrentEntityChanged<IViews>>(msgSource).Subscribe(x => handleViewIdChanged(x.EntityId));      
                  
		}
		private void handleEntityIdChanged(int entityId)
		{
					
			if(entityId != EntityStates.NullEntity)
			if(CurrentEntity.Id == EntityStates.NullEntity || CurrentEntity.Id != entityId)
			{
				CurrentEntity = CreateNullEntity();
				FilterExpression = new List<Expression<Func<IViewEntity, bool>>>() {x => x.EntityId == entityId};
			}
		}
		private void handleViewIdChanged(int entityId)
		{
					
			if(entityId != EntityStates.NullEntity)
			if(CurrentEntity.Id == EntityStates.NullEntity || CurrentEntity.Id != entityId)
			{
				CurrentEntity = CreateNullEntity();
				FilterExpression = new List<Expression<Func<IViewEntity, bool>>>() {x => x.ViewId == entityId};
			}
		}
 
	}
}
