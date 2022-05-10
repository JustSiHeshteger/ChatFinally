using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatNaFive.Model
{
    [Serializable]
    internal class BaseUser
    {
        private string _userName;
        public string UserName { get; set; }
    }
}
