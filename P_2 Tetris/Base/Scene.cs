using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Base;

public abstract class Scene
{
    protected HashSet<GameObject> _objects = new HashSet<GameObject>();
    public abstract void Init();

    public abstract void LoadContent(ContentManager contentManager);
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