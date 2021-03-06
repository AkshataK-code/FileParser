using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileParser;
using System.Collections.Concurrent;

namespace FileConversionCoordinatorTests
{
    [TestClass]
    public class FileConversionCoordinatorTests
    {
        private BlockingCollection<string> messageQueue;
        private FileProcessor fileProcessorMoq;
        private FileReader fileReaderMoq;
        FileConversionCoordinatorTests()
        {
            messageQueue = new BlockingCollection<string>();
            fileProcessorMoq = new FileProcessor(messageQueue);
            fileReaderMoq = new FileReader(messageQueue);
        }

        [TestMethod]
        public void CanProcessSmallFile()
        {
            string inputFile = @"..\Mock\sample1.txt"; //File to be processed
            string startDate = "1990-01-03"; //Start date
            string stopDate = "1990-01-07";  //Stop date 

            fileProcessorMoq.SetInputFile(inputFile);

            var coordinator = new FileConversionCoordinator(fileReaderMoq, fileProcessorMoq);
            string returnVal = coordinator.ConvertFile(inputFile, startDate, stopDate);
            Assert.AreEqual(Constants.FILE_CONVERTED, returnVal);
        }

        [TestMethod]
        public void CanProcessMediumFile()
        {
            string inputFile = @"..\Mock\sample2.txt"; //File to be processed
            string startDate = "1990-01-03"; //Start date
            string stopDate = "1990-01-12";  //Stop date 

            fileProcessorMoq.SetInputFile(inputFile);

            var coordinator = new FileConversionCoordinator(fileReaderMoq, fileProcessorMoq);
            string returnVal = coordinator.ConvertFile(inputFile, startDate, stopDate);
            Assert.AreEqual(Constants.FILE_CONVERTED, returnVal);
        }


        [TestMethod]
        public void CanProcessBigFile()
        {
            string inputFile = @"..\Mock\sample3.txt"; //File to be processed
            string startDate = "2000-06-04"; //Start date
            string stopDate = "2002-12-11";  //Stop date 

            fileProcessorMoq.SetInputFile(inputFile);

            var coordinator = new FileConversionCoordinator(fileReaderMoq, fileProcessorMoq);
            string returnVal = coordinator.ConvertFile(inputFile, startDate, stopDate);
            Assert.AreEqual(Constants.FILE_CONVERTED, returnVal);
        }

        public void TestWhenStartDateIsNotPresent()
        {
            string inputFile = @"..\Mock\sample1.txt"; //File to be processed
            string startDate = ""; //Start date
            string stopDate = "1990-01-07";  //Stop date 

            fileProcessorMoq.SetInputFile(inputFile);

            var coordinator = new FileConversionCoordinator(fileReaderMoq, fileProcessorMoq);
            string returnVal = coordinator.ConvertFile(inputFile, startDate, stopDate);
            Assert.AreEqual(Constants.START_DATE_END_DATE_NOT_PRESENT, returnVal);
        }

        public void TestWhenStopDateIsOutOfRange()
        {
            string inputFile = @"..\Mock\sample1.txt"; //File to be processed
            string startDate = "1990-01-01"; //Start date
            string stopDate = "1990-01-07";  //Stop date 

            fileProcessorMoq.SetInputFile(inputFile);

            var coordinator = new FileConversionCoordinator(fileReaderMoq, fileProcessorMoq);
            string returnVal = coordinator.ConvertFile(inputFile, startDate, stopDate);
            Assert.AreEqual(Constants.START_DATE_END_DATE_NOT_PRESENT, returnVal);
        }

        public void TestWhenFileIsNotPresent()
        {
            string inputFile = @"..\Mock\sample10.txt"; //File to be processed
            string startDate = "1990-01-01"; //Start date
            string stopDate = "1990-01-07";  //Stop date 

            fileProcessorMoq.SetInputFile(inputFile);

            var coordinator = new FileConversionCoordinator(fileReaderMoq, fileProcessorMoq);
            string returnVal = coordinator.ConvertFile(inputFile, startDate, stopDate);
            Assert.AreEqual(Constants.FILE_DOES_NOT_EXIST, returnVal);
        }
    }
}
