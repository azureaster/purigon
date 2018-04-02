<?php
	$servername = "127.0.0.1";
	$server_username = "root";
	$server_password = "autoset";
	$dbName = "purigon";
	
	$mapId = $_POST["mapIDPost"];
	$whichBest = $_POST["bestRecordPost"];
	
	//Make Connection
	$conn = new mysqli($servername, $server_username, $server_password, $dbName);
	
	if(!$conn){
		die("Connection Failed. ". mysqli_connect_error());
	}
	
	//$sql = "SELECT UID, MID, DAILYBEST, TOTALBEST FROM userrecord WHERE MID = '".$mapId."' ORDER BY TOTALBEST DESC";
	
	$q = "SELECT userrecord.*, userinfo.NAME, userinfo.CHARNUM, userinfo.SKILLNUM FROM userrecord INNER JOIN userinfo ON userrecord.UID = userinfo.ID WHERE userrecord.UID=userinfo.ID AND userrecord.MID='".$mapId."' ORDER BY '".$whichBest."' DESC";
	$q = str_replace("'","",$q); 
	
	$result = mysqli_query($conn, $q);
	
	if(mysqli_num_rows($result) > 0){
		while($row = mysqli_fetch_assoc($result)){ 
			echo "MID:".$row['MID']. "|UID:".$row['UID'] . "|NAME:".$row['NAME'] . "|CHARNUM:".$row['CHARNUM'] . "|SKILLNUM:".$row['SKILLNUM'] . "|DAILYBEST:".$row['DAILYBEST']. "|TOTALBEST:".$row['TOTALBEST']. "|PRACTICEBEST:".$row['PRACTICEBEST']. ";";
		}
	}
	else echo "No such mapId: ".$mapId
?>