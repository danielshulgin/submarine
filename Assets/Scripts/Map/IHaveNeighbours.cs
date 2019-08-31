using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


interface IHaveNeighbours<T>
{
    T[] Neighbours { get; set; }
}

