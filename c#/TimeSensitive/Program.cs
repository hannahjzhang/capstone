// TODO: take timer code and implement in the game

using System;
using System.Collections.Generic;
using System.Threading;

namespace SimpleGameWithmap
{
	// Entity class is for any moving characters
	class Entity
	{
		// instance variables for the entity
		public int x, y; // coordinates
		public char icon; // character
		public ConsoleColor m_color; // color
		public Action onUpdate;

		// constructor for the entity
		public Entity(int x, int y, char a_icon, ConsoleColor a_color)
		{
			this.x = x;
			this.y = y;
			this.icon = a_icon;
			this.m_color = a_color;
		}

		// function to draw the entity
		public void Draw()
		{
			ConsoleColor normalColor = Console.ForegroundColor;
			Console.SetCursorPosition(x, y);
			Console.ForegroundColor = m_color;
			Console.Write(icon);
			Console.ForegroundColor = normalColor;
		}

		// function to move the character
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
	// draws the board and implements logic for the game
	class Program
	{
		public static void Main(string[] args)
		{
			// default color of the screen
			ConsoleColor normalColor = Console.ForegroundColor;
			// draw the map with a bunch of strings
			string mapStr =
			".............................." +
			".............................." +
			".............................." +
			"....#........................." +
			"....#............########....." +
			"....####........#............." +
			"....#...........#..#####......" +
			"....#...........#......###...." +
			"....#...........#####....###.." +
			"................#..........#.." +
			"...........................#.." +
			"................#..........#.." +
			"................#..........#.." +
			"................#####D######.." +
			"..............................";

			// TODO: write code that reads through mapstr and finds proper width and height for map (instead of doing it manually)
			// load the map from another file
			// need help getting maze.txt
			mapStr = System.IO.File.ReadAllText("../../../maze.txt");
			Console.WriteLine(mapStr);
			Console.ReadKey();
			char[,] map = new char[21, 44];

			// converting the map into a 2d array
			int strIndex = 0;
			for (int row = 0; row < map.GetLength(0); ++row)
			{
				for (int col = 0; col < map.GetLength(1); ++col)
				{
					map[row, col] = mapStr[strIndex++];
				}
			}
			int width = map.GetLength(1), height = map.GetLength(0);

			// characters that the player cannot pass through
			string blockingWall = "#";
			// int width = 30, height = 15, oxPlayer = 0, oyPlayer = 0, oxEnemy = 0, oyEnemy = 0;
			int enemyTimer = 0;
			int enemyWaitTime = 1000;
			// if the game is running or not
			bool running = true; 
			// use entity defined above
			Entity item = new Entity(19, 10, '$', ConsoleColor.Yellow);
			Entity player = new Entity(3, 4, '@', ConsoleColor.Green);
			Entity enemy = new Entity(14, 7, 'X', ConsoleColor.Red);
			List<Entity> entities = new List<Entity>();
			entities.Add(player);
			entities.Add(item);
			entities.Add(enemy);
			Random rand = new Random(0);
			ConsoleKeyInfo userInput = new ConsoleKeyInfo();

			// moving the player based on input
			player.onUpdate = () => {
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
				// if the key is collected, give a message and exit the game
				if (touchingKey)
				{
					entities.Remove(item);
					blockingWall = blockingWall.Replace("D", "");
					Console.WriteLine("\rKey Collected!");
					running = false;
				}
				// if the player touches the enemy, give a message and exit the game
				if (touchingEnemy)
				{
					running = false;
					Console.WriteLine("\rGame Over");
				}
			};

			// move the enemy randomly upon user input
			enemy.onUpdate = () =>
			{
				enemyWaitTime -= deltaTime;
				if (enemyWaitTime > 0)
				{
					return;
				}
				enemyWaitTime = 1000;
				oxEnemy = enemy.x;
				oyEnemy = enemy.y;
				string enemyMoves = "wasd";
				char enemyMove = enemyMoves[rand.Next() % enemyMoves.Length];
				enemy.Move(enemyMove);
				//enemyTimer = now + 1000;

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
				// draw the map
				Console.SetCursorPosition(0, 0);
				for (int row = 0; row < height; ++row)
				{
					for (int col = 0; col < width; ++col)
					{
						char c = map[row, col];
						Console.Write(c);
					}
					Console.Write('\n');
				}
				entities.ForEach(e => e.Draw()); // drawing the entities
				Console.SetCursorPosition(0, height);

				int now = System.Environment.TickCount; //tick --> ms
				deltaTime = now - lastTime;
				lastTime = now;
				Console.WriteLine(now + " " + deltaTime + " ");
				int soon = now + 50;

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