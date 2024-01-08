using Godot;
using System;
using System.Text;

/// <summary>
/// MEDIA class represents a Godot Control node for displaying individual media items.
/// </summary>
public partial class MEDIA : Control
{
    // Properties for storing media information
    private int med_id;
    private bool is_debug;
    private string name;
    private int year;
    private string poster;
    private string poster_large;
    private string synopsis;
    private string rating;

    // UI elements
    private Label label;
    private TextureRect image;

    // HTTP request for downloading images
    private HttpRequest downloader;

    // Path to the details scene
    [Export] private string details_path;

    // PackedScene for the details scene
    private PackedScene details;

    /// <summary>
    /// Sets the credentials for the media item.
    /// </summary>
    public void setcreds(int id, bool debug, string namee, int yearr, string posterr, string posterr_large, string synopsiss, string agerating)
    {
        med_id = id;
        is_debug = debug;
        name = namee;
        year = yearr;
        poster = posterr;
        poster_large = posterr_large;
        synopsis = synopsiss;
        rating = agerating;

        // Load the details scene
        details = (PackedScene)GD.Load(details_path);
    }

    /// <summary>
    /// Updates the media item with its information.
    /// </summary>
    public void update_media()
    {
        label.Text = name;

        // Initiate an HTTP request to download the poster image
        downloader.RequestCompleted += OnComplete;
        downloader.Request(poster);
    }

    /// <summary>
    /// Called when the MEDIA node enters the scene.
    /// </summary>
    public override void _Ready()
    {
        // Initialize UI elements
        label = GetNode<Label>("MarginContainer/VBoxContainer/Label");
        downloader = GetNode<HttpRequest>("HTTPRequest");
        image = GetNode<TextureRect>("MarginContainer/VBoxContainer/TextureRect");

        // Update the media item
        update_media();
    }

    /// <summary>
    /// Callback function called when the HTTP request for the poster is completed.
    /// Handles the downloaded image and displays it in the TextureRect.
    /// </summary>
    private void OnComplete(long result, long code, string[] headers, byte[] body)
    {
        if (code == 200)
        {
            // Successfully downloaded the image
            ImageTexture imageTexture = new ImageTexture();
            Image image = new Image();
            image.LoadJpgFromBuffer(body);
            imageTexture = ImageTexture.CreateFromImage(image);
            this.image.Texture = imageTexture;
        }
        else
        {
            GD.Print("Error loading image:", code);
        }
    }

    /// <summary>
    /// Called when a button is pressed to open the details scene.
    /// </summary>
    private void OnButtonPressed()
    {
        // Instantiate the details scene and set its properties
        var instance = (details)details.Instantiate();
        instance.Name = name;
        instance.Year = year;
        instance.Poster = poster_large;
        instance.Rating = rating;
        instance.Synopsis = synopsis;

        // Start the details scene
        instance.Start();

        // Add the details scene as a child of the root
        GetNode("/root").AddChild(instance);
    }
}
