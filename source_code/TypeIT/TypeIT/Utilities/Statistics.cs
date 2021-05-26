using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeIT.Utilities
{
    class Statistics
    {
        double WPM = 0;
        double average = 0;
        int mistakes = 0;
        ConsoleKeyInfo keyInfo;
        string text = "hi"; //hard coded should be changed later.
        StringBuilder stringBuilder = new StringBuilder();
        int totalWordsWritten = 0; //this should be incremented everytime a word is written. Could be wrong
         

        /*
         * This class will be hardcoded but will be changed later with the actual documents when parsed.  
         */
        private double wordsPerMinute(double minutes, double words) //this will not have input through the method
        {
            WPM =  minutes / words;
            return WPM; //thsi will be stored in the .TypeIT user. 
        }

        private void mistakesDone()
        {
            for (int i = 0; i < text.Length; i++)
            {
                stringBuilder.Append(keyInfo.Key);

                if (keyInfo.Equals(text[i]))
                {
                    
                } else
                {
                    mistakes++; //when writing the document, and a mistake is done, this should be added into the .TypeIT user statistics.
                }
            }            
        }

        private double averageSpeed(double time)
        {
            average =  WPM / time;
            return average;  //this will be stored in the .TypeIT user. 
        }

        private int accuracy()
        {
             int accuracy = totalWordsWritten / mistakes;
            return accuracy; 
        } 
    }
}
