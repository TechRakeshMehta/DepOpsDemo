#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  Net Data Contract Serializer OperationBehavior.cs
// Purpose:   Net Data Contract Serializer OperationBehavior
//

#endregion


#region Namespace

#region System defined

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Xml;

#endregion

#endregion

namespace SPI
{
    /// <summary>
    /// Net DataContract Serializer OperationBehavior class
    /// </summary>
    public class SysXNetDataContractSerializerOperationBehavior :

        DataContractSerializerOperationBehavior
    {
        /// <summary>
        /// Net DataContract Serializer OperationBehavior
        /// </summary>
        /// <param name="operationDescription">OperationDescription</param>
        public SysXNetDataContractSerializerOperationBehavior(

            OperationDescription operationDescription) :

            base(operationDescription) { }
        /// <summary>
        /// CreateSerializer
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="name">String</param>
        /// <param name="ns">String</param>
        /// <param name="knownTypes">IList(Type)</param>
        /// <returns>XmlObjectSerializer</returns>
        public override XmlObjectSerializer CreateSerializer(

            Type type, String name, String ns, IList<Type> knownTypes)
        {

            return new NetDataContractSerializer();

        }


        /// <summary>
        /// CreateSerializer
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="name">XmlDictionaryString</param>
        /// <param name="ns">XmlDictionaryString</param>
        /// <param name="knownTypes">IList(Type)</param>
        /// <returns></returns>
        public override XmlObjectSerializer CreateSerializer(Type type,

            XmlDictionaryString name, XmlDictionaryString ns,

            IList<Type> knownTypes)
        {

            return new NetDataContractSerializer();

        }
        /// <summary>
        /// Registers a NetDataContractSerializer behavior for a service host.
        /// </summary>
        /// <param name="serviceHost">ServiceHostBase</param>
        public static void RegisterNetDataContractSerializerBehavior(ServiceHostBase serviceHost)
        {
            foreach (ServiceEndpoint endPoint in serviceHost.Description.Endpoints)
            {
                foreach (OperationDescription desc in endPoint.Contract.Operations)
                {
                    DataContractSerializerOperationBehavior dcsOperationBehavior = desc.Behaviors.Find<DataContractSerializerOperationBehavior>();
                    if (dcsOperationBehavior != null)
                    {
                        int idx = desc.Behaviors.IndexOf(dcsOperationBehavior);
                        desc.Behaviors.Remove(dcsOperationBehavior);
                        desc.Behaviors.Insert(idx, new SysXNetDataContractSerializerOperationBehavior(desc));
                    }
                }

            }
        }
        /// <summary>
        /// Creates a client NetDataContractSerializer channel.
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="EndpointName"></param>
        /// <returns></returns>
        public static T CreateClientChannel<T>(String EndpointName)
        {
            ChannelFactory<T> factory = new ChannelFactory<T>(EndpointName);

            foreach (OperationDescription desc in factory.Endpoint.Contract.Operations)
            {
                DataContractSerializerOperationBehavior dcsOperationBehavior = desc.Behaviors.Find<DataContractSerializerOperationBehavior>();
                if (dcsOperationBehavior != null)
                {
                    int idx = desc.Behaviors.IndexOf(dcsOperationBehavior);
                    desc.Behaviors.Remove(dcsOperationBehavior);
                    desc.Behaviors.Insert(idx, new SysXNetDataContractSerializerOperationBehavior(desc));
                }
            }

            return factory.CreateChannel();
        }
    }
}
