using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace Terraria
{
    public class PowerUIElement : PowerUI
    {
        public PowerUIElement parent;

        public List<PowerUIElement> children = new List<PowerUIElement>();

        public Vector2 MyRelativePos;

        public Vector2 MySize;

        public Action MouseUponMe;

        public Action MouseNotUponMe;

        public Action MouseClickMe;

        public Texture2D MyTexture;

        public Point MyCenterType;

        public int MyID;

        public object MyCunstomData;

        public bool UseMouseFix;

        public bool IsMouseUpon;

        public bool IsFocusOn;

        public bool CanFocusMe;

        public Vector2 MyPosition;

        public bool ShouldActive;

        protected Vector2 MyCenterFix;

        private bool MouseDown;

        private bool LastMouseStatus;

        private bool First;

        public PowerUIElement()
        {
            ShouldActive = true;
            UseMouseFix = true;
            parent = null;
            MyPosition = Vector2.Zero;
            MySize = Vector2.Zero;
            MyTexture = null;
            MyID = -1;
            MyCunstomData = null;
            MyCenterType = CenterType.TopLeft;
            MouseClickMe = (Action)Delegate.Combine(MouseClickMe, new Action(Upon));
            MouseUponMe = (Action)Delegate.Combine(MouseUponMe, new Action(Upon));
            MouseNotUponMe = (Action)Delegate.Combine(MouseNotUponMe, new Action(NotUpon));
            CanFocusMe = true;
            First = true;
        }

        public void Active()
        {
            if (First)
            {
                FirstActive();
                First = false;
            }
            PreActive();
            bool flag = false;
            TouchCollection touchLocations = TouchPanel.GetState();
            for (int i = 0; i < touchLocations.Count; i++)
            {
                flag = Helper.InTheRange(MyPosition - MyCenterFix, MySize, touchLocations[i].Position / Main.UIScale);
            }
            MyPosition = ((parent == null) ? MyRelativePos : (MyRelativePos + parent.MyPosition));
            if (MySize == Vector2.Zero && MyTexture != null)
            {
                MySize = new Vector2(MyTexture.Width, MyTexture.Height);
            }
            if ((!LastMouseStatus && flag) || (!UseMouseFix && flag))
            {
                MouseDown = true;
            }
            else
            {
                MouseDown = false;
            }
            MyCenterFix = MySize;
            MyCenterFix.X *= ((MyCenterType.X == 1) ? 0f : ((MyCenterType.X == 2) ? 0.5f : 1f));
            MyCenterFix.Y *= ((MyCenterType.Y == 1) ? 0f : ((MyCenterType.Y == 2) ? 0.5f : 1f));
            IsFocusOn = flag && CanFocusMe;
            foreach (PowerUIElement child in children)
            {
                if (child != null && child.IsFocusOn)
                {
                    IsFocusOn = false;
                }
            }
            if (MouseDown && IsFocusOn)
            {
                MouseUponMe();
                MouseClickMe();
            }
            else if (!flag)
            {
                MouseNotUponMe();
            }
            LastMouseStatus = IsFocusOn;
            ActiveChildren();
            PostActive();
        }

        private void Upon()
        {
            IsMouseUpon = true;
            Main.LocalPlayer.mouseInterface = true;
        }

        private void NotUpon()
        {
            IsMouseUpon = false;
        }

        public virtual void PostActive()
        {
        }

        public virtual void PreActive()
        {
        }

        public virtual void FirstActive()
        {
        }

        public void Append(PowerUIElement child)
        {
            child.parent = this;
            children.Add(child);
        }

        public bool Subtract(PowerUIElement child)
        {
            if (children.Contains(child))
            {
                child.parent = null;
                return children.Remove(child);
            }
            return false;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (MyTexture != null)
            {
                MyCenterFix = MySize;
                MyCenterFix.X *= ((MyCenterType.X == 1) ? 0f : ((MyCenterType.X == 2) ? 0.5f : 1f));
                MyCenterFix.Y *= ((MyCenterType.Y == 1) ? 0f : ((MyCenterType.Y == 2) ? 0.5f : 1f));
                spriteBatch.Draw(MyTexture, MyPosition - MyCenterFix, Color.White);
            }
            DrawChildren(spriteBatch);
        }

        public void DrawChildren(SpriteBatch spriteBatch)
        {
            foreach (PowerUIElement child in children)
            {
                if (child != null && child.ShouldActive)
                {
                    child.Draw(spriteBatch);
                }
            }
        }

        public void ActiveChildren()
        {
            foreach (PowerUIElement child in children)
            {
                if (child != null && child.ShouldActive)
                {
                    child.Active();
                }
            }
        }

        public override string ToString()
        {
            return "Position:" + MyPosition.ToString() + "ID:" + MyID;
        }
    }
}
