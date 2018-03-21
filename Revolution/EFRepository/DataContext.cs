using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using SystemInterfaces;
using Common.DataEntites;
using DomainUtilities;
using EventAggregator;
using EventMessages.Events;
using GenSoft.DBContexts;
using GenSoft.Entities;
using Microsoft.EntityFrameworkCore;
using Utilities;
using Entity = GenSoft.Entities.Entity;
using EntityEvents = RevolutionData.Context.Entity;
using System.Linq.Dynamic;
using System.Reactive.Linq;
using Common;
using MoreLinq;
using Process.WorkFlow;
using RevolutionEntities.Process;
using CommandType = System.Data.CommandType;
using StateEventInfo = RevolutionEntities.Process.StateEventInfo;


namespace EFRepository
{
    public class DataContext:BaseRepository<DataContext>
    {
        
        private static readonly DataContext instance = new DataContext();
        public static DataContext Instance => instance;
        static DataContext() { }

        private DataContext(){}
        

        public  void Create(ICreateEntity msg)
        {
            throw new NotImplementedException();
        }

        public void UpdateEntityWithChanges(IUpdateEntityWithChanges msg)
        {
            try
            {
                if (!(msg.ProcessInfo.Process.Applet is IDbApplet dbInfo)) return;
                if (msg.Changes.Count == 0) return;
                
                    var entity = msg.Entity;
                    
                    entity.ApplyChanges(msg.Changes.Where(x => x.Value != null));
                    var sql = "";
                    Func<int, string> selectSql = id =>
                        $"Select * From {msg.EntityType.EntitySetName} Where Id = {id}";
                    int entityId = 0;
                if (msg.Entity.Id == 0)
                {
                    if (msg.EntityType.ParentEntityType.Name != DynamicEntityType.NullEntityType().Name)
                    {
                        var changes = msg.Changes;
                        if(!changes.ContainsKey("Id")) changes.Add("Id", 0);
                        sql =
                            $"Insert Into {msg.EntityType.EntitySetName}({changes.Where(x => x.Value != null).Select(x => x.Key).Aggregate((current, next) => current + "," + next)})" +
                            $"  OUTPUT Inserted.Id " +
                            $" Values ({changes.Where(x => x.Value != null).Select(x => $"'{x.Value}'").Aggregate((current, next) => $"{current},{next}")})";
                    }
                    else
                    {
                        var changes = msg.Changes;
                        if(changes.ContainsKey("Id"))changes.Remove("Id");
                        sql =
                            $"Insert Into {msg.EntityType.EntitySetName}({changes.Where(x => x.Value != null).Select(x => x.Key).Aggregate((current, next) => current + "," + next)})" +
                            $"  OUTPUT Inserted.Id " +
                            $" Values ({changes.Where(x => x.Value != null).Select(x => $"'{x.Value}'").Aggregate((current, next) => $"{current},{next}")})";
                    }

                    using (var conn = new SqlConnection(dbInfo.DbConnectionString))
                    {
                        var cmd = conn.CreateCommand();
                        cmd.CommandText = Sql.CleanSql(sql);
                        cmd.CommandType = CommandType.Text;
                        conn.Open();
                        entityId = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
                else
                {
                    entityId = msg.Entity.Id;
                    sql =
                        $"Update {msg.EntityType.Name} Set {msg.Changes.Where(x => x.Value != null).Select(x => $"{x.Key}='{x.Value.ToString()}'").Aggregate((current, next) => current + "," + next)}" +
                        $" Where Id = {entityId}";
                    using (var conn = new SqlConnection(dbInfo.DbConnectionString))
                    {
                        var cmd = conn.CreateCommand();
                        cmd.CommandText = sql;
                        cmd.CommandType = CommandType.Text;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                using (var conn = new SqlConnection(dbInfo.DbConnectionString))
                    {
                        var cmd = conn.CreateCommand();
                        cmd.CommandText = selectSql.Invoke(entityId);
                        cmd.CommandType = CommandType.Text;

                    conn.Open();
                        var reader = cmd.ExecuteReader();
                        IDynamicEntity newEntity = null;
                        while (reader.Read())
                        {
                            var aDict = Enumerable.Range(0, reader.FieldCount)
                                .ToDictionary(reader.GetName, reader.GetValue);
                            newEntity = new DynamicEntity(msg.EntityType, entityId,aDict);
                        }

                        if(newEntity != null)
                        msg.Changes.Where(x => msg.EntityType.CachedProperties.ContainsKey(x.Key)).ForEach(change => msg.EntityType.CachedProperties[change.Key].AddOrUpdate((int)newEntity.Properties["Id"], change.Value));
                        
                        
                        EventMessageBus.Current.Publish(
                            new EntityWithChangesUpdated(newEntity, msg.Changes,
                                new RevolutionEntities.Process.StateEventInfo(msg.Process,
                                    RevolutionData.Context.EventFunctions.UpdateEventData(msg.EntityType.Name,
                                        EntityEvents.Events.EntityUpdated)), msg.Process,
                                Source));
                    }

                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public  void AddEntity(IAddOrGetEntityWithChanges msg)
        {
            throw new NotImplementedException();
        }

        public void LoadEntitySetWithChanges(IGetEntitySetWithChanges msg)
        {
            

            if (!(msg.ProcessInfo.Process.Applet is IDbApplet dbInfo)) return;


            string selectSql;
            if (msg.EntityType.ParentEntityType != null && msg.EntityType.ParentEntityType.Properties.Any(x => x.Key == "Name" && x.IsComputed == false))
            {
                selectSql = $"Select {msg.EntityType.Name}.*, parent.Name From {msg.EntityType.EntitySetName} inner join {msg.EntityType.ParentEntityType.EntitySetName} parent on {msg.EntityType.EntitySetName}.Id = parent.Id" +
                            $" Where {GetWhereStr(msg.EntityType.Name,msg.Changes)}";
            }
            else
            {
                selectSql = $"Select * From {msg.EntityType.EntitySetName} " +
                        $"Where {GetWhereStr(msg.EntityType.Name, msg.Changes)}";
            }
            
            using (var conn = new SqlConnection(dbInfo.DbConnectionString))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = selectSql;
                cmd.CommandType = CommandType.Text;

                conn.Open();

                

            var reader = cmd.ExecuteReader();
                var entities = new List<IDynamicEntity>();
                while (reader.Read())
                {
                    var aDict = Enumerable.Range(0, reader.FieldCount)
                        .ToDictionary(reader.GetName, reader.GetValue);

                    entities.Add(new DynamicEntity(msg.EntityType, Convert.ToInt32(aDict["Id"]), aDict));
                }


                EventMessageBus.Current.Publish(
                    new EntitySetWithChangesLoaded(msg.EntityType, entities, msg.Changes,
                        new RevolutionEntities.Process.StateEventInfo(msg.Process,
                            RevolutionData.Context.EventFunctions.UpdateEventData(msg.EntityType.Name,
                                EntityEvents.Events.EntitySetLoaded)), msg.Process,
                        Source));
            }

        }

        private  string GetWhereStr(string entityTypeName, Dictionary<string, object> changes)
        {
            var whereStr = changes.Aggregate("", (str, itm) => str + ($"{entityTypeName}.{itm.Key} = '{itm.Value}' and"));
            whereStr = whereStr.TrimEnd(" and");
            return whereStr;
        }

        public  void LoadEntitySet(ILoadEntitySet msg)
        {
            if (!(msg.ProcessInfo.Process.Applet is IDbApplet dbInfo)) return;
            
                

            string selectSql;
            if (msg.EntityType.ParentEntityType != null && msg.EntityType.ParentEntityType.Properties.Any(x => x.Key == "Name" && x.IsComputed == false))
            {
                selectSql = $"Select {msg.EntityType.EntitySetName}.*, parent.Name From {msg.EntityType.EntitySetName} inner join {msg.EntityType.ParentEntityType.EntitySetName} parent on {msg.EntityType.EntitySetName}.Id = parent.Id" ;
            }
            else
            {
                selectSql = $"Select * From {msg.EntityType.EntitySetName} ";
            }
            using (var conn = new SqlConnection(dbInfo.DbConnectionString))
                {
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = selectSql;
                    cmd.CommandType = CommandType.Text;

                    conn.Open();

                    var reader = cmd.ExecuteReader();
                    var res = new List<IDynamicEntity>();
                    while (reader.Read())
                    {
                        var aDict = Enumerable.Range(0, reader.FieldCount)
                            .ToDictionary(reader.GetName, reader.GetValue);
                        res.Add(new DynamicEntity(msg.EntityType, Convert.ToInt32(aDict["Id"]), aDict));
                    }

                    EventMessageBus.Current.Publish(
                        new EntitySetLoaded(msg.EntityType, res,
                            new StateEventInfo(msg.Process,
                                RevolutionData.Context.EventFunctions.UpdateEventData(msg.EntityType.Name,
                                    EntityEvents.Events.EntitySetLoaded)), msg.Process, Source));
                }
            

        }

        
        public  void LoadEntitySetWithFilter(ILoadEntitySetWithFilter msg)
        {
            throw new NotImplementedException();
        }

        public  void LoadEntitySetWithFilterWithIncludes(ILoadEntitySetWithFilterWithIncludes msg)
        {
            throw new NotImplementedException();
        }

        public  void DeleteEntity(IDeleteEntity msg)
        {
            throw new NotImplementedException();
        }

        public  void GetEntityById(IGetEntityById msg)
        {
            throw new NotImplementedException();
        }

        public void GetEntityWithChanges(IGetEntityWithChanges msg)
        {
            if (!(msg.ProcessInfo.Process.Applet is IDbApplet dbInfo)) return;


            
            string selectSql;
            if (msg.EntityType.ParentEntityType != null && msg.EntityType.ParentEntityType.Properties.Any(x => x.Key == "Name" && x.IsComputed == false))
            {
                selectSql = $"Select {msg.EntityType.EntitySetName}.*, parent.Name From {msg.EntityType.EntitySetName} inner join {msg.EntityType.ParentEntityType.EntitySetName} parent on {msg.EntityType.EntitySetName}.Id = parent.Id" +
                            $" Where {GetWhereStr(msg.EntityType.Name, msg.Changes)}";
            }
            else
            {
                selectSql = $"Select * From {msg.EntityType.EntitySetName} " +
                            $"Where {GetWhereStr(msg.EntityType.EntitySetName, msg.Changes)}";
            }

            using (var conn = new SqlConnection(dbInfo.DbConnectionString))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = selectSql;
                cmd.CommandType = CommandType.Text;

                conn.Open();
                var reader = cmd.ExecuteReader();
                IDynamicEntity entity = null;
                while (reader.Read())
                {
                    var aDict = Enumerable.Range(0, reader.FieldCount)
                        .ToDictionary(reader.GetName, reader.GetValue);
                    entity = new DynamicEntity(msg.EntityType, Convert.ToInt32(aDict["Id"]), aDict);
                    break; // to select first
                }

                if (entity != null)
                {
                    EventMessageBus.Current.Publish(
                        new EntityWithChangesFound(entity, msg.Changes,
                            new RevolutionEntities.Process.StateEventInfo(msg.Process,
                                RevolutionData.Context.EventFunctions.UpdateEventData(msg.EntityType.Name,
                                    EntityEvents.Events.EntityFound)), msg.Process,
                            Source));
                }
                else
                {
                    EventMessageBus.Current.Publish(
                        new EntityWithChangesFound(
                            DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(msg.EntityType.Name).DefaultEntity(),
                            msg.Changes,
                            new RevolutionEntities.Process.StateEventInfo(msg.Process,
                                RevolutionData.Context.EventFunctions.UpdateEventData(msg.EntityType.Name,
                                    EntityEvents.Events.EntityFound)), msg.Process,
                            Source));
                }




            }
        }


    }
}