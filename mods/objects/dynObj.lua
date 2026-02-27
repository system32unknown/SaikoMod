function onAction(self)
	local yandpos = yand.transform.position
	local playerpos = player.transform.position
	
	yandNav.enabled = false
	yand.transform.position = playerpos
	yandNav.enabled = true
	player.transform.position = yandpos
	self:Destroy()
end