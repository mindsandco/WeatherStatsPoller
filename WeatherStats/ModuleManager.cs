using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules;

namespace WeatherStats
{
    public class ModuleManager
    {
        private List<ModuleBase> modules = new List<ModuleBase>();
        private bool keepRunning;

        public void AddModule(ModuleBase module)
        {
            this.modules.Add(module);
        }

        public void StartAllModules()
        {
            this.keepRunning = true;
            foreach (var module in  this.modules)
            {
                var rs = module.StartModule();
                Debug.Assert(rs, $"Failed to start module {module.ModuleName}");
            }
        }

        public void StopAllModules()
        {
            foreach (var module in this.modules)
            {
                var rs = module.StopModule();
                Debug.Assert(rs, $"Failed to stop module {module.ModuleName}");
                module.Dispose();
            }

            modules.Clear();
            this.keepRunning = false;
        }

        public bool KeepRunnning
        {
            get { return this.keepRunning; }
        }
    }
}
