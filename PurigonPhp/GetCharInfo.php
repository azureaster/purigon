<?php
	$servername = "127.0.0.1";
	$server_username = "root";
	$server_password = "autoset";
	$dbName = "purigon";
	
	$charId = $_POST["charIDPost"];
	
	//Make Connection
	$conn = new mysqli($servername, $server_username, $server_password, $dbName);
	
	if(!$conn){
		die("Connection Failed. ". mysqli_connect_error());
	}
	
	//$sql = "SELECT UID, MID, DAILYBEST, TOTALBEST FROM userrecord WHERE MID = '".$mapId."' ORDER BY TOTALBEST DESC";
	
	$q = "SELECT CID, CTYPE, HP, BSPEED, MSPEED, ACCEL, WEIGHT FROM charinfo WHERE CID='".$charId."'";
	
	$result = mysqli_query($conn, $q);
	
	if(mysqli_num_rows($result) > 0){
		while($row = mysqli_fetch_assoc($result)){ 
			echo "CID:".$row['CID']. "|CTYPE:".$row['CTYPE'] . "|HP:".$row['HP'] . "|BSPEED:".$row['BSPEED'] . "|MSPEED:".$row['MSPEED'] . "|ACCEL:".$row['ACCEL']. "|WEIGHT:".$row['WEIGHT']. ";";
		}
	}
	else echo "No such charId: ".$charId ;

?>