using System;
using System.Collections.Generic;
using FUTWatcher.API.Models;

namespace FUTWatcher.API.Data.Interfaces {
    public interface INationRepository : IRepository<Nation>
    {
        IEnumerable<Nation> GetAllOrdered();
    }
}