<?php
	$servername = "127.0.0.1";
	$server_username = "root";
	$server_password = "autoset";
	$dbName = "purigon";
	
	$userID = $_POST["userIDPost"];
	$mapID = $_POST["mapIDPost"];
	
	//Make Connection
	$conn = new mysqli($servername, $server_username, $server_password, $dbName);
	
	if(!$conn){
		die("Connection Failed. ". mysqli_connect_error());
	}
	
	$sql = "SELECT UID, MID, DAILYBEST, TOTALBEST FROM userrecord WHERE MID = '".$mapID."' ";
	$q = "SELECT userinfo.*, useritem.gold FROM userinfo Inner join useritem on userinfo.userId = useritem.userId where userinfo.userId='".$userId."';";
	
	$result = mysqli_query($conn, $sql);
	
	if(mysqli_num_rows($result) > 0){
		while($row = mysqli_fetch_assoc($result)){ 
			echo "UID:".$row['UID'] . "|MID:".$row['MID']. "|DAILYBEST:".$row['DAILYBEST']. "|TOTALBEST:".$row['TOTALBEST']. ";";
		}
	}
	else echo "No such MapID: ".$mapID ;

	

?>