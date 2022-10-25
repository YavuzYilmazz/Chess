using System;
using System.IO;

namespace project_3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();
            char[,] board = new char[8, 8];
            int counter = 0;
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    board[i, j] = '.';
                }
            }
            //red
            board[0, 0] = 'r'; board[0, 1] = 'n'; board[0, 2] = 'b'; board[0, 3] = 'q'; board[0, 4] = 'k'; board[0, 5] = 'b'; board[0, 6] = 'n'; board[0, 7] = 'r';
            board[1, 0] = 'p'; board[1, 1] = 'p'; board[1, 2] = 'p'; board[1, 3] = 'p'; board[1, 4] = 'p'; board[1, 5] = 'p'; board[1, 6] = 'p'; board[1, 7] = 'p';
            //blue
            board[7, 0] = 'R'; board[7, 1] = 'N'; board[7, 2] = 'B'; board[7, 3] = 'Q'; board[7, 4] = 'K'; board[7, 5] = 'B'; board[7, 6] = 'N'; board[7, 7] = 'R';
            board[6, 0] = 'P'; board[6, 1] = 'P'; board[6, 2] = 'P'; board[6, 3] = 'P'; board[6, 4] = 'P'; board[6, 5] = 'P'; board[6, 6] = 'P'; board[6, 7] = 'P';

            char[] coordinate_row = { '8', '7', '6', '5', '4', '3', '2', '1' };
            char[] coordinate_col = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };

            //starting location
            int cursorx = 5, cursory = 17;
            bool string_mod = false;

            //This program works via flag. 
            //The flag changes to true when selecting which piece move. 
            //It changes to false after showing places where it can move. 
            //After selecting where the piece moves, it changes to true if the motion follows the rules.
            bool flag = false;

            string WhiteMove = "";
            string BlackMove = "";
            string White = "";
            string Black = "";
            string Move = "";

            int s_board_col = 0, s_board_row = 0, s_cursorx = 0, s_cursory = 0, s_temp_cursorx = 0, s_temp_cursory = 0;

            //Cursorx and cursory indicate where the cursor is momentarily, temp_cursorx and temp_cursory indicate where the selected track is.
            //This temp_ notation has been implemented for all related variables.

            int board_col = (cursorx - 5) / 4;
            int board_row = (cursory - 3) / 2;

            int temp_col = board_col;
            int temp_row = board_row;

            int temp_cursorx = cursorx;
            int temp_cursory = cursory;

            char temp = board[board_row, board_col];

            bool control_check_blue = false;
            bool control_check_red = false;

            int check_cursorx = 0;
            int check_cursory = 0;

            bool en_passant = false;
            bool mode_play = true;
            bool mode_demo = false;
            bool castling_blue = true;
            bool castling_red = true;
            bool temp_castling_blue = castling_blue;
            bool temp_castling_red = castling_red;
            bool hint = false;
            int temp_counter_for_en_passant = 0;
            do
            {
                bool temp_control_check_blue = false;
                bool temp_control_check_red = false;
                bool show_check_blue = false;
                bool show_check_red = false;
                bool hint_capture = false;

                int x1 = 5; // left border
                int x2 = 33; // right border
                int y1 = 3; // upper border
                int y2 = 17; // bottom border


                bool capture = false;
                bool passant_done = false;
                bool queenside_castling = false;
                bool kingside_castling = false;
                bool temp_capture = capture;
                bool temp_passant_done = passant_done;
                bool temp_queenside_castling = queenside_castling;
                bool temp_kingside_castling = kingside_castling;

                bool promotion = false;
                bool temp_promotion = promotion;

                int counter_for_hint = 0;
                int temp_counter = counter;
                int temp_counter_for_check = temp_counter;
                ConsoleKeyInfo move_cursor;
                ConsoleKeyInfo move_piece;
                ConsoleKeyInfo read;
                Console.SetCursorPosition(0, 0);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("HINT-'H' DEMO MOD -'ESC' PLAY MOD -'BACKSPACE' STRING MOD 'SPACE'");

                //properties of the selected piece
                if (string_mod == true || mode_demo == true)
                {
                    board_col = s_board_col;
                    board_row = s_board_row;
                    temp_col = s_board_col;
                    temp_row = s_board_row;
                    cursorx = s_cursorx;
                    cursory = s_cursory;
                    temp_cursorx = s_temp_cursorx;
                    temp_cursory = s_temp_cursory;
                    temp = board[board_row, board_col];
                }
                else if (string_mod == false && mode_demo == false)
                {
                    board_col = (cursorx - 5) / 4;
                    board_row = (cursory - 3) / 2;
                    temp_col = board_col;
                    temp_row = board_row;
                    temp_cursorx = cursorx;
                    temp_cursory = cursory;
                    temp = board[board_row, board_col];
                }
                // If the user pressed 'H', which will using for ask hint, then the program will continue from here
                if (hint == true)
                {
                    char[] pieces = new char[6];

                    if (temp_counter % 2 == 0)
                    {
                        pieces[0] = 'P'; pieces[1] = 'R'; pieces[2] = 'N'; pieces[3] = 'B'; pieces[4] = 'Q'; pieces[5] = 'K';
                    }
                    else
                    {
                        pieces[0] = 'p'; pieces[1] = 'r'; pieces[2] = 'n'; pieces[3] = 'b'; pieces[4] = 'q'; pieces[5] = 'k';
                    }
                    for (int i = 0; i < pieces.Length; i++)
                    {
                        for (int j = 0; j < board.GetLength(0); j++)
                        {
                            for (int k = 0; k < board.GetLength(1); k++)
                            {
                                // Trying to find out which piece is in this location ( location depends to the chess board ) 
                                if (pieces[i] == board[j, k])
                                {
                                    int[,] coordinate = new int[2, 50];
                                    int hint_cursorx = 4 * k + 5;
                                    int hint_cursory = 2 * j + 3;
                                    char hint_temp = board[j, k];

                                    for (int l = 0; l < coordinate.GetLength(1); l++)
                                    {
                                        // Writing a capture situation depending to the piece
                                        if ((hint_temp == 'R' || hint_temp == 'r') && rook(board, hint_cursorx, hint_cursory, hint_capture).Item2 == true)
                                        {

                                            coordinate[0, l] = rook(board, hint_cursorx, hint_cursory, hint_capture).Item1[0, l];
                                            coordinate[1, l] = rook(board, hint_cursorx, hint_cursory, hint_capture).Item1[1, l];

                                        }
                                        else if ((hint_temp == 'N' || hint_temp == 'n') && knight(board, hint_cursorx, hint_cursory, hint_capture).Item2 == true)
                                        {

                                            coordinate[0, l] = knight(board, hint_cursorx, hint_cursory, hint_capture).Item1[0, l];
                                            coordinate[1, l] = knight(board, hint_cursorx, hint_cursory, hint_capture).Item1[1, l];
                                        }
                                        else if ((hint_temp == 'B' || hint_temp == 'b'))
                                        {

                                            coordinate[0, l] = bishop(board, hint_cursorx, hint_cursory, hint_capture).Item1[0, l];
                                            coordinate[1, l] = bishop(board, hint_cursorx, hint_cursory, hint_capture).Item1[1, l];
                                        }
                                        else if ((hint_temp == 'Q' | hint_temp == 'q') && queen(board, hint_cursorx, hint_cursory, hint_capture).Item2 == true)
                                        {

                                            coordinate[0, l] = queen(board, hint_cursorx, hint_cursory, hint_capture).Item1[0, l];
                                            coordinate[1, l] = queen(board, hint_cursorx, hint_cursory, hint_capture).Item1[1, l];
                                        }
                                        else if ((hint_temp == 'K' || hint_temp == 'k') && king(board, hint_cursorx, hint_cursory, castling_blue, castling_red, control_check_blue, control_check_red, hint_capture).Item2 == true)
                                        {

                                            coordinate[0, l] = king(board, hint_cursorx, hint_cursory, castling_blue, castling_red, control_check_blue, control_check_red, hint_capture).Item1[0, l];
                                            coordinate[1, l] = king(board, hint_cursorx, hint_cursory, castling_blue, castling_red, control_check_blue, control_check_red, hint_capture).Item1[1, l];
                                        }
                                        else if ((hint_temp == 'P' || hint_temp == 'p') && pawn(board, hint_cursorx, hint_cursory, hint_temp, en_passant, hint_capture).Item2 == true)
                                        {

                                            coordinate[0, l] = pawn(board, hint_cursorx, hint_cursory, hint_temp, en_passant, hint_capture).Item1[0, l];
                                            coordinate[1, l] = pawn(board, hint_cursorx, hint_cursory, hint_temp, en_passant, hint_capture).Item1[1, l];
                                        }

                                        if (coordinate[0, l] != 0)
                                        {
                                            int hint_board_col = (coordinate[0, l] - 5) / 4;
                                            int hint_board_row = (coordinate[1, l] - 3) / 2;
                                            if (board[hint_board_row, hint_board_col] != '.')
                                            {
                                                string hint_str = "";
                                                string capt = "x";
                                                string capt_location = Convert.ToString(coordinate_col[hint_board_col]);
                                                counter_for_hint += 10;

                                                if (hint_temp == 'R' || hint_temp == 'r' || hint_temp == 'N' || hint_temp == 'n' || hint_temp == 'B' || hint_temp == 'b' || hint_temp == 'Q' || hint_temp == 'q' || hint_temp == 'K' || hint_temp == 'k')
                                                {
                                                    hint_str = $"{Convert.ToChar(Convert.ToString(hint_temp).ToUpper())}{capt}{coordinate_col[hint_board_col]}{8 - hint_board_row}";
                                                }
                                                else if (hint_temp == 'P' || hint_temp == 'p')
                                                {
                                                    hint_str = $"{coordinate_col[k]}{capt}{capt_location}{8 - hint_board_row}";

                                                }

                                                Console.SetCursorPosition(counter_for_hint - 7, 23);
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.WriteLine(hint_str);
                                            }
                                        }
                                        hint = false;
                                    }
                                }
                            }
                        }
                    }
                }
                hint_capture = false;
                hint_capture = false;
                //enters after the piece is selected
                if (flag == true)
                {
                    //shows the movements the element can move
                    if (board[board_row, board_col] == 'P' || board[board_row, board_col] == 'p')
                    {
                        pawn(board, temp_cursorx, temp_cursory, temp, en_passant, hint_capture);
                        Console.ForegroundColor = ConsoleColor.Black;
                        flag = false;
                    }
                    else if (board[board_row, board_col] == 'N' || board[board_row, board_col] == 'n')
                    {
                        knight(board, temp_cursorx, temp_cursory, hint_capture);
                        Console.ForegroundColor = ConsoleColor.Black;
                        flag = false;
                    }
                    else if (board[board_row, board_col] == 'K' || board[board_row, board_col] == 'k')
                    {
                        king(board, temp_cursorx, temp_cursory, castling_blue, castling_red, control_check_blue, control_check_red, hint_capture);
                        Console.ForegroundColor = ConsoleColor.Black;
                        flag = false;
                    }
                    else if (board[board_row, board_col] == 'R' || board[board_row, board_col] == 'r')
                    {
                        rook(board, temp_cursorx, temp_cursory, hint_capture);
                        Console.ForegroundColor = ConsoleColor.Black;
                        flag = false;
                    }
                    else if (board[board_row, board_col] == 'B' || board[board_row, board_col] == 'b')
                    {
                        bishop(board, temp_cursorx, temp_cursory, hint_capture);
                        Console.ForegroundColor = ConsoleColor.Black;
                        flag = false;
                    }
                    else if (board[board_row, board_col] == 'Q' || board[board_row, board_col] == 'q')
                    {
                        queen(board, temp_cursorx, temp_cursory, hint_capture);
                        Console.ForegroundColor = ConsoleColor.Black;
                        flag = false;
                    }
                    else if (board[board_row, board_col] == '.')
                        flag = true;

                    //It enters after the places where the piece canare shown on the screen.
                    while (string_mod == false && mode_demo == false)
                    {
                        Console.SetCursorPosition(cursorx, cursory);
                        //The location of the selected piece is selected with the arrow keys
                        if (string_mod == false && mode_demo == false)
                        {
                            move_piece = Console.ReadKey(true);
                            if (move_piece.Key == ConsoleKey.RightArrow && cursorx < x2)
                            {
                                Console.SetCursorPosition(cursorx, cursory);
                                cursorx += 4;
                            }
                            if (move_piece.Key == ConsoleKey.LeftArrow && cursorx > x1)
                            {
                                Console.SetCursorPosition(cursorx, cursory);
                                cursorx -= 4;
                            }
                            if (move_piece.Key == ConsoleKey.UpArrow && cursory > y1)
                            {
                                Console.SetCursorPosition(cursorx, cursory);
                                cursory -= 2;
                            }
                            if (move_piece.Key == ConsoleKey.DownArrow && cursory < y2)
                            {
                                Console.SetCursorPosition(cursorx, cursory);
                                cursory += 2;
                            }
                            if (move_piece.Key == ConsoleKey.Enter && flag == false)
                                break;
                        }

                    }
                    //updates array's location
                    board_col = (cursorx - 5) / 4;
                    board_row = (cursory - 3) / 2;

                    //It can be en passant until two rounds have passed after 2 moves forward.
                    if (temp_cursory + 4 == cursory || temp_cursory - 4 == cursory)
                    {
                        en_passant = true;
                        temp_counter_for_en_passant = temp_counter;
                    }
                    else if (temp_counter_for_en_passant + 2 == temp_counter)
                        en_passant = false;
                    if (control_check_blue == true || control_check_red == true)
                    {
                        temp_counter_for_check = temp_counter + 1;
                    }
                    if (temp != '.')
                    {
                        //The coordinates that the selected piece can move is assigned to this array.
                        int[,] coordinate = new int[2, 50];
                        for (int i = 0; i < coordinate.GetLength(1); i++)
                        {
                            if (temp == 'R' || temp == 'r')
                            {
                                coordinate[0, i] = rook(board, temp_cursorx, temp_cursory, hint_capture).Item1[0, i];
                                coordinate[1, i] = rook(board, temp_cursorx, temp_cursory, hint_capture).Item1[1, i];
                            }
                            else if (temp == 'N' || temp == 'n')
                            {
                                coordinate[0, i] = knight(board, temp_cursorx, temp_cursory, hint_capture).Item1[0, i];
                                coordinate[1, i] = knight(board, temp_cursorx, temp_cursory, hint_capture).Item1[1, i];
                            }
                            else if (temp == 'B' || temp == 'b')
                            {
                                coordinate[0, i] = bishop(board, temp_cursorx, temp_cursory, hint_capture).Item1[0, i];
                                coordinate[1, i] = bishop(board, temp_cursorx, temp_cursory, hint_capture).Item1[1, i];
                            }
                            else if (temp == 'Q' | temp == 'q')
                            {
                                coordinate[0, i] = queen(board, temp_cursorx, temp_cursory, hint_capture).Item1[0, i];
                                coordinate[1, i] = queen(board, temp_cursorx, temp_cursory, hint_capture).Item1[1, i];
                            }
                            else if (temp == 'K' || temp == 'k')
                            {
                                coordinate[0, i] = king(board, temp_cursorx, temp_cursory, castling_blue, castling_red, control_check_blue, control_check_red, hint_capture).Item1[0, i];
                                coordinate[1, i] = king(board, temp_cursorx, temp_cursory, castling_blue, castling_red, control_check_blue, control_check_red, hint_capture).Item1[1, i];
                            }
                            else if (temp == 'P' || temp == 'p')
                            {
                                coordinate[0, i] = pawn(board, temp_cursorx, temp_cursory, temp, en_passant, hint_capture).Item1[0, i];
                                coordinate[1, i] = pawn(board, temp_cursorx, temp_cursory, temp, en_passant, hint_capture).Item1[1, i];
                            }
                        }
                        char[,] temp_board = new char[board.GetLength(0), board.GetLength(1)];
                        for (int i = 0; i < board.GetLength(0); i++)
                        {
                            for (int j = 0; j < board.GetLength(1); j++)
                            {
                                temp_board[i, j] = board[i, j];
                            }
                        }
                        //Checks if there is a position in the array equal to the selected position.
                        for (int i = 1; i < coordinate.GetLength(1); i++)
                        {
                            //Unchecked situation
                            if (coordinate[0, i] == cursorx && coordinate[1, i] == cursory)
                            {
                                //for the castling special move
                                if (temp == 'K' || temp == 'R')
                                    temp_castling_blue = false;

                                else if (temp == 'k' || temp == 'r')
                                    temp_castling_red = false;

                                if (temp == 'K' && cursorx == temp_cursorx - 8)
                                {
                                    temp_board[7, 0] = 'K';
                                    temp_board[7, 1] = 'K';
                                    temp_board[7, 2] = 'K';
                                    temp_board[7, 3] = 'K';
                                    temp_queenside_castling = true;
                                }
                                else if (temp == 'K' && cursorx == temp_cursorx + 8)
                                {
                                    temp_board[7, 7] = 'K';
                                    temp_board[7, 6] = 'K';
                                    temp_board[7, 5] = 'K';
                                    temp_kingside_castling = true;
                                }
                                else if (temp == 'k' && cursorx == temp_cursorx - 8)
                                {
                                    temp_board[0, 0] = 'k';
                                    temp_board[0, 1] = 'k';
                                    temp_board[0, 2] = 'k';
                                    temp_board[0, 3] = 'k';
                                    temp_queenside_castling = true;
                                }
                                else if (temp == 'k' && cursorx == temp_cursorx + 8)
                                {
                                    temp_board[0, 7] = 'k';
                                    temp_board[0, 7] = 'k';
                                    temp_board[0, 5] = 'k';
                                    temp_kingside_castling = true;
                                }
                                //for promotion special move
                                if (temp == 'P' && cursory == 3)
                                {
                                    temp_board[board_row, board_col] = 'Q';
                                    temp_promotion = true;
                                }
                                if (temp == 'p' && cursory == 17)
                                {
                                    temp_board[board_row, board_col] = 'q';
                                    temp_promotion = true;
                                }
                                if (temp_board[board_row, board_col] != '.')
                                    temp_capture = true;
                                //for the en passant special move
                                if (cursorx != temp_cursorx && board[board_row, board_col] == '.' && (temp == 'p' || temp == 'P'))
                                {
                                    temp_board[temp_row, temp_col] = '.';
                                    temp_board[temp_row, board_col] = '.';
                                    temp_board[board_row, board_col] = temp;
                                }
                                else
                                {
                                    temp_board[temp_row, temp_col] = '.';
                                    temp_board[board_row, board_col] = temp;
                                }
                                break;
                            }
                        }
                        //checks whether a check has been taken
                        char[] pieces_check = new char[6];
                        if (temp_counter % 2 == 1)
                        {
                            pieces_check[0] = 'P'; pieces_check[1] = 'R'; pieces_check[2] = 'N'; pieces_check[3] = 'B'; pieces_check[4] = 'Q'; pieces_check[5] = 'K';
                        }
                        else
                        {
                            pieces_check[0] = 'p'; pieces_check[1] = 'r'; pieces_check[2] = 'n'; pieces_check[3] = 'b'; pieces_check[4] = 'q'; pieces_check[5] = 'k';
                        }
                        for (int m = 0; m < pieces_check.Length; m++)
                        {
                            for (int j = 0; j < temp_board.GetLength(0); j++)
                            {
                                for (int k = 0; k < temp_board.GetLength(1); k++)
                                {
                                    if (pieces_check[m] == temp_board[j, k])
                                    {
                                        int[,] coordinate_check = new int[2, 50];
                                        int hint_cursorx = 4 * k + 5;
                                        int hint_cursory = 2 * j + 3;
                                        char hint_temp = temp_board[j, k];
                                        if (hint_temp == 'R' || hint_temp == 'r')
                                        {
                                            temp_control_check_blue = rook(temp_board, hint_cursorx, hint_cursory, hint_capture).Item3;
                                            temp_control_check_red = rook(temp_board, hint_cursorx, hint_cursory, hint_capture).Item4;
                                            check_cursorx = hint_cursorx;
                                            check_cursory = hint_cursory;
                                        }
                                        else if (hint_temp == 'N' || hint_temp == 'n')
                                        {
                                            temp_control_check_blue = knight(temp_board, hint_cursorx, hint_cursory, hint_capture).Item3;
                                            temp_control_check_red = knight(temp_board, hint_cursorx, hint_cursory, hint_capture).Item4;
                                            check_cursorx = hint_cursorx;
                                            check_cursory = hint_cursory;
                                        }
                                        else if (hint_temp == 'B' || hint_temp == 'b')
                                        {
                                            temp_control_check_blue = bishop(temp_board, hint_cursorx, hint_cursory, hint_capture).Item3;
                                            temp_control_check_red = bishop(temp_board, hint_cursorx, hint_cursory, hint_capture).Item4;
                                            check_cursorx = hint_cursorx;
                                            check_cursory = hint_cursory;
                                        }
                                        else if (hint_temp == 'Q' | hint_temp == 'q')
                                        {
                                            temp_control_check_blue = queen(temp_board, hint_cursorx, hint_cursory, hint_capture).Item3;
                                            temp_control_check_red = queen(temp_board, hint_cursorx, hint_cursory, hint_capture).Item4;
                                            check_cursorx = hint_cursorx;
                                            check_cursory = hint_cursory;
                                        }
                                        else if (hint_temp == 'K' || hint_temp == 'k')
                                        {
                                            temp_control_check_blue = king(temp_board, hint_cursorx, hint_cursory, temp_castling_blue, temp_castling_red, control_check_blue, control_check_red, hint_capture).Item3;
                                            temp_control_check_red = king(temp_board, hint_cursorx, hint_cursory, temp_castling_blue, temp_castling_red, control_check_blue, control_check_red, hint_capture).Item4;
                                            check_cursorx = hint_cursorx;
                                            check_cursory = hint_cursory;
                                        }
                                        else if (hint_temp == 'P' || hint_temp == 'p')
                                        {
                                            temp_control_check_blue = pawn(temp_board, hint_cursorx, hint_cursory, hint_temp, en_passant, hint_capture).Item3;
                                            temp_control_check_red = pawn(temp_board, hint_cursorx, hint_cursory, hint_temp, en_passant, hint_capture).Item4;
                                            check_cursorx = hint_cursorx;
                                            check_cursory = hint_cursory;
                                        }
                                        if (temp_control_check_blue == true || temp_control_check_red == true)
                                            break;
                                    }
                                }
                                if (temp_control_check_blue == true || temp_control_check_red == true)
                                    break;
                            }
                            if (temp_control_check_blue == true || temp_control_check_red == true)
                                break;
                        }
                        control_check_blue = temp_control_check_blue;
                        control_check_red = temp_control_check_red;
                        //Check situation
                        for (int i = 1; i < coordinate.GetLength(1); i++)
                        {
                            if (coordinate[0, i] == cursorx && coordinate[1, i] == cursory)
                            {
                                //temporarily created elements are assigned real values 
                                if (control_check_red == false && temp_counter % 2 == 0)
                                {
                                    if (temp == 'K' || temp == 'R')
                                        castling_blue = false;

                                    else if (temp == 'k' || temp == 'r')
                                        castling_red = false;

                                    if (temp == 'K' && cursorx == temp_cursorx - 8)
                                    {
                                        board[7, 0] = '.';
                                        board[7, 3] = 'R';
                                        queenside_castling = true;
                                    }
                                    else if (temp == 'K' && cursorx == temp_cursorx + 8)
                                    {
                                        board[7, 7] = '.';
                                        board[7, 5] = 'R';
                                        kingside_castling = true;
                                    }
                                    else if (temp == 'k' && cursorx == temp_cursorx - 8)
                                    {
                                        board[0, 0] = '.';
                                        board[0, 3] = 'r';
                                        queenside_castling = true;
                                    }
                                    else if (temp == 'k' && cursorx == temp_cursorx + 8)
                                    {
                                        board[0, 7] = '.';
                                        board[0, 5] = 'r';
                                        kingside_castling = true;
                                    }
                                    if (board[board_row, board_col] != '.')
                                        capture = true;

                                    if (cursorx != temp_cursorx && board[board_row, board_col] == '.' && (temp == 'p' || temp == 'P'))
                                    {
                                        passant_done = true;
                                        flag = true;
                                        board[temp_row, temp_col] = '.';
                                        board[temp_row, board_col] = '.';
                                        board[board_row, board_col] = temp;
                                        counter++;
                                    }
                                    else
                                    {
                                        flag = true;
                                        board[temp_row, temp_col] = '.';
                                        board[board_row, board_col] = temp;
                                        counter++;
                                    }
                                    //for promotion special move
                                    if (temp == 'P' && cursory == 3)
                                    {
                                        board[board_row, board_col] = 'Q';
                                        promotion = true;
                                    }
                                    if (temp == 'p' && cursory == 17)
                                    {
                                        board[board_row, board_col] = 'q';
                                        promotion = true;
                                    }
                                }
                                else if (control_check_blue == false && temp_counter % 2 == 1)
                                {
                                    // Controls for the castling move and the el-passant
                                    if (temp == 'K' || temp == 'R')
                                        castling_blue = false;

                                    else if (temp == 'k' || temp == 'r')
                                        castling_red = false;

                                    if (temp == 'K' && cursorx == temp_cursorx - 8)
                                    {
                                        board[7, 0] = '.';
                                        board[7, 3] = 'R';
                                        queenside_castling = true;
                                    }
                                    else if (temp == 'K' && cursorx == temp_cursorx + 8)
                                    {
                                        board[7, 7] = '.';
                                        board[7, 5] = 'R';
                                        kingside_castling = true;
                                    }
                                    else if (temp == 'k' && cursorx == temp_cursorx - 8)
                                    {
                                        board[0, 0] = '.';
                                        board[0, 3] = 'r';
                                        queenside_castling = true;
                                    }
                                    else if (temp == 'k' && cursorx == temp_cursorx + 8)
                                    {
                                        board[0, 7] = '.';
                                        board[0, 5] = 'r';
                                        kingside_castling = true;
                                    }
                                    if (board[board_row, board_col] != '.')
                                        capture = true;
                                    if (cursorx != temp_cursorx && board[board_row, board_col] == '.' && (temp == 'p' || temp == 'P'))
                                    {
                                        passant_done = true;
                                        flag = true;
                                        board[temp_row, temp_col] = '.';
                                        board[temp_row, board_col] = '.';
                                        board[board_row, board_col] = temp;
                                        counter++;
                                    }
                                    else
                                    {
                                        flag = true;
                                        board[temp_row, temp_col] = '.';
                                        board[board_row, board_col] = temp;
                                        counter++;
                                    }
                                    //for promotion special move
                                    if (temp == 'P' && cursory == 3)
                                    {
                                        board[board_row, board_col] = 'Q';
                                        promotion = true;
                                    }
                                    if (temp == 'p' && cursory == 17)
                                    {
                                        board[board_row, board_col] = 'q';
                                        promotion = true;
                                    }
                                }
                                else
                                    flag = false;
                                break;
                            }
                            else
                                flag = false;
                        }
                        // Informing the user if his/her king under threat
                        if ((control_check_blue == true || control_check_red == true) && flag == false)
                        {
                            Console.SetCursorPosition(3, 23);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("The opponent made a check");
                            flag = true;
                        }
                        else if (flag == false && (cursorx != temp_cursorx || cursory != temp_cursory))
                        {
                            Console.SetCursorPosition(3, 23);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("This move is against the rules");
                            flag = true;
                        }
                    }
                    if (temp == 'R' || temp == 'r')
                    {
                        show_check_blue = rook(board, cursorx, cursory, hint_capture).Item3;
                        show_check_red = rook(board, cursorx, cursory, hint_capture).Item4;
                    }
                    else if (temp == 'N' || temp == 'n')
                    {
                        show_check_blue = knight(board, cursorx, cursory, hint_capture).Item3;
                        show_check_red = knight(board, cursorx, cursory, hint_capture).Item4;
                    }
                    else if (temp == 'B' || temp == 'b')
                    {
                        show_check_blue = bishop(board, cursorx, cursory, hint_capture).Item3;
                        show_check_red = bishop(board, cursorx, cursory, hint_capture).Item4;
                    }
                    else if (temp == 'Q' | temp == 'q')
                    {
                        show_check_blue = queen(board, cursorx, cursory, hint_capture).Item3;
                        show_check_red = queen(board, cursorx, cursory, hint_capture).Item4;

                    }
                    else if (temp == 'K' || temp == 'k')
                    {
                        show_check_blue = king(board, cursorx, cursory, temp_castling_blue, temp_castling_red, control_check_blue, control_check_red, hint_capture).Item3;
                        show_check_red = king(board, cursorx, cursory, temp_castling_blue, temp_castling_red, control_check_blue, control_check_red, hint_capture).Item4;
                    }
                    else if (temp == 'P' || temp == 'p')
                    {
                        show_check_blue = pawn(board, cursorx, cursory, temp, en_passant, hint_capture).Item3;
                        show_check_red = pawn(board, cursorx, cursory, temp, en_passant, hint_capture).Item4;
                    }
                    if (temp_counter + 1 == counter)
                    {
                        // Checking if the user choose a piece and did not move it
                        if (cursorx != temp_cursorx || cursory != temp_cursory)
                        {
                            // and if he/she moved a piece, then checking capture, promotion, el-passant, castling situations
                            string capt = "";
                            string capt_location = "";
                            string promo = "";
                            string passant = "";
                            string check = "";
                            if (capture == true)
                            {
                                capt = "x";
                                capt_location = Convert.ToString(coordinate_col[board_col]);
                            }
                            if (show_check_blue == true || show_check_red == true)
                            {
                                check = "+";
                            }
                            if (promotion == true)
                            {
                                promo = "Q";
                            }
                            if (passant_done == true)
                            {
                                passant = "e.p.";
                                capt = "x";
                                capt_location = Convert.ToString(coordinate_col[board_col]);
                            }
                            // Writing moves as a notation
                            if (temp == 'R' || temp == 'r' || temp == 'N' || temp == 'n' || temp == 'B' || temp == 'b' || temp == 'Q' || temp == 'q' || temp == 'K' || temp == 'k')
                            {
                                if (temp_counter % 2 == 0)
                                {
                                    if (kingside_castling == true)
                                    {
                                        WhiteMove = "O-O";
                                    }
                                    else if (queenside_castling == true)
                                    {
                                        WhiteMove = "O-O-O";
                                    }
                                    else
                                    {
                                        WhiteMove = $"{Convert.ToChar(Convert.ToString(temp).ToUpper())}{capt}{coordinate_col[board_col]}{8 - board_row}{check}";
                                    }
                                }
                                else if (temp_counter % 2 == 1)
                                {
                                    if (kingside_castling == true)
                                    {
                                        BlackMove = "O-O";
                                    }
                                    else if (queenside_castling == true)
                                    {
                                        BlackMove = "O-O-O";
                                    }
                                    else
                                    {
                                        BlackMove = $"{Convert.ToChar(Convert.ToString(temp).ToUpper())}{capt}{coordinate_col[board_col]}{8 - board_row}{check}";
                                    }
                                }
                            }
                            else if (temp == 'P' || temp == 'p')
                            {
                                if (temp_counter % 2 == 0)
                                {
                                    if (en_passant == true && cursorx != temp_cursorx)
                                    {
                                        WhiteMove = $"{coordinate_col[temp_col]}{capt}{capt_location}{8 - board_row}{promo}{passant}{check}";
                                    }
                                    else
                                    {
                                        WhiteMove = $"{coordinate_col[temp_col]}{capt}{capt_location}{8 - board_row}{promo}{passant}{check}";
                                    }
                                }
                                else
                                {
                                    if (en_passant == true && cursorx != temp_cursorx)
                                    {
                                        BlackMove = $"{coordinate_col[temp_col]}{capt}{capt_location}{8 - board_row}{promo}{passant}{check}";
                                    }
                                    else
                                    {
                                        BlackMove = $"{coordinate_col[temp_col]}{capt}{capt_location}{8 - board_row}{promo}{passant}{check}";
                                    }
                                }
                            }
                        }
                    }
                }
                // Writing the chess board
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(0, 0);
                Console.WriteLine();
                Console.WriteLine("  +---------------------------------+");
                Console.WriteLine("  |                                 |");
                for (int i = 0; i < coordinate_row.GetLength(0); i++)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write($" {coordinate_row[i]}|");
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write($"  {Color(board[i, 0], show_check_blue, show_check_red, counter)}   ");
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write($"{Color(board[i, 1], show_check_blue, show_check_red, counter)}   ");
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write($"{Color(board[i, 2], show_check_blue, show_check_red, counter)}   ");
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write($"{Color(board[i, 3], show_check_blue, show_check_red, counter)}   ");
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write($"{Color(board[i, 4], show_check_blue, show_check_red, counter)}   ");
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write($"{Color(board[i, 5], show_check_blue, show_check_red, counter)}   ");
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write($"{Color(board[i, 6], show_check_blue, show_check_red, counter)}   ");
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write($"{Color(board[i, 7], show_check_blue, show_check_red, counter)}");
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("  | ");
                    Console.WriteLine("  |                                 | ");
                }
                Console.WriteLine("  +---------------------------------+");
                Console.Write("     ");
                for (int i = 0; i < coordinate_col.Length; i++)
                {
                    Console.Write(coordinate_col[i] + "   ");
                }
                int TurnNumber = temp_counter / 2;
                // Writing chess notations one under the onder
                if ((string_mod == true || mode_demo == true) && temp_counter + 1 == counter)
                {
                    if (temp_counter % 2 == 0)
                    {
                        Console.SetCursorPosition(50, 3 + TurnNumber);
                        Console.Write($"{TurnNumber + 1}. {WhiteMove}");
                    }
                    else
                    {
                        Console.SetCursorPosition(65, 3 + TurnNumber);
                        Console.Write($"{BlackMove}");
                    }
                }
                else if (string_mod == false && temp_counter + 1 == counter)
                {
                    if (temp_counter % 2 == 0)
                    {
                        Console.SetCursorPosition(50, 3 + TurnNumber);
                        Console.Write($"{TurnNumber + 1}. {WhiteMove}");
                    }
                    else
                    {
                        Console.SetCursorPosition(65, 3 + TurnNumber);
                        Console.Write($"{BlackMove}");
                    }
                }
                // If the user want to play from the beginning, then play mode will continue.
                if (mode_play == true)
                {
                    StreamWriter f = File.CreateText("Play_Mode.txt");
                    StreamWriter f2;
                    if (counter == 1)
                    {
                        f2 = File.CreateText("Demo_Mode.txt");
                    }
                    else
                    {
                        f2 = File.AppendText("Demo_Mode.txt");
                    }
                    // Writing every move as a chess notation to a text file for the Demo Mode
                    if (temp_counter % 2 == 0 && temp_counter + 1 == counter)
                    {
                        f.WriteLine(WhiteMove);
                        f2.WriteLine(WhiteMove);
                    }
                    else if (temp_counter + 1 == counter)
                    {
                        f.WriteLine(BlackMove);
                        f2.WriteLine(BlackMove);
                    }
                    f.Close();
                    f2.Close();
                }
                flag = false;
                while (true)
                {
                    x1 = 5; // sol sınır
                    x2 = 33; // sağ sınır
                    y1 = 3; // üst sınır
                    y2 = 17; // alt sınır
                    if (string_mod == false && mode_demo == false)
                        Console.SetCursorPosition(cursorx, cursory);
                    if (string_mod == true || mode_demo == true)
                    {
                        // If the user wants to continue from where he/she left the game, then he/she should go for the demo mode.
                        Console.SetCursorPosition(3, 25);
                        Console.Write("White's Move is:                               ");
                        Console.SetCursorPosition(3, 26);
                        Console.Write("Black's Move is:                               ");
                        StreamReader f = File.OpenText("Demo_Mode.txt");
                        string demo_text = "";
                        while (!f.EndOfStream)
                        {
                            demo_text = demo_text + f.ReadLine();
                            if (!f.EndOfStream)
                                demo_text += "\n";
                        }
                        f.Close();
                        string[] moves = null;
                        moves = demo_text.Split('\n');
                        // Taking all moves from text file and playing them one-by-one
                        temp_counter = counter;
                        if (temp_counter % 2 == 0)
                        {
                            Console.SetCursorPosition(20, 25);
                            if (mode_demo == true && temp_counter<moves.Length)
                            {
                                White = moves[temp_counter];
                                Console.WriteLine(White);
                                read = Console.ReadKey(true);

                                if (read.Key == ConsoleKey.Backspace && mode_demo == true)
                                {
                                    mode_play = true;
                                    mode_demo = false;
                                    break;
                                }
                            }
                            else if (string_mod==true)
                            {
                                White = Console.ReadLine();
                            }
                            else
                            {
                                mode_play = true;
                                mode_demo = false;
                            }
                            WhiteMove = White;
                            Move = WhiteMove;
                        }
                        else
                        {
                            Console.SetCursorPosition(20, 26);
                            if (mode_demo == true && temp_counter < moves.Length)
                            {
                                Black = moves[temp_counter];
                                Console.WriteLine(Black);
                                read = Console.ReadKey(true);
                                if (read.Key == ConsoleKey.Backspace && mode_demo == true)
                                {
                                    mode_play = true;
                                    mode_demo = false;
                                    break;
                                }
                            }
                            else if (string_mod == true)
                            {
                                Black = Console.ReadLine();
                            }
                            else
                            {
                                mode_play = true;
                                mode_demo = false;
                            }
                            BlackMove = Black;
                            Move = BlackMove;
                        }
                        // Deleting '+' and 'Q' symbols from the notation for the playing with no mistake.
                        if (Move[Move.Length - 1] == '+')
                        {
                            Move = Move.Substring(0, Move.Length - 1);
                        }
                        if (Move[Move.Length - 1] == 'Q')
                        {
                            Move = Move.Substring(0, Move.Length - 1);
                        }
                        // Calculations depending to the length of the notation for playing via chess notations 
                        if (Move.Length == 2)
                        {
                            int i;
                            int j;
                            bool control = true;
                            char first_letter = 'P';
                            for (i = 0; i < coordinate_col.Length; i++)
                            {
                                if (Move[0] == coordinate_col[i])
                                    break;
                            }
                            s_cursorx = 4 * i + 5;
                            s_cursory = (8 - Convert.ToInt32(Convert.ToString(Move[1]))) * 2 + 3;
                            for (j = board.GetLength(1) - 1; j >= 0; j--)
                            {
                                if (temp_counter % 2 == 0)
                                    first_letter = 'P';
                                else
                                    first_letter = 'p';

                                if (board[j, i] == first_letter)
                                {
                                    int[,] coordinate = new int[2, 50];
                                    for (int l = 0; l < coordinate.GetLength(1); l++)
                                    {
                                        coordinate[0, l] = pawn(board, 4 * i + 5, 2 * j + 3, board[j, i], en_passant, hint_capture).Item1[0, l];
                                        coordinate[1, l] = pawn(board, 4 * i + 5, 2 * j + 3, board[j, i], en_passant, hint_capture).Item1[1, l];
                                    }
                                    for (int l = 1; l < coordinate.GetLength(1); l++)
                                    {
                                        if (coordinate[0, l] == s_cursorx && coordinate[1, l] == s_cursory)
                                        {
                                            control = false;
                                            break;
                                        }
                                    }
                                    if (control == false)
                                        break;
                                }
                            }
                            if (board[8 - Convert.ToInt32(Convert.ToString(Move[1])), i] != '.' && control == false)
                            {
                                Console.SetCursorPosition(3, 23);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("This move is against the rules");
                            }
                            else if (control == false)
                            {
                                s_board_col = i;
                                s_board_row = j;
                                s_temp_cursorx = s_cursorx;
                                s_temp_cursory = 2 * s_board_row + 3;
                                flag = true;
                            }
                            else if (control == true)
                            {
                                Console.SetCursorPosition(3, 23);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("This move is against the rules");
                            }
                            break;
                        }
                        else if (Move.Length == 3)
                        {
                            if (Move[0] == 'R' || Move[0] == 'N' || Move[0] == 'B' || Move[0] == 'Q' || Move[0] == 'K')
                            {
                                int i;
                                int j = 0;
                                int k;
                                for (k = 0; k < coordinate_col.Length; k++)
                                {
                                    if (Move[1] == coordinate_col[k])
                                        break;
                                }
                                s_cursorx = 4 * k + 5;
                                s_cursory = (8 - Convert.ToInt32(Convert.ToString(Move[2]))) * 2 + 3;
                                bool control = true;
                                for (i = 0; i < board.GetLength(0); i++)
                                {
                                    for (j = 0; j < board.GetLength(1); j++)
                                    {
                                        char first_letter;
                                        if (temp_counter % 2 == 0)
                                            first_letter = Move[0];
                                        else
                                            first_letter = Convert.ToChar(Convert.ToString(Move[0]).ToLower());
                                        if (board[i, j] == first_letter)
                                        {
                                            int[,] coordinate = new int[2, 50];
                                            for (int l = 0; l < coordinate.GetLength(1); l++)
                                            {
                                                if (Move[0] == 'R')
                                                {
                                                    coordinate[0, l] = rook(board, 4 * j + 5, 2 * i + 3, hint_capture).Item1[0, l];
                                                    coordinate[1, l] = rook(board, 4 * j + 5, 2 * i + 3, hint_capture).Item1[1, l];
                                                }
                                                else if (Move[0] == 'N')
                                                {
                                                    coordinate[0, l] = knight(board, 4 * j + 5, 2 * i + 3, hint_capture).Item1[0, l];
                                                    coordinate[1, l] = knight(board, 4 * j + 5, 2 * i + 3, hint_capture).Item1[1, l];
                                                }
                                                else if (Move[0] == 'B')
                                                {
                                                    coordinate[0, l] = bishop(board, 4 * j + 5, 2 * i + 3, hint_capture).Item1[0, l];
                                                    coordinate[1, l] = bishop(board, 4 * j + 5, 2 * i + 3, hint_capture).Item1[1, l];
                                                }
                                                else if (Move[0] == 'Q')
                                                {
                                                    coordinate[0, l] = queen(board, 4 * j + 5, 2 * i + 3, hint_capture).Item1[0, l];
                                                    coordinate[1, l] = queen(board, 4 * j + 5, 2 * i + 3, hint_capture).Item1[1, l];
                                                }
                                                else if (Move[0] == 'K')
                                                {
                                                    coordinate[0, l] = king(board, 4 * j + 5, 2 * i + 3, castling_blue, castling_red, control_check_blue, control_check_red, hint_capture).Item1[0, l];
                                                    coordinate[1, l] = king(board, 4 * j + 5, 2 * i + 3, castling_blue, castling_red, control_check_blue, control_check_red, hint_capture).Item1[1, l];
                                                }
                                            }
                                            for (int l = 1; l < coordinate.GetLength(1); l++)
                                            {
                                                if (coordinate[0, l] == s_cursorx && coordinate[1, l] == s_cursory)
                                                {
                                                    control = false;
                                                    break;
                                                }
                                            }
                                            if (control == false)
                                                break;
                                        }
                                    }
                                    if (control == false)
                                        break;
                                }
                                if (board[8 - Convert.ToInt32(Convert.ToString(Move[2])), k] != '.' && control == false)
                                {
                                    Console.SetCursorPosition(3, 23);
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("This move is against the rules");
                                }
                                else if (control == false)
                                {
                                    s_temp_cursorx = 4 * j + 5;
                                    s_temp_cursory = 2 * i + 3;
                                    s_board_row = i;
                                    s_board_col = j;
                                    flag = true;
                                }
                                else if (control == true)
                                {
                                    Console.SetCursorPosition(3, 23);
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("This move is against the rules");
                                }
                                break;
                            }
                            if (Move[0] == '0' && Move[1] == '-' && Move[2] == '0')
                            {
                                int i = 0;
                                if (temp_counter % 2 == 0)
                                {
                                    i = 7;
                                }
                                if (temp_counter % 2 == 1)
                                {
                                    i = 0;
                                }
                                int j = 4;

                                s_board_col = j;
                                s_board_row = i;
                                s_cursorx = 4 * j + 5 + 8;
                                s_cursory = 2 * i + 3;
                                s_temp_cursorx = 4 * j + 5;
                                s_temp_cursory = 2 * i + 3;
                                flag = true;
                                break;
                            }
                        }
                        else if (Move.Length == 4)
                        {
                            if (Move[1] == 'x' && Move[0] != 'P' && Move[0] != 'R' && Move[0] != 'N' && Move[0] != 'B' && Move[0] != 'Q' && Move[0] != 'K')
                            {
                                int i;
                                int j;
                                int k;
                                bool control = true;
                                char first_letter = 'P';
                                for (i = 0; i < coordinate_col.Length; i++)
                                {
                                    if (Move[0] == coordinate_col[i])
                                        break;
                                }
                                for (k = 0; k < coordinate_col.Length; k++)
                                {
                                    if (Move[2] == coordinate_col[k])
                                        break;
                                }
                                s_cursorx = 4 * k + 5;
                                s_cursory = (8 - Convert.ToInt32(Convert.ToString(Move[3]))) * 2 + 3;
                                for (j = board.GetLength(1) - 1; j >= 0; j--)
                                {
                                    if (temp_counter % 2 == 0)
                                        first_letter = 'P';
                                    else
                                        first_letter = 'p';

                                    if (board[j, i] == first_letter)
                                    {
                                        int[,] coordinate = new int[2, 50];
                                        for (int l = 0; l < coordinate.GetLength(1); l++)
                                        {
                                            coordinate[0, l] = pawn(board, 4 * i + 5, 2 * j + 3, board[j, i], en_passant, hint_capture).Item1[0, l];
                                            coordinate[1, l] = pawn(board, 4 * i + 5, 2 * j + 3, board[j, i], en_passant, hint_capture).Item1[1, l];
                                        }
                                        for (int l = 1; l < coordinate.GetLength(1); l++)
                                        {
                                            if (coordinate[0, l] == s_cursorx && coordinate[1, l] == s_cursory)
                                            {
                                                control = false;
                                                break;
                                            }
                                        }
                                        if (control == false)
                                            break;
                                    }
                                }
                                if (control == false)
                                {
                                    s_board_col = i;
                                    s_board_row = j;
                                    s_cursorx = 4 * k + 5;
                                    s_cursory = (8 - Convert.ToInt32(Convert.ToString(Move[3]))) * 2 + 3;
                                    s_temp_cursorx = 4 * i + 5;
                                    s_temp_cursory = 2 * s_board_row + 3;
                                    flag = true;
                                }
                                else if (control == true)
                                {
                                    Console.SetCursorPosition(3, 23);
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("This move is against the rules");
                                }
                                break;
                            }
                            else if ((Move[0] == 'R' || Move[0] == 'N') && Move[1] != 'x')
                            {
                                int i;
                                int j;
                                int k;
                                for (i = 0; i < coordinate_col.Length; i++)
                                {
                                    if (Move[1] == coordinate_col[i])
                                        break;
                                }
                                for (j = board.GetLength(1) - 1; j >= 0; j--)
                                {
                                    char first_letter;
                                    if (temp_counter % 2 == 0)
                                        first_letter = Move[0];
                                    else
                                        first_letter = Convert.ToChar(Convert.ToString(Move[0]).ToLower());

                                    if (board[j, i] == first_letter)
                                        break;
                                }
                                for (k = 0; k < coordinate_col.Length; k++)
                                {
                                    if (Move[2] == coordinate_col[k])
                                        break;
                                }
                                s_board_col = i;
                                s_board_row = j;
                                s_cursorx = 4 * k + 5;
                                s_cursory = (8 - Convert.ToInt32(Convert.ToString(Move[3]))) * 2 + 3;
                                s_temp_cursorx = 4 * i + 5;
                                s_temp_cursory = 2 * s_board_row + 3;
                                flag = true;
                                break;
                            }
                            else if ((Move[0] == 'R' || Move[0] == 'N' || Move[0] == 'B' || Move[0] == 'Q' || Move[0] == 'K') && Move[1] == 'x')
                            {
                                int i;
                                int j = 0;
                                int k;
                                for (k = 0; k < coordinate_col.Length; k++)
                                {
                                    if (Move[2] == coordinate_col[k])
                                        break;
                                }
                                s_cursorx = 4 * k + 5;
                                s_cursory = (8 - Convert.ToInt32(Convert.ToString(Move[3]))) * 2 + 3;
                                bool control = true;
                                for (i = 0; i < board.GetLength(0); i++)
                                {
                                    for (j = 0; j < board.GetLength(1); j++)
                                    {
                                        char first_letter;
                                        if (temp_counter % 2 == 0)
                                            first_letter = Move[0];
                                        else
                                            first_letter = Convert.ToChar(Convert.ToString(Move[0]).ToLower());
                                        if (board[i, j] == first_letter)
                                        {
                                            int[,] coordinate = new int[2, 50];
                                            for (int l = 0; l < coordinate.GetLength(1); l++)
                                            {
                                                if (Move[0] == 'R')
                                                {
                                                    coordinate[0, l] = rook(board, 4 * j + 5, 2 * i + 3, hint_capture).Item1[0, l];
                                                    coordinate[1, l] = rook(board, 4 * j + 5, 2 * i + 3, hint_capture).Item1[1, l];
                                                }
                                                else if (Move[0] == 'N')
                                                {
                                                    coordinate[0, l] = knight(board, 4 * j + 5, 2 * i + 3, hint_capture).Item1[0, l];
                                                    coordinate[1, l] = knight(board, 4 * j + 5, 2 * i + 3, hint_capture).Item1[1, l];
                                                }
                                                else if (Move[0] == 'B')
                                                {
                                                    coordinate[0, l] = bishop(board, 4 * j + 5, 2 * i + 3, hint_capture).Item1[0, l];
                                                    coordinate[1, l] = bishop(board, 4 * j + 5, 2 * i + 3, hint_capture).Item1[1, l];
                                                }
                                                else if (Move[0] == 'Q')
                                                {
                                                    coordinate[0, l] = queen(board, 4 * j + 5, 2 * i + 3, hint_capture).Item1[0, l];
                                                    coordinate[1, l] = queen(board, 4 * j + 5, 2 * i + 3, hint_capture).Item1[1, l];
                                                }
                                                else if (Move[0] == 'K')
                                                {
                                                    coordinate[0, l] = king(board, 4 * j + 5, 2 * i + 3, castling_blue, castling_red, control_check_blue, control_check_red, hint_capture).Item1[0, l];
                                                    coordinate[1, l] = king(board, 4 * j + 5, 2 * i + 3, castling_blue, castling_red, control_check_blue, control_check_red, hint_capture).Item1[1, l];
                                                }
                                            }
                                            for (int l = 1; l < coordinate.GetLength(1); l++)
                                            {
                                                if (coordinate[0, l] == s_cursorx && coordinate[1, l] == s_cursory)
                                                {
                                                    control = false;
                                                    break;
                                                }
                                            }
                                            if (control == false)
                                                break;
                                        }
                                    }
                                    if (control == false)
                                        break;
                                }
                                if (board[8 - Convert.ToInt32(Convert.ToString(Move[3])), k] == '.' && control == false)
                                {
                                    Console.SetCursorPosition(3, 23);
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Wrong view no capture status");
                                }
                                else if (control == false)
                                {
                                    s_temp_cursorx = 4 * j + 5;
                                    s_temp_cursory = 2 * i + 3;
                                    s_board_row = i;
                                    s_board_col = j;
                                    flag = true;
                                }
                                else if (control == true)
                                {
                                    Console.SetCursorPosition(3, 23);
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("This move is against the rules");
                                }
                                break;
                            }
                        }
                        else if (Move.Length == 5)
                        {
                            if (Move[0] == '0' && Move[1] == '-' && Move[2] == '0' && Move[3] == '-' && Move[4] == '0')
                            {
                                int i = 0;
                                if (temp_counter % 2 == 0)
                                {
                                    i = 7;
                                }
                                if (temp_counter % 2 == 1)
                                {
                                    i = 0;
                                }
                                int j = 4;
                                s_board_col = j;
                                s_board_row = i;
                                s_cursorx = 4 * j + 5 - 8;
                                s_cursory = 2 * i + 3;
                                s_temp_cursorx = 4 * j + 5;
                                s_temp_cursory = 2 * i + 3;
                                flag = true;
                                break;
                            }
                        }
                        if (Move.Length == 8 && Move[4] == 'e' && Move[5] == '.' && Move[6] == 'p' && Move[7] == '.')
                        {
                            if (Move[1] == 'x' && Move[0] != 'P' && Move[0] != 'R' && Move[0] != 'N' && Move[0] != 'B' && Move[0] != 'Q' && Move[0] != 'K')
                            {
                                int i;
                                int j;
                                int k;
                                for (i = 0; i < coordinate_col.Length; i++)
                                {
                                    if (Move[0] == coordinate_col[i])
                                        break;
                                }
                                for (j = board.GetLength(1) - 1; j >= 0; j--)
                                {
                                    if (temp_counter % 2 == 0)
                                    {
                                        if (board[j, i] == 'P')
                                            break;
                                    }
                                    else
                                    {
                                        if (board[j, i] == 'p')
                                            break;
                                    }
                                }
                                for (k = 0; k < coordinate_col.Length; k++)
                                {
                                    if (Move[2] == coordinate_col[k])
                                        break;
                                }
                                s_board_col = i;
                                s_board_row = j;
                                s_cursorx = 4 * k + 5;
                                s_cursory = (8 - Convert.ToInt32(Convert.ToString(Move[3]))) * 2 + 3;
                                s_temp_cursorx = 4 * i + 5;
                                s_temp_cursory = 2 * s_board_row + 3;
                                flag = true;
                                break;
                            }
                        }
                    }
                    if (Console.KeyAvailable)
                    {
                        char x = board[(cursory - 3) / 2, ((cursorx - 5) / 4)];
                        move_cursor = Console.ReadKey(true);
                        // The user should press 'H' for ask hints
                        if (move_cursor.Key == ConsoleKey.H)
                        {
                            Console.SetCursorPosition(3, 25);
                            Console.WriteLine("pressed H!");
                            Console.ForegroundColor = ConsoleColor.Green;
                            if (counter % 2 == 0)
                            {
                                Console.SetCursorPosition(3, 26);
                                Console.Write("Hint: ");
                            }
                            else
                            {
                                Console.SetCursorPosition(3, 26);
                                Console.WriteLine("Hint: ");
                            }
                            hint = true;
                            break;
                        }
                        // The user should press 'ESC' for going to demo mode
                        if (move_cursor.Key == ConsoleKey.Escape && mode_play == true)
                        {
                            mode_play = false;
                            mode_demo = true;
                            break;
                        }
                        if (move_cursor.Key == ConsoleKey.Escape && mode_demo == true)
                        {
                            mode_play = true;
                            mode_demo = false;
                            break;
                        }
                        // The user should press 'Space' for going to string mode
                        if (move_cursor.Key == ConsoleKey.Spacebar && string_mod == false && mode_play == true)
                        {
                            string_mod = true;
                            break;
                        }
                        else if (move_cursor.Key == ConsoleKey.Spacebar && string_mod == true)
                            string_mod = false;
                        if (string_mod == false)
                        {
                            if (move_cursor.Key == ConsoleKey.RightArrow && cursorx < x2)
                            {
                                if (string_mod == false)
                                    Console.SetCursorPosition(cursorx, cursory);
                                cursorx += 4;
                                board_col = (cursorx - 5) / 4;
                                board_row = (cursory - 3) / 2;
                            }
                            if (move_cursor.Key == ConsoleKey.LeftArrow && cursorx > x1)
                            {
                                if (string_mod == false)
                                    Console.SetCursorPosition(cursorx, cursory);
                                cursorx -= 4;
                                board_col = (cursorx - 5) / 4;
                                board_row = (cursory - 3) / 2;

                            }
                            if (move_cursor.Key == ConsoleKey.UpArrow && cursory > y1)
                            {
                                if (string_mod == false)
                                    Console.SetCursorPosition(cursorx, cursory);
                                cursory -= 2;
                                board_col = (cursorx - 5) / 4;
                                board_row = (cursory - 3) / 2;
                            }
                            if (move_cursor.Key == ConsoleKey.DownArrow && cursory < y2)
                            {
                                if (string_mod == false)
                                    Console.SetCursorPosition(cursorx, cursory);
                                cursory += 2;
                                board_col = (cursorx - 5) / 4;
                                board_row = (cursory - 3) / 2;
                            }
                            if (move_cursor.Key == ConsoleKey.Enter && (x == 'R' || x == 'N' || x == 'B' || x == 'Q' || x == 'K' || x == 'B' || x == 'N' || x == 'R' || x == 'P'))
                            {
                                if (play_mod(counter))
                                {
                                    flag = true;
                                    temp_cursorx = cursorx;
                                    temp_cursory = cursory;
                                    break;
                                }
                            }
                            if (move_cursor.Key == ConsoleKey.Enter && (x == 'r' || x == 'n' || x == 'b' || x == 'q' || x == 'k' || x == 'b' || x == 'n' || x == 'r' || x == 'p'))
                            {
                                if (!play_mod(counter))
                                {
                                    flag = true;
                                    temp_cursorx = cursorx;
                                    temp_cursory = cursory;
                                    break;
                                }
                            }
                        }
                    }
                }
                Console.SetCursorPosition(3, 23);
                Console.WriteLine("                                                                                                                                                                        ");
            } while (true);
        }
        public static bool play_mod(int counter)
        {
            // A counter for the playing one by one
            if (counter % 2 == 0)
                return true;
            else
                return false;
        }
        public static (int[,], bool, bool, bool) pawn(char[,] board, int temp_cursorx, int temp_cursory, char temp, bool en_passant, bool hint_capture)
        {
            // Creating necessary variables for the function
            int counter = 1;
            int temp_board_col = (temp_cursorx - 5) / 4;
            int temp_board_row = (temp_cursory - 3) / 2;
            char capture_right = '.';
            char capture_left = '.';
            int[,] coordinate = new int[2, 50];
            // This array will hold positions which the chosen pawn can go
            bool pawncontrolblue;
            bool pawncontrolred;
            bool capture = false;
            bool check_blue = false;
            bool check_red = false;
            // "temp" will hold the chosen piece
            if (temp == 'P')
            {
                if (temp_board_row != 0)
                {
                    char forward = board[temp_board_row - 1, temp_board_col];
                    // Checking the borders
                    if (temp_board_col != 7)
                        capture_right = board[temp_board_row - 1, temp_board_col + 1];
                    if (temp_board_col != 0)
                        capture_left = board[temp_board_row - 1, temp_board_col - 1];
                    pawncontrolblue = false;
                    // If there is a space forward of the chosen pawn, then there will be shown as 'o'
                    if (forward == '.' && !hint_capture)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.SetCursorPosition(temp_cursorx, temp_cursory - 2);
                        Console.Write('o');
                        coordinate[0, counter] = temp_cursorx;
                        coordinate[1, counter] = temp_cursory - 2;
                        counter++;
                        pawncontrolblue = true;
                    }
                    // A pawn can be moved 2 forward if it did not moved before. We checked this here
                    if (temp_cursory == 15 && !hint_capture)
                    {
                        if (board[temp_board_row - 2, temp_board_col] == '.' && pawncontrolblue == true)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.SetCursorPosition(temp_cursorx, temp_cursory - 4);
                            Console.Write('o');
                            coordinate[0, counter] = temp_cursorx;
                            coordinate[1, counter] = temp_cursory - 4;
                            counter++;
                        }
                    }
                    if (temp_board_col != 7)
                    {
                        // If there are some pieces which can be capture by chosen pawn, then these pieces will shown as 'x'
                        if ((capture_right == 'r' || capture_right == 'n' || capture_right == 'b' || capture_right == 'q' || capture_right == 'p'))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.SetCursorPosition(temp_cursorx + 4, temp_cursory - 2);
                            Console.Write('x');
                            coordinate[0, counter] = temp_cursorx + 4;
                            coordinate[1, counter] = temp_cursory - 2;
                            counter++;
                            capture = true;
                        }
                        else if (capture_right == 'k' && !hint_capture)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.SetCursorPosition(temp_cursorx + 4, temp_cursory - 2);
                            Console.Write('K');
                            check_blue = true;
                        }
                        // Specifing this capture situation for the el-passant
                        if (temp_board_row == 3 && board[3, temp_board_col + 1] == 'p' && en_passant == true)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.SetCursorPosition(temp_cursorx + 4, temp_cursory - 2);
                            Console.Write('x');
                            coordinate[0, counter] = temp_cursorx + 4;
                            coordinate[1, counter] = temp_cursory - 2;
                            counter++;
                            capture = true;
                        }
                    }
                    if (temp_board_col != 0)
                    {
                        if ((capture_left == 'r' || capture_left == 'n' || capture_left == 'b' || capture_left == 'q' || capture_left == 'p'))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.SetCursorPosition(temp_cursorx - 4, temp_cursory - 2);
                            Console.Write('x');
                            coordinate[0, counter] = temp_cursorx - 4;
                            coordinate[1, counter] = temp_cursory - 2;
                            counter++;
                            capture = true;
                        }
                        else if (capture_left == 'k' && !hint_capture)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.SetCursorPosition(temp_cursorx - 4, temp_cursory - 2);
                            Console.Write('K');
                            check_blue = true;
                        }
                        if (temp_board_row == 3 && board[3, temp_board_col - 1] == 'p' && en_passant == true)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.SetCursorPosition(temp_cursorx - 4, temp_cursory - 2);
                            Console.Write('x');
                            coordinate[0, counter] = temp_cursorx - 4;
                            coordinate[1, counter] = temp_cursory - 2;
                            counter++;
                            capture = true;
                        }
                    }
                }

                return (coordinate, capture, check_blue, check_red);
            }
            // Same controlls for the red pawns
            else if (temp == 'p')
            {
                if (temp_board_row != 7)
                {
                    pawncontrolred = false;
                    char forward = board[temp_board_row + 1, temp_board_col];
                    if (temp_board_col != 7)
                        capture_right = board[temp_board_row + 1, temp_board_col + 1];
                    if (temp_board_col != 0)
                        capture_left = board[temp_board_row + 1, temp_board_col - 1];
                    if (forward == '.' && !hint_capture)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.SetCursorPosition(temp_cursorx, temp_cursory + 2);
                        Console.Write('o');
                        coordinate[0, counter] = temp_cursorx;
                        coordinate[1, counter] = temp_cursory + 2;
                        counter++;
                        pawncontrolred = true;
                    }
                    if (temp_cursory == 5)
                    {
                        if (board[temp_board_row + 2, temp_board_col] == '.' && pawncontrolred == true && !hint_capture)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.SetCursorPosition(temp_cursorx, temp_cursory + 4);
                            Console.Write('o');
                            coordinate[0, counter] = temp_cursorx;
                            coordinate[1, counter] = temp_cursory + 4;
                            counter++;
                        }
                    }
                    if (temp_board_col != 7)
                    {
                        if ((capture_right == 'R' || capture_right == 'N' || capture_right == 'B' || capture_right == 'Q' || capture_right == 'P'))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.SetCursorPosition(temp_cursorx + 4, temp_cursory + 2);
                            Console.Write('x');
                            coordinate[0, counter] = temp_cursorx + 4;
                            coordinate[1, counter] = temp_cursory + 2;
                            capture = true;
                            counter++;
                        }
                        else if (capture_right == 'K' && !hint_capture)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.SetCursorPosition(temp_cursorx + 4, temp_cursory + 2);
                            Console.Write('K');
                            check_red = true;
                        }
                        if (temp_board_row == 4 && board[4, temp_board_col + 1] == 'P' && en_passant == true)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.SetCursorPosition(temp_cursorx + 4, temp_cursory + 2);
                            Console.Write('x');
                            coordinate[0, counter] = temp_cursorx + 4;
                            coordinate[1, counter] = temp_cursory + 2;
                            capture = true;
                            counter++;
                        }
                    }
                    if (temp_board_col != 0)
                    {
                        if ((capture_left == 'R' || capture_left == 'N' || capture_left == 'N' || capture_left == 'Q' || capture_left == 'P'))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.SetCursorPosition(temp_cursorx - 4, temp_cursory + 2);
                            Console.Write('x');
                            coordinate[0, counter] = temp_cursorx - 4;
                            coordinate[1, counter] = temp_cursory + 2;
                            capture = true;
                            counter++;
                        }
                        else if (capture_left == 'K' && !hint_capture)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.SetCursorPosition(temp_cursorx - 4, temp_cursory + 2);
                            Console.Write('K');
                            check_red = true;
                        }
                        if (temp_board_row == 4 && board[4, temp_board_col - 1] == 'P' && en_passant == true)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.SetCursorPosition(temp_cursorx - 4, temp_cursory + 2);
                            Console.Write('x');
                            coordinate[0, counter] = temp_cursorx - 4;
                            coordinate[1, counter] = temp_cursory + 2;
                            capture = true;
                            counter++;
                        }
                    }
                }



                return (coordinate, capture, check_blue, check_red);
            }
            else
                return (coordinate, capture, check_blue, check_red);
        }
        public static (int[,], bool, bool, bool) king(char[,] board, int temp_cursorx, int temp_cursory, bool castling_blue, bool castling_red, bool control_check_blue, bool control_check_red, bool hint_capture)
        {
            int[,] king_border = { { 0, 0, 4, 4, 4, -4, -4, -4 }, { 2, -2, 2, -2, 0, 2, -2, 0 } };
            // This border array will hold where the king can move based on cursorx and cursory
            int[,] coordinate = new int[2, 50];
            int counter = 1;
            int x1 = 5; // left sınır
            int x2 = 33; // right sınır
            int y1 = 3; // top sınır
            int y2 = 17; // bottom sınır
            bool check_blue = false;
            bool check_red = false;

            bool capture = false;
            if (board[(temp_cursory - 3) / 2, (temp_cursorx - 5) / 4] == 'K')
            {
                for (int i = 0; i < king_border.GetLength(1); i++)
                {
                    if (temp_cursorx + king_border[0, i] >= x1 && temp_cursorx + king_border[0, i] <= x2 && temp_cursory + king_border[1, i] >= y1 && temp_cursory + king_border[1, i] <= y2)
                    {
                        char x = board[(temp_cursory + king_border[1, i] - 3) / 2, (temp_cursorx + king_border[0, i] - 5) / 4];
                        if (x == '.' && !hint_capture)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.SetCursorPosition(temp_cursorx + king_border[0, i], temp_cursory + king_border[1, i]);
                            Console.Write('o');
                            coordinate[0, counter] = temp_cursorx + king_border[0, i];
                            coordinate[1, counter] = temp_cursory + king_border[1, i];
                            counter++;
                        }
                        else if (x == 'r' || x == 'n' || x == 'b' || x == 'q' || x == 'p')
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.SetCursorPosition(temp_cursorx + king_border[0, i], temp_cursory + king_border[1, i]);
                            Console.Write('x');
                            coordinate[0, counter] = temp_cursorx + king_border[0, i];
                            coordinate[1, counter] = temp_cursory + king_border[1, i];
                            counter++;
                            capture = true;
                        }

                    }
                }
                if (control_check_blue == false && control_check_red == false && castling_blue == true && board[7, 1] == '.' && board[7, 2] == '.' && board[7, 3] == '.' && !hint_capture)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.SetCursorPosition(temp_cursorx - 8, temp_cursory);
                    Console.Write('o');
                    coordinate[0, counter] = temp_cursorx - 8;
                    coordinate[1, counter] = temp_cursory;
                    counter++;
                }
                else if (control_check_blue == false && control_check_red == false && castling_blue == true && board[7, 5] == '.' && board[7, 6] == '.' && !hint_capture)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.SetCursorPosition(temp_cursorx + 8, temp_cursory);
                    Console.Write('o');
                    coordinate[0, counter] = temp_cursorx + 8;
                    coordinate[1, counter] = temp_cursory;
                    counter++;
                }
            }
            else if (board[(temp_cursory - 3) / 2, (temp_cursorx - 5) / 4] == 'k')
            {
                for (int i = 0; i < king_border.GetLength(1); i++)
                {

                    if (temp_cursorx + king_border[0, i] >= x1 && temp_cursorx + king_border[0, i] <= x2 && temp_cursory + king_border[1, i] >= y1 && temp_cursory + king_border[1, i] <= y2)
                    {
                        char x = board[(temp_cursory + king_border[1, i] - 3) / 2, (temp_cursorx + king_border[0, i] - 5) / 4];
                        if (x == '.' && !hint_capture)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.SetCursorPosition(temp_cursorx + king_border[0, i], temp_cursory + king_border[1, i]);
                            Console.Write('o');
                            coordinate[0, counter] = temp_cursorx + king_border[0, i];
                            coordinate[1, counter] = temp_cursory + king_border[1, i];
                            counter++;
                        }
                        else if (x == 'R' || x == 'N' || x == 'B' || x == 'Q' || x == 'P')
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.SetCursorPosition(temp_cursorx + king_border[0, i], temp_cursory + king_border[1, i]);
                            Console.Write('x');
                            coordinate[0, counter] = temp_cursorx + king_border[0, i];
                            coordinate[1, counter] = temp_cursory + king_border[1, i];
                            counter++;
                            capture = true;
                        }

                    }
                }
                if (control_check_blue == false && control_check_red == false && castling_red == true && board[0, 1] == '.' && board[0, 2] == '.' && board[0, 3] == '.' && !hint_capture)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.SetCursorPosition(temp_cursorx - 8, temp_cursory);
                    Console.Write('o');
                    coordinate[0, counter] = temp_cursorx - 8;
                    coordinate[1, counter] = temp_cursory;
                    counter++;
                }
                else if (control_check_blue == false && control_check_red == false && castling_red == true && board[0, 5] == '.' && board[0, 6] == '.' && !hint_capture)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.SetCursorPosition(temp_cursorx + 8, temp_cursory);
                    Console.Write('o');
                    coordinate[0, counter] = temp_cursorx + 8;
                    coordinate[1, counter] = temp_cursory;
                    counter++;
                }
            }
            return (coordinate, capture, check_blue, check_red);
        }
        public static (int[,], bool, bool, bool) knight(char[,] board, int temp_cursorx, int temp_cursory, bool hint_capture)
        {
            int[,] knight_border = { { 4, 8, 8, 4, -4, -8, -8, -4 }, { 4, 2, -2, -4, -4, -2, 2, 4 } };
            // This border array will hold where the knight can move based on cursorx and cursory
            int[,] coordinate = new int[2, 50];
            int counter = 1;
            int x1 = 5; // left sınır
            int x2 = 33; // right sınır
            int y1 = 3; // top sınır
            int y2 = 17; // bottom sınır
            bool capture = false;
            bool check_blue = false;
            bool check_red = false;
            if (board[(temp_cursory - 3) / 2, (temp_cursorx - 5) / 4] == 'N')
            {
                for (int i = 0; i < knight_border.GetLength(1); i++)
                {
                    if (temp_cursorx + knight_border[0, i] >= x1 && temp_cursorx + knight_border[0, i] <= x2 && temp_cursory + knight_border[1, i] >= y1 && temp_cursory + knight_border[1, i] <= y2)
                    {
                        char x = board[(temp_cursory + knight_border[1, i] - 3) / 2, (temp_cursorx + knight_border[0, i] - 5) / 4];
                        if (x == '.' && !hint_capture)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.SetCursorPosition(temp_cursorx + knight_border[0, i], temp_cursory + knight_border[1, i]);
                            Console.Write('o');
                            coordinate[0, counter] = temp_cursorx + knight_border[0, i];
                            coordinate[1, counter] = temp_cursory + knight_border[1, i];
                            counter++;
                        }
                        else if (x == 'r' || x == 'n' || x == 'b' || x == 'q' || x == 'p')
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.SetCursorPosition(temp_cursorx + knight_border[0, i], temp_cursory + knight_border[1, i]);
                            Console.Write('x');
                            coordinate[0, counter] = temp_cursorx + knight_border[0, i];
                            coordinate[1, counter] = temp_cursory + knight_border[1, i];
                            counter++;
                            capture = true;
                        }
                        else if (x == 'k' && !hint_capture)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.SetCursorPosition(temp_cursorx + knight_border[0, i], temp_cursory + knight_border[1, i]);
                            Console.Write('K');
                            check_blue = true;
                        }
                    }
                }
            }
            else if (board[(temp_cursory - 3) / 2, (temp_cursorx - 5) / 4] == 'n')
            {
                for (int i = 0; i < knight_border.GetLength(1); i++)
                {
                    if (temp_cursorx + knight_border[0, i] >= x1 && temp_cursorx + knight_border[0, i] <= x2 && temp_cursory + knight_border[1, i] >= y1 && temp_cursory + knight_border[1, i] <= y2)
                    {
                        char x = board[(temp_cursory + knight_border[1, i] - 3) / 2, (temp_cursorx + knight_border[0, i] - 5) / 4];
                        if (x == '.' && !hint_capture)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.SetCursorPosition(temp_cursorx + knight_border[0, i], temp_cursory + knight_border[1, i]);
                            Console.Write('o');
                            coordinate[0, counter] = temp_cursorx + knight_border[0, i];
                            coordinate[1, counter] = temp_cursory + knight_border[1, i];
                            counter++;
                        }
                        else if (x == 'R' || x == 'N' || x == 'B' || x == 'Q' || x == 'P')
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.SetCursorPosition(temp_cursorx + knight_border[0, i], temp_cursory + knight_border[1, i]);
                            Console.Write('x');
                            coordinate[0, counter] = temp_cursorx + knight_border[0, i];
                            coordinate[1, counter] = temp_cursory + knight_border[1, i];
                            counter++;
                            capture = true;
                        }
                        else if (x == 'K' && !hint_capture)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.SetCursorPosition(temp_cursorx + knight_border[0, i], temp_cursory + knight_border[1, i]);
                            Console.Write('K');
                            check_red = true;
                        }
                    }
                }
            }
            return (coordinate, capture, check_blue, check_red);
        }
        public static (int[,], bool, bool, bool) bishop(char[,] board, int temp_cursorx, int temp_cursory, bool hint_capture)
        {
            int counter = 1;
            int[,] coordinate = new int[2, 50];
            char x;
            int[,] bishop_border = { { 1, -1, -1, +1 }, { 1, -1, 1, -1 } };
            // This border array will hold where the bishop can move based on cursorx and cursory
            bool capture = false;
            bool check_blue = false;
            bool check_red = false;
            if (board[(temp_cursory - 3) / 2, (temp_cursorx - 5) / 4] == 'B')
            {
                for (int j = 0; j < bishop_border.GetLength(1); j++)
                {
                    for (int i = 1; i <= board.GetLength(0); i++)
                    {
                        if ((((((temp_cursorx - 5) / 4) + (i * bishop_border[0, j])) < board.GetLength(0))) && ((((temp_cursorx - 5) / 4) + (i * bishop_border[0, j])) >= 0) && ((((temp_cursory - 3) / 2) + (i * bishop_border[1, j])) < board.GetLength(1)) && ((((temp_cursory - 3) / 2) + (i * bishop_border[1, j])) >= 0))
                        {

                            x = board[(((temp_cursory - 3) / 2) + (i * bishop_border[1, j])), ((temp_cursorx - 5) / 4) + (i * bishop_border[0, j])];
                            if (x == '.' && !hint_capture)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.SetCursorPosition(temp_cursorx + 4 * (i * bishop_border[0, j]), temp_cursory + 2 * (i * bishop_border[1, j]));
                                Console.Write('o');
                                coordinate[0, counter] = temp_cursorx + 4 * (i * bishop_border[0, j]);
                                coordinate[1, counter] = temp_cursory + 2 * (i * bishop_border[1, j]);
                                counter++;
                            }
                            else if (x == 'r' || x == 'n' || x == 'b' || x == 'q' || x == 'b' || x == 'n' || x == 'r' || x == 'p')
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.SetCursorPosition(temp_cursorx + 4 * (i * bishop_border[0, j]), temp_cursory + 2 * (i * bishop_border[1, j]));
                                Console.Write('x');
                                coordinate[0, counter] = temp_cursorx + 4 * (i * bishop_border[0, j]);
                                coordinate[1, counter] = temp_cursory + 2 * (i * bishop_border[1, j]);
                                counter++;
                                capture = true;
                                break;
                            }
                            else if (x == 'k' && !hint_capture)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.SetCursorPosition(temp_cursorx + 4 * (i * bishop_border[0, j]), temp_cursory + 2 * (i * bishop_border[1, j]));
                                Console.Write('K');
                                check_blue = true;
                                break;
                            }
                            else
                                break;
                        }
                    }
                }
            }
            else if (board[(temp_cursory - 3) / 2, (temp_cursorx - 5) / 4] == 'b')
            {
                for (int j = 0; j < bishop_border.GetLength(1); j++)
                {
                    for (int i = 1; i <= board.GetLength(0); i++)
                    {
                        if ((((((temp_cursorx - 5) / 4) + (i * bishop_border[0, j])) < board.GetLength(0))) && ((((temp_cursorx - 5) / 4) + (i * bishop_border[0, j])) >= 0) && ((((temp_cursory - 3) / 2) + (i * bishop_border[1, j])) < board.GetLength(1)) && ((((temp_cursory - 3) / 2) + (i * bishop_border[1, j])) >= 0))
                        {

                            x = board[(((temp_cursory - 3) / 2) + (i * bishop_border[1, j])), ((temp_cursorx - 5) / 4) + (i * bishop_border[0, j])];
                            if (x == '.' && !hint_capture)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.SetCursorPosition(temp_cursorx + 4 * (i * bishop_border[0, j]), temp_cursory + 2 * (i * bishop_border[1, j]));
                                Console.Write('o');
                                coordinate[0, counter] = temp_cursorx + 4 * (i * bishop_border[0, j]);
                                coordinate[1, counter] = temp_cursory + 2 * (i * bishop_border[1, j]);
                                counter++;
                            }
                            else if (x == 'R' || x == 'N' || x == 'B' || x == 'Q' || x == 'B' || x == 'N' || x == 'R' || x == 'P')
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.SetCursorPosition(temp_cursorx + 4 * (i * bishop_border[0, j]), temp_cursory + 2 * (i * bishop_border[1, j]));
                                Console.Write('x');
                                coordinate[0, counter] = temp_cursorx + 4 * (i * bishop_border[0, j]);
                                coordinate[1, counter] = temp_cursory + 2 * (i * bishop_border[1, j]);
                                capture = true;
                                counter++;
                                break;
                            }
                            else if (x == 'K' && !hint_capture)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.SetCursorPosition(temp_cursorx + 4 * (i * bishop_border[0, j]), temp_cursory + 2 * (i * bishop_border[1, j]));
                                Console.Write('K');
                                check_red = true;
                                break;
                            }
                            else
                                break;
                        }
                    }
                }
            }
            return (coordinate, capture, check_blue, check_red);
        }
        public static (int[,], bool, bool, bool) rook(char[,] board, int temp_cursorx, int temp_cursory, bool hint_capture)
        {
            int counter = 1;
            int[,] coordinate = new int[2, 50];
            char x;
            int[,] rook_border = { { 1, -1, 0, 0 }, { 0, 0, 1, -1 } };
            // This border array will hold where the rook can move based on cursorx and cursory
            bool capture = false;
            bool check_blue = false;
            bool check_red = false;
            if (board[(temp_cursory - 3) / 2, (temp_cursorx - 5) / 4] == 'R')
            {
                for (int j = 0; j < rook_border.GetLength(1); j++)
                {
                    for (int i = 1; i <= board.GetLength(0); i++)
                    {
                        if ((((((temp_cursorx - 5) / 4) + (i * rook_border[0, j])) < board.GetLength(0))) && ((((temp_cursorx - 5) / 4) + (i * rook_border[0, j])) >= 0) && ((((temp_cursory - 3) / 2) + (i * rook_border[1, j])) < board.GetLength(1)) && ((((temp_cursory - 3) / 2) + (i * rook_border[1, j])) >= 0))
                        {

                            x = board[(((temp_cursory - 3) / 2) + (i * rook_border[1, j])), ((temp_cursorx - 5) / 4) + (i * rook_border[0, j])];
                            if (x == '.' && !hint_capture)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.SetCursorPosition(temp_cursorx + 4 * (i * rook_border[0, j]), temp_cursory + 2 * (i * rook_border[1, j]));
                                Console.Write('o');
                                coordinate[0, counter] = temp_cursorx + 4 * (i * rook_border[0, j]);
                                coordinate[1, counter] = temp_cursory + 2 * (i * rook_border[1, j]);
                                counter++;
                            }
                            else if (x == 'r' || x == 'n' || x == 'b' || x == 'q' || x == 'b' || x == 'n' || x == 'r' || x == 'p')
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.SetCursorPosition(temp_cursorx + 4 * (i * rook_border[0, j]), temp_cursory + 2 * (i * rook_border[1, j]));
                                Console.Write('x');
                                coordinate[0, counter] = temp_cursorx + 4 * (i * rook_border[0, j]);
                                coordinate[1, counter] = temp_cursory + 2 * (i * rook_border[1, j]);
                                capture = true;
                                counter++;
                                break;
                            }
                            else if (x == 'k' && !hint_capture)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.SetCursorPosition(temp_cursorx + 4 * (i * rook_border[0, j]), temp_cursory + 2 * (i * rook_border[1, j]));
                                Console.Write('K');
                                check_blue = true;
                                break;
                            }
                            else
                                break;
                        }
                    }
                }
            }
            else if (board[(temp_cursory - 3) / 2, (temp_cursorx - 5) / 4] == 'r')
            {
                for (int j = 0; j < rook_border.GetLength(1); j++)
                {
                    for (int i = 1; i <= board.GetLength(0); i++)
                    {
                        if ((((((temp_cursorx - 5) / 4) + (i * rook_border[0, j])) < board.GetLength(0))) && ((((temp_cursorx - 5) / 4) + (i * rook_border[0, j])) >= 0) && ((((temp_cursory - 3) / 2) + (i * rook_border[1, j])) < board.GetLength(1)) && ((((temp_cursory - 3) / 2) + (i * rook_border[1, j])) >= 0))
                        {
                            x = board[(((temp_cursory - 3) / 2) + (i * rook_border[1, j])), ((temp_cursorx - 5) / 4) + (i * rook_border[0, j])];
                            if (x == '.' && !hint_capture)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.SetCursorPosition(temp_cursorx + 4 * (i * rook_border[0, j]), temp_cursory + 2 * (i * rook_border[1, j]));
                                Console.Write('o');
                                coordinate[0, counter] = temp_cursorx + 4 * (i * rook_border[0, j]);
                                coordinate[1, counter] = temp_cursory + 2 * (i * rook_border[1, j]);
                                counter++;
                            }
                            else if (x == 'R' || x == 'N' || x == 'B' || x == 'Q' || x == 'B' || x == 'N' || x == 'R' || x == 'P')
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.SetCursorPosition(temp_cursorx + 4 * (i * rook_border[0, j]), temp_cursory + 2 * (i * rook_border[1, j]));
                                Console.Write('x');
                                coordinate[0, counter] = temp_cursorx + 4 * (i * rook_border[0, j]);
                                coordinate[1, counter] = temp_cursory + 2 * (i * rook_border[1, j]);
                                counter++;
                                capture = true;
                                break;
                            }
                            else if (x == 'K' && !hint_capture)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.SetCursorPosition(temp_cursorx + 4 * (i * rook_border[0, j]), temp_cursory + 2 * (i * rook_border[1, j]));
                                Console.Write('K');
                                check_red = true;
                                break;
                            }
                            else
                                break;
                        }
                    }
                }
            }
            return (coordinate, capture, check_blue, check_red);
        }
        public static (int[,], bool, bool, bool) queen(char[,] board, int temp_cursorx, int temp_cursory, bool hint_capture)
        {
            int counter = 1;
            int[,] coordinate = new int[2, 50];
            char x;
            bool capture = false;
            int[,] queen_border = { { 1, -1, -1, +1, 1, -1, 0, 0 }, { 1, -1, 1, -1, 0, 0, 1, -1 } };
            // This border array will hold where the queen can move based on cursorx and cursory
            bool check_blue = false;
            bool check_red = false;
            if (board[(temp_cursory - 3) / 2, (temp_cursorx - 5) / 4] == 'Q')
            {
                for (int j = 0; j < queen_border.GetLength(1); j++)
                {
                    for (int i = 1; i <= board.GetLength(0); i++)
                    {
                        if ((((((temp_cursorx - 5) / 4) + (i * queen_border[0, j])) < board.GetLength(0))) && ((((temp_cursorx - 5) / 4) + (i * queen_border[0, j])) >= 0) && ((((temp_cursory - 3) / 2) + (i * queen_border[1, j])) < board.GetLength(1)) && ((((temp_cursory - 3) / 2) + (i * queen_border[1, j])) >= 0))
                        {

                            x = board[(((temp_cursory - 3) / 2) + (i * queen_border[1, j])), ((temp_cursorx - 5) / 4) + (i * queen_border[0, j])];
                            if (x == '.' && !hint_capture)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.SetCursorPosition(temp_cursorx + 4 * (i * queen_border[0, j]), temp_cursory + 2 * (i * queen_border[1, j]));
                                Console.Write('o');
                                coordinate[0, counter] = temp_cursorx + 4 * (i * queen_border[0, j]);
                                coordinate[1, counter] = temp_cursory + 2 * (i * queen_border[1, j]);
                                counter++;
                            }
                            else if (x == 'r' || x == 'n' || x == 'b' || x == 'q' || x == 'b' || x == 'n' || x == 'r' || x == 'p')
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.SetCursorPosition(temp_cursorx + 4 * (i * queen_border[0, j]), temp_cursory + 2 * (i * queen_border[1, j]));
                                Console.Write('x');
                                coordinate[0, counter] = temp_cursorx + 4 * (i * queen_border[0, j]);
                                coordinate[1, counter] = temp_cursory + 2 * (i * queen_border[1, j]);
                                capture = true;
                                counter++;
                                break;
                            }
                            else if (x == 'k' && !hint_capture)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.SetCursorPosition(temp_cursorx + 4 * (i * queen_border[0, j]), temp_cursory + 2 * (i * queen_border[1, j]));
                                Console.Write('K');
                                check_blue = true;
                                break;
                            }
                            else
                                break;
                        }
                    }
                }
            }
            else if (board[(temp_cursory - 3) / 2, (temp_cursorx - 5) / 4] == 'q')
            {
                for (int j = 0; j < queen_border.GetLength(1); j++)
                {
                    for (int i = 1; i <= board.GetLength(0); i++)
                    {
                        if ((((((temp_cursorx - 5) / 4) + (i * queen_border[0, j])) < board.GetLength(0))) && ((((temp_cursorx - 5) / 4) + (i * queen_border[0, j])) >= 0) && ((((temp_cursory - 3) / 2) + (i * queen_border[1, j])) < board.GetLength(1)) && ((((temp_cursory - 3) / 2) + (i * queen_border[1, j])) >= 0))
                        {

                            x = board[(((temp_cursory - 3) / 2) + (i * queen_border[1, j])), ((temp_cursorx - 5) / 4) + (i * queen_border[0, j])];
                            if (x == '.' && !hint_capture)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.SetCursorPosition(temp_cursorx + 4 * (i * queen_border[0, j]), temp_cursory + 2 * (i * queen_border[1, j]));
                                Console.Write('o');
                                coordinate[0, counter] = temp_cursorx + 4 * (i * queen_border[0, j]);
                                coordinate[1, counter] = temp_cursory + 2 * (i * queen_border[1, j]);
                                counter++;
                            }
                            else if (x == 'R' || x == 'N' || x == 'B' || x == 'Q' || x == 'B' || x == 'N' || x == 'R' || x == 'P')
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.SetCursorPosition(temp_cursorx + 4 * (i * queen_border[0, j]), temp_cursory + 2 * (i * queen_border[1, j]));
                                Console.Write('x');
                                coordinate[0, counter] = temp_cursorx + 4 * (i * queen_border[0, j]);
                                coordinate[1, counter] = temp_cursory + 2 * (i * queen_border[1, j]);
                                capture = true;
                                counter++;
                                break;
                            }
                            else if (x == 'K' && !hint_capture)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.SetCursorPosition(temp_cursorx + 4 * (i * queen_border[0, j]), temp_cursory + 2 * (i * queen_border[1, j]));
                                Console.Write('K');
                                check_red = true;
                                break;
                            }
                            else
                                break;
                        }

                    }
                }
            }
            return (coordinate, capture, check_blue, check_red);
        }
        public static char Color(char i, bool control_check_blue, bool control_check_red, int counter)
        {
            if (control_check_blue)
            {
                if (i == 'k')
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    i = Convert.ToChar((Convert.ToString(i).ToUpper()));

                    return i;
                }
            }
            else
            {
                if (i == 'k')
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    i = Convert.ToChar((Convert.ToString(i).ToUpper()));

                    return i;
                }
            }
            if (control_check_red)
            {
                if (i == 'K')
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    i = Convert.ToChar((Convert.ToString(i).ToUpper()));

                    return i;
                }
            }
            else
            {
                if (i == 'K')
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    i = Convert.ToChar((Convert.ToString(i).ToUpper()));

                    return i;
                }
            }
            if (i == 'r' || i == 'n' || i == 'b' || i == 'q' || i == 'b' || i == 'n' || i == 'r' || i == 'p')
            {
                Console.ForegroundColor = ConsoleColor.Red;
                i = Convert.ToChar((Convert.ToString(i).ToUpper()));

                return i;
            }
            else if (i == 'R' || i == 'N' || i == 'B' || i == 'Q' || i == 'B' || i == 'N' || i == 'R' || i == 'P')
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                i = Convert.ToChar((Convert.ToString(i).ToUpper()));

                return i;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Black;
                return i;
            }

        }
    }
}
