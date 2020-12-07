using System;
using System.Collections.Generic;

namespace Step_1
{
    class Program
    {
        /// Old Code
        /*
        static bool RandomScore(double[] Array){
            bool add_random_score = false;
            Random rnd = new Random();

            for (int index = 0; index < Array.Length; index++){

                Array[index] = rnd.Next(1, 13);  // creates a number between 1 and 12

                if (index == (Array.Length -1) ){
                    add_random_score = true;
                }

                // Console.WriteLine(index + ": "+Array[index]); // Print values of the array
            }
            return add_random_score;
        }
        */

        //[Part 3]Randomize player score
        static double RandomScore(){
            Random rnd = new Random();
            return rnd.Next(1, 13);
        }

        // [Part 4]Add random score to database based on the players still in game and the game's number
        static bool AddRandomScore(double[,] database, int number_of_the_game, double[] PlayerScore_Array){
            
            bool add_random_score = false;

            // Update database
            for(int index = 0; index < database.GetLength(0); index++){

                // Check if the player was not eliminated
                if(PlayerScore_Array[index] >= 0){
                    database[index, number_of_the_game] = RandomScore();
                }
                
                // Check end of the task
                if (index == (database.GetLength(0)-1) ){
                    add_random_score = true;
                }
            }

            //Update PlayerScore_Array
            for (int index_array = 0; index_array < PlayerScore_Array.Length; index_array++){
                double sum_score = 0;
                for (int index_database=0; index_database <= number_of_the_game; index_database++){
                    sum_score += database[index_array,index_database];
                }
                PlayerScore_Array[index_array] = sum_score/(number_of_the_game+1);
            }


            return add_random_score;
        }





        static void Main()
        {

            // [Part 1]Creation of the array which defined the score of a player
            double[] PlayerScore_Array = new double[100];
                // Initialize score to 0 for each player
            for (int index = 0; index < PlayerScore_Array.Length; index++){
                PlayerScore_Array[index] = 0;
            }
            DisplayArray(PlayerScore_Array);
            
            // [Part 2]Creation Database
            double[,] database = new double[100,32];
                // Initialize database
            for (int index_row=0; index_row < database.GetLength(0); index_row++){
                for (int index_column=0; index_column < database.GetLength(1); index_column++){
                    database[index_row, index_column] = 0;
                }
            }
            
                // Creation resultats of the first game
            AddRandomScore(database, 0, PlayerScore_Array);
                // Display database
            //DisplayDatabase(database);
            Console.WriteLine();
                // Creation resultats of the first game
            AddRandomScore(database, 1, PlayerScore_Array);
                // Display database
            DisplayDatabase(database);
            Console.WriteLine();
            DisplayArray(PlayerScore_Array);
        }


        static void DisplayArray(double[] PlayerScore_Array){
            // for (int index = 0; index < PlayerScore_Array.Length; index++){
            //     if(index%10 == 0){
            //         Console.WriteLine();
            //     }
            //     if(index>=10){
            //         Console.Write(index + " ");
            //     }
            //     else{
            //         Console.Write(index + "  ");
            //     }
            // }
            Console.WriteLine();
            for (int index = 0; index < PlayerScore_Array.Length; index++){
                if(index%10 == 0){
                    Console.WriteLine();
                }
                if(PlayerScore_Array[index]>=10){
                    Console.Write(PlayerScore_Array[index] + " ");
                }
                else{
                    Console.Write(PlayerScore_Array[index] + "  ");
                }
            }
            Console.WriteLine();
        }
        
        static void DisplayDatabase(double[,] database){
            for (int index_row=0; index_row < database.GetLength(0); index_row++){
                for (int index_column=0; index_column < database.GetLength(1); index_column++){

                    if(database[index_row, index_column]>=10){
                        Console.Write(database[index_row, index_column] + " " );
                    }
                    else{
                        Console.Write(database[index_row, index_column] + "  " );
                    }
                }
                Console.WriteLine();
            }
        }

    }

}

