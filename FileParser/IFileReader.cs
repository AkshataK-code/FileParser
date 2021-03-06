using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace FileParser
{
    public interface IFileReader
    {  
        public string ReadFile(String filePath, string StartDate, string EndDate);

        public void QueueForProcessing(string Message);

        public bool GetDate(string inputString, out DateTime value);
    }
}
