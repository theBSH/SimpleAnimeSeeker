extends PopupPanel

var label : Label

func _ready():
	label = $Control/Label

func _on_close_requested():
	hide()

func sh():
	show()
