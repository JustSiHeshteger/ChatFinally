using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore.Model
{
    [Serializable]
    internal class BaseUser
    {
        private string _userName;
        private string _id;

        public string UserName { get => _userName; set => _userName = value; }
        public string Id { get => _id; set => _id = value; }
    }
}
