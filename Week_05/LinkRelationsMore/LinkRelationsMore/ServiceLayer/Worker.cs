using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using LinkRelationsMore.Models;

namespace LinkRelationsMore.ServiceLayer
{
    // Unit of work class

    public class Worker : IDisposable
    {
        private ApplicationDbContext _ds = new ApplicationDbContext();
        private bool disposed = false;

        // Properties for each repository...
        // (use the "propf" code snippet)
        // Custom getters only, for each repository

        private Vehicle_repo _vehicle;

        public Vehicle_repo Vehicles
        {
            get
            {
                if (_vehicle == null)
                {
                    _vehicle = new Vehicle_repo(_ds);
                }
                return _vehicle;
            }
        }

        private Manufacturer_repo _manufacturer;

        public Manufacturer_repo Manufacturers
        {
            get
            {
                if (_manufacturer == null)
                {
                    _manufacturer = new Manufacturer_repo(_ds);
                }
                return _manufacturer;
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
