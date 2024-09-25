using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework.Graphics;

public class VerticalInventory : UIState
{
    public UIPanel InventoryPanel;

    public override void OnInitialize()
    {
        InventoryPanel = new UIPanel();
        InventoryPanel.Width.Set(52, 0f); // 设置宽度，适应一个物品槽
        InventoryPanel.Height.Set(52 * 10, 0f); // 设置高度，适应10个物品槽
        InventoryPanel.BackgroundColor = new Color(73, 94, 171);

        for (int i = 0; i < 10; i++)
        {
            var itemSlot = new ItemSlotWrapper(Main.LocalPlayer.inventory[i]);
            itemSlot.Top.Set(i * 52, 0f); // 垂直布局
            itemSlot.Left.Set(0, 0f);
            itemSlot.Width.Set(50, 0f);
            itemSlot.Height.Set(50, 0f);
            InventoryPanel.Append(itemSlot);
        }

        Append(InventoryPanel);
    }
}

// 修改后的物品槽封装
public class ItemSlotWrapper : UIElement
{
    private  Item _item;
    private readonly int _context;
    private readonly int _slot;

    public ItemSlotWrapper(Item item, int context = ItemSlot.Context.InventoryCoin, int slot = 0)
    {
        _item = item;
        _context = context;
        _slot = slot;
        Width.Set(50, 0f);
        Height.Set(50, 0f);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);
        ItemSlot.Draw(spriteBatch, ref _item, _context, GetDimensions().Position());
    }
}
