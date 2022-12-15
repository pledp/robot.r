using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace pLdevTest
{
    public class PlayGround
    {
        bool lightsOn = false;

        private Vector2[,] tilesMovement;
        private int[,] randomTime;
        private float[,] tileOpacity;
        private bool[,] goDown;
        private double[,] elapsedTime;

        public Rectangle playground;
        private int width;
        private Texture2D pgTexture;
        public static Color pgColor;
        public static Color pgColor2;

        public Gem[] gems;
        public EnemyBlock[] enemies;
        public ColoredBlock[] coloredBlocks;
        public List<Bullet> bullets;

        Texture2D lightMask;

        RenderTarget2D lightsTarget;
        RenderTarget2D mainTarget;

        Effect lightingEffect;

        Texture2D blackRectangle;

        public PlaygroundPlayer player;
        public FinishFlag finishFlag;
        public PlayGround(GraphicsDevice _graphics, int playgroundWidth)
        {
            // Create a playground
            width = playgroundWidth;
            playground = new Rectangle(_graphics.Viewport.Width - width - 50, 10, width, 400);
            bullets = new List<Bullet>();
            
            pgTexture = new Texture2D(_graphics, 1, 1);
            pgColor = new Color(153, 225, 217);
            pgColor2 = new Color(133, 205, 197);
            pgTexture.SetData(new[] { pgColor });

            // Create a player on the playground. Move in a 21x15 grid.
            player = new PlaygroundPlayer(_graphics, playground.X, playground.Y);
            finishFlag = new FinishFlag(_graphics, playground.X, playground.Y, 0,0);
            blackRectangle = new Texture2D(_graphics, 1, 1);
            blackRectangle.SetData(new[] { Color.Black });

            CreateTiles();
        }
        public void CreateTiles()
        {
            player.objectPos = new Vector2(0, 0);
            player.elapsedTime = 0;

            tilesMovement = new Vector2[22, 16];
            randomTime = new int[22, 16];
            tileOpacity = new float[22, 16];
            goDown = new bool[22, 16];
            elapsedTime = new double[22, 16];

            Random rnd = new Random();
            for (int x = 0; x < 22; x++)
            {
                for (int y = 0; y < 16; y++)
                {
                    randomTime[x, y] = 30;
                    tileOpacity[x, y] = 0f;
                    goDown[x, y] = false;
                    elapsedTime[x, y] = 0f;
                }
            }
        }
        public void LoadContent(ContentManager Content, GraphicsDevice _graphics)
        {
            lightMask = Content.Load<Texture2D>("lightmask");
            lightingEffect = Content.Load<Effect>("effect1");

            var pp = _graphics.PresentationParameters;
            lightsTarget = new RenderTarget2D(
                _graphics, pp.BackBufferWidth, pp.BackBufferHeight);
            mainTarget = new RenderTarget2D(
                _graphics, pp.BackBufferWidth, pp.BackBufferHeight);

            player.LoadContent(Content);
        }
        public void Update(GameTime gameTime)
        {
            player.Update(gameTime);

            try
            {
                foreach (Bullet bullet in bullets)
                {
                    if (!bullet.spent)
                    {
                        bullet.Update(gameTime);
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }

            if (MissionHandler.MissionCategory[MissionHandler.Mission] == MissionTypes.CoinLevel && gems != null)
            {
                foreach (Gem gem in gems)
                {
                    if (!gem.PickedUp)
                    {
                        gem.Update(gameTime);
                    }

                }
            }

            if (MissionHandler.MissionCategory[MissionHandler.Mission] == MissionTypes.SortLevel && coloredBlocks != null)
            {
                foreach (ColoredBlock block in coloredBlocks)
                {
                    if(!block.sorted)
                    {
                        block.Update(gameTime);
                    }
                }
            }

            else if (MissionHandler.MissionCategory[MissionHandler.Mission] == MissionTypes.FlagLevel)
            {
                finishFlag.Update(gameTime);
            }

            else if ((MissionHandler.MissionCategory[MissionHandler.Mission] == MissionTypes.EnemyLevel || MissionHandler.MissionCategory[MissionHandler.Mission] == MissionTypes.KillLevel) && enemies != null)
            {
                foreach (EnemyBlock enemy in enemies)
                {
                    if (!enemy.killed)
                    {
                        enemy.Update(gameTime);
                    }
                }
            }

            // Create tile transistion
            for (int x = 0; x < 22; x++)
            {
                for (int y = 0; y < 16; y++)
                {
                    elapsedTime[x,y] += gameTime.ElapsedGameTime.TotalSeconds;
                    if (tilesMovement[x,y].Y == playground.Y + (y * 25) - randomTime[x, y]/2 || goDown[x,y])
                    {
                        if (!goDown[x,y])
                        {
                            goDown[x,y] = true;
                            elapsedTime[x, y] = 0;
                        }
                        tilesMovement[x, y] = TileTransistion(new Vector2(playground.X + (x * 25), playground.Y + (y * 25)), gameTime, new Vector2(playground.X + (x * 25), playground.Y + (y * 25) - randomTime[x, y]/2), elapsedTime[x, y], x, y);
                    }
                    else
                    {
                        tilesMovement[x, y] = TileTransistion(new Vector2(playground.X + (x * 25), playground.Y + (y * 25) - randomTime[x, y]/2), gameTime, new Vector2(playground.X + (x * 25), playground.Y + (y * 25) + randomTime[x, y]), elapsedTime[x,y], x, y);
                        tileOpacity[x, y] = TileTransistion(new Vector2(1f, 0f), gameTime, new Vector2(0f), elapsedTime[x,y], x, y).X;
                    }
                }
            }
        }

        public void DrawBoard(SpriteBatch _spriteBatch, GameTime gameTime, GraphicsDeviceManager _graphics)
        {
            /*for (int y = 0; y < amountOfTiles.Length; y++)
            {
                for(int x = 0; x < amountOfTiles[y]; x++)
                {
                    _spriteBatch.Draw(pgTexture, new Rectangle(playground.X + (x * 25), playground.Y + (y * 25), 25, 25), pgColor);
                }
            }*/

            Color printColor;
            for (int x = 0; x < 22; x++)
{
                for(int y = 0; y < 16; y++)
                {
                    if(y % 2 == 0)
                    {
                        if(x % 2 == 0)
                        {
                            printColor = pgColor;
                        }
                        else
                        {
                            printColor = pgColor2;
                        }
                    }
                    else
                    {
                        if (x % 2 == 0)
                        {
                            printColor = pgColor2;
                        }
                        else
                        {
                            printColor = pgColor;
                        }
                    }

                    _spriteBatch.Draw(pgTexture, new Rectangle((int)tilesMovement[x,y].X, (int)tilesMovement[x,y].Y, 25, 25), printColor * tileOpacity[x,y]);
                }
            }

            //_spriteBatch.Draw(pgTexture, playground, pgColor);
            if (MissionHandler.MissionCategory[MissionHandler.Mission] == MissionTypes.FlagLevel)
            {
                finishFlag.Draw(_spriteBatch, gameTime, _graphics);
            }

            if (MissionHandler.MissionCategory[MissionHandler.Mission] == MissionTypes.CoinLevel && gems != null)
            {
                foreach(Gem gem in gems)
                {
                    if (!gem.PickedUp)
                    {
                        gem.Draw(_spriteBatch, gameTime, _graphics);
                    }
                    
                }
            }

            if (MissionHandler.MissionCategory[MissionHandler.Mission] == MissionTypes.SortLevel && coloredBlocks != null)
            {
                foreach (ColoredBlock block in coloredBlocks)
                {
                    block.Draw(_spriteBatch, gameTime, _graphics);
                }
            }

            else if ((MissionHandler.MissionCategory[MissionHandler.Mission] == MissionTypes.EnemyLevel || MissionHandler.MissionCategory[MissionHandler.Mission] == MissionTypes.KillLevel) && enemies != null)
            {
                foreach (EnemyBlock enemy in enemies)
                {
                    if(!enemy.killed)
                    {
                        enemy.Draw(_spriteBatch, gameTime, _graphics);
                    }
                   
                }
            }

            foreach (Bullet bullet in bullets)
            {
                if(!bullet.spent)
                {
                    bullet.Draw(_spriteBatch, gameTime, _graphics);
                }
            }

            player.Draw(_spriteBatch, gameTime, _graphics);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, GraphicsDeviceManager _graphics)
        {
            spriteBatch.End();
            _graphics.GraphicsDevice.SetRenderTarget(lightsTarget);
            _graphics.GraphicsDevice.Clear(Color.Transparent);

            // Create light mask
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            Vector2 lightSize = new Vector2(1000, 1000);
            spriteBatch.Draw(lightMask, new Rectangle(playground.X + (player.posX * 25) - ((int)lightSize.X/2) + 12, playground.Y + (player.posY * 25) - ((int)lightSize.Y / 2) + 6, (int)lightSize.X, (int)lightSize.Y),  Color.White);
            spriteBatch.End();

            // Draw to render texture
            _graphics.GraphicsDevice.SetRenderTarget(mainTarget);
            _graphics.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            DrawBoard(spriteBatch, gameTime, _graphics);
            spriteBatch.End();

            _graphics.GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin();

        }
        public void Draw2(SpriteBatch spriteBatch, GameTime gameTime, GraphicsDeviceManager _graphics)
        {
            spriteBatch.Draw(blackRectangle, playground, Color.Black);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            lightingEffect.Parameters["lightMask"].SetValue(lightsTarget);
            lightingEffect.CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(mainTarget, new Vector2(0, 0), Color.White);
            spriteBatch.End();
            spriteBatch.Begin();
        }
        public void CreateLevel(int level)
        {
            // Setup levels
            switch (level)
            {
                case 8:
                    finishFlag.posX = 21;
                    finishFlag.posY = 15;
                    break;

                case 9:
                    int gemIndex = 0;
                    gems = new Gem[20];
                    for(int x = 0; x < 10; x++)
                    {
                        gems[x] = new Gem(playground.X, playground.Y, 1+x, 1+x, x);
                        if(x == 9)
                        {
                            gems[x+1] = new Gem(playground.X, playground.Y, x+2, 1+x, x+1);
                            gemIndex = x+2;
                        }
                    }
                    int z = 0;
                    for (int y = 9; y > 0; y--)
                    {
                        gems[gemIndex+z] = new Gem(playground.X, playground.Y, 12+z, y, gemIndex+z);
                        z++;
                    }

                    break;

                case 10:
                    enemies = new EnemyBlock[16];
                    for(int y = 0; y < 16; y++)
                    {
                        enemies[y] = new EnemyBlock(playground.X, playground.Y, 21-y, y, y);
                    }
                    break;

                case 11:
                    enemies = new EnemyBlock[22];
                    for (int y = 0; y < 22; y++)
                    {
                        enemies[y] = new EnemyBlock(playground.X, playground.Y, y, 15, y);
                    }
                    break;

                case 12:
                    gems = new Gem[22*16];
                    for (int x = 0; x < 22; x++)
                    {
                        for(int y = 0; y < 16; y++)
                        {
                            gems[(x*16) + y] = new Gem(playground.X, playground.Y, x, y, (x * 16) + y);
                        }
                    }
                    break;
                case 13:
                    finishFlag.posX = 21;
                    finishFlag.posY = 23;
                    break;
                case 14:
                    var rand = new Random();
                    MissionHandler.AmountOfEnemies = 20;
                    enemies = new EnemyBlock[20];
                    for (int y = 0; y < 20; y++)
                    {
                        enemies[y] = new EnemyBlock(playground.X, playground.Y, 21 + (y * 2), rand.Next(0,16), y);
                    }
                    break;

                case 15:
                    MissionHandler.AmountOfColorBlocks = 22;
                    coloredBlocks = new ColoredBlock[22];
                    for (int y = 0; y < 23; y++)
                    {
                        coloredBlocks[y] = new ColoredBlock(playground.X, playground.Y, y, 5, y);
                    }
                    break;
            }
        }

        public void UpdateProportions(GraphicsDevice _graphics)
        {
            playground.X = _graphics.Viewport.Width - width - 50;
            player.UpdateProportions(_graphics, playground.X);


            finishFlag.UpdateProportions(_graphics, playground.X);


            if (MissionHandler.MissionCategory[MissionHandler.Mission] == MissionTypes.CoinLevel && gems != null)
            {
                foreach (Gem gem in gems)
                {
                    gem.UpdateProportions(_graphics, playground.X);
                }
            }
            else if (MissionHandler.MissionCategory[MissionHandler.Mission] == MissionTypes.EnemyLevel && enemies != null)
            {
                foreach (EnemyBlock enemy in enemies)
                {
                    enemy.UpdateProportions(_graphics, playground.X);
                }
            }

            if(lightsOn)
            {
                lightsTarget = new RenderTarget2D(
                    _graphics, _graphics.Viewport.Width, _graphics.Viewport.Height);
                mainTarget = new RenderTarget2D(
                    _graphics, _graphics.Viewport.Width, _graphics.Viewport.Height);
            }
        }
        public Vector2 TileTransistion(Vector2 newPos, GameTime gameTime, Vector2 startingSize, double elapsedTime, int x, int y)
        {
            Vector2 updatedPos;
            float desiredDuration = ((x + y) * 0.1f);
            float percentageComplete = (float)elapsedTime / desiredDuration;
            updatedPos = Vector2.Lerp(startingSize, newPos, MathHelper.SmoothStep(0, 1, percentageComplete));

            return updatedPos;
        }
    }
}
