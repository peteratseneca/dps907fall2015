using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using Lab3.Models;

namespace Lab3.ServiceLayer
{
    // Unit of work class

    public class Worker : IDisposable
    {
        private ApplicationDbContext _ds = new ApplicationDbContext();
        private bool disposed = false;

        // Properties for each repository

        private Artist_repo _man;

        // Other repos go here...

        // Custom getters for each repository
        // For example...
        // public Product_repo Products

        public Artist_repo Artists
        {
            get
            {
                if (this._man == null)
                {
                    this._man = new Artist_repo(_ds);
                }
                return this._man;
            }
        }

        // Other custom getters go here...

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
