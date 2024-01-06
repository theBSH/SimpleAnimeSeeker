using Godot;
using System;
using BackEnd;
using System.Collections.Generic;

public partial class main : Control
{
	BackEnd.BackEnd Be = new BackEnd.BackEnd();

	[Export]string media;
	[Export]int item_size_x;
	[Export]int item_size_y;
	[Export]bool debug = false;
	int max_slot = 50;
	private void OnButtonPressed()
	{
		var grid = GetNode<GridContainer>("Buttom/ScrollContainer/GridContainer");
		foreach (Control i in grid.GetChildren())
		{
			i.QueueFree();
		}
		var textbox = GetNode<LineEdit>("top/HBoxContainer/LineEdit");
		string INP;
		if (textbox.Text != null)
		{
			INP = textbox.Text;
		}
		else
			INP = "null";
		List<Med> meds;
		if (debug)
		{
			meds = Be.Search("debug");
		}
		else
		{
			meds = Be.Search(INP);
		}

		var rescontaner = GetNode<ScrollContainer>("Buttom/ScrollContainer");
		grid.CustomMinimumSize = new Vector2(item_size_x * 4,(max_slot / 4) * item_size_y);
		PackedScene Media = (PackedScene)GD.Load(media);

		foreach (Med med in meds)
		{
			var mediainstance = (MEDIA)Media.Instantiate();
			mediainstance.setcreds(med.id,med.is_debug,med.name,med.year,med.poster);
			grid.AddChild(mediainstance);
		}
	}
}
