using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Base;

//Base class for updatable gameobjects
public class GameObject
{
    HashSet<GameObject> children = new();
    public virtual void Update(GameTime gameTime)
    {
        foreach (GameObject child in children)
        {
            child.Update(gameTime);
        }
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        foreach (GameObject child in children)
        {
            child.Draw(spriteBatch);
        }
    }
}