// Requirments:
//  1. <Grid x:Name="coverGrid" Panel.ZIndex="1" Background="#E5000000" Visibility="Collapsed"/> - fills screen to show UserControl inside it.
//  2. byte error;  - 0 will prevent the coverGrid from closing
//  3. RunGC();     - included below, will run GC when UserControls manipulated...

// What it does:
//  Pass your UserControl as parameter, method will check if it exists in coverGrid.Children list, 
//  if yes, it will remove it (close window), if no other children exists in coverGrid, close it)
//  if not, add given UserControl to the coverGrid, if other children already exists in coverGrid,
//  it will hide all other child UserControls, when current UserControl closed it will show previous
//  UserControl child, if no other child exists, it will close coverGrid.


#region PanelController

/// <summary>Manage UserControls and grid displays - hide all but current</summary>
/// <param name="Panel">The UserControl that will be displayed (use 'this' to remove current panel from grid)</param>
public void PanelController(UserControl Panel, bool FullScreen = true, bool StartUp = false)
{
    // check if given panel is exist in the grid: 
    //  YES -> remove it by activating its close method.
    //  NO -> add it to the grid.

    bool remove = false; // check if this panel is exist in the grid remove it
    bool showGrid = false; // check if the last panel is visible

    // Check if Panel already exist, if yes - Remove it
    if (coverGrid.Children.Count > 0)
    {
        // reverse loop
        for (int i = coverGrid.Children.Count - 1; i > -1; i--) { if (coverGrid.Children[i] == Panel) { remove = true; break; } }
    }

    if (remove == true) // REMOVE Panel
    {
        coverGrid.Children.Remove(Panel); // remove this panel from grid

        panelMsgTimerTick(null, null); // hide error message if it was visible

        if (coverGrid.Children.Count > 0) // if there any children left, show the last one
        {
            (coverGrid.Children[coverGrid.Children.Count - 1]).Visibility = Visibility.Visible;
        }
    }
    else // Add panel
    {
        // hide all panels before adding panel
        for (int i = 0; i < coverGrid.Children.Count; i++)
        {
            (coverGrid.Children[i]).Visibility = Visibility.Hidden;
        }

        coverGrid.Children.Add(Panel); // Add Panel
    }


    // check if the last panel is visible
    if (coverGrid.Children.Count > 0)
    {
        // check if last panel only is visible to make sure there is no error
        if ((coverGrid.Children[coverGrid.Children.Count - 1]).Visibility == Visibility.Visible)
        {
            showGrid = true;
        }
    }

    coverGrid.UpdateLayout();

    // show/hide coverGrid - depends on children count
    if (showGrid)
    {
        // grid settings - set full cover or Main menu visible
        if (error == 0)
        {
            coverGrid.Margin = new Thickness(0);
            coverGrid.Background = Brushes.DarkGray; // set different background color if coverGrid is permanent
        }
        else
        {
            coverGrid.Margin = new Thickness(0, 27, 0, 0);
            coverGrid.Background = chkSideBar.Foreground;
        }

        coverGrid.Visibility = Visibility.Visible; // show grid
    }
    else // hide grid
    {
        // if its not startup grid - hide it, *startup grid cannot be removed/closed
        if (error == 0)
        {
            // prevent ClientView Context menu bug - open context menu visible on top of coverGrid
            /*if (clView != null)
            {
                clView.Visibility = Visibility.Visible;
                clView = null;
            }*/

            coverGrid.Visibility = Visibility.Hidden;
            coverGrid.Children.Clear(); // remove all childs from coverGrid
        }
    }

    RunGC(); // activate GC
}

/// <summary>Remove all panels and hide grid</summary>
/// <param name="CloseAll">true to remove all</param>
public void PanelController(bool CloseAll)
{
    if (CloseAll)
    {
        coverGrid.Visibility = Visibility.Hidden;
        coverGrid.Children.Clear();

        panelMsgTimerTick(null, null); // hide error message if it was visible

        RunGC(); // run GC to free memory
    }
}

#endregion


// used in PanelController methods
// experimental - call GC to free up memory, hard collect
public static void RunGC()
{
    Main.UpdateLayout();

    // [TODO] Modify this function - add parameters
    GC.Collect(0, GCCollectionMode.Forced);
    GC.WaitForPendingFinalizers();

    GC.Collect(1, GCCollectionMode.Forced);
    GC.WaitForPendingFinalizers();

    GC.Collect(2, GCCollectionMode.Forced);
    GC.WaitForPendingFinalizers();

    // addon
    GC.Collect();
    GC.WaitForPendingFinalizers();
    GC.Collect();
}