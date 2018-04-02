<?php
	$servername = "127.0.0.1";
	$server_username = "root";
	$server_password = "autoset";
	$dbName = "purigon";

	//Make Connection
	$conn = new mysqli($servername, $server_username, $server_password, $dbName);
	//Check Connection
	if(!$conn){
		die("Connection Failed. ". mysqli_connect_error());
	}
//	else echo("Connection Success");
	
	$sql = "SELECT CID, CTYPE, HP, BSPEED, MSPEED, ACCEL, WEIGHT FROM charinfo";
	$result = mysqli_query($conn, $sql);
	
	if(mysqli_num_rows($result) > 0){
		//show data for each row
		while($row = mysqli_fetch_assoc($result)){
			echo "CID:".$row['CID'] . "|CTYPE:".$row['CTYPE']. "|HP:".$row['HP']. "|BSPEED:".$row['BSPEED']. "|MSPEED:".$row['MSPEED']. "|ACCEL:".$row['ACCEL']. "|WEIGHT:".$row['WEIGHT'] . ";";
			
		}
	}

?>