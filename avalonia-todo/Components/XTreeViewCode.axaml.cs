using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;

namespace avalonia_todo.Components;

public partial class XTreeViewCode : UserControl{
    public XTreeViewCode(){
        
        // 0. 必须调用，加载样式
        InitializeComponent();
        
        var globalTreeView = new TreeView();

        globalTreeView.ItemsSource = new List<TreeNode>{
            new(){
                Name = "L1",
                SubNodes =[
                    new TreeNode{ Name = "L1-1" },
                    new TreeNode{ Name = "L1-2" }
                ]
            },
            new(){ Name = "L2" }
        };
        globalTreeView.ItemTemplate = new FuncDataTemplate<TreeNode>(
            GenerateTreeItem, true);
        
        Control GenerateTreeItem(TreeNode node, INameScope nameScope){
            
            // 名称
            var textBlock = new TextBlock{
                [!TextBlock.TextProperty] = new Binding(nameof(TreeNode.Name))
            };
            
            // 创建 TreeViewItem 包装
            var treeViewItem = new TreeViewItem{
                Header = textBlock,
                [!ItemsControl.ItemsSourceProperty] = new Binding(
                    nameof(TreeNode.SubNodes)),
                // 设置 ItemTemplate 为自身，实现递归
                ItemTemplate = globalTreeView.ItemTemplate
            };

            // 添加鼠标事件处理，防止事件冒泡
            treeViewItem.PointerEntered += (sender, e) => {
                e.Handled = true; // 阻止事件冒泡到父项
            };
            
            treeViewItem.PointerExited += (sender, e) => {
                e.Handled = true; // 阻止事件冒泡到父项
            };

            return treeViewItem;
        }
        
        Content = globalTreeView;
    }
    
    public class TreeNode{
        public string Name{ get; set; }
        public List<TreeNode> SubNodes{ get; set; } = new();
    }
}