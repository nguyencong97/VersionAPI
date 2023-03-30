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
    public class UnitOfWorkLBCosting : IDisposable
    {
        private EntitiesLBCosting context;
        public EntitiesLBCosting Context
        {
            get
            {
                return context;
            }
        }

        public UnitOfWorkLBCosting(EntitiesLBCosting context)
        {
            this.context = context;
        }

        #region Repositories
        private GenericRepositoriesLBCosting<V_FARMER_GETLIST> v_FARMER_GETLIST;
        public GenericRepositoriesLBCosting<V_FARMER_GETLIST> V_FARMER_GETLIST
        {
            get
            {
                if (v_FARMER_GETLIST == null)
                {
                    this.v_FARMER_GETLIST = new GenericRepositoriesLBCosting<V_FARMER_GETLIST>(context);
                }
                return v_FARMER_GETLIST;
            }
        }


        private GenericRepositoriesLBCosting<V_LOCATION_GETLIST> v_LOCATION_GETLIST;
        public GenericRepositoriesLBCosting<V_LOCATION_GETLIST> V_LOCATION_GETLIST
        {
            get
            {
                if (v_LOCATION_GETLIST == null)
                {
                    this.v_LOCATION_GETLIST = new GenericRepositoriesLBCosting<V_LOCATION_GETLIST>(context);
                }
                return v_LOCATION_GETLIST;
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


      

    }
}


