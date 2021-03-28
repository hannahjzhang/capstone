using System;
namespace SimpleGame {
	class Program {
		public static void Main(string[] args) {
			// TODO: boundary check for x and y values so player doesn't get off screen, win location
			int width = 30, height = 15, x = 3, y = 4;
			char player = '@', world = '.', winSymbol = '!';
			(int row, int col) winLocation = (3, 3);
			// ((int)Rando.Next%width,(int)Rando.Next%height);
			bool running = true;
			while (running) {

				// draw the board in addition to the win location
				Console.SetCursorPosition(0, 0);
				for(int row = 0; row < height; ++row) {
					for(int col = 0; col < width; ++col) {
						if (row == winLocation.row && col == winLocation.col)
                        {
							Console.Write(winSymbol);
                        }
						else
                        {
							Console.Write(world);
						}
					}
					Console.Write('\n');
				}
				Console.SetCursorPosition(x, y);


				// change color
				ConsoleColor f = Console.ForegroundColor;
				Console.ForegroundColor = ConsoleColor.Green;
				Console.Write(player);
				Console.ForegroundColor = f;
				Console.SetCursorPosition(0, height);

				// check for win
				if (x == winLocation.row && y == winLocation.col)
                {
					Console.WriteLine("You win!!");
					running = false;
					break;
                }

				// user input
				ConsoleKeyInfo userInput = Console.ReadKey();
				// update the game
				switch (userInput.KeyChar) {
				case 'w': --y; break;
				case 'a': --x; break;
				case 's': ++y; break;
				case 'd': ++x; break;
				case 'q':  case (char)27: running = false; break;
				}
			}
		}
	}
}
