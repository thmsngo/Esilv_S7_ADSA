using System;
using System.Collections.Generic;

namespace Step_1
{
    class Program
    {
        //[Part 3] Randomize player score
        static double RandomScore(){
            Random rnd = new Random();
            return rnd.Next(1, 13);
        }

        // [Part 4] Add random score to database based on the players still in game and the game's number
        static bool AddRandomScore(double[,] database, int number_of_the_game, double[] playerscore_array){
            
            bool add_random_score = false;

            // Update database
            for(int index = 0; index < database.GetLength(0); index++){

                // Check if the player was not eliminated
                if(playerscore_array[index] >= 0){
                    database[index, number_of_the_game] = RandomScore();
                }
                
                // Check end of the task
                if (index == (database.GetLength(0)-1) ){
                    add_random_score = true;
                }
            }

            //Update playerscore_array
            for (int index_array = 0; index_array < playerscore_array.Length; index_array++){
                if(playerscore_array[index_array] >= 0){
                    double sum_score = 0;
                    for (int index_database=0; index_database <= number_of_the_game; index_database++){
                        sum_score += database[index_array,index_database];
                    }
                    playerscore_array[index_array] = sum_score/(number_of_the_game+1);
                }
            }

            return add_random_score;
        }
        // [Part 5] Create random games based on the database
        static int[,] Games_Random(double[,] database, double[] playerscore_array){
            Random rnd=new Random();
            int counter = 0;
            // Count the number of players still in the game
            foreach(double score in playerscore_array){
                if(score >= 0){
                    counter++;
                }
            }

            //Retrieves the indexes of the players who are playing
            int[] players = new int[counter];
            int cmpt = 0;
            for(int index = 0; index <= database.GetLength(0); index++){
                if (playerscore_array[index] >= 0){
                    players[cmpt] = index;
                    cmpt++;
                }
            }
            
            //Mix value of sorted players
            for (int i = 0; i < players.Length ; i++)
            {
                int j = rnd.Next(i, players.Length);
                int temp = players[i];
                players[i] = players[j];
                players[j] = temp;
            } 

            // Create and store the games in a matrix
            int[,] games_random = new int[Convert.ToInt32(counter/10), 10];
            cmpt = 0;
            for(int index_row = 0; index_row < games_random.GetLength(0); index_row++){
                for(int index_column = 0; index_column < games_random.GetLength(1); index_column++){
                    games_random[index_row, index_column] = players[cmpt];
                    cmpt++;
                }
            }

            return games_random;
        }
        // [Part 6] Create games based on ranking
        static int[,] Games_Ranking(double[] playerscore_array){

            // Count the number of players still in the game
            int counter = 0;
            foreach(double score in playerscore_array){
                if(score >= 0){
                    counter++;
                }
            }

            // Copy array's values in a matrix
            int count_index = 0;
            double[,] sort_ranking = new double[counter,2];
            for(int index = 0; index < sort_ranking.GetLength(0); index++){
                if(playerscore_array[index] >= 0){
                    sort_ranking[count_index, 0] = index;
                    sort_ranking[count_index, 1] = playerscore_array[index];
                    count_index++;
                }
            }

            // Sort the matrix
            double temp1, temp2;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 9-i; j++)
                {
                    if (sort_ranking[j, 1] < sort_ranking[j + 1, 1]) // column 1 entry comparison
                    {
                    temp1 = sort_ranking[j, 0];              // swap both column 0 and column 1
                    temp2 = sort_ranking[j, 1];

                    sort_ranking[j, 0] = sort_ranking[j+1, 0];
                    sort_ranking[j, 1] = sort_ranking[j+1, 1];

                    sort_ranking[j+1, 0] = temp1;
                    sort_ranking[j+1, 1] = temp2;
                    }
                }
            }

            // Create and store the games in a matrix
            int[,] games_ranking = new int[Convert.ToInt32(counter/10), 10];
            int cmpt = 0;
            for(int index_row = 0; index_row < games_ranking.GetLength(0); index_row++){
                for(int index_column = 0; index_column < games_ranking.GetLength(1); index_column++){
                    games_ranking[index_row, index_column] = Convert.ToInt32(sort_ranking[cmpt,1]);
                    cmpt++;
                }
            }

