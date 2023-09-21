#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace JKFrame.Editor 
{
    public class JKFrameEditor : OdinMenuEditorWindow
    {
        [MenuItem("JKFrame/JKFrameEditor")]
        static void OpenEasyRPGEditor()
        {
            var window = GetWindow<JKFrameEditor>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1000, 500);
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(true);
            tree.DefaultMenuStyle.IconSize = 28.00f;
            tree.Config.DrawSearchToolbar = true;

            // Adds the character overview table.
            //UnitOverview.Instance.UpdateUnitOverview();

            tree.Add("GameSetting", JKFrameRoot.Setting);

            //tree.Add("Units", new UnitTable(UnitOverview.Instance.AllUnitDatas));

            // Adds all characters.
            //tree.AddAllAssetsAtPath("Units", "Assets/EasyRPG/Config/Unit", typeof(UnitData), true, true);

            // Add all scriptable object items.
            //tree.AddAllAssetsAtPath("", "Assets/EasyRPG/Config/Items", typeof(Item), true)
            //    .ForEach(this.AddDragHandles);

            //tree.AddAllAssetsAtPath("Skills", "Assets/EasyRPG/Config/Skill", typeof(SkillData), true)
            //    .ForEach(this.AddDragHandles);

            // Add drag handles to items, so they can be easily dragged into the inventory if characters etc...
            //tree.EnumerateTree().Where(x => x.Value as Item).ForEach(AddDragHandles);

            //// Add icons to characters and items.
            //tree.EnumerateTree().AddIcons<UnitData>(x => x.IconBlackTex);
            //tree.EnumerateTree().AddIcons<Item>(x => x.Icon);
            //tree.EnumerateTree().AddIcons<SkillData>(x => x.Icon);
            return tree;
        }

        private void AddDragHandles(OdinMenuItem menuItem)
        {
            menuItem.OnDrawItem += x => DragAndDropUtilities.DragZone(menuItem.Rect, menuItem.Value, false, false);
        }

        protected override void OnBeginDrawEditors()
        {
            var selected = this.MenuTree.Selection.FirstOrDefault();
            var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;

            // Draws a toolbar with the name of the currently selected menu item.
            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                if (selected != null)
                {
                    GUILayout.Label(selected.Name);
                }

                //if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Item")))
                //{
                //    ScriptableObjectCreator.ShowDialog<Item>("Assets/EasyRPG/Config/Items", obj =>
                //    {
                //        //obj.NameID = obj.name;
                //        //obj.DesID = obj.name;
                //        base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                //    });
                //}

                //if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Unit")))
                //{
                //    ScriptableObjectCreator.ShowDialog<UnitData>("Assets/EasyRPG/Config/Unit", obj =>
                //    {
                //        TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                //    });
                //}

                //if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Skill")))
                //{
                //    ScriptableObjectCreator.ShowDialog<SkillData>("Assets/EasyRPG/Config/Skill", obj =>
                //    {
                //        //obj.EnemyTeamList = new Dictionary<string, List<EnemyTeamInfo>>();
                //        TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                //    });
                //}

                //if (SirenixEditorGUI.ToolbarButton(new GUIContent("Apply")))
                //{
                //    UnitOverview.Instance.UpdateUnitOverview();
                    //ConfigManager.Instance.AllEnemys = AssetDatabase.FindAssets("t:EnemyData")
                    //    .Select(guid => AssetDatabase.LoadAssetAtPath<EnemyData>(AssetDatabase.GUIDToAssetPath(guid))).OrderBy(enemy => enemy.ID)
                    //    .ToArray();
                    //ConfigManager.Instance.AllWeapons = AssetDatabase.FindAssets("t:WeaponItem")
                    //    .Select(guid => AssetDatabase.LoadAssetAtPath<WeaponItem>(AssetDatabase.GUIDToAssetPath(guid)))
                    //    .ToArray();
                    //ConfigManager.Instance.AllArmors = AssetDatabase.FindAssets("t:ArmorItem")
                    //    .Select(guid => AssetDatabase.LoadAssetAtPath<ArmorItem>(AssetDatabase.GUIDToAssetPath(guid)))
                    //    .ToArray();
                    //ConfigManager.Instance.AllProps = AssetDatabase.FindAssets("t:PropItem")
                    //    .Select(guid => AssetDatabase.LoadAssetAtPath<PropItem>(AssetDatabase.GUIDToAssetPath(guid)))
                    //    .ToArray();
                //}
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }
    }
}
#endif

