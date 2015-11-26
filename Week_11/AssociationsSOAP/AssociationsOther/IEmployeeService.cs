using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AssociationsOther
{
    // Attention - ServiceContract attribute is required on the interface (or class)
    [ServiceContract]
    public interface IEmployeeService
    {
        // Attention - OperationContract attribute is required on the method
        [OperationContract]
        string HelloWorld();

        [OperationContract]
        IEnumerable<Controllers.EmployeeBase> AllEmployees();

        [OperationContract]
        Controllers.EmployeeBase EmployeeById(int? id);

        [OperationContract]
        Controllers.EmployeeBase AddEmployee(Controllers.EmployeeAdd newItem);
    }
}
