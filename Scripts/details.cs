using Godot;
using System;

public partial class details : Control
{
	public string name;
    public int year;
    public string poster;
    public string synopsis;
    public string rating;

	public void start()
	{
		var name_label = GetNode<Label>("Name");
		name_label.Text = name;
		var year_label = GetNode<Label>("Year");
		year_label.Text = "Year: " + year;
		var rating_label = GetNode<Label>("Rating");
		rating_label.Text = "Age rating: " + rating;
		var synopsis_label = GetNode<Label>("ScrollContainer/Synopsis");
		synopsis_label.Text = synopsis;
	}

    public override void _Ready()
    {
        HttpRequest httpmodual = GetNode<HttpRequest>("HTTPRequest");
		httpmodual.RequestCompleted += OnComplete;
		httpmodual.Request(poster);
    }

    private void OnComplete(long result, long code, string[] headers, byte[] body)
	{
		if (code == 200)
		{
			Image image = new Image();
			ImageTexture imageTexture = new ImageTexture();
			image.LoadJpgFromBuffer(body);
			imageTexture = ImageTexture.CreateFromImage(image);
			var poster_rect = GetNode<TextureRect>("TextureRect");
			poster_rect.Texture = imageTexture;
		}
		else
			GD.Print("Error");
	}

	private void OnButtonPressed()
	{
		this.QueueFree();
	}

}
