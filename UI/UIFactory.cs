﻿using LansUILib.ui;
using LansUILib.ui.components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
namespace LansUILib
{
    public class UIFactory
    {
        private static void SetCursorOnHover(LComponent component, string cursor, Vector2 offset)
        {
            component.MouseEnter += delegate (MouseState state)
            {
               // UISystem.Instance.SetCursor(ModContent.Request<Texture2D>(cursor, AssetRequestMode.ImmediateLoad), offset);
            };
            component.MouseExit += delegate (MouseState state)
            {
               // UISystem.Instance.ClearCursor();
            };
        }
        public static LComponent CreatePanel(string name, PanelSettings settings, bool draggable = true, bool resizeable = true)
        {
            var panel = new LComponent(name, settings);
            panel.MouseInteraction = true;
            var backgroundTexture = ModContent.Request<Texture2D>("Terraria/Images/UI/PanelBackground", AssetRequestMode.ImmediateLoad);
            panel.image = new LImage(new WrapperLColor(new Color(63, 82, 151) * 0.7f), new WrapperLSprite(backgroundTexture), new CornerBox(12, 12, 12, 12));
            var borderTexture = ModContent.Request<Texture2D>("Terraria/Images/UI/PanelBorder", AssetRequestMode.ImmediateLoad);
            panel.border = new LImage(new WrapperLColor(Color.Black), new WrapperLSprite(borderTexture), new CornerBox(12, 12, 12, 12));

            if (draggable)
            {
                var dragging = false;
                panel.MouseDown += delegate (MouseState state) { dragging = true; };

                panel.MouseUp += delegate (MouseState state) { dragging = false; };

                panel.MouseMove += delegate (MouseState state)
                {
                    if (dragging)
                    {
                        panel.Move(state.deltaX, state.deltaY);
                        panel.Invalidate();
                    }
                };

                SetCursorOnHover(panel, "LansUILib/move", new Vector2(-12, -12));
            }
            if (resizeable)
            {
                {
                    var resize = new LComponent(name + "resize1");
                    resize.MouseInteraction = true;
                    resize.SetAnchors(0, 0, 1, 0);
                    //resize.border = new LImage(new WrapperLColor(Color.Black), new WrapperLSprite(borderTexture), new CornerBox(12, 12, 12, 12));

                    var dragging = false;
                    resize.MouseDown += delegate (MouseState state) { dragging = true; };

                    resize.MouseUp += delegate (MouseState state) { dragging = false; };

                    resize.MouseMove += delegate (MouseState state)
                    {
                        if (dragging)
                        {
                            panel.Move(0, state.deltaY);
                            panel.Resize(0, -state.deltaY);
                            panel.Invalidate();
                        }
                    };

                    SetCursorOnHover(resize, "LansUILib/resizevertical", new Vector2(-12, -12));
                    panel.Add(resize);
                }
                {
                    var resize = new LComponent(name + "resize2");
                    resize.MouseInteraction = true;
                    resize.SetAnchors(0, 1, 1, 1);
                    resize.SetMargins(10, -10, 10, 0);
                      var dragging = false;
                    resize.MouseDown += delegate (MouseState state) { dragging = true; };
                    resize.MouseUp += delegate (MouseState state) { dragging = false; };

                    resize.MouseMove += delegate (MouseState state)
                    {
                        if (dragging)
                        {
                            panel.Resize(0, state.deltaY);
                            panel.Invalidate();
                        }
                    };
                    SetCursorOnHover(resize, "LansUILib/resizevertical", new Vector2(-12, -12));
                    panel.Add(resize);
                }
                {
                    var resize = new LComponent(name + "resize3");
                    resize.MouseInteraction = true;
                    resize.SetAnchors(0, 0, 0, 1);
                    resize.SetMargins(0, 10, -10, 10);
                      var dragging = false;
                    resize.MouseDown += delegate (MouseState state) { dragging = true; };
                    resize.MouseUp += delegate (MouseState state) { dragging = false; };

                    resize.MouseMove += delegate (MouseState state)
                    {
                        if (dragging)
                        {
                            panel.Move(state.deltaX, 0);
                            panel.Resize(-state.deltaX, 0);
                            panel.Invalidate();
                        }
                    };
                    SetCursorOnHover(resize, "LansUILib/resizehorizontal", new Vector2(-12, -12));
                    panel.Add(resize);
                }
                {
                    var resize = new LComponent(name + "resize4");
                    resize.MouseInteraction = true;
                    resize.SetAnchors(1, 0, 1, 1);
                    resize.SetMargins(-10, 10, 0, 10);
                      var dragging = false;
                    resize.MouseDown += delegate (MouseState state) { dragging = true; };
                    resize.MouseUp += delegate (MouseState state) { dragging = false; };

                    resize.MouseMove += delegate (MouseState state)
                    {
                        if (dragging)
                        {
                            panel.Resize(state.deltaX, 0);
                            panel.Invalidate();
                        }
                    };
                    SetCursorOnHover(resize, "LansUILib/resizehorizontal", new Vector2(-12, -12));
                    panel.Add(resize);
                }
            }

            return panel;
        }
        public static LComponent CreatePanel(string name, bool draggable = true, bool resizeable = true)
        {
            return CreatePanel(name, null, draggable, resizeable);
        }
        public static LComponent CreatePanel(string name)
        {
            return CreatePanel(name, null, false, false);
        }
        public static ScrollPanel CreateScrollPanel()
        {
            var panel = new LComponent("ScrollPanel");
            var backgroundTexture = ModContent.Request<Texture2D>("LansUILib/move", AssetRequestMode.ImmediateLoad);
            var maskPanel = new LComponent("ScrollPanelMask");
            maskPanel.isMask = true;
                maskPanel.SetMargins(0, 0, 20, 0);
            panel.Add(maskPanel);
            var contentPanel = new LComponent("ScrollPanelContent");
            contentPanel.SetAnchors(0, 0, 1, 0);
            contentPanel.SetMargins(0, 0, 0, -500);
            maskPanel.Add(contentPanel);

            var scrollbar = UIFactory.CreateScrollbar();
            scrollbar.scrollbarComponent.SetAnchors(1, 0, 1, 1);
            scrollbar.scrollbarComponent.SetMargins(-20, 0, 0, 0);
            
            panel.Add(scrollbar.scrollbarComponent);
            var scrollpanelComponent = new ScrollPanel(scrollbar, panel, maskPanel, contentPanel);

            return scrollpanelComponent;
        }
        public static Scrollbar CreateScrollbar()
        {
            var backgroundTexture = ModContent.Request<Texture2D>("LansUILib/move", AssetRequestMode.ImmediateLoad);

            var panel = new LComponent("Scrollbar");
            panel.image = new LImage(new WrapperLColor(Color.White),
                new WrapperLSprite(
                    Main.Assets.Request<Texture2D>("Images/UI/Scrollbar", AssetRequestMode.ImmediateLoad)
                ), new CornerBox(6, 6, 6, 6)
            );
            var handleTexture = new WrapperLSprite(
                    Main.Assets.Request<Texture2D>("Images/UI/ScrollbarInner", AssetRequestMode.ImmediateLoad)
                );
            
            panel.MouseInteraction = true;
            var handle = new LComponent("ScrollbarHandle");
            handle.MouseInteraction = true;


            var defaultImage = new LImage(new WrapperLColor(Color.White * 0.8f), handleTexture, new CornerBox(6, 6, 6, 6));
            var hoverImage = new LImage(new WrapperLColor(Color.White), handleTexture, new CornerBox(6, 6, 6, 6));

            handle.image = defaultImage;
            panel.Add(handle);
            var bar = new Scrollbar(panel, handle, 0.7f, 0.3f);
            handle.MouseEnter += delegate (MouseState e)
            {
                handle.image = hoverImage;
            };

            handle.MouseExit += delegate (MouseState e)
            {
                handle.image = defaultImage;
            };
            return bar;
        }
        public static LButton CreateButton(string buttonText)
        {
            var panel = CreatePanel("Button");
            panel.MouseInteraction = true;
            var text = CreateText(buttonText);
            panel.Add(text);


            return new LButton(panel,text, new WrapperLColor(Color.White), new WrapperLColor(Color.White * 0.8f), new WrapperLColor(Color.White * 0.6f));
        }
        public static LComponent CreateText(string value, bool useLayout = false)
        {
            var text = new LComponent("Text");
            text.text = value;
            text.textColor = new WrapperLColor(Color.White);
            if (useLayout)
            {
                text.SetLayout(new WrapperLayoutText());
            }

            return text;
        }
        public static LComponent CreateImage(string texture, bool useLayout = false)
        {
            var component = new LComponent("Image");
            var backgroundTexture = ModContent.Request<Texture2D>(texture, AssetRequestMode.ImmediateLoad);
            component.image = new LImage(new WrapperLColor(Color.White), new WrapperLSprite(backgroundTexture));
            if (useLayout)
            {
                component.SetLayout(new WrapperLayout(backgroundTexture, null));
            }

            return component;
        }
        public static LComponent CreateImage(Asset<Texture2D> texture, DrawAnimation animation, bool useLayout = false)
        {
            var component = new LComponent("Image");
            component.image = new LImage(new WrapperLColor(Color.White), new WrapperLSprite(texture, animation));
            if (useLayout)
            {
                component.SetLayout(new WrapperLayout(texture, animation));
            }

            return component;
        }
        public static LComponent CreateImage(Asset<Texture2D> texture, Rectangle rectangle, bool useLayout = false)
        {
            var component = new LComponent("Image");
            component.image = new LImage(new WrapperLColor(Color.White), new WrapperLSprite(texture, null, rectangle));
            if (useLayout)
            {
                component.SetLayout(new WrapperLayout(texture, null, rectangle));
            }

            return component;
        }
        public static LComponent CreateItemSlot(LItemSlot lItemSlot, Func<Item, bool> acceptItem = null)
        {
            
            var panel = CreatePanel("ItemSlot");
            panel.MouseInteraction = true;
            panel.SetLayout(new LayoutSize(36, 36));
            var itemSprite = new LComponent("ItemSlotSprite");

            var updateItemSprite = delegate ()
            {
                if (lItemSlot.Item.type > ItemID.None)
                {
                    itemSprite.image = new LImage(new WrapperLColor(Color.White), 
                        new WrapperLSprite(TextureAssets.Item[lItemSlot.Item.type], Main.itemAnimations[lItemSlot.Item.type]), ImageFillMode.Normal, new Vector2(32, 32));
                }
                else
                {
                    itemSprite.image = null;
                }
            };

            lItemSlot.OnChanged += delegate ()
            {
                updateItemSprite();
            };

            updateItemSprite();


            panel.MouseUp += delegate (MouseState state)
            {
                if (!Main.mouseItem.IsAir)
                {
                    var error = false;
                    if (acceptItem == null)
                    {
                        if (lItemSlot.type == LItemSlotType.PetAndLight && !Main.vanityPet[Main.mouseItem.buffType] && !Main.lightPet[Main.mouseItem.buffType])
                        {
                            error = true;
                        }
                        if (lItemSlot.type == LItemSlotType.Pet && !Main.vanityPet[Main.mouseItem.buffType])
                        {
                            error = true;
                        }
                        if (lItemSlot.type == LItemSlotType.Light && !Main.lightPet[Main.mouseItem.buffType])
                        {
                            error = true;
                        }
                    }
                    else
                    {
                        error = !acceptItem(Main.mouseItem);
                    }

                    if (error)
                    {
                        Main.NewText("Cannot swap item, as the slot does not accept the given item.", new Color(255, 0, 0));
                        return;
                    }
                }
                var curr = Main.mouseItem;
                Main.mouseItem = lItemSlot.Item;
                lItemSlot.Item = curr;
                panel.Invalidate();
            };

            panel.Add(itemSprite);
            return panel;
        }
       
    }
}
