using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using LocalSecurity.Models;

namespace LocalSecurity.ServiceLayer
{
    // Unit of work class

    public class Worker : IDisposable
    {
        private ApplicationDbContext _ds = new ApplicationDbContext();
        private bool disposed = false;

        // Properties for each repository...
        // (use the "propf" code snippet)
        // Custom getters only, for each repository

        private Identity_repo _ident;

        public Identity_repo Identity
        {
            get
            {
                if (_ident == null)
                {
                    _ident = new Identity_repo(_ds);
                }
                return _ident;
            }
        }

        private ExceptionInfo_repo _ex;

        public ExceptionInfo_repo Exceptions
        {
            get
            {
                if (_ex == null)
                {
                    _ex = new ExceptionInfo_repo(_ds);
                }
                return _ex;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _ds.Dispose();
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
