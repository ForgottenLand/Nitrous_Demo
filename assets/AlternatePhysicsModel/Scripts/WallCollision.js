function OnCollisionEnter(myCollision : Collision){
	if(myCollision.gameObject.name == "Road_Coll01_Side" || myCollision.gameObject.name == "Road_Coll03_Side" || myCollision.gameObject.name == "Road_Coll_Side" || myCollision.gameObject.name == "Arcade Lambo" || myCollision.gameObject.name == "Charger"){
		Handheld.Vibrate();
	}
}