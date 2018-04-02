<?php
	$servername = "127.0.0.1";
	$server_username = "root";
	$server_password = "autoset";
	$dbName = "purigon";
	
	$userID = $_POST["userIDPost"];
	
	//Make Connection
	$conn = new mysqli($servername, $server_username, $server_password, $dbName);
	
	if(!$conn){
		die("Connection Failed. ". mysqli_connect_error());
	}
	
	$sql = "SELECT ID, NAME, LV, EXP, CHARNUM, SKILLNUM FROM userinfo WHERE ID = '".$userID."' ";
	$result = mysqli_query($conn, $sql);
	
	if(mysqli_num_rows($result) > 0){
		while($row = mysqli_fetch_assoc($result)){ 
			echo "ID:".$row['ID'] . "|NAME:".$row['NAME']. "|LV:".$row['LV']. "|EXP:".$row['EXP']. "|CHARNUM:".$row['CHARNUM']. "|SKILLNUM:".$row['SKILLNUM']. ";";
		}
	}
	else echo "No such ID: ".$userID ;

?>