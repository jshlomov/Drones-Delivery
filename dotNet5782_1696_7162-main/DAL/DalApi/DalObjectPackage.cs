using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalObject
{
    internal partial class DalObject : DalApi.IDal
    {
        /// <summary>
        /// adding package to the data base
        /// </summary>
        public int AddPackage(Package package)
        {
            package.ID = GetPackageRunID();
            DataSource.Packages.Add(package);
            return DataSource.Config.packegeRunId; //promoted in function GetPackageRunID used in ConsoleUI
        }

        /// <summary>
        /// return package's run id and promote it in one
        /// </summary>
        private int GetPackageRunID()
        {
            return DataSource.Config.packegeRunId++;
        }

        /// <summary>
        /// assigning in the package's properties the id of the drone who will take it.
        /// </summary>
        public void AssignPackageToDrone(int droneId, int packageId)
        {
            int index = DataSource.Packages.FindIndex(i => i.ID == packageId);
            if (index == -1)
                throw new IdIsNotExistExeption("the id was not found.");
            Package p = DataSource.Packages[index];
            p.DroneId = droneId;
            p.assigning = DateTime.Now;
            DataSource.Packages[index] = p;
            return;

        }

        /// <summary>
        /// update in the package's properties that the package picked up in this moment (date and time).
        /// </summary>
        public void ColectingPackage(int packageId)
        {
            int index = DataSource.Packages.FindIndex(i => i.ID == packageId);
            if (index == -1)
                throw new IdIsNotExistExeption("the id was not found.");
            Package p = DataSource.Packages[index];
            p.PickedUp = DateTime.Now;
            DataSource.Packages[index] = p;
            return;

        }

        /// <summary>
        /// update in the package properties that the package delivered in this moment (date and time).
        /// </summary>
        public void PackageDelivering(int packageId)
        {
            int index = DataSource.Packages.FindIndex(i => i.ID == packageId);
            if (index == -1)
                throw new IdIsNotExistExeption("the id was not found.");
            Package p = DataSource.Packages[index];
            p.Delivered = DateTime.Now;
            DataSource.Packages[index] = p;
            return;
        }

        /// <summary>
        /// return specific package with the same ID.
        /// </summary>
        /// <param name="id">package's ID</param>
        /// <returns></returns>
        public Package GetPackage(int id)
        {
            int index = DataSource.Packages.FindIndex(i => i.ID == id);
            if (index == -1)
                throw new IdIsNotExistExeption("the id is not found.");
            return DataSource.Packages[index];
        }

        /// <summary>
        /// return a list with all Packages.
        /// </summary>
        public IEnumerable<Package> GetAllPackages(Predicate<Package> filter = null)
        {
            return DataSource.Packages.FindAll(x => filter == null ? true : filter(x) && x.Deleted == false);
        }

        /// <summary>
        /// delete package from the data. (not realy delete it but marking it as a "deleted".)
        /// </summary>
        /// <param name="id"></param>
        public void RemovePackage(int id)
        {
            int index = DataSource.Packages.FindIndex(x => x.ID == id);
            Package p = DataSource.Packages[index];
            p.Deleted = true;
            DataSource.Packages[index] = p;
        }
    }
}
