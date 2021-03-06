File Parser

Input:
This application takes 3 command line arguments.
1. Name of file to be parsed
2. Start date
3. End date

How File Parser works :
FileParser application makes use of BlockingCollection which provides an event-driven, concurrent approach that abstracts away the low-level threading details.
One thread (FileReader) reads one line at a time and adds it a queue.
The second thread (FileProcessor ) takes the added line from queue and converts it to json format.
The consumer(FileProcessor )  and producer(FileReader)  threads have a shared resource – the work queue.


A dockerFile has also been created in the application.






