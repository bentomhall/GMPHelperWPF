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
        private IStorageFile file;

        public ExportWriter(IStorageFile storageFile)
        {
            file = storageFile;
        }

        public async Task WriteFile<T>(IExporter<T> exporter, IEnumerable<T> data)
        {
            var contents = exporter.Marshal(data);
            await FileIO.WriteTextAsync(file, contents);
        }
    }
}
