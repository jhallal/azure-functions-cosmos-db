using AFC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFC.Services
{
    public interface IDriverService
    {
        List<Driver> GetDriversByCity(string city);
    }
}
