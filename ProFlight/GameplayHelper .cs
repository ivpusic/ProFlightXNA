using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.IO.IsolatedStorage;
using System.IO;
using Microsoft.Xna.Framework.Input.Touch;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Devices;
using Microsoft.Xna.Framework.Media;
using AlienGameSample;
using attackGame.Game_parts;


namespace attackGame
{
    public class GameplayHelper : GameScreen
    {

        ////shooter varibles...
        public static int playerHealth;
        public static bool isPauseGame = false;
        //Number that holds the player score
        public int score = 0;
        // The font used to display UI elements
        SpriteFont font;

        // A movement speed for the player
        float playerMoveSpeed;
        public int enemyDamege = 10;
        public int strongerEnemyDamage = 30;

        public int enemyHealth = 4;
        public int strongerEnemyHealth = 10;



        //vibracija pri eksplozijama
        VibrateController vibrate = VibrateController.Default;

        // Enemies
        Texture2D enemyTexture;
        List<Enemy> enemies;

        List<Stone> stones;

        Parallaxingbackground bgLayer1, bgLayer2;
        Texture2D mainBackground;

        // The rate at which the enemies appear
        TimeSpan enemySpawnTime;
        TimeSpan previousSpawnTime;

        TimeSpan strongerEnemySpawnTime;
        TimeSpan storngerEnemyPreviousSpawnTime;

        public float strongerEnemySpawnValue = 3f;

        // The rate at wich the health appear
        TimeSpan healthSpawnTime;
        TimeSpan healthPreviousSpawnTime;

        // The rate at wich the health appear
        TimeSpan bulletsSpawnTime;
        TimeSpan bulletsPreviousSpawnTime;

        TimeSpan specialBagTime;
        TimeSpan specialBagPreviousSpawnTime;

        // The rate at wich the stone appear
        TimeSpan stoneSpawnTime;
        TimeSpan stonePreviousSpawnTime;
        public float enemyMoveSpeed = 6f;
        public float strongerEnemyMoveSpeed = 7f;
        // A random number generator
        Random random;
        Texture2D specialBagTexture;
        Texture2D projectileTexture;
        List<Projectile> projectiles;

        List<Health> healths;

        List<Enemy2> strongerEnemies;

        List<Bullet> bullets;

        List<SpecialBullet> specialBullets;
        public static bool updateGameTime = true;
        List<SpecialBag> specialBags;

        // The rate of fire of the player laser
        TimeSpan fireTime;
        TimeSpan previousFireTime;

        TimeSpan specialBulletFireTime;
        TimeSpan previousSpecialBulletFireTime;

        Texture2D explosionTexture;
        List<Animation> explosions;

        Texture2D stoneTexture;
        Texture2D bulletsTexture;
        // The sound that is played when a laser is fired
        SoundEffect laserSound;

        Texture2D strongerEnemiesTexture;

        // The sound used when the player or an enemy dies
        SoundEffect explosionSound;
        public static List<bool> options = new List<bool>();
        // The music played during gameplay
        public static Song gameplayMusic;

        Rectangle worldBounds;
        Player player;
        const float screenHeight = 800.0f;
        const float screenWidth = 480.0f;

        public float spawnTime = 1.5f;
        public int countBullets = 500;
        public int countSpecialBullets = 5;
        public double advancedRate = 15f;
        public int strongEnemiesCount = 10;
        public int strongEnemiesCountOrigin = 10;
        float transitionFactor; // 0.0f == day, 1.0f == night
        float transitionRate = 0.5f;
        public static bool optionChanged = false;
        public Rectangle retTest;
        Vector2 sunPosition;

        Texture2D laserTexture;

        //my textures
        Texture2D playerTexture;
        Texture2D healthTexture;

        Texture2D enemies2;

        Texture2D specialBulletTexture;

        SpriteFont scoreFont;
        SpriteFont menuFont;

        ContentManager contentManager;
        SpriteBatch spriteBatch;
        GraphicsDevice graphicsDevice;

        ISOptions iso;
        // A movement speed for the player


        public float widthWorld;
        public float heightWorld;

        public GameplayHelper(ContentManager contentManager, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            iso = new ISOptions();
            playerHealth = 100;
            //Set player's score to zero
            score = 0;
            playerHealth = 100;
            //Enable the FreeDrag gesture.
            TouchPanel.EnabledGestures = GestureType.Tap;
            TouchPanel.EnabledGestures = GestureType.Hold;

            retTest = new Rectangle();
            widthWorld = graphicsDevice.Viewport.TitleSafeArea.Width;
            heightWorld = graphicsDevice.Viewport.TitleSafeArea.Height;
            // TODO: Perform additional initializations

            // Set the laser to fire every quarter second
            fireTime = TimeSpan.FromSeconds(.2f);



            sunPosition = new Vector2(16, 16);

            InitializeAssets(contentManager, spriteBatch, graphicsDevice);

        }

        //iz nekog razloga ne radi serijalizacija ako nemam barem jedan konstruktor bez argumenata 
        public GameplayHelper()
        {

        }

        /* funkcija za "otežavanje" igre */
        public void AdvancedLevelSpawn()
        {
            if (spawnTime <= 0.3f)
            {

                enemyHealth += 3;
                strongerEnemyHealth += 3;
                spawnTime = 1f;
                AdvancedLevelHealth();
                AdvancedLevelScore();
                countBullets += 100;
                countSpecialBullets += 5;
                enemyDamege += 15;
                strongerEnemyDamage += 20;
                DangerusEnemies();
                if (strongerEnemySpawnValue <= 0.4f) strongerEnemySpawnValue = 2f;
                else strongerEnemySpawnValue -= 0.2f;
            }

            if (spawnTime > 0.4f && spawnTime < 1f) AdvancedLevelSpeed();
            if (spawnTime > 0.4f) spawnTime -= 0.2f;
            enemySpawnTime = TimeSpan.FromSeconds(spawnTime);
        }

        public void DangerusEnemies()
        {
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                enemies[i].enemyMoveSpeed = 18;
            }
        }

        public void AdvancedLevelSpeed()
        {
            enemyMoveSpeed += 2;
            strongerEnemyMoveSpeed += 2;
            for (int i = enemies.Count - 1; i >= 0; i--)
            {

                enemies[i].enemyMoveSpeed = enemyMoveSpeed;
            }
            for (int i = strongerEnemies.Count - 1; i >= 0; i--)
            {
                strongerEnemies[i].enemyMoveSpeed += strongerEnemyMoveSpeed;
            }
        }

