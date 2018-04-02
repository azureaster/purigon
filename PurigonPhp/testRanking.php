<?php
	$servername = "127.0.0.1";
	$server_username = "root";
	$server_password = "autoset";
	$dbName = "purigon";
	
	$userId = //$_POST["userIDPost"];
	$mapId = 2;//$_POST["mapIDPost"];
	$whichBest = "DAILYBEST";//$_POST["bestRecordPost"];
	
	//Make Connection
	$conn = new mysqli($servername, $server_username, $server_password, $dbName);
	
	if(!$conn){
		die("Connection Failed. ". mysqli_connect_error());
	}
	
	
	$q = "SELECT UID, MID, DAILYBEST, TOTALBEST FROM userrecord WHERE MID = '".$mapId."' ORDER BY '".$whichBest."' DESC";
	$q = str_replace("'","",$q); 
	echo $q;
	//$q = "SELECT userrecord.*, userinfo.NAME, userinfo.CHARNUM, userinfo.SKILLNUM FROM userrecord INNER JOIN userinfo ON userrecord.UID = userinfo.ID WHERE userrecord.UID=userinfo.ID AND userrecord.MID='".$mapId."' ORDER BY '".$whichBest."' DESC";
	
	$result = mysqli_query($conn, $q);
	
	if(mysqli_num_rows($result) > 0){
		while($row = mysqli_fetch_assoc($result)){ 
			echo "MID:".$row['MID']. "|UID:".$row['UID'] . "|DAILYBEST:".$row['DAILYBEST']. "|TOTALBEST:".$row['TOTALBEST']. ";";
		}
	}
	else echo "No such mapId: ".$mapId ;

?>