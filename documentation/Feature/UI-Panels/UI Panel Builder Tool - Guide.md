---
tags:
  - author/christian
  - front-end
  - feature/ui-panels
---
>[!warning] IMPORTANT
>This is a tool that is meant to be used for speeding up the process of the informational panels, This does not create the content for those panels. 
>
>This is purely to not have to go through the tedious process of setting up and assigning every individual panel to every button.
#### System Summary
- **Register scene-based panels** (no type mismatch).
- **Optionally use prefabs** for future expansion.
- **Build UI sequences** in the custom tool.
- **Click a button** to open a step-by-step panel flow.

### **1. Scene Setup**
1. **Create** a empty `GameObject` named **`ScenePanelRegistry`** (or place the script on your Main Camera)
	- Attach the **`ScenePanelRegistry`** script.
	- This script scans your scene for panels with `ScenePanelID`.
2. **Add `ScenePanelID`** to Each panel in the Scene
	- For each UI Panel, add the `ScenePanelID` component.
	- Set a **unique** `panelID` (this exact string will be used in the editor if you choose `SceneObject` type).
3. **Create** an empty `GameObject` named **`UIManager`** (or place the script on your Main Camera)
	- Attach the **`UIManager`** script.
	- Assign the `Left`, `Right`, `Close` buttons in the inspector (for Next/Previous/Close of the panels).
4. **Create** an empty `GameObject` named **`UIButtonManager`** (or place the script on your Main Camera)
	- Attach the **`UIButtonHandler`** script.
	- In the inspector:
		- Assign your **`UISequenceDatabase`** asset (after completing step 2).
		- Assign a `uiParent` (usually your main `Canvas`) so that instantiated prefabs appear on-screen. (If this option is used instead of SceneObjects).

### **2.  Create the UISequence Database Asset**
1. In your **Project** window, right-click and choose:
	- **`Create > UI > UISequence Database`**
2. Give it an appropriate name for a panel database (You only need one of these per scene).
This ScriptableObject holds the **button-to-sequence data**.

### **3. Use the Custom Editor Tool**
1. **Open** the **UI Sequence Editor** via:
	- **`Tools > UI Sequence Editor`**
2. In the window:
	- Assign the **`UISequenceDatabase`** asset in the **Database** field.
	- Enter a **Button Name** (the **name** of a `Button` GameObject in your scene).
	- **Add Panel Steps**:
		- Select **Panel Type** `SceneObject` or `Prefab`.
		- If `SceneObject`, type the **`panelID`** that matches the **`panelID`** that was given to each individual panel.
		- If `Prefab`, drag in a **Prefab** of that panel from your project folder.
	- Click **`Save Sequence`** to store this data in the asset.

### **4. Connecting Buttons to Sequences Automatically**
**`UIButtonHandler`** scans **all** `Button` components in the scene at runtime. If a button's **name** matches the **`buttonName`** from the database, it assigns an onClick event:
```csharp
button.onClick.AddListener(() => {
      var panels = UISequenceResolver.ResolvePanels(panelData, uiParent);
      UIManager.Instance.OpenPanelSequence(panels);
  });
```
