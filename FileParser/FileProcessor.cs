using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using FileParser.Models;

namespace FileParser
{
    /*
     * FileProcessor class checks for a message in the queue .Once a message arrives, it processes it
     *  
     */
    public class FileProcessor : IFileProcessor
    {
        private readonly BlockingCollection<string> messageQueue;

        private string jsonFilePath;

        /*
         * Constructor
         */
        public FileProcessor(BlockingCollection<string> messageQueue)
        {
            this.messageQueue = messageQueue;//Initialise the message queue            
        }


        /*
         * Sets the input file path
         */
        public void SetInputFile(string inputFilePath)
        {
            jsonFilePath = Path.GetDirectoryName(inputFilePath) + Path.DirectorySeparatorChar +
                                      Path.GetFileNameWithoutExtension(inputFilePath) + ".json";

            //If file already exists, delete it
            if (File.Exists(jsonFilePath))
            {
                File.Delete(jsonFilePath);
            }
        }



        /*
         * Function which checks if queue contains any message 
         * If it has received a message it calls ProcessLine() to process it further         * 
         * It also writes the processed message to a json file 
         */
        public void StartProcessing()
        {
            try
            {
                while (true)
                {
                    var message = messageQueue.Take(); //Blocks until a new message is available

                    bool outValue = false;

                    //Process message
                    var formattedString = this.ProcessLine(message, out outValue);

                    if (!outValue)
                    {
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                //Message Queue is now empty
                Console.WriteLine("Exception in StartProcessing :", e);                
            }
        }


        public void StopProcessing()
        {
            messageQueue.Dispose();
        }

        /*
         * Function to convert the line into json format
           public string ProcessLine(string line, out bool outValue) 
        */
        public string ProcessLine(string line, out bool outValue)
        {   
            try
            {
                string retVal = Constants.ERROR;
                if (!line.Equals("EOF"))
                {                    

                    //Split line on space delimiter
                    string[] lineArray = line.Split(' ');

                    //Read the split words into ProcessedOutput class object
                    int index = 0;

                    ProcessedOutput output = new ProcessedOutput();
                    foreach (string substring in lineArray)
                    {
                        if (index == 0)
                        {
                            output.eventDate = substring; //extracted date
                        }
                        else if (index == 1)
                        {
                            output.emailID = substring; //extracted emailID
                        }
                        else if (index == 2)
                        {
                            output.sessionID = substring; //extracted sessionID
                        }
                        index++;
                    }

                    //Convert the object to json string
                    string formattedString = "";

                    formattedString = JValue.Parse(JsonConvert.SerializeObject(output)).ToString(Formatting.Indented);

                    //Write to json file
                    retVal = WriteToFile(formattedString);

                    outValue = true;
                }
                else
                {
                    Console.WriteLine("End of file");
                    StopProcessing();
                    retVal = Constants.RETURN_OK;
                    outValue = false;
                }

                return retVal;

            }
            catch (Exception e)
            {
                //Message Queue is now empty
                Console.WriteLine("Exception in WriteToFile :", e);
                outValue = false;
                return Constants.EXCEPTION;
            }
        }


        /*
         * Function to write the string to json file
         */
        public string WriteToFile(string outputString)
        {
            try
            {
                string retVal = Constants.ERROR;

                if (File.Exists(jsonFilePath))
                {
                    //Append ",\n" only if this is not the first iteration of writing line to file
                    outputString = ",\n" + outputString;
                }

                List<string> dataToBeWritten = new List<string>();
                dataToBeWritten.Add(outputString);                   

                System.IO.File.AppendAllText(jsonFilePath, outputString);                    
                
                retVal = Constants.RETURN_OK;

                return retVal;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception in WriteToFile :", e);
                return Constants.EXCEPTION;
            }

        }
    }
}
