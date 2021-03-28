using System;
using System.Collections.Generic;
using System.Threading;

namespace SimpleGameWithmap
{
	// Entity class is for any moving characters
	class Entity
	{
		public int x, y; // coords of the entity
		public char icon; // what character represents the entitiy 
		public ConsoleColor m_color; // what color is the entitiy
		public Action onUpdate;

		// constructor for the entity
		public Entity(int x, int y, char a_icon, ConsoleColor a_color)
		{
			this.x = x;
			this.y = y;
			this.icon = a_icon;
			this.m_color = a_color;
		}

		// draws the entity
		public void Draw()
		{
			ConsoleColor normalColor = Console.ForegroundColor;
			Console.SetCursorPosition(x, y);
			Console.ForegroundColor = m_color;
			Console.Write(icon);
			Console.ForegroundColor = normalColor;
		}

		// moves the character
		public void Move(char move)
		{
			switch (move)
			{
				case 'w': --y; break;
				case 'a': --x; break;
				case 's': ++y; break;
				case 'd': ++x; break;
			}
		}
	}

	class Program
	{
		public static void Main(string[] args)
		{
			ConsoleColor normalColor = Console.ForegroundColor; // default color of the screen
			string mapStr =
			".............................." +
			".............................." +
			".............................." +
			".............................." +
			".................########....." +
			"................#............." +
			"................#..#####......" +
			"................#......###...." +
			"................#####....###.." +
			"................#..........#.." +
			"................#..........#.." +
			"................#..........#.." +
			"................#..........#.." +
			"................#####D######.." +
			".............................."; 
			mapStr = System.IO.File.ReadAllText("../../../maze.txt");
			// Console.WriteLine(mapStr);
			// Console.ReadKey();

			// convert the loaded map from maze.txt into a 2d array
			int firstNewLine = 0; // value for the width
			bool isDone = false;
			int count = 0; // value for the height

			for (int i = 0; i < mapStr.Length; i++)
            {
				// Console.Write($"({i}{mapStr[i]})");
				if (mapStr[i] == '\n')
                {
					count++;
					if (isDone == false)
                    {
						firstNewLine = i;
						isDone = true;
					}
                }
            }
			firstNewLine++; // always one off
			// Console.WriteLine(firstNewLine);
			// Console.WriteLine(count);

			char[,] map = new char[count, firstNewLine]; // print with computed values

			int strIndex = 0;
			for (int row = 0; row < map.GetLength(0); ++row)
			{
				for (int col = 0; col < map.GetLength(1); ++col)
				{
					map[row, col] = mapStr[strIndex++];
				}
			}
			string blockingWall = "#D"; // characters that the player cannot pass through
			int width = firstNewLine, height = count, oxPlayer = 0, oyPlayer = 0, oxEnemy = 0, oyEnemy = 0;
			bool running = true; // if the game is running or not
			Entity item = new Entity(19, 10, '$', ConsoleColor.Yellow);
			Entity player = new Entity(3, 4, '@', ConsoleColor.Green);
			Entity enemy = new Entity(14, 7, 'X', ConsoleColor.Red);
			List<Entity> entities = new List<Entity>();
			entities.Add(player);
			entities.Add(item);
			entities.Add(enemy);
			Random rand = new Random(0);
			ConsoleKeyInfo userInput = new ConsoleKeyInfo();
			player.onUpdate = () => {
				// move player
				oxPlayer = player.x;
				oyPlayer = player.y;

				switch (userInput.KeyChar)
				{
					case 'w': case 'a': case 's': case 'd': player.Move(userInput.KeyChar); break;
					case 'q': case (char)27: running = false; break;
				}


				bool touchingKey = (player.y == item.y && player.x == item.x);
				bool touchingEnemy = (player.y == enemy.y && player.x == enemy.x);
				bool outOfBounds = player.x < 0 || player.x >= width || player.y < 0 || player.y >= height;
				bool wallHit = !outOfBounds && blockingWall.IndexOf(map[player.y, player.x]) >= 0;

				// if the player is touching a barrier
				if (outOfBounds || wallHit)
				{
					if (wallHit)
					{
						Console.Write('\a');
					}
					player.x = oxPlayer;
					player.y = oyPlayer;
				}
				// deciding if the key was collected or not
				if (touchingKey)
				{
					entities.Remove(item);
					blockingWall = blockingWall.Replace("D", "");
					Console.WriteLine("Key Collected!");
				}
				// if the player touches the enemy
				if (touchingEnemy)
				{
					running = false;
					Console.WriteLine("\rGame Over");
				}
			};
			enemy.onUpdate = () =>
			{
				// move enemy
				oxEnemy = enemy.x;
				oyEnemy = enemy.y;
				string enemyMoves = "wasd";
				char enemyMove = enemyMoves[rand.Next() % enemyMoves.Length];
				enemy.Move(enemyMove);
				bool outOfBounds = enemy.x < 0 || enemy.x >= width || enemy.y < 0 || enemy.y >= height;
				bool wallHit = !outOfBounds && blockingWall.IndexOf(map[enemy.y, enemy.x]) >= 0;

				// if the enemy is touching a barrier
				if (outOfBounds || wallHit)
				{
					if (wallHit)
					{
						Console.Write('\a');
					}
					enemy.x = oxEnemy;
					enemy.y = oyEnemy;
				}
			};


			// game loop
			while (running)
			{
				//drawing
				Console.SetCursorPosition(0, 0);
				for (int row = 0; row < height; ++row)
				{
					for (int col = 0; col < width-1; ++col)
					{
						Console.Write(map[row, col]);
					}
					Console.Write('\n');
				}
				entities.ForEach(e => e.Draw()); // drawing the entities
				Console.SetCursorPosition(0, height);

				int now = System.Environment.TickCount; //tick --> ms
				Console.WriteLine(now);
				int soon = now + 2000;

				// getting user input
				if (!Console.KeyAvailable)
				{
					while (System.Environment.TickCount <= soon && !Console.KeyAvailable)
					{
						Thread.Sleep(1);
					}
				}
				// TODO Turn into ternary
				if (Console.KeyAvailable)
				{
					userInput = Console.ReadKey(); // blocking call
				}
				else
				{
					userInput = new ConsoleKeyInfo();
				}


				//update
				for (int i = 0; i < entities.Count; i++)
				{
					if (entities[i].onUpdate != null)
					{
						entities[i].onUpdate.Invoke();
					}
				}
			}
		}
	}
}