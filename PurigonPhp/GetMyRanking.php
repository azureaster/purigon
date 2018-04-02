<?php
	$servername = "127.0.0.1";
	$server_username = "root";
	$server_password = "autoset";
	$dbName = "purigon";
	
	$userId = $_POST["userIDPost"];
	$mapId = $_POST["mapIDPost"];
	
	$conn = new mysqli($servername, $server_username, $server_password, $dbName);
	
	if(!$conn){
		die("Connection Failed. ". mysqli_connect_error());
	}
	
	$q = "SELECT MID, UID, TOTALBEST, DAILYBEST, PRACTICEBEST FROM userrecord WHERE MID='".$mapId."' AND UID='".$userId."'";
	
	$result = mysqli_query($conn, $q);
	
	if(mysqli_num_rows($result) > 0){
		while($row = mysqli_fetch_assoc($result)){ 
			echo "MID:".$row['MID']. "|UID:".$row['UID'] . "|DAILYBEST:".$row['DAILYBEST'] . "|TOTALBEST:".$row['TOTALBEST'] . "|PRACTICEBEST:".$row['PRACTICEBEST'] . ";";
		}
	}
	else echo "No such userId: ".$userId ;

?>