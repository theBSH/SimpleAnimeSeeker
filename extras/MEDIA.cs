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
    Label label;
    TextureRect iimage;

    HttpRequest downloader;

    public void setcreds(int id,bool debug,string namee,int yearr,string posterr)
    {
        med_id = id;
        is_debug = debug;
        name = namee;
        year = yearr;
        poster = posterr;
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
}