using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace DLL.Repositories
{
    public class UnitOfWorkJapfaMap : IDisposable
    {
        private EntitiesMap context;
        public EntitiesMap Context
        {
            get
            {
                return context;
            }
        }

        public UnitOfWorkJapfaMap(EntitiesMap context)
        {
            this.context = context;
        }

        #region Repositories
        private GenericRepositoriesJapfaMap<TBLOCATIONTEMP> tBLOCATIONTEMP_Repository;
        public GenericRepositoriesJapfaMap<TBLOCATIONTEMP> TBLOCATIONTEMP_Repository
        {
            get
            {
                if (tBLOCATIONTEMP_Repository == null)
                {
                    this.tBLOCATIONTEMP_Repository = new GenericRepositoriesJapfaMap<TBLOCATIONTEMP>(context);
                }
                return tBLOCATIONTEMP_Repository;
            }
        }


        private GenericRepositoriesJapfaMap<TBLLOCATION> tBLLOCATION_Repository;
        public GenericRepositoriesJapfaMap<TBLLOCATION> TBLLOCATION_Repository
        {
            get
            {
                if (tBLLOCATION_Repository == null)
                {
                    this.tBLLOCATION_Repository = new GenericRepositoriesJapfaMap<TBLLOCATION>(context);
                }
                return tBLLOCATION_Repository;
            }
        }

        private GenericRepositoriesJapfaMap<TBLOCATIONUPDATING> tBLOCATIONUPDATING_Repository;
        public GenericRepositoriesJapfaMap<TBLOCATIONUPDATING> TBLOCATIONUPDATING_Repository
        {
            get
            {
                if (tBLOCATIONUPDATING_Repository == null)
                {
                    this.tBLOCATIONUPDATING_Repository = new GenericRepositoriesJapfaMap<TBLOCATIONUPDATING>(context);
                }
                return tBLOCATIONUPDATING_Repository;
            }
        }


        private GenericRepositoriesJapfaMap<TBLOCATIONSTATU> tBLOCATIONSTATU_Repository;
        public GenericRepositoriesJapfaMap<TBLOCATIONSTATU> TBLOCATIONSTATU_Repository
        {
            get
            {
                if (tBLOCATIONSTATU_Repository == null)
                {
                    this.tBLOCATIONSTATU_Repository = new GenericRepositoriesJapfaMap<TBLOCATIONSTATU>(context);
                }
                return tBLOCATIONSTATU_Repository;
            }
        }

        private GenericRepositoriesJapfaMap<TBPICTUREFORLOCATION> tBPICTUREFORLOCATION_Repository;
        public GenericRepositoriesJapfaMap<TBPICTUREFORLOCATION> TBPICTUREFORLOCATION_Repository
        {
            get
            {
                if (tBPICTUREFORLOCATION_Repository == null)
                {
                    this.tBPICTUREFORLOCATION_Repository = new GenericRepositoriesJapfaMap<TBPICTUREFORLOCATION>(context);
                }
                return tBPICTUREFORLOCATION_Repository;
            }
        }

        #endregion

        public int Save()
        {
            try
            {
                return context.SaveChanges();

            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
            {
                throw ex.InnerException.InnerException;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);
                var fullErrorMessage = string.Join("; ", errorMessages);
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                throw new System.Data.Entity.Validation.DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public void SaveLog(string email)
        {
            int id = 1;
            try
            {
                context.Configuration.ProxyCreationEnabled = false;

                var changeEntities = context.ChangeTracker.Entries()
                        .Where(p => p.State == EntityState.Added || p.State == EntityState.Modified || p.State == EntityState.Deleted).ToList();
                var now = DateTime.Now;

                foreach (var change in changeEntities)
                {
                    var entityName = ObjectContext.GetObjectType(change.Entity.GetType()).Name;

                    if (change.State == EntityState.Added)
                    {
                        ObjectContext objectContext = ((IObjectContextAdapter)this.context).ObjectContext;
                        var retval = objectContext.MetadataWorkspace
                            .GetType(change.Entity.GetType().Name, "Model", System.Data.Entity.Core.Metadata.Edm.DataSpace.CSpace)
                            .MetadataProperties
                            .Where(mp => mp.Name == "KeyMembers")
                            .First().Value as System.Data.Entity.Core.Metadata.Edm.ReadOnlyMetadataCollection<System.Data.Entity.Core.Metadata.Edm.EdmMember>;
                        var keyNames = retval.Select(x => x.Name);
                        var keyValues = new List<string>();

                        foreach (var keyName in keyNames)
                        {
                            var keyValue = change.CurrentValues.GetValue<object>(keyName);
                            keyValues.Add(keyValue.ToString());
                        }
                        context.TBLOGs.Add(new TBLOG
                        {
                            LOGID = (int)context.TBLOGs.DefaultIfEmpty().Max(x => x == null ? 0 : x.LOGID) + id++,
                            TIME = now,
                            EMAIL = email,
                            TYPE = "INSERT",
                            TABLE_NAME = entityName,
                            KEY_NAME = string.Join(";", keyNames),
                            KEY_VALUE = string.Join(";", keyValues),
                            CONTENT = JsonConvert.SerializeObject(change.Entity, new JsonSerializerSettings
                            {
                                ContractResolver = new CustomResolver(),
                                PreserveReferencesHandling = PreserveReferencesHandling.None,
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                                Formatting = Formatting.Indented
                            }),
                        });
                    }
                    else if (change.State == EntityState.Modified)
                    {
                        var objectStateEntry = ((IObjectContextAdapter)this.context).ObjectContext.ObjectStateManager.GetObjectStateEntry(change.Entity);
                        var keyNameValues = objectStateEntry.EntityKey.EntityKeyValues;
                        var keyNames = keyNameValues.Select(x => x.Key);
                        var keyValues = keyNameValues.Select(x => x.Value);

                        var DatabaseValues = change.GetDatabaseValues();

                        var content = string.Empty;
                        foreach (var prop in change.OriginalValues.PropertyNames)
                        {
                            string originalValue = DatabaseValues.GetValue<object>(prop) == null ? null : DatabaseValues.GetValue<object>(prop).ToString();
                            string currentValue = change.CurrentValues[prop] == null ? null : change.CurrentValues[prop].ToString();

                            if (currentValue != originalValue)
                            {
                                if (currentValue == null)
                                {
                                    content += string.Format(";{0}:{1}|{2}", prop, originalValue, currentValue);
                                }
                                else
                                {
                                    if (currentValue.EndsWith(".0"))
                                    {
                                        if (currentValue.Replace(".0", "") != originalValue)
                                        {
                                            content += string.Format(";{0}:{1}|{2}", prop, originalValue, currentValue);
                                        }
                                    }
                                    else
                                    {
                                        content += string.Format(";{0}:{1}|{2}", prop, originalValue, currentValue);
                                    }
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(content))
                        {
                            context.TBLOGs.Add(new TBLOG
                            {
                                LOGID = (int)context.TBLOGs.DefaultIfEmpty().Max(x => x == null ? 0 : x.LOGID) + id++,
                                TIME = now,
                                TYPE = "UPDATE",
                                EMAIL = email,
                                TABLE_NAME = entityName,
                                KEY_NAME = string.Join(";", keyNames),
                                KEY_VALUE = string.Join(";", keyValues),
                                CONTENT = content.Remove(0, 1), //remove ";",
                            });
                        }
                    }
                    else if (change.State == EntityState.Deleted)
                    {
                        var objectStateEntry = ((IObjectContextAdapter)this.context).ObjectContext.ObjectStateManager.GetObjectStateEntry(change.Entity);
                        var keyNameValues = objectStateEntry.EntityKey.EntityKeyValues;
                        var keyNames = keyNameValues.Select(x => x.Key);
                        var keyValues = keyNameValues.Select(x => x.Value);

                        context.TBLOGs.Add(new TBLOG
                        {
                            LOGID = (int)context.TBLOGs.DefaultIfEmpty().Max(x => x == null ? 0 : x.LOGID) + id++,
                            TIME = now,
                            TYPE = "DELETE",
                            EMAIL = email,
                            TABLE_NAME = entityName,
                            KEY_NAME = string.Join(";", keyNames),
                            KEY_VALUE = string.Join(";", keyValues),
                            CONTENT = JsonConvert.SerializeObject(change.Entity, new JsonSerializerSettings
                            {
                                ContractResolver = new CustomResolver(),
                                PreserveReferencesHandling = PreserveReferencesHandling.None,
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                                Formatting = Formatting.Indented
                            }),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                var u = ex.Message;
            }

        }


    }
}


class CustomResolver : DefaultContractResolver
{
    private readonly List<string> _namesOfVirtualPropsToKeep = new List<string>(new String[] { });

    public CustomResolver() { }

    public CustomResolver(IEnumerable<string> namesOfVirtualPropsToKeep)
    {
        this._namesOfVirtualPropsToKeep = namesOfVirtualPropsToKeep.Select(x => x.ToLower()).ToList();
    }

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        JsonProperty prop = base.CreateProperty(member, memberSerialization);
        var propInfo = member as PropertyInfo;
        if (propInfo != null)
        {
            if (propInfo.GetMethod.IsVirtual && !propInfo.GetMethod.IsFinal
                && !_namesOfVirtualPropsToKeep.Contains(propInfo.Name.ToLower()))
            {
                prop.ShouldSerialize = obj => false;
            }
        }
        return prop;
    }
}