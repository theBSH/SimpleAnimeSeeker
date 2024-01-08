using Godot;
using System;

/// <summary>
/// Details class represents a Godot Control node for displaying detailed information about an anime.
/// This class is used in conjunction with MEDIA.cs to populate and display anime details.
/// </summary>
public partial class details : Control
{
    // Variables (will be set in MEDIA.cs)
    public string Name;
    public int Year;
    public string Poster;
    public string Synopsis;
    public string Rating;

    /// <summary>
    /// Sets up the Details node with information obtained from MEDIA.cs.
    /// </summary>
    public void Start()
    {
        var nameLabel = GetNode<Label>("Name");
        nameLabel.Text = Name;

        var yearLabel = GetNode<Label>("Year");
        yearLabel.Text = "Year: " + Year;

        var ratingLabel = GetNode<Label>("Rating");
        ratingLabel.Text = "Age rating: " + Rating;

        var synopsisLabel = GetNode<Label>("ScrollContainer/Synopsis");
        synopsisLabel.Text = Synopsis;
    }

    /// <summary>
    /// Called when the Details node enters the scene.
    /// Initiates the download of the anime poster using an HTTP request.
    /// </summary>
    public override void _Ready()
    {
        // We will download the poster from the URL (the URL here is Poster)
        HttpRequest httpModule = GetNode<HttpRequest>("HTTPRequest");
        httpModule.RequestCompleted += OnComplete;
        httpModule.Request(Poster);
    }

    /// <summary>
    /// Callback function called when the HTTP request for the poster is completed.
    /// Handles the downloaded image and displays it in the TextureRect.
    /// </summary>
    private void OnComplete(long result, long code, string[] headers, byte[] body)
    {
        // Check if it has successfully downloaded the image
        if (code == 200)
        {
            // We make the image a texture and set the PosterRect (the TextureRect that we created) texture
            Image image = new Image();
            ImageTexture imageTexture = new ImageTexture();
            image.LoadJpgFromBuffer(body);
            imageTexture = ImageTexture.CreateFromImage(image);

            var posterRect = GetNode<TextureRect>("TextureRect");
            posterRect.Texture = imageTexture;
        }
        else
        {
            GD.Print("Error");
        }
    }

    /// <summary>
    /// Callback function called when the back button is pressed.
    /// Destroys the instance of the Details node.
    /// </summary>
    private void OnButtonPressed()
    {
        this.QueueFree();
    }
}
