using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Base;

public class GameObject
{
    HashSet<GameObject> children;
    public void Update(GameTime gameTime)
    {
        foreach (GameObject child in children)
        {
            child.Update(gameTime);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (GameObject child in children)
        {
            child.Draw(spriteBatch);
        }
    }
}