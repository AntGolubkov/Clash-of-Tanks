using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ClashOfTanks.GUI.Utility
{
    sealed class AssemblyChecker
    {
        private List<AssemblyName> MissingAssemblies { get; set; } = null;

        public string GetMissingAssemblies()
        {
            if (MissingAssemblies == null)
            {
                MissingAssemblies = new List<AssemblyName>();
                RecursiveCheck(Assembly.GetEntryAssembly());
            }
            
            if (MissingAssemblies.Count > 0)
            {
                StringBuilder result = new StringBuilder();

                foreach (string assemblyName in MissingAssemblies.Select(a => a.Name).OrderBy(n => n))
                {
                    if (result.Length > 0)
                    {
                        result.AppendLine();
                    }

                    result.Append($"{assemblyName}.dll");
                }

                return result.ToString();
            }
            else
            {
                return null;
            }
        }

        private void RecursiveCheck(Assembly referencingAssembly)
        {
            foreach (AssemblyName assembly in referencingAssembly.GetReferencedAssemblies())
            {
                Assembly loadedAssembly;

                try
                {
                    loadedAssembly = Assembly.ReflectionOnlyLoad(assembly.FullName);
                }
                catch (FileNotFoundException)
                {
                    MissingAssemblies.Add(assembly);
                    continue;
                }

                if (!loadedAssembly.GlobalAssemblyCache)
                {
                    RecursiveCheck(loadedAssembly);
                }
            }
        }
    }
}
