using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileParser
{
    /*
     * FileReader class reads the file one line at a time based on start date and end date
     * and adds it to a message queue.
     *  
     */
    public class FileReader : IFileReader
    {
        //Message queue for File Reader
        private readonly BlockingCollection<string> messageQueue;
        
        public FileReader(BlockingCollection<string> messageQueue)
        {
            this.messageQueue = messageQueue;//Initialise the message queue
        }

        /*
         * Function to read the given file one line at a time based on start date and end date (inclusive)
         * and adds it to a queue 
         * Parameters 
         * path : Input Paramater
         * Contains the file path
         * 
         * StartDate : Input Parameter
         * Contains the start date
         * 
         * EndDate : Input Parameter
         * Contains the end date
         */
        public string ReadFile(String filePath, string StartDate, string EndDate)
        {
            try
            {
                string retString = Constants.ERROR;

                //Check if the file exists
                if (File.Exists(filePath))
                {
                    if (new FileInfo(filePath).Length == 0)
                    {
                        Console.WriteLine("File is empty\n");//Log the error
                    }
                    else
                    {
                        String line = "";

                        DateTime start = DateTime.Parse(StartDate);// convert string to DateTime format
                        DateTime end = DateTime.Parse(EndDate);

                        DateTime value = new DateTime();

                        using (StreamReader reader = new StreamReader(filePath))
                        {
                            //Read the file one line at a time
                            while ((line = reader.ReadLine()) != null)
                            {

                                //Extract date from the read line
                                if (GetDate(line, out value))
                                {
                                    if (value < start)//If date is less than the start date, move to the next line in file
                                    {
                                        continue;
                                    }
                                    else if (value > end)//If date is greater than the end date, stop processing the file
                                    {
                                        Console.WriteLine("File reading is complete\n");
                                        retString = Constants.RETURN_OK;

                                        //Add EOF to indicate End Of File
                                        QueueForProcessing("EOF");
                                        break;
                                    }
                                    else
                                    {
                                        //Add the read line to the queue
                                        QueueForProcessing(line);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Error in date format in the read line");//Log the error
                                    retString = Constants.ERROR;
                                }
                            }
                            if (value < end)//The end date parameter is less than the end date available in the file
                            {
                               
                                
                                //End date was not found in the file
                                Console.WriteLine("Start date or End date was not found in the file. File reading is complete\n");
                                retString = Constants.START_DATE_END_DATE_NOT_PRESENT;
                                

                                //Add EOF to indicate End Of File
                                QueueForProcessing("EOF");

                                
                            }
                        }
                    }            
                }
                else
                {
                    Console.WriteLine("File does not exist!! Please check the path");//Log the error
                    retString = Constants.FILE_DOES_NOT_EXIST;
                }
                return retString;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception :", e);                
                return Constants.EXCEPTION;
            }
        }


       /*
       * Function to add the message to the queue
       */
       public void QueueForProcessing(string Message)
       {
            //Produce new messages by adding them to the BlockingCollection
            messageQueue.Add(Message);
       }

       /*
       * Function extracts date from the string and returns true if string has been successfully converted to DateTime format.
       * It has a out parameter which contains the converted value.
       * It returns false otherwise.
       * Input string format is assumed to be as this example 
       * "1990-01-01T02:29:00Z kelsi_brekke@schuppe.biz 21728403-8671-429f-99bd-404936312a55"
       * 
       * Parameters 
       * inputString : Input Paramater
       * Contains the line read from the file
       * 
       * value : Out Parameter
       * Contains the converted string in DateTime format
       */
        public bool GetDate(string inputString, out DateTime value)
        {            
            value = default; //Initialise value                      
            bool retVal = false; //Initialise to false
            string[] words = inputString.Split('T'); //Split the string to get the date 

            foreach (var word in words)
            {
                //read the first extracted word
                DateTime output;
                retVal = DateTime.TryParse(word, out output);
                value = output; //Output date
                break;//Read the first extracted string and break out of the loop
            }
            return retVal;//Tells whether string has been successfully converted
        }
    }
}
