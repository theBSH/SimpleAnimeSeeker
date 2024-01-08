using Godot;
using System;
using BackEnd;
using System.Collections.Generic;
using System.Net.NetworkInformation;

/// <summary>
/// Main class represents the main control node for the application.
/// </summary>
public partial class main : Control
{
    private BackEnd.BackEnd Be = new BackEnd.BackEnd();

    [Export] private string media;
    [Export] private int itemSizeX;
    [Export] private int itemSizeY;
    [Export] private bool debug = false;

    [Signal] public delegate void noInternetEventHandler();

    private const int MaxSlot = 40;

    /// <summary>
    /// Called when the button is pressed to initiate the search.
    /// </summary>
    private void OnButtonPressed()
    {
        if (IsInternetAvailable())
        {
            var grid = GetNode<GridContainer>("Buttom/ScrollContainer/GridContainer");

            // Clear existing items in the grid
            foreach (Control i in grid.GetChildren())
            {
                i.QueueFree();
            }

            var textbox = GetNode<LineEdit>("top/HBoxContainer/LineEdit");
            string inputText = textbox.Text ?? "null";

            List<Med> meds;
            if (debug)
            {
                meds = new List<Med>();
                GD.Print("your in debug!");
            }
            else
            {
                // Perform a search using the input text
                meds = Be.Search(inputText);
            }

            var resContainer = GetNode<ScrollContainer>("Buttom/ScrollContainer");
            grid.CustomMinimumSize = new Vector2(itemSizeX * 4, (MaxSlot / 4) * itemSizeY);
            PackedScene mediaScene = (PackedScene)GD.Load(media);

            // Populate the grid with media instances
            foreach (Med med in meds)
            {
                var mediaInstance = (MEDIA)mediaScene.Instantiate();
                mediaInstance.setcreds(med.id, med.is_debug, med.name, med.year, med.poster, med.poster_large, med.synopsis, med.agerating);
                grid.AddChild(mediaInstance);
            }
        }
        else
        {
            // Signal that there is no internet connection
            EmitSignal(SignalName.noInternet);
        }
    }

    /// <summary>
    /// Checks if the internet is available by pinging Google's public DNS server.
    /// </summary>
    /// <returns>True if internet is available, false otherwise.</returns>
    private static bool IsInternetAvailable()
    {
        try
        {
            Ping ping = new Ping();
            PingReply reply = ping.Send("8.8.8.8", 1000);

            return reply != null && reply.Status == IPStatus.Success;
        }
        catch (PingException)
        {
            return false;
        }
    }
}
