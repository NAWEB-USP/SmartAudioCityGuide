using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAudioCityGuide.Services
{
    public interface ICodeServices : IDisposable
    {
        string findFirstCode();
    }
}