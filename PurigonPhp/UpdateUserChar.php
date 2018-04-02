<?php
	$servername = "127.0.0.1";
	$server_username = "root";
	$server_password = "autoset";
	$dbName = "purigon";
	
	$userID = $_POST["userIDPost"];
	$charId = $_POST["charIDPost"];
	$skillId = $_POST["skillIDPost"];
	
	//Make Connection
	$conn = new mysqli($servername, $server_username, $server_password, $dbName);
	
	if(!$conn){
		die("Connection Failed. ". mysqli_connect_error());
	}
	
	$sql = "UPDATE userinfo SET CHARNUM = '".$charId."', SKILLNUM = '".$skillId."' WHERE ID='".$userID."'";
	
	$result = mysqli_query($conn, $sql);
	
	if(mysqli_num_rows($result) > 0){
		while($row = mysqli_fetch_assoc($result)){ 
			echo "ID:".$row['ID'] . "|CHARNUM:".$row['CHARNUM']. ";";
		}
	}
	else echo "SAVED" ;

?>