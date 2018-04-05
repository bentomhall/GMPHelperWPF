using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMPHelperWPF.Export
{
    public interface IExporter<T>
    {
        string Marshal(IEnumerable<T> t);
    }

    public class ExportWriter
    {
        private string file;

        public ExportWriter(string storageFile)
        {
            file = storageFile;
        }

        public void WriteFile<T>(IExporter<T> exporter, IEnumerable<T> data)
        {
            var contents = exporter.Marshal(data);
            System.IO.File.WriteAllText(file, contents);
        }
    }
}
