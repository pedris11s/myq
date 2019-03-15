using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyQ
{
    public class User
    {
        public User(){}

        public User(string nam, string pass)
        {
            Name = nam;
            Pass = pass;
        }

        public string Name { get; set; }
        public string Pass { get; set; }
        public string Restante { get; set; }
        public string Consumida { get; set; }
        public string Total { get; set; }
        public float RestanteFloat { get; set; }
        public float ConsumidaFloat { get; set; }
        public float TotalFloat { get; set; }

    }
}