            return games_ranking;
        }

        // Remove 10 worse ranked players
        static void Remove_Players(double[] playerscore_array){
            // Count the number of players still in the game
            int counter = 0;
            for (int index = 0; index < playerscore_array.Length; index++){
                if(playerscore_array[index] >= 0){
                    counter++;
                }
            }

            //Create a copy of the array including only scores of non eliminated players and sort this array
            double[] sorted = new double[counter];
            counter = 0;
            for (int index = 0; index < playerscore_array.Length; index++){
                if(playerscore_array[index] >= 0){
                    sorted[counter] = playerscore_array[index];
                    counter++;
                }
            }
            Array.Sort(sorted);

            bool finded_value;
            //Console.Write("Eliminated players: ");
            for(int index= 0; index < 10; index++){
                finded_value = false;
                for (int index_array = 0; (index_array < playerscore_array.Length) && (finded_value==false) ; index_array++){
                    if(playerscore_array[index_array] == sorted[index]){
                        playerscore_array[index_array] = -1;
                        //Console.Write(index_array+" ");
                        finded_value = true;
                    }
                }
            }
        }

        // [Part 7] Drop the players and play game until the last 10 players
        static void Qualification(double[,] database, double[] playerscore_array){
            
            int game_number = 0;
            int round_number = 1;
            //int num_eliminated = 0;
            int[] eliminated = new int[90];

            for(int index_round = 0; index_round < 9; index_round++){
                //Round n
                for(int index=0; index < 3; index++){
                    AddRandomScore(database, game_number, playerscore_array);
                    game_number++;
                }
                //Console.WriteLine("\nRound number: " + round_number);
                Remove_Players(playerscore_array);
                round_number++;
            }

            int count = 0;
            Console.Write("Remaining players: ");
            for (int i = 0; i < playerscore_array.Length; i++){
                if (playerscore_array[i] == (-1)){
                    count++;
                }
                else{
                    Console.Write(i+" ");
                }
            }
        }

        // [Part 8] Display the TOP10 players and the podium after the final game
        static void Final(double[,] database, double[] playerscore_array){
            // Reset scores
            for (int index = 0; index < playerscore_array.Length; index++){
                if(playerscore_array[index] >=0){
                    playerscore_array[index] = 0;
                }   
            }
            int game_number = 27;
            
            // Play five games
            for(int index_round = 0; index_round < 5; index_round++){
                AddRandomScore(database, game_number, playerscore_array);
                game_number++;
            }
            
            // Copy array's values in a matrix
            int count_index = 0;
            double[,] sort_ranking = new double[10,2];
            for(int index = 0; index < playerscore_array.GetLength(0); index++){
                if(playerscore_array[index] >= 0){
                    sort_ranking[count_index, 0] = index;
                    sort_ranking[count_index, 1] = playerscore_array[index];
                    count_index++;
                }
            }
            // Sort the matrix
            double temp1, temp2;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < (9-i); j++)
                {
                    if (sort_ranking[j, 1] < sort_ranking[j + 1, 1]) // column 1 entry comparison
                    {
                    temp1 = sort_ranking[j, 0];              // swap both column 0 and column 1
                    temp2 = sort_ranking[j, 1];

                    sort_ranking[j, 0] = sort_ranking[j+1, 0];
                    sort_ranking[j, 1] = sort_ranking[j+1, 1];

                    sort_ranking[j+1, 0] = temp1;
                    sort_ranking[j+1, 1] = temp2;
                    }
                }
            }

            // Display Ranking
            Console.WriteLine("\n\nFinal Ranking:");
            for(int i = 0; i < sort_ranking.GetLength(0); i++){
                if(i==9){
                    Console.Write((i+1)+"| Player ");
                    if(sort_ranking[i,0]>=10){
                        Console.Write(sort_ranking[i,0]+" with a score of "+String.Format("{0:0.00}", sort_ranking[i,1]) + "\n");
                    }
                    else{
                        Console.Write(" "+sort_ranking[i,0]+" with a score of "+String.Format("{0:0.00}", sort_ranking[i,1]) + "\n");
                    }
                }
                else{
                    Console.Write(" "+(i+1)+"| Player ");
                    if(sort_ranking[i,0]>=10){
                        Console.Write(sort_ranking[i,0]+" with a score of "+String.Format("{0:0.00}", sort_ranking[i,1]) + "\n");
                    }
                    else{
                        Console.Write(" "+sort_ranking[i,0]+" with a score of "+String.Format("{0:0.00}", sort_ranking[i,1]) + "\n");
                    }
                }
            }
            
        }

        static void Main()
        {
            // [Part 1] Creation of the array which defined the score of a player
            double[] playerscore_array = new double[100];
                // Initialize score to 0 for each player
            for (int index = 0; index < playerscore_array.Length; index++){
                playerscore_array[index] = 0;
            }
            
            //DisplayArray(playerscore_array);
            
            // [Part 2] Creation Database
            double[,] database = new double[100,32]; // Because we have 100 players and 32 games maximum
                // Initialize database: 0 to each box
            for (int index_row=0; index_row < database.GetLength(0); index_row++){
                for (int index_column=0; index_column < database.GetLength(1); index_column++){
                    database[index_row, index_column] = 0;
                }
            }
            // Drop the players and play game until remain last 10 players
            Qualification(database,playerscore_array);
            // Display the TOP10 players and the podium after the final game
            Final(database, playerscore_array);
            
        }


        static void DisplayArray(double[] playerscore_array){
            for (int index = 0; index < playerscore_array.Length; index++){
                if(index%20 == 0){
                    Console.WriteLine();
                }
                if(playerscore_array[index]>=10){
                    Console.Write(String.Format("{0:0.00}", playerscore_array[index]) + " ");
                }
                else{
                    Console.Write(String.Format("{0:0.00}", playerscore_array[index]) + "  ");
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

