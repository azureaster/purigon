<?php
	$servername = "127.0.0.1";
	$server_username = "root";
	$server_password = "autoset";
	$dbName = "purigon";
	
	$userInputEmail = $_POST["userEmailPost"];
	
	$conn = new mysqli($servername, $server_username, $server_password, $dbName);
	
	if(!$conn){
		die("Connection Failed. ". mysqli_connect_error());
	}
	$sql = "SELECT ID, PW FROM userinfo WHERE EMAIL = '".$userInputEmail."' ";
	$result = mysqli_query($conn, $sql);
	
	if(mysqli_num_rows($result) > 0){
		while($row = mysqli_fetch_assoc($result)){ 
			echo "ID:".$row['ID'] ."|PW:".$row['PW']. ";";
		}
	}
	
	else echo "INVALID";

?>