        public void AdvancedLevelScore()
        {
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                enemies[i].Value += 50;
            }
            for (int i = strongerEnemies.Count - 1; i >= 0; i--)
            {
                strongerEnemies[i].Value += 50;
            }
        }

        public void AdvancedLevelHealth()
        {
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                enemies[i].Health += 10;
            }
            for (int i = strongerEnemies.Count - 1; i >= 0; i--)
            {
                strongerEnemies[i].Health += 10;
            }
        }

        public void Draw(float totalElapsedSeconds)
        {
            spriteBatch.Begin();

            //DrawBackground(totalElapsedSeconds);

            //draw bgr
            //spriteBatch.Draw(mainBackground, Vector2.Zero, Color.White);
            spriteBatch.Draw(mainBackground, new Vector2(480, 0), null, Color.White, 1.56f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            // Draw the moving background
            bgLayer1.Draw(spriteBatch);
            bgLayer2.Draw(spriteBatch);


            player.Draw(spriteBatch);

            // Draw the Enemies
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Draw(spriteBatch);
            }

            // Draw the bullets
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Draw(spriteBatch);
            }

            // Draw the special bags
            for (int i = 0; i < specialBags.Count; i++)
            {
                specialBags[i].Draw(spriteBatch);
            }

            //Draw the stronger enemies
            for (int i = 0; i < strongerEnemies.Count; i++)
            {
                strongerEnemies[i].Draw(spriteBatch);
            }

            // Draw the Projectiles
            for (int i = 0; i < projectiles.Count; i++)
            {
                projectiles[i].Draw(spriteBatch);
            }

            // Draw the special bullets
            for (int i = 0; i < specialBullets.Count; i++)
            {
                specialBullets[i].Draw(spriteBatch);
            }

            // Draw the explosions
            for (int i = 0; i < explosions.Count; i++)
            {
                explosions[i].Draw(spriteBatch);
            }

            // Draw the health
            for (int i = 0; i < healths.Count; i++)
            {
                healths[i].Draw(spriteBatch);
            }

            // Draw the stones
            for (int i = 0; i < stones.Count; i++)
            {
                stones[i].Draw(spriteBatch);
            }

            // Draw the score
            //spriteBatch.DrawString(font, "score: " + score, new Vector2(graphicsDevice.Viewport.Width, 300), Color.Black);
            spriteBatch.DrawString(font, "score: " + score, new Vector2(graphicsDevice.Viewport.Width, 0), Color.White, 1.55f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            // Draw the player health
            spriteBatch.DrawString(font, "health: " + player.Health, new Vector2(graphicsDevice.Viewport.Width - 40, 0), Color.White, 1.55f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            spriteBatch.Draw(laserTexture, new Vector2(440, 280), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, ": " + countBullets, new Vector2(480, 300), Color.White, 1.55f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

            spriteBatch.Draw(specialBulletTexture, new Vector2(440, 420), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, ": " + countSpecialBullets, new Vector2(480, 440), Color.White, 1.55f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            spriteBatch.End();

        }

        private void DrawBackground(float elapsedTime)
        {
            transitionFactor += transitionRate * elapsedTime;
            if (transitionFactor < 0.0f)
            {
                transitionFactor = 0.0f;
                transitionRate = 0.0f;
            }
            if (transitionFactor > 1.0f)
            {
                transitionFactor = 1.0f;
                transitionRate = 0.0f;
            }

            Vector3 day = Color.White.ToVector3();
            Vector3 night = new Color(80, 80, 180).ToVector3();
            Vector3 dayClear = Color.CornflowerBlue.ToVector3();
            Vector3 nightClear = night;
            Color clear = new Color(Vector3.Lerp(dayClear, nightClear, 1f));
            Color tint = new Color(Vector3.Lerp(day, night, 1f));

            // Clear the background, using the day/night color
            graphicsDevice.Clear(Color.White);

            //// Draw the mountains
            //if (player.Health < 80) spriteBatch.Draw(mountainsTexture, new Vector2(0, screenHeight - mountainsTexture.Height), Color.Black);
            //else spriteBatch.Draw(mountainsTexture, new Vector2(0, screenHeight - mountainsTexture.Height), Color.White);

            //// Draw the hills
            //spriteBatch.Draw(hillsTexture, new Vector2(0, screenHeight - hillsTexture.Height), tint);

            //// Draw the ground
            //spriteBatch.Draw(groundTexture, new Vector2(0, screenHeight - groundTexture.Height), tint);
        }


        private void AddEnemy()
        {
            //Debug.WriteLine(enemyMoveSpeed);
            // Create the animation object
            Animation enemyAnimation = new Animation();

            // Initialize the animation with the correct animation information
            enemyAnimation.Initialize(enemyTexture, Vector2.Zero, 60, 32, 6, 30, Color.White, 1f, true);

            // Randomly generate the position of the enemy
            Vector2 position = new Vector2(random.Next(enemyTexture.Height, graphicsDevice.Viewport.Width + enemyTexture.Height / 2), graphicsDevice.Viewport.Height);

            // Create an enemy
            Enemy enemy = new Enemy();

            // Initialize the enemy
            enemy.Initialize(enemyAnimation, position);
            if (enemyMoveSpeed >= 14) enemyMoveSpeed = 9;
            enemy.enemyMoveSpeed = enemyMoveSpeed;
            enemy.Damage = enemyDamege;
            enemy.Health = enemyHealth;
            // Add the enemy to the active enemies list
            enemies.Add(enemy);
        }

        private void AddStrongerEnemy()
        {
            // Create the animation object
            Animation enemyAnimation = new Animation();

            // Initialize the animation with the correct animation information
            enemyAnimation.Initialize(strongerEnemiesTexture, Vector2.Zero, 60, 32, 6, 30, Color.White, 1f, true);

            // Randomly generate the position of the enemy
            Vector2 position = new Vector2(random.Next(strongerEnemiesTexture.Height, graphicsDevice.Viewport.Width + strongerEnemiesTexture.Height / 2), graphicsDevice.Viewport.Height);

            // Create an enemy
            Enemy2 enemy = new Enemy2();

            // Initialize the enemy
            enemy.Initialize(enemyAnimation, position);
            if (strongerEnemyMoveSpeed >= 16) strongerEnemyMoveSpeed = 10;
            enemy.enemyMoveSpeed = strongerEnemyMoveSpeed;
            enemy.Damage = strongerEnemyDamage;
            enemy.Health = strongerEnemyHealth;
            // Add the enemy to the active enemies list
            strongerEnemies.Add(enemy);
        }

        private void AddProjectile(Vector2 position)
        {
            Projectile projectile = new Projectile();
            projectile.Initialize(graphicsDevice.Viewport, projectileTexture, position);
            projectiles.Add(projectile);
        }

        public void AddHealth(Vector2 position)
        {
            Health health = new Health();
            health.Initialize(graphicsDevice.Viewport, healthTexture, position);
            healths.Add(health);
        }

        public void AddBullet(Vector2 position)
        {
            Bullet bullet = new Bullet();
            bullet.Initialize(graphicsDevice.Viewport, bulletsTexture, position);
            bullets.Add(bullet);
        }

        public void AddSpecialBag(Vector2 position)
        {
            SpecialBag bag = new SpecialBag();
            bag.Initialize(graphicsDevice.Viewport, specialBagTexture, position);
            specialBags.Add(bag);
        }

        public void AddSpecialBullet(Vector2 position)
        {
            SpecialBullet bullet = new SpecialBullet();
            bullet.Initialize(graphicsDevice.Viewport, specialBulletTexture, position);
            specialBullets.Add(bullet);
        }

        public void AddStone(Vector2 position)
        {
            Animation stoneAnimation = new Animation();

            stoneAnimation.Initialize(stoneTexture, Vector2.Zero, 300, 58, 3, 30, Color.White, 1f, true);
            Vector2 Position = new Vector2(random.Next(stoneTexture.Height, graphicsDevice.Viewport.Width + stoneTexture.Height / 2), graphicsDevice.Viewport.Height);

            Stone stone = new Stone();

            stone.Initialize(stoneAnimation, Position);
            stones.Add(stone);
        }

        private void AddExplosion(Vector2 position)
        {
            Animation explosion = new Animation();
            explosion.Initialize(explosionTexture, position, 134, 134, 12, 45, Color.White, 1f, false);
            explosions.Add(explosion);

            //kada eksplodira nesto, vibriraj
            if (options.Count > 0 && options[1] == true)
            {

                vibrate.Start(TimeSpan.FromMilliseconds(100));
            }

        }

        public void Update(GameTime gameTime, Vector2 position)
        {
            if (updateGameTime)
            {
                advancedRate = gameTime.TotalGameTime.TotalSeconds;
                storngerEnemyPreviousSpawnTime = gameTime.TotalGameTime;
                previousSpawnTime = gameTime.TotalGameTime;
                updateGameTime = false;
            }
            //Debug.WriteLine("advanced rate" + advancedRate);
            //Debug.WriteLine(gameTime.TotalGameTime.TotalSeconds);
            if (!isPauseGame)
            {
                if (Convert.ToDouble(gameTime.TotalGameTime.TotalSeconds) > advancedRate)
                {
                    advancedRate += 8;
                    Debug.WriteLine("tu je");
                    Debug.WriteLine(enemySpawnTime.ToString());
                    AdvancedLevelSpawn();
                }
            }


            //ukoliko smo mjenjali nesto u postavkama, ponovo uèitaj opcije
            if (GameplayHelper.optionChanged)
            {
                LoadOptions();
                GameplayHelper.optionChanged = false;
            }

            if (options[0] == false)
            {
                StopMusic();
            }

            player.Update(gameTime);

            //Update the player
            UpdatePlayer(gameTime, position);

            // Update the enemies
            UpdateEnemies(gameTime);

            // Update the collision
            UpdateCollision();

            // Update the projectiles
            UpdateProjectiles();

            // Update helth
            UpdateHealth(gameTime);

            UpdateSpecialBullets();

            UpdateExplosions(gameTime);

            UpdateBullets(gameTime);

            UpdateSpecialBag(gameTime);

            UpdateStones(gameTime);

            //update backgound
            bgLayer1.Update();
            bgLayer2.Update();

        }

        public void UpdateSpecialBag(GameTime gameTime)
        {

            if (gameTime.TotalGameTime - specialBagPreviousSpawnTime > specialBagTime)
            {
                specialBagPreviousSpawnTime = gameTime.TotalGameTime;

                // Add an Enemy
                //TODO : ispravi dodavanje pomoci!!!!
                Vector2 position = new Vector2(random.Next(specialBagTexture.Height, graphicsDevice.Viewport.Width + specialBagTexture.Height / 2), graphicsDevice.Viewport.Height);
                AddSpecialBag(position);
            }

            // Update the Enemies
            for (int i = specialBags.Count - 1; i >= 0; i--)
            {
                specialBags[i].Update();

                if (specialBags[i].Active == false)
                {
                    // If not active and health <= 0
                    if (specialBags[i].health <= 0)
                    {
                        countSpecialBullets += 2;
                        Vector2 explosionPosition = new Vector2(specialBags[i].Position.X + playerTexture.Height + 60, specialBags[i].Position.Y - 20 /*oduzni 20, za realniji efekt*/);
                        // Add an explosion
                        AddExplosion(explosionPosition);
                        if (options[0] == true)
                        {
                            explosionSound.Play();
                        }
                    }
                    specialBags.RemoveAt(i);
                }
            }
        }

        public void UpdateBullets(GameTime gameTime)
        {
            // Spawn a new enemy enemy every 1.5 seconds
            if (gameTime.TotalGameTime - bulletsPreviousSpawnTime > bulletsSpawnTime)
            {
                bulletsPreviousSpawnTime = gameTime.TotalGameTime;

                // Add an Enemy
                //TODO : ispravi dodavanje pomoci!!!!
                Vector2 position = new Vector2(random.Next(bulletsTexture.Height, graphicsDevice.Viewport.Width + bulletsTexture.Height / 2), graphicsDevice.Viewport.Height);
                AddBullet(position);
            }

            // Update the Enemies
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                bullets[i].Update();

                if (bullets[i].Active == false)
                {
                    // If not active and health <= 0
                    if (bullets[i].health <= 0)
                    {
                        Vector2 explosionPosition = new Vector2(bullets[i].Position.X + playerTexture.Height + 60, bullets[i].Position.Y - 20 /*oduzni 20, za realniji efekt*/);
                        // Add an explosion
                        AddExplosion(explosionPosition);
                        countBullets += 100;
                        if (options[0] == true)
                        {
                            explosionSound.Play();
                        }
                    }
                    bullets.RemoveAt(i);
                }
            }
        }

        private void UpdatePlayer(GameTime gameTime, Vector2 position)
        {
            playerHealth = player.Health;
            bool accelo = true;
            bool touch = false;
            //koristi touch pad
            if (!accelo)
            {
                while (TouchPanel.IsGestureAvailable)
                {
                    GestureSample gesture = TouchPanel.ReadGesture();
                    if (gesture.GestureType == GestureType.FreeDrag)
                    {
                        player.Position += gesture.Delta;
                    }
                }
            }

            // koristi accelometar
            if (accelo) player.Position += position * playerMoveSpeed;



            // Make sure that the player does not go out of bounds
            player.Position.X = MathHelper.Clamp(player.Position.X, 110, worldBounds.Width + 220 - player.Width);
            player.Position.Y = MathHelper.Clamp(player.Position.Y, 0, worldBounds.Height - player.Height);

            //ukoliko je korisnik klikno na ekran, povecaj brzinu ispaljivanja metaka
            TouchCollection touchCollection = TouchPanel.GetState();

            if (touchCollection.Count == 1 && countBullets > 0)
            {
                fireTime = TimeSpan.FromSeconds(.08f);
                touch = true;
                countBullets--;
            }

            if (touchCollection.Count == 2 && countSpecialBullets > 0)
            {
                //fireTime = TimeSpan.FromSeconds(.5f);
                touch = true;
                //add special bullets
                if (gameTime.TotalGameTime - previousSpecialBulletFireTime > specialBulletFireTime)
                {
                    // Reset our current time
                    previousSpecialBulletFireTime = gameTime.TotalGameTime;

                    Vector2 projectilePosition = new Vector2(player.Position.X - 120, player.Position.Y + playerTexture.Width / 11);
                    // Add the projectile, but add it to the front and center of the player
                    AddSpecialBullet(projectilePosition /*+ new Vector2(player.Height / 2, 0)*/);
                    if (options[0] == true)
                    {
                        laserSound.Play();
                    }
                    countSpecialBullets--;
                }
            }

            if (!touch) fireTime = TimeSpan.FromSeconds(.2f);
            // Fire only every interval we set as the fireTime
            if (gameTime.TotalGameTime - previousFireTime > fireTime)
            {
                // Reset our current time
                previousFireTime = gameTime.TotalGameTime;
                //Vector2 projectilePosition = new Vector2(player.Position.X - playerTexture.Height / 2, player.Position.Y + playerTexture.Height);
                Vector2 projectilePosition = new Vector2(player.Position.X - 110, player.Position.Y + playerTexture.Width / 11);
                // Add the projectile, but add it to the front and center of the player
                //Debug.WriteLine("x: " + player.Position.X + "y: " + player.Position.Y);
                AddProjectile(projectilePosition);
                if (options[0] == true)
                {
                    laserSound.Play();
                }
            }


            // reset score if player health goes to zero
            if (player.Health <= 0)
            {
                player.Health = 100;
                //score = 0;
            }

        }

        private void UpdateExplosions(GameTime gameTime)
        {
            for (int i = explosions.Count - 1; i >= 0; i--)
            {
                explosions[i].Update(gameTime);
                if (explosions[i].Active == false)
                {
                    explosions.RemoveAt(i);
                }
            }
        }

        private void UpdateProjectiles()
        {
            // Update the Projectiles
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                projectiles[i].Update();

                if (projectiles[i].Active == false)
                {
                    projectiles.RemoveAt(i);
                }
            }

        }

        private void UpdateSpecialBullets()
        {
            // Update the Projectiles
            for (int i = specialBullets.Count - 1; i >= 0; i--)
            {
                specialBullets[i].Update();

                if (specialBullets[i].Active == false)
                {
                    specialBullets.RemoveAt(i);
                }
            }

        }

        public void bulletsColision()
        {
            Rectangle rectangle1;
            Rectangle rectangle2;

            // Only create the rectangle once for the player
            rectangle1 = new Rectangle((int)player.Position.X - 130,
            (int)player.Position.Y + 20,
            player.Width / 2 - 30,
            player.Height);

            // Do the collision between the player and the bullets
            for (int i = 0; i < bullets.Count; i++)
            {
                rectangle2 = new Rectangle((int)bullets[i].Position.X - 40,
                (int)bullets[i].Position.Y - 60,
                bullets[i].Width,
                bullets[i].Height / 2);

                // Determine if the two objects collided with each
                // other
                if (rectangle1.Intersects(rectangle2))
                {
                    // Subtract the health from the player based on
                    // the enemy damage
                    player.Health -= bullets[i].Damage;

                    // Since the enemy collided with the player
                    // destroy it
                    bullets[i].health = 0;

                    playerHealth = player.Health;
                    // If the player health is less than zero we died
                    if (player.Health <= 0)
                    {
                        player.Active = false;
                    }

                }

            }

            // Projectile vs health Collision
            for (int i = 0; i < projectiles.Count; i++)
            {
                //Debug.WriteLine(projectiles[i].Width);
                rectangle1 = new Rectangle((int)projectiles[i].Position.X -
                    projectiles[i].Width / 2, (int)projectiles[i].Position.Y -
                    projectiles[i].Height / 2, projectiles[i].Width, projectiles[i].Height);
                for (int j = 0; j < bullets.Count; j++)
                {

                    // Create the rectangles we need to determine if we collided with each other
                    rectangle2 = new Rectangle((int)bullets[j].Position.X - bullets[j].Width + 35,
                    (int)bullets[j].Position.Y - bullets[j].Height,
                    bullets[j].Width / 2, bullets[j].Height);

                    // Determine if the two objects collided with each other
                    if (rectangle1.Intersects(rectangle2))
                    {
                        bullets[j].health -= projectiles[i].Damage;
                        projectiles[i].Active = false;
                    }
                }
            }

            // special projectile vs health Collision
            for (int i = 0; i < specialBullets.Count; i++)
            {
                //Debug.WriteLine(projectiles[i].Width);
                rectangle1 = new Rectangle((int)specialBullets[i].Position.X -
                    specialBullets[i].Width / 2, (int)specialBullets[i].Position.Y -
                    specialBullets[i].Height / 2, specialBullets[i].Width, specialBullets[i].Height);
                for (int j = 0; j < bullets.Count; j++)
                {

                    // Create the rectangles we need to determine if we collided with each other
                    rectangle2 = new Rectangle((int)bullets[j].Position.X - bullets[j].Width + 35,
                    (int)bullets[j].Position.Y - bullets[j].Height,
                    bullets[j].Width / 2, bullets[j].Height);

                    // Determine if the two objects collided with each other
                    if (rectangle1.Intersects(rectangle2))
                    {
                        bullets[j].health -= specialBullets[i].Damage;
                        specialBullets[i].Active = true;
                    }
                }
            }

        }

        public void specialBagsColision()
        {
            Rectangle rectangle1;
            Rectangle rectangle2;

            // Only create the rectangle once for the player
            //player position
            rectangle1 = new Rectangle((int)player.Position.X - 140,
            (int)player.Position.Y + 20,
            player.Width / 2 - 30,
            player.Height / 2);

            // Do the collision between the player and the special bugs
            for (int i = 0; i < specialBags.Count; i++)
            {
                rectangle2 = new Rectangle((int)specialBags[i].Position.X - 60,
                (int)specialBags[i].Position.Y - 40,
                specialBags[i].Width,
                specialBags[i].Height / 2);

                //Debug.WriteLine("x: " + (int)specialBags[i].Position.X + " y: " + (int)specialBags[i].Position.Y);

                // Determine if the two objects collided with each
                // other
                if (rectangle1.Intersects(rectangle2))
                {
                    // Subtract the health from the player based on
                    // the enemy damage
                    player.Health -= specialBags[i].Damage;

                    // Since the enemy collided with the player
                    // destroy it
                    specialBags[i].health = 0;

                    playerHealth = player.Health;
                    // If the player health is less than zero we died
                    if (player.Health <= 0)
                    {
                        player.Active = false;
                    }

                }

            }

            // Projectile vs special bags Collision
            for (int i = 0; i < projectiles.Count; i++)
            {
                //Debug.WriteLine(projectiles[i].Width);
                rectangle1 = new Rectangle((int)projectiles[i].Position.X -
                    projectiles[i].Width / 2, (int)projectiles[i].Position.Y -
                    projectiles[i].Height / 2, projectiles[i].Width, projectiles[i].Height);
                for (int j = 0; j < specialBags.Count; j++)
                {

                    // Create the rectangles we need to determine if we collided with each other
                    rectangle2 = new Rectangle((int)specialBags[j].Position.X - specialBags[j].Width + 35,
                    (int)specialBags[j].Position.Y - specialBags[j].Height,
                    specialBags[j].Width / 2, specialBags[j].Height);

                    // Determine if the two objects collided with each other
                    if (rectangle1.Intersects(rectangle2))
                    {
                        specialBags[j].health -= projectiles[i].Damage;
                        projectiles[i].Active = false;
                    }
                }
            }

            // special projectile vs special bags Collision
            for (int i = 0; i < specialBullets.Count; i++)
            {
                //Debug.WriteLine(projectiles[i].Width);
                rectangle1 = new Rectangle((int)specialBullets[i].Position.X -
                    specialBullets[i].Width / 2, (int)specialBullets[i].Position.Y -
                    specialBullets[i].Height / 2, specialBullets[i].Width, specialBullets[i].Height);
                for (int j = 0; j < specialBags.Count; j++)
                {

                    // Create the rectangles we need to determine if we collided with each other
                    rectangle2 = new Rectangle((int)specialBags[j].Position.X - specialBags[j].Width + 35,
                    (int)specialBags[j].Position.Y - specialBags[j].Height,
                    specialBags[j].Width / 2, specialBags[j].Height);

                    // Determine if the two objects collided with each other
                    if (rectangle1.Intersects(rectangle2))
                    {
                        specialBags[j].health -= specialBullets[i].Damage;
                        specialBullets[i].Active = true;
                    }
                }
            }

        }

        public void stonesColision()
        {
            Rectangle rectangle1;
            Rectangle rectangle2;

            // Only create the rectangle once for the player
            rectangle1 = new Rectangle((int)player.Position.X - player.Width / 3,
            (int)player.Position.Y + player.Width / 4,
            player.Width / 2,
            player.Height);

            // Do the collision between the player and the stones
            for (int i = 0; i < stones.Count; i++)
            {
                rectangle2 = new Rectangle((int)stones[i].Position.X,
                (int)stones[i].Position.Y,
                stones[i].Width,
                stones[i].Height / 2);

                // Determine if the two objects collided with each
                // other
                if (rectangle1.Intersects(rectangle2))
                {
                    // Subtract the health from the player based on
                    // the enemy damage
                    player.Health -= stones[i].Damage;
                    //Debug.WriteLine(player.Health + "uso je tu");
                    // Since the enemy collided with the player
                    // destroy it
                    stones[i].Health = 0;

                    playerHealth = player.Health;
                    // If the player health is less than zero we died
                    if (player.Health <= 0)
                    {
                        player.Active = false;
                    }

                }

            }

            // Projectile vs stones Collision
            for (int i = 0; i < projectiles.Count; i++)
            {
                //Debug.WriteLine(projectiles[i].Width);
                rectangle1 = new Rectangle((int)projectiles[i].Position.X -
                    projectiles[i].Width / 2, (int)projectiles[i].Position.Y -
                    projectiles[i].Height / 2, projectiles[i].Width, projectiles[i].Height);
                for (int j = 0; j < stones.Count; j++)
                {

                    // Create the rectangles we need to determine if we collided with each other
                    rectangle2 = new Rectangle((int)stones[j].Position.X - stones[j].Width - 20,
                    (int)stones[j].Position.Y - stones[j].Height,
                    stones[j].Width / 2, stones[j].Height / 2);

                    // Determine if the two objects collided with each other
                    if (rectangle1.Intersects(rectangle2))
                    {
                        stones[j].Health -= projectiles[i].Damage;
                        projectiles[i].Active = false;
                    }
                }
            }
            Rectangle _rectangle1, _rectangle2;
            // special projectiles vs stones Collision
            for (int i = 0; i < specialBullets.Count; i++)
            {
                //Debug.WriteLine(projectiles[i].Width);
                _rectangle1 = new Rectangle((int)specialBullets[i].Position.X -
                    specialBullets[i].Width / 2, (int)specialBullets[i].Position.Y -
                    specialBullets[i].Height / 2, specialBullets[i].Width, specialBullets[i].Height);
                for (int j = 0; j < stones.Count; j++)
                {

                    // Create the rectangles we need to determine if we collided with each other
                    _rectangle2 = new Rectangle((int)stones[j].Position.X - stones[j].Width - 20,
                    (int)stones[j].Position.Y - stones[j].Height,
                    stones[j].Width / 2, stones[j].Height / 2);

                    // Determine if the two objects collided with each other
                    if (_rectangle1.Intersects(_rectangle2))
                    {
                        stones[j].Health -= specialBullets[i].Damage;
                        specialBullets[i].Active = true;
                    }
                }
            }
        }

        public void strongerEnemiesColision()
        {
            // Use the Rectangle's built-in intersect function to 
            // determine if two objects are overlapping
            Rectangle rectangle1;
            Rectangle rectangle2;

            // Only create the rectangle once for the player
            rectangle1 = new Rectangle((int)player.Position.X - player.Width / 6,
            (int)player.Position.Y + player.Width / 3,
            player.Width / 4,
            player.Height + 50);

            // Do the collision between the player and the enemies
            for (int i = 0; i < strongerEnemies.Count; i++)
            {
                rectangle2 = new Rectangle((int)strongerEnemies[i].Position.X + 30,
                (int)strongerEnemies[i].Position.Y,
                strongerEnemies[i].Width / 2,
                strongerEnemies[i].Height / 2);

                // Determine if the two objects collided with each
                // other
                if (rectangle1.Intersects(rectangle2))
                {
                    // Subtract the health from the player based on
                    // the enemy damage
                    player.Health -= strongerEnemies[i].Damage;

                    // Since the enemy collided with the player
                    // destroy it
                    strongerEnemies[i].Health = 0;
                    playerHealth = player.Health;
                    // If the player health is less than zero we died
                    if (player.Health <= 0)
                    {
                        player.Active = false;
                        //ExitScreen();
                        //ScreenManager.AddScreen(new PhoneMainMenu());
                    }

                }

            }


            // Projectile vs Enemy Collision
            for (int i = 0; i < projectiles.Count; i++)
            {
                rectangle1 = new Rectangle((int)projectiles[i].Position.X -
                    projectiles[i].Width / 2, (int)projectiles[i].Position.Y -
                    projectiles[i].Height / 2, projectiles[i].Width, projectiles[i].Height);
                for (int j = 0; j < strongerEnemies.Count; j++)
                {

                    // Create the rectangles we need to determine if we collided with each other
                    rectangle2 = new Rectangle((int)strongerEnemies[j].Position.X - strongerEnemies[j].Width - 20,
                    (int)strongerEnemies[j].Position.Y - strongerEnemies[j].Height,
                    strongerEnemies[j].Width / 2, strongerEnemies[j].Height / 2);

                    // Determine if the two objects collided with each other
                    if (rectangle1.Intersects(rectangle2))
                    {
                        strongerEnemies[j].Health -= projectiles[i].Damage;
                        projectiles[i].Active = false;
                    }
                }
            }

            // special projectiles vs StrongerEnemy Collision
            for (int i = 0; i < specialBullets.Count; i++)
            {
                rectangle1 = new Rectangle((int)specialBullets[i].Position.X -
                    specialBullets[i].Width / 2, (int)specialBullets[i].Position.Y -
                    specialBullets[i].Height / 2, specialBullets[i].Width, specialBullets[i].Height);
                for (int j = 0; j < strongerEnemies.Count; j++)
                {

                    // Create the rectangles we need to determine if we collided with each other
                    rectangle2 = new Rectangle((int)strongerEnemies[j].Position.X - strongerEnemies[j].Width,
                    (int)strongerEnemies[j].Position.Y - strongerEnemies[j].Height,
                    strongerEnemies[j].Width / 2, strongerEnemies[j].Height / 2);

                    // Determine if the two objects collided with each other
                    if (rectangle1.Intersects(rectangle2))
                    {
                        strongerEnemies[j].Health -= specialBullets[i].Damage;
                        specialBullets[i].Active = true;
                    }
                }
            }

        }


        private void UpdateCollision()
        {
            // Use the Rectangle's built-in intersect function to 
            // determine if two objects are overlapping
            Rectangle rectangle1;
            Rectangle rectangle2;

            // Kreiramo pravokutnik koji æe omeðavati igraèa
            rectangle1 = new Rectangle((int)player.Position.X - player.Width / 6,
            (int)player.Position.Y + player.Width / 3,
            player.Width / 4,
            player.Height + 50);

            // Rješavanje kolizije izmeðu igraèa i neprijatelja
            for (int i = 0; i < enemies.Count; i++)
            {
                // Kreiranje pravokutnika koji æe omeðavati neprijatelja
                rectangle2 = new Rectangle((int)enemies[i].Position.X + 30,
                (int)enemies[i].Position.Y,
                enemies[i].Width / 2,
                enemies[i].Height / 2);

                // Ukoliko su dva objekta u koliziji -> igrac i neprijatelj
                if (rectangle1.Intersects(rectangle2))
                {
                    // Oduzmi zdravlja igraca
                    player.Health -= enemies[i].Damage;
                    // Oduzmi zdravlje neprijatelja
                    enemies[i].Health = 0;
                    playerHealth = player.Health;
                    // Ukoliko je zdravlje igraca nje ili jednako 0
                    if (player.Health <= 0)
                    {
                        player.Active = false;
                    }
                }
            }

            //player position
            rectangle1 = new Rectangle((int)player.Position.X - 140,
            (int)player.Position.Y + 70,
            player.Width / 2 - 30,
            player.Height / 2);

            // Do the collision between the player and the healths
            for (int i = 0; i < healths.Count; i++)
            {
                rectangle2 = new Rectangle((int)healths[i].Position.X - 40,
                (int)healths[i].Position.Y - 40,
                healths[i].Width,
                healths[i].Height);
                // Determine if the two objects collided with each
                // other
                if (rectangle1.Intersects(rectangle2))
                {
                    // Subtract the health from the player based on
                    // the enemy damage
                    player.Health -= healths[i].Damage;

                    // Since the enemy collided with the player
                    // destroy it
                    healths[i].health = 0;
                    //player.Health += healths[i].toPlayerHealth;
                    playerHealth = player.Health;
                    // If the player health is less than zero we died
                    if (player.Health <= 0)
                    {
                        player.Active = false;
                    }

                }

            }


            // Projectile vs Enemy Collision
            for (int i = 0; i < projectiles.Count; i++)
            {
                rectangle1 = new Rectangle((int)projectiles[i].Position.X -
                    projectiles[i].Width / 2, (int)projectiles[i].Position.Y -
                    projectiles[i].Height / 2, projectiles[i].Width, projectiles[i].Height);
                for (int j = 0; j < enemies.Count; j++)
                {

                    // Create the rectangles we need to determine if we collided with each other
                    rectangle2 = new Rectangle((int)enemies[j].Position.X - enemies[j].Width,
                    (int)enemies[j].Position.Y - enemies[j].Height,
                    enemies[j].Width / 2, enemies[j].Height / 2);

                    // Determine if the two objects collided with each other
                    if (rectangle1.Intersects(rectangle2))
                    {
                        enemies[j].Health -= projectiles[i].Damage;
                        projectiles[i].Active = false;
                    }
                }
            }

            // special bullets vs Enemy Collision
            for (int i = 0; i < specialBullets.Count; i++)
            {
                rectangle1 = new Rectangle((int)specialBullets[i].Position.X -
                    specialBullets[i].Width / 2, (int)specialBullets[i].Position.Y -
                    specialBullets[i].Height / 2, specialBullets[i].Width, specialBullets[i].Height);
                for (int j = 0; j < enemies.Count; j++)
                {

                    // Create the rectangles we need to determine if we collided with each other
                    rectangle2 = new Rectangle((int)enemies[j].Position.X - enemies[j].Width,
                    (int)enemies[j].Position.Y - enemies[j].Height,
                    enemies[j].Width / 2, enemies[j].Height / 2);

                    // Determine if the two objects collided with each other
                    if (rectangle1.Intersects(rectangle2))
                    {
                        enemies[j].Health -= specialBullets[i].Damage;
                        specialBullets[i].Active = true;
                    }
                }
            }

            // Projectile vs health Collision
            for (int i = 0; i < projectiles.Count; i++)
            {
                //Debug.WriteLine(projectiles[i].Width);
                rectangle1 = new Rectangle((int)projectiles[i].Position.X -
                    projectiles[i].Width / 2, (int)projectiles[i].Position.Y -
                    projectiles[i].Height / 2, projectiles[i].Width, projectiles[i].Height);
                for (int j = 0; j < healths.Count; j++)
                {

                    // Create the rectangles we need to determine if we collided with each other
                    rectangle2 = new Rectangle((int)healths[j].Position.X - healths[j].Width + 20,
                    (int)healths[j].Position.Y - healths[j].Height,
                    healths[j].Width / 2, healths[j].Height / 2);

                    // Determine if the two objects collided with each other
                    if (rectangle1.Intersects(rectangle2))
                    {
                        healths[j].health -= projectiles[i].Damage;
                        projectiles[i].Active = false;
                    }
                }
            }

            // special bullets vs health Collision
            for (int i = 0; i < specialBullets.Count; i++)
            {
                //Debug.WriteLine(projectiles[i].Width);
                rectangle1 = new Rectangle((int)specialBullets[i].Position.X -
                    specialBullets[i].Width / 2, (int)specialBullets[i].Position.Y -
                    specialBullets[i].Height / 2, specialBullets[i].Width, specialBullets[i].Height);
                for (int j = 0; j < healths.Count; j++)
                {

                    // Create the rectangles we need to determine if we collided with each other
                    rectangle2 = new Rectangle((int)healths[j].Position.X - healths[j].Width + 20,
                    (int)healths[j].Position.Y - healths[j].Height,
                    healths[j].Width / 2, healths[j].Height / 2);

                    // Determine if the two objects collided with each other
                    if (rectangle1.Intersects(rectangle2))
                    {
                        healths[j].health -= specialBullets[i].Damage;
                        specialBullets[i].Active = true;
                    }
                }
            }

            //stonesColision();
            strongerEnemiesColision();
            bulletsColision();
            specialBagsColision();
        }

        private void UpdateStones(GameTime gameTime)
        {
            // Spawn a new enemy enemy every 1.5 seconds
            if (gameTime.TotalGameTime - stonePreviousSpawnTime > stoneSpawnTime)
            {
                //Debug.WriteLine(healths.Count);
                stonePreviousSpawnTime = gameTime.TotalGameTime;

                // Add an Enemy
                //TODO : ispravi dodavanje pomoci!!!!
                Vector2 position = new Vector2(random.Next(stoneTexture.Height, graphicsDevice.Viewport.Width + stoneTexture.Height / 2), graphicsDevice.Viewport.Height);
                AddStone(position);
            }

            // Update the Enemies
            for (int i = stones.Count - 1; i >= 0; i--)
            {
                stones[i].Update(gameTime);

                if (stones[i].Active == false)
                {
                    // If not active and health <= 0
                    if (stones[i].Health <= 0)
                    {
                        Vector2 explosionPosition = new Vector2(stones[i].Position.X + playerTexture.Height, stones[i].Position.Y - 20 /*oduzni 20, za realniji efekt*/);
                        // Add an explosion
                        AddExplosion(explosionPosition);
                        if (options[0] == true)
                        {
                            explosionSound.Play();
                        }

                    }
                    stones.RemoveAt(i);
                }
            }
        }

        private void UpdateHealth(GameTime gameTime)
        {
            // Spawn a new enemy enemy every 1.5 seconds
            if (gameTime.TotalGameTime - healthPreviousSpawnTime > healthSpawnTime)
            {
                healthPreviousSpawnTime = gameTime.TotalGameTime;

                // Add an Enemy
                //TODO : ispravi dodavanje pomoci!!!!
                Vector2 position = new Vector2(random.Next(healthTexture.Height, graphicsDevice.Viewport.Width + healthTexture.Height / 2), graphicsDevice.Viewport.Height);
                AddHealth(position);
            }

            // Update the Enemies
            for (int i = healths.Count - 1; i >= 0; i--)
            {
                healths[i].Update();

                if (healths[i].Active == false)
                {
                    // If not active and health <= 0
                    if (healths[i].health <= 0)
                    {
                        player.Health += healths[i].toPlayerHealth;
                        Vector2 explosionPosition = new Vector2(healths[i].Position.X + playerTexture.Height + 70, healths[i].Position.Y - 30 /*oduzni 20, za realniji efekt*/);
                        // Add an explosion
                        AddExplosion(explosionPosition);
                        if (options[0] == true)
                        {

                            explosionSound.Play();
                        }

                    }
                    healths.RemoveAt(i);
                }
            }
        }

        private void UpdateEnemies(GameTime gameTime)
        {
            // Spawn a new enemy enemy every 1.5 seconds
            //Debug.WriteLine(gameTime.ElapsedGameTime.Seconds);
            if (updateGameTime)
            {

            }
            if (gameTime.TotalGameTime - previousSpawnTime > enemySpawnTime)
            {
                previousSpawnTime = gameTime.TotalGameTime;
                //if (strongEnemiesCount <= 0)
                //{
                //    AddStrongerEnemy();
                //    //nakon odredjenog vremena dodaju se jaci neprijatelji....na pocetku je to nakon sto se stvori 10 obicnih neprijatelja
                //    strongEnemiesCount = strongEnemiesCountOrigin;
                //}
                //else
                //{
                // Add an Enemy
                AddEnemy();
                //    strongEnemiesCount--;
                //}
            }

            if (gameTime.TotalGameTime - storngerEnemyPreviousSpawnTime > strongerEnemySpawnTime)
            {
                storngerEnemyPreviousSpawnTime = gameTime.TotalGameTime;

                AddStrongerEnemy();
            }


            // Update the Enemies
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                enemies[i].Update(gameTime);

                if (enemies[i].Active == false)
                {
                    // If not active and health <= 0
                    if (enemies[i].Health <= 0)
                    {
                        Vector2 explosionPosition = new Vector2(enemies[i].Position.X + playerTexture.Height + 20, enemies[i].Position.Y - 10 /*oduzni 20, za realniji efekt*/);
                        // Add an explosion
                        AddExplosion(explosionPosition);
                        if (options[0] == true)
                        {
                            explosionSound.Play();

                        }
                        //Add to the player's score
                        score += enemies[i].Value;

                    }
                    enemies.RemoveAt(i);
                }
            }

            // Update the stronger Enemies
            for (int i = strongerEnemies.Count - 1; i >= 0; i--)
            {
                strongerEnemies[i].Update(gameTime);

                if (strongerEnemies[i].Active == false)
                {
                    // If not active and health <= 0
                    if (strongerEnemies[i].Health <= 0)
                    {
                        Vector2 explosionPosition = new Vector2(strongerEnemies[i].Position.X + playerTexture.Height + 20, strongerEnemies[i].Position.Y - 20 /*oduzni 20, za realniji efekt*/);
                        // Add an explosion
                        AddExplosion(explosionPosition);
                        if (options[0] == true)
                        {
                            explosionSound.Play();
                        }
                        //Add to the player's score
                        score += strongerEnemies[i].Value;

                    }
                    strongerEnemies.RemoveAt(i);
                }
            }

        }

        public bool HandleInput(bool pauseGame, Vector3 acceleration, TouchCollection touchInfo)
        {
            if (pauseGame)
            {
                updateGameTime = true;
                return true;
            }
            else return false;
        }

        public void InitializeAssets(ContentManager contentManager, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {

            this.contentManager = contentManager;
            this.spriteBatch = spriteBatch;
            this.graphicsDevice = graphicsDevice;

            // TODO: Perform additional initializations
            // Initialize the enemies list
            enemies = new List<Enemy>();
            healths = new List<Health>();
            stones = new List<Stone>();
            bullets = new List<Bullet>();

            //backgrounds initalize
            bgLayer1 = new Parallaxingbackground();
            bgLayer2 = new Parallaxingbackground();

            bgLayer1.Initialize(contentManager, "bgLayer1a", graphicsDevice.Viewport.Height, 8);
            bgLayer2.Initialize(contentManager, "bgLayer2a", graphicsDevice.Viewport.Height, 7);
            mainBackground = contentManager.Load<Texture2D>("mainbackgrounda");

            // Set the time keepers to zero
            previousSpawnTime = TimeSpan.Zero;
            healthPreviousSpawnTime = TimeSpan.Zero;
            specialBagPreviousSpawnTime = TimeSpan.Zero;
            bulletsPreviousSpawnTime = TimeSpan.Zero;
            storngerEnemyPreviousSpawnTime = TimeSpan.Zero;
            worldBounds = new Rectangle(0, 0, (int)screenWidth, (int)screenHeight);
            player = new Player();

            //spawn times...
            //enemySpawnTime = TimeSpan.FromSeconds(2f);
            enemySpawnTime = TimeSpan.FromSeconds(1.5f);
            healthSpawnTime = TimeSpan.FromSeconds(5f);
            specialBulletFireTime = TimeSpan.FromSeconds(2f);
            bulletsSpawnTime = TimeSpan.FromSeconds(7f);
            stoneSpawnTime = TimeSpan.FromSeconds(6f);
            specialBagTime = TimeSpan.FromSeconds(18f);
            strongerEnemySpawnTime = TimeSpan.FromSeconds(strongerEnemySpawnValue);

            explosions = new List<Animation>();
            projectiles = new List<Projectile>();
            strongerEnemies = new List<Enemy2>();
            specialBullets = new List<SpecialBullet>();
            specialBags = new List<SpecialBag>();
            // Initialize our random number generator
            random = new Random();
        }

        public static void PauseMusic()
        {
            if (!attackGame.isBackgroundSong)
            {
                try
                {
                    // Play the music
                    MediaPlayer.Pause();

                    // Loop the currently playing song
                    MediaPlayer.IsRepeating = true;
                }
                catch { }
            }
        }

        public static void ResumeSong()
        {
            if (!attackGame.isBackgroundSong)
            {
                try
                {
                    // Play the music
                    MediaPlayer.Resume();

                    // Loop the currently playing song
                    MediaPlayer.IsRepeating = true;
                }
                catch { }
            }
        }

        public static void PlayMusic(Song song)
        {
            // Due to the way the MediaPlayer plays music,
            // we have to catch the exception. Music will play when the game is not tethered
            if (!attackGame.isBackgroundSong)
            {
                try
                {
                    // Play the music
                    MediaPlayer.Volume = 0.2f;
                    MediaPlayer.Play(song);

                    // Loop the currently playing song
                    MediaPlayer.IsRepeating = true;
                }
                catch { }
            }
        }

        public static void StopMusic()
        {
            // Due to the way the MediaPlayer plays music,
            // we have to catch the exception. Music will play when the game is not tethered
            if (!attackGame.isBackgroundSong)
            {
                try
                {
                    // Play the music
                    MediaPlayer.Stop();

                    // Loop the currently playing song
                    //MediaPlayer.IsRepeating = true;
                }
                catch { }
            }
        }


        public void LoadOptions()
        {
            options = iso.LoadOptions("options.xml");
        }

        public void LoadContent()
        {

            Debug.WriteLine("loaaad: " + spawnTime.ToString());

            LoadOptions();
            playerTexture = contentManager.Load<Texture2D>("shipAnimation");
            Animation playerAnimation = new Animation();

            //Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            Vector2 playerPosition = new Vector2(300, 5);
            playerAnimation.Initialize(playerTexture, Vector2.Zero, 156, 49, 8, 30, Color.White, 1f, true);
            //shooter
            playerMoveSpeed = 14f;
            //Enable the FreeDrag gesture.
            TouchPanel.EnabledGestures = GestureType.FreeDrag;
            TouchPanel.EnabledGestures = GestureType.Tap;
            TouchPanel.EnabledGestures = GestureType.Hold;
            specialBulletTexture = contentManager.Load<Texture2D>("rocket");
            // Load the score font
            font = contentManager.Load<SpriteFont>("dobarFontJe");
            strongerEnemiesTexture = contentManager.Load<Texture2D>("mineAnimation1");
            // Load the music
            gameplayMusic = contentManager.Load<Song>("menuMusic");
            // Load the laser and explosion sound effect
            stoneTexture = contentManager.Load<Texture2D>("stone");
            laserSound = contentManager.Load<SoundEffect>("sound/laserFire");
            explosionSound = contentManager.Load<SoundEffect>("sound/explosion");
            bulletsTexture = contentManager.Load<Texture2D>("bullets");
            healthTexture = contentManager.Load<Texture2D>("health");
            specialBagTexture = contentManager.Load<Texture2D>("rockets");
            explosionTexture = contentManager.Load<Texture2D>("explosion");
            player.Initialize(playerAnimation, playerPosition);
            projectileTexture = contentManager.Load<Texture2D>("laser");


            laserTexture = contentManager.Load<Texture2D>("laser");

            scoreFont = contentManager.Load<SpriteFont>("ScoreFont");
            menuFont = contentManager.Load<SpriteFont>("MenuFont");
            //shooter
            enemyTexture = contentManager.Load<Texture2D>("mineAnimation");

            //highscores

        }

    }
}
