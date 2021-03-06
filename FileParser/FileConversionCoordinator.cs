using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FileParser
{
    public class FileConversionCoordinator
    {
        private readonly IFileReader fileReader;
        private readonly IFileProcessor fileProcessor;


        public FileConversionCoordinator(IFileReader fileReader, IFileProcessor fileProcessor)
        {
            //Initialise the file reader and file processor handles
            this.fileReader = fileReader;
            this.fileProcessor = fileProcessor;
        }

        //Handles the file read and file processing functions
        public string ConvertFile(string inputFile, string startDate, string stopDate)
        {
            try
            {
                //Start the file processor in another thread
                Task.Run(() =>
                {
                    fileProcessor.StartProcessing();
                });

                Console.WriteLine("Parser has started working..\n ");

                fileProcessor.SetInputFile(inputFile);//Set file path

                fileReader.ReadFile(inputFile, startDate, stopDate);//Read file

                return Constants.FILE_CONVERTED;
            }
            catch (Exception e)
            {               
                //Log the exception
                Console.WriteLine("Exception in ConvertFile :", e);

                return Constants.ERROR;
            }

        }
    }
}
