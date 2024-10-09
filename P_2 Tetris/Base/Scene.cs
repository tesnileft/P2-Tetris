using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Base;

public class Scene
{
    HashSet<GameObject> _objects;
    public virtual void Init()
    {
        //Load all your stuff here
    }
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        foreach (GameObject o in _objects)
        {
            o.Draw(spriteBatch);
        }
    }

    public virtual void Update(GameTime gameTime)
    {
        foreach (GameObject o in _objects)
        {
            o.Update(gameTime);
        }
    }
}