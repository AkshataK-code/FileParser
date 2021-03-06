using System;
using System.Collections.Generic;
using System.Text;

namespace FileParser
{
    public interface IFileProcessor
    {
        void StartProcessing();

        public void StopProcessing();

        public string ProcessLine(string line, out bool outValue);

        public string WriteToFile(string outputString);

        public void SetInputFile(string inputFilePath);
    }
}
