using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace FileParser
{
    public static class Constants
    {
        public const string FILE_DOES_NOT_EXIST = "File does not exist!! Please check the path";
        public const string RETURN_OK = "Ok";
        public const string EXCEPTION = "Exception :";
        public const string ERROR = "Error";
        public const string FILE_CONVERTED = "File has been converted";
        public const string START_DATE_END_DATE_NOT_PRESENT = "Start date or End date was not found in the file. File reading is complete";
    }
    class Program
    {
        static void Main(String[] args)
        {
            if (String.IsNullOrEmpty(args[0])|| String.IsNullOrEmpty(args[1])||String.IsNullOrEmpty(args[2]))
            {
                Console.WriteLine("One or more arguments are empty\n");
            }
            else
            {
                string inputFile = args[0]; //File to be processed
                string startDate = args[1]; //Start date
                string stopDate = args[2];  //Stop date 

                var messageQueue = new BlockingCollection<string>();
                var fileProcessor = new FileProcessor(messageQueue);
                //fileProcessor.SetInputFile(args[0]);//Set file path

                var fileReader = new FileReader(messageQueue);

                //Process File
                var coordinator = new FileConversionCoordinator(fileReader, fileProcessor);
                coordinator.ConvertFile(inputFile, startDate, stopDate);

            }
            
        }

    }
}
