using Godot;
using System;
using System.Text;

public partial class MEDIA : Control
{
    int med_id;
    bool is_debug;
    string name;
    int year;
    string poster;
    string poster_large;
    string synopsis;
    string rating;
    Label label;
    TextureRect iimage;

    HttpRequest downloader;
    [Export]string details_path;
    PackedScene details;

    public void setcreds(int id,bool debug,string namee,int yearr,string posterr,string posterr_large,string synopsiss,string agerating)
    {
        med_id = id;
        is_debug = debug;
        name = namee;
        year = yearr;
        poster = posterr;
        poster_large = posterr_large;
        synopsis = synopsiss;
        rating = agerating;
        details = (PackedScene)GD.Load(details_path);
    }

    public void update_media()
    {
        label.Text = name;

        downloader.RequestCompleted += OnComplete;

        downloader.Request(poster);
    }

    public override void _Ready()
    {
        label = GetNode<Label>("MarginContainer/VBoxContainer/Label");
        downloader = GetNode<HttpRequest>("HTTPRequest");
        iimage = GetNode<TextureRect>("MarginContainer/VBoxContainer/TextureRect");
        update_media();
    }

    private void OnComplete(long result, long code, string[] headers, byte[] body)
    {
        if (code == 200)
        {
            
            ImageTexture imageTexture = new ImageTexture();
            Image image = new Image();
            image.LoadJpgFromBuffer(body);
            imageTexture = ImageTexture.CreateFromImage(image);
            iimage.Texture = imageTexture;
        }
        else
        {
            GD.Print("Error loading image:", code);
        }
    }

    private void OnButtonPressed()
    {
        var instance = (details)details.Instantiate();
        instance.name = name;
        instance.year = year;
        instance.poster = poster_large;
        instance.rating = rating;
        instance.synopsis = synopsis;
        instance.start();
        GetNode("/root").AddChild(instance);
    }
}