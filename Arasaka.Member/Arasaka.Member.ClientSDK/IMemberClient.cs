using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Arasaka.Member.ClientSDK.Models;

namespace Arasaka.Member.ClientSDK
{
    /// <summary>
    /// Member API 客戶端
    /// </summary>
    public interface IMemberClient : IMemberWriteClient, IMemberReadOnlyClient
    {


    }
}